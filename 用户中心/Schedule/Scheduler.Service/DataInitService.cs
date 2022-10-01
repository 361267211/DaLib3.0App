using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scheduler.Service.Entity;
using Scheduler.Service.Utils;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler.Service
{
    /// <summary>
    /// 数据初始化服务
    /// </summary>
    public class DataInitService
    {
        private SqlSugarClient _Db;

        private IConfigurationRoot _JobConfig;

        private JObject _TaskConfigObject;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config"></param>
        /// <param name="schedulerEntityList"></param>
        public void Init(IConfigurationRoot config, JObject configObject)
        {
            _JobConfig = config;
            _TaskConfigObject = configObject;
            //获取数据库操作对象
            _Db = DataHelper.GetInstance(config);
            //创建任务列表
            _Db.CodeFirst.InitTables(typeof(SchedulerEntity), FileHelper.ReadFileToText("Scripts/CreateSchedulerEntity.txt"));
            //创建任务执行日志
            _Db.CodeFirst.InitTables(typeof(SchedulerLogEntity), FileHelper.ReadFileToText("Scripts/CreateSchedulerLogEntity.txt"));
            //初始化业务任务同步任务
            InitTaskJobs();
        }

        /// <summary>
        /// 根据配置文件初始化任务，和数据库对比，更新或者删除任务
        /// </summary>
        private void InitTaskJobs()
        {
            List<SchedulerEntity> schedulerEntityList = new List<SchedulerEntity>();
            //从配置文件中读取任务配置

            var taskArray = (JArray)_TaskConfigObject["Jobs"];
            foreach (var item in taskArray)
            {
                schedulerEntityList.Add(new SchedulerEntity
                {
                    TenantId = item["TenantId"].ToString(),
                    JobName = item["JobName"].ToString(),
                    Cron = item["Cron"].ToString(),
                    AssemblyFullName = item["AssemblyFullName"].ToString(),
                    ClassFullName = item["ClassFullName"].ToString(),
                    TaskParam = item["TaskParam"].ToString(),
                    AdapterAssemblyFullName = item["AdapterAssemblyFullName"].ToString(),
                    AdapterClassFullName = item["AdapterClassFullName"].ToString(),
                    AdapterParm = item["AdapterParm"].ToString(),
                    JobGroup = SchedulerKey.SCHEDULERENTITY_KEY,
                    JobStatus = JobStatus.RUN,
                    IsRepeat = true
                });
            }

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
                    IsRepeat = true
                });
            }

            var dbSchedulerEntityList = _Db.Queryable<SchedulerEntity>().Where(c => c.IsDelete == 0).ToList();
            schedulerEntityList.ForEach(c =>
            {
                if (dbSchedulerEntityList.Any(d => d.JobName == c.JobName))
                {
                    c.Id = dbSchedulerEntityList.First(d => d.JobName == c.JobName).Id;
                    _Db.Updateable(c).ExecuteCommand();
                }
                else
                {
                    _Db.Insertable(c).ExecuteCommand();
                }
            });

        }
    }
}
