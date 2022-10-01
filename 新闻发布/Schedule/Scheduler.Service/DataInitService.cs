using Microsoft.Extensions.Configuration;
using Scheduler.Service.Entity;
using Scheduler.Service.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Service
{
    /// <summary>
    /// 数据初始化服务
    /// </summary>
    public class DataInitService
    {
        private SqlSugarClient _db;

        private IConfigurationRoot _jobConfig;

        public void Init(IConfigurationRoot config, List<SchedulerEntity> schedulerEntityList)
        {
            _jobConfig = config;
            _db = DataHelper.GetInstance(config);//获取数据库操作对象
            //创建任务列表
            _db.CodeFirst.InitTables(typeof(SchedulerEntity), FileHelper.ReadFileToText("Scripts/CreateSchedulerEntity.txt"));
            //创建任务执行日志
            _db.CodeFirst.InitTables(typeof(SchedulerLogEntity), FileHelper.ReadFileToText("Scripts/CreateSchedulerLogEntity.txt"));
            //初始化业务任务同步任务
            InitTaskJobs(schedulerEntityList);
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        private void InitTaskJobs(List<SchedulerEntity> schedulerEntityList)
        {

            if (!schedulerEntityList.Any(c => c.ClassFullName == "Scheduler.Core.Job.SchedulerJob"))
            {
                //初始化一个任务，用来处理动态管理任务
                schedulerEntityList.Insert(0, new SchedulerEntity()
                {
                    TenantId = SchedulerKey.PUBLICTENANT_KEY,
                    AssemblyFullName = "Scheduler.Core",
                    ClassFullName = "Scheduler.Core.Job.SchedulerJob",
                    AdapterAssemblyFullName = "",
                    TaskParam = "",
                    AdapterParm = "",
                    AdapterClassFullName = "",
                    JobStatus = JobStatus.RUN,
                    Cron = "0/15 * * * * ?",
                    CronRemark = "",
                    Remark = "实时监控任务改变列表,调度自运行监控任务,勿删",
                    JobName = "SchedulerJob",
                    JobGroup = SchedulerKey.SCHEDULER_JOBGROUP_KEY,
                });
            }

            //if (!schedulerEntityList.Any(c => c.ClassFullName == "Scheduler.Core.Job.TriggerTimeJob"))
            //{
            //    schedulerEntityList.Insert(0, new SchedulerEntity()
            //    {
            //        AssemblyFullName = "Scheduler.Core",
            //        ClassFullName = "Scheduler.Core.Job.TriggerTimeJob",
            //        AdapterAssemblyFullName = "",
            //        TaskParam = "",
            //        AdapterParm = "",
            //        AdapterClassFullName = "",
            //        JobStatus = JobStatus.RUN,
            //        Cron = "00 59 23 * * ?",
            //        CronRemark = "",
            //        Remark = "计算明天任务触发时间列表,调度自运行监控任务,勿删",
            //        JobName = "TriggerTimeJob",
            //        JobGroup = SchedulerKey.SCHEDULER_JOBGROUP_KEY,
            //    });
            //}

            var dbSchedulerEntityList = _db.Queryable<SchedulerEntity>().Where(c => c.IsDelete == 0).ToList();
            schedulerEntityList.ForEach(c =>
            {
                if (dbSchedulerEntityList.Any(d => d.JobName == c.JobName))
                {
                    c.Id = dbSchedulerEntityList.First(d => d.JobName == c.JobName).Id;
                    _db.Updateable(c).ExecuteCommand();
                }
                else
                {
                    _db.Insertable(c).ExecuteCommand();
                }
            });

        }
    }
}
