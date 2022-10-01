using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using Scheduler.Service;
using Scheduler.Service.Entity;
using Scheduler.Service.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Core
{
    /// <summary>
    /// 任务调度中心，后续操作都基于任务中心
    /// </summary>
    public class SchedulerCenter
    {
        /// <summary>
        /// Quartz任务调度器
        /// </summary>
        private IScheduler _scheduler;
        /// <summary>
        /// 服务配置
        /// </summary>
        public IConfigurationRoot SmartTaskConfig => ConfigHelper.TaskConfig;
        /// <summary>
        /// 任务调度中心
        /// </summary>
        public static readonly SchedulerCenter Instance;

        /// <summary>
        /// 任务表(用来维护调度任务与数据库记录任务同步更新)
        /// </summary>
        public List<SchedulerEntity> SchedulerEntityList = new List<SchedulerEntity>();

        /// <summary>
        /// 用于初始化任务实体列表
        /// </summary>
        public List<SchedulerEntity> ScheduleEntityListInit = new List<SchedulerEntity>();

        static SchedulerCenter()
        {
            try
            {
                Instance = new SchedulerCenter();
            }
            catch (Exception ex)
            {
                LogManager.Info(ex.Message);
            }
        }

        private IScheduler Scheduler
        {
            get
            {
                if (this._scheduler != null)
                {
                    return this._scheduler;
                }

                // 从Factory中获取Scheduler实例
                NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" },
                };

                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                return this._scheduler = factory.GetScheduler().Result;
            }
        }

        public async Task RunAsync()
        {
            try
            {
                //初始数据
                var dataInitService = new DataInitService();
                dataInitService.Init(SmartTaskConfig, ScheduleEntityListInit);

                if (!this.Scheduler.IsStarted)
                {
                    //添加调度任务
                    var listTask = new SchedulerDataService(SmartTaskConfig).GetSchedulerEntityList(true);
                    if (listTask != null && listTask.Count > 0)
                    {
                        foreach (var item in listTask)
                        {
                            if (CreateSchedulerJob(item).GetAwaiter().GetResult())
                            {
                                Console.WriteLine("CreateSchedulerJob Succed,Id:" + item.Id + " JobName:" + item.JobName);
                            }
                        }
                    }
                    await this.Scheduler.Start();
                }
                LogManager.Info(string.Format("定时任务启动 AT {0} -------------------------------", DateTime.Now.ToLocalTime().ToString()));
            }
            catch (Exception ex)
            {
                LogManager.Error(string.Format("定时服务启动异常,ex:{0}", ex.Message));

                Thread.Sleep(60000); //启动失败自动重试

                LogManager.Info(string.Format("尝试重新启动!"));

                await RunAsync();
            }
        }

        /// <summary>
        /// 调度新增一个任务，如果任务设置的时间永远不会触发，则修改任务状态为JobStatus.NOFIRED
        /// </summary>
        /// <param name="schedulerEntity">任务实体</param>
        public async Task<bool> CreateSchedulerJob(SchedulerEntity schedulerEntity)
        {
            try
            {
                //反射获取任务执行类
                var jobType = FileHelper.GetAbsolutePath(schedulerEntity.AssemblyFullName, schedulerEntity.ClassFullName);
                // 定义这个工作，并将其绑定到我们的IJob实现类
                IJobDetail job = new JobDetailImpl(schedulerEntity.Id + "", schedulerEntity.JobGroup, jobType);
                job.JobDataMap.Put(SchedulerKey.SCHEDULERENTITY_KEY, schedulerEntity);
                // 创建触发器
                ITrigger trigger;
                //校验是否正确的执行周期表达式
                if (!string.IsNullOrEmpty(schedulerEntity.Cron) && CronExpression.IsValidExpression(schedulerEntity.Cron))
                {
                    trigger = CreateCronTrigger(schedulerEntity);
                }
                else
                {
                    trigger = CreateSimpleTrigger(schedulerEntity);
                }

                // 告诉Quartz使用我们的触发器来安排作业
                await this.Scheduler.ScheduleJob(job, trigger);
                SchedulerEntityList.Add(schedulerEntity);

                return true;
            }
            catch (Exception ex)
            {
                schedulerEntity.JobStatus = JobStatus.NOFIRED;

                var obj = SchedulerEntityList.FirstOrDefault(c => c.Id == schedulerEntity.Id);
                if (obj != null)
                {
                    SchedulerEntityList.Remove(obj);
                    SchedulerEntityList.Add(schedulerEntity);
                }
                else
                {
                    SchedulerEntityList.Add(schedulerEntity);
                }


                LogManager.Warning("CreateSchedulerJob ：" + schedulerEntity.Id + " ex:" + ex.Message);
            }

            return false;

        }

        /// <summary>
        /// 从调度计划中删除任务
        /// </summary>
        /// <param name="schedulerEntity">任务实体</param>
        public async Task<bool> DeleteSchedulerJob(SchedulerEntity schedulerEntity)
        {
            var jk = new JobKey(schedulerEntity.Id + "", schedulerEntity.JobGroup);


            if (await this.Scheduler.CheckExists(jk))
            {
                try
                {
                    SchedulerEntityList.RemoveAll(c => c.Id == schedulerEntity.Id);
                    if (await this.Scheduler.UnscheduleJob(new TriggerKey(schedulerEntity.Id + "", schedulerEntity.JobGroup))) //删除任务对应的Trigger
                    {
                        await this.Scheduler.DeleteJob(jk); //删除任务
                    }
                    else
                    {
                        await this.Scheduler.DeleteJob(jk); //删除任务
                    }

                    return true;

                }
                catch (Exception ex)
                {

                    LogManager.Info("DeleteSchedulerJob ：" + ex.Message);
                }
            }
            else
            {
                SchedulerEntityList.RemoveAll(c => c.Id == schedulerEntity.Id);
            }

            return true;
        }

        /// <summary>
        /// 更新调度中的任务（如果存在先删除再添加，不存在直接添加）
        /// </summary>
        /// <param name="schedulerEntity">任务实体</param>
        /// <returns></returns>
        public async Task<bool> UpdateSchedulerJob(SchedulerEntity schedulerEntity)
        {
            var jk = new JobKey(schedulerEntity.Id + "", schedulerEntity.JobGroup);
            if (await this.Scheduler.CheckExists(jk))
            {
                if (DeleteSchedulerJob(schedulerEntity).GetAwaiter().GetResult())
                {
                    if (CreateSchedulerJob(schedulerEntity).GetAwaiter().GetResult())
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (CreateSchedulerJob(schedulerEntity).GetAwaiter().GetResult())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 暂停调度计划中的任务,线程挂起中
        /// </summary>
        /// <param name="schedulerEntity">任务实体</param>
        public async void PauseSchedulerJob(SchedulerEntity schedulerEntity)
        {
            try
            {
                var jk = new JobKey(schedulerEntity.Id + "", schedulerEntity.JobGroup);
                if (await this.Scheduler.CheckExists(jk))//任务是否存在
                {
                    if (this.Scheduler.PauseJob(jk).IsCompletedSuccessfully)
                    {
                        Console.WriteLine("PauseSchedulerJob:" + schedulerEntity.Id);
                        SchedulerEntityList.FirstOrDefault(c => c.Id == schedulerEntity.Id).JobStatus = JobStatus.PAUSE;

                    }
                }
                else
                {
                    //  Console.WriteLine(" PauseSchedulerJob schedulerEntity Not Run:" + schedulerEntity.Id);
                }
            }
            catch (Exception ex)
            {
                LogManager.Warning("PauseSchedulerJob ：" + schedulerEntity.Id + " ex:" + ex.Message);
            }
        }

        /// <summary>
        /// 恢复调度计划任务
        /// </summary>
        /// <param name="schedulerEntity">任务实体</param>
        /// <returns></returns>
        public async void ResumeSchedulerJob(SchedulerEntity schedulerEntity)
        {
            try
            {
                //检查任务是否已存在
                var jk = new JobKey(schedulerEntity.Id + "", schedulerEntity.JobGroup);
                if (await this.Scheduler.CheckExists(jk))
                {
                    if (SchedulerEntityList.FirstOrDefault(c => c.Id == schedulerEntity.Id).JobStatus == JobStatus.PAUSE)
                    {
                        await this.Scheduler.ResumeJob(jk); //恢复任务
                        SchedulerEntityList.FirstOrDefault(c => c.Id == schedulerEntity.Id).JobStatus = JobStatus.RUN;
                        Console.WriteLine("ResumeSchedulerJob:" + schedulerEntity.Id);
                    }
                }
                else
                {
                    // Console.WriteLine(" ResumeSchedulerJob schedulerEntity Not Run:" + schedulerEntity.Id);
                }


            }
            catch (Exception ex)
            {
                LogManager.Warning("ResumeSchedulerJob ：" + schedulerEntity.Id + " ex:" + ex.Message);
            }

        }

        /// <summary>
        /// 创建类型Simple的触发器
        /// </summary>
        /// <param name="schedulerEntity"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(SchedulerEntity schedulerEntity)
        {
            //作业触发器
            if (schedulerEntity.RunTimes > 0)
            {
                var trigger = TriggerBuilder.Create()
               .WithIdentity(schedulerEntity.Id + "", schedulerEntity.JobGroup)
               .StartAt(schedulerEntity.BeginTime ?? DateTime.Now)//开始时间
                                                                  //.StartAt(m.BeginTime)//开始时间
                                                                  //.EndAt(m.EndTime)//结束数据
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(schedulerEntity.IntervalSecond)//执行时间间隔，单位秒
                   .WithRepeatCount(schedulerEntity.RunTimes))//执行次数、默认从0开始
                   .ForJob(schedulerEntity.Id + "", schedulerEntity.JobGroup)//作业名称
               .Build();
                ((SimpleTriggerImpl)trigger).MisfireInstruction = MisfireInstruction.SimpleTrigger.FireNow;//错过立即执行一次
                return trigger;
            }
            else
            {
                var trigger = TriggerBuilder.Create()
               .WithIdentity(schedulerEntity.Id + "", schedulerEntity.JobGroup)
               .StartAt(schedulerEntity.BeginTime ?? DateTime.Now)//开始时间
                                                                  //.StartAt(m.BeginTime)//开始时间
                                                                  //.EndAt(m.EndTime)//结束数据
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(schedulerEntity.IntervalSecond)//执行时间间隔，单位秒
                   .RepeatForever())//无限循环
                   .ForJob(schedulerEntity.Id + "", schedulerEntity.JobGroup)//作业名称
               .Build();
                ((SimpleTriggerImpl)trigger).MisfireInstruction = MisfireInstruction.SimpleTrigger.FireNow;//错过立即执行一次
                return trigger;
            }

        }

        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="schedulerEntity"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(SchedulerEntity schedulerEntity)
        {

            // 作业触发器
            var trigger = TriggerBuilder.Create()
                   .WithIdentity(schedulerEntity.Id + "", schedulerEntity.JobGroup)
                   //.StartNow()
                   .StartAt(schedulerEntity.BeginTime ?? DateTime.Now)//开始时间
                                                                      //.EndAt(m.EndTime)//结束数据
                   .WithCronSchedule(schedulerEntity.Cron)//指定cron表达式
                   .ForJob(schedulerEntity.Id + "", schedulerEntity.JobGroup)//作业名称
                   .Build();
            ((CronTriggerImpl)trigger).MisfireInstruction = MisfireInstruction.CronTrigger.FireOnceNow;//错过立即执行一次
            return trigger;
        }
    }
}
