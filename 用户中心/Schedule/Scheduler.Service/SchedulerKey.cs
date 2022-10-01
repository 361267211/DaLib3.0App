using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Service
{
    /// <summary>
    /// 调度使用常量
    /// </summary>
    public class SchedulerKey
    {
        /// <summary>
        /// 调度组（调度自运行定时任务）
        /// </summary>
        public const string SCHEDULER_JOBGROUP_KEY = "SchedulerJobGroup";
        /// <summary>
        /// 保持任务的KEY值
        /// </summary>
        public const string SCHEDULERENTITY_KEY = "SCHEDULERENTITY_KEY";
        /// <summary>
        /// 任务执行日志
        /// </summary>
        public const string SCHEDULERLOGENTITY_KEY = "SCHEDULERLOGENTITY_KEY";
        /// <summary>
        /// 公用Tenant
        /// </summary>
        public const string PUBLICTENANT_KEY = "Public";
    }
}
