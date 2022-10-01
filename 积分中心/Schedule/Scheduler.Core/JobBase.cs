using Quartz;
using Scheduler.Service;
using Scheduler.Service.Entity;
using Scheduler.Service.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core
{
    public abstract class JobBase : IJob
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public virtual string JobName => this.GetType().Name;

        /// <summary>
        /// 系统参数
        /// </summary>
        public virtual string SysKeyCollects => "";

        /// <summary>
        /// 任务自定义的参数(用来定义参数的KEY值，多个以;隔开)
        /// </summary>
        public virtual string KeyCollects => "";

        /// <summary>
        /// 任务对象
        /// </summary>
        public SchedulerEntity SchedulerEntity;

        /// <summary>
        /// 调度对象
        /// </summary>
        public SchedulerCenter SchedulerCenter = SchedulerCenter.Instance;

        /// <summary>
        /// JOB上下文对象
        /// </summary>
        public IJobExecutionContext JobExecutionContext;

        protected SqlSugarClient TenantDb;

        /// <summary>
        /// 任务日志记录服务
        /// </summary>
        protected readonly SchedulerLogEntityService schedulerLogEntityService = new SchedulerLogEntityService(SchedulerCenter.Instance.SmartTaskConfig);

        ///// <summary>
        ///// 任务未来触发时间服务
        ///// </summary>
        //public readonly TriggerTimeEntityService triggerTimeEntityService = new TriggerTimeEntityService(SchedulerCenter.Instance.SmartTaskConfig);

        ///// <summary>
        ///// 任务运行日志服务
        ///// </summary>
        //protected readonly SchedulerJobLogEntityService schedulerJobLogEntityService = new SchedulerJobLogEntityService(SchedulerCenter.Instance.SmartTaskConfig);

        /// <summary>
        /// 任务数据服务
        /// </summary>
        //protected readonly SchedulerDataService schedulerDataService = new SchedulerDataService(SchedulerCenter.Instance.SmartTaskConfig);

        /// <summary>
        /// 将日志数据写入任务上下文对象
        /// </summary>
        /// <param name="schedulerLogEntity"></param>
        private void AddSchedulerLogEntity(SchedulerLogEntity schedulerLogEntity)
        {
            if (JobExecutionContext.MergedJobDataMap.ContainsKey(SchedulerKey.SCHEDULERLOGENTITY_KEY))
            {
                var obj = JobExecutionContext.MergedJobDataMap[SchedulerKey.SCHEDULERLOGENTITY_KEY];
                if (obj != null)
                {
                    (obj as List<SchedulerLogEntity>).Add(schedulerLogEntity);
                }
            }
            else
            {
                var list = new List<SchedulerLogEntity>
                {
                    schedulerLogEntity
                };
                JobExecutionContext.MergedJobDataMap.Add(SchedulerKey.SCHEDULERLOGENTITY_KEY, list);
            }
        }

        /// <summary>
        /// 获取任务上下文对象日志列表
        /// </summary>
        public List<SchedulerLogEntity> GetSchedulerLogEntitys(IJobExecutionContext _context)
        {

            if (_context.MergedJobDataMap.ContainsKey(SchedulerKey.SCHEDULERLOGENTITY_KEY))
            {
                var obj = _context.MergedJobDataMap[SchedulerKey.SCHEDULERLOGENTITY_KEY];
                if (obj != null)
                    return _context.MergedJobDataMap[SchedulerKey.SCHEDULERLOGENTITY_KEY] as List<SchedulerLogEntity>;
            }
            return new List<SchedulerLogEntity>();

        }

        /// <summary>
        /// 任务参数
        /// </summary>
        public string TaskParams;


        /// <summary>
        /// 记录到本任务日志文件中,按任务JOBNAME命名日志文件
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="jobLogCode">日志类型</param>
        /// <param name="immediately">是否立即写入数据库</param>
        /// <returns></returns>
        protected int WriteLog(int id, string content, int status)
        {
            if (string.IsNullOrEmpty(content))
                return 0;
            var schedulerLogEntity = schedulerLogEntityService.GetSchedulerLogEntity(id);
            try
            {
                if (SchedulerEntity != null)
                {

                    LogManager.Log(SchedulerEntity.JobName, content);
                    if (schedulerLogEntity == null)
                    {
                        schedulerLogEntity = new SchedulerLogEntity()
                        {
                            TenantId = SchedulerEntity.TenantId,
                            Context = content,
                            JobId = SchedulerEntity.Id,
                            JobName = SchedulerEntity.JobName,
                            Params = SchedulerEntity.TaskParam,
                            StartTime = DateTime.Now,
                            Status = status,
                            BatchId = SchedulerEntity.Id.ToString(),
                            CreateTime = DateTime.Now
                        };
                        return schedulerLogEntityService.AddSchedulerLogEntity(schedulerLogEntity);
                    }
                    else
                    {
                        schedulerLogEntity.Context = content;
                        schedulerLogEntity.Status = status;
                        if (schedulerLogEntity.Status != 0)
                        {
                            schedulerLogEntity.EndTime = DateTime.Now;
                        }
                        return schedulerLogEntityService.UpdateSchedulerLogEntity(schedulerLogEntity);
                    }
                }
                else
                {
                    LogManager.Error("SchedulerEntity 不存在");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex.Message);
                return 0;
            }

        }

        protected bool LogCheckExpired(string JobName, TimeSpan timeSpan)
        {
            try
            {
                return schedulerLogEntityService.CheckLogExpired(JobName, timeSpan);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 真实业务方法(子类继承实现)
        /// </summary>
        public abstract void DoWork();

        /// <summary>
        /// 开始业务执行
        /// </summary>
        /// <param name="jobExecutionContext"></param>
        /// <returns></returns>
        async Task IJob.Execute(IJobExecutionContext jobExecutionContext)
        {
            try
            {
                JobExecutionContext = jobExecutionContext;

                SchedulerEntity = jobExecutionContext.MergedJobDataMap[SchedulerKey.SCHEDULERENTITY_KEY] as SchedulerEntity;

                TenantDb = DataHelper.GetTenantDb(SchedulerCenter.Instance.SmartTaskConfig, SchedulerEntity.TenantId);

                //初始化任务参数
                if (!string.IsNullOrEmpty(SchedulerEntity.TaskParam))
                {
                    TaskParams = SchedulerEntity.TaskParam;
                }
            }
            catch (Exception ex)
            {
                LogManager.Log(SchedulerEntity.JobName, string.Format("任务参数异常:{0}", ex.Message));
                throw new JobExecutionException(ex, false);
            }
            try
            {
                //开始执行任务
                await Task.Factory.StartNew(DoWork);
            }
            catch (Exception ex)
            {
                LogManager.Log(SchedulerEntity.JobName, string.Format("ex:{0}", ex.Message));
                throw new JobExecutionException(ex, false);
            }
        }

    }
}
