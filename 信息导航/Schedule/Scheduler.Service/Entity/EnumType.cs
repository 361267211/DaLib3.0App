using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Service.Entity
{
    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// 运行中
        /// </summary>
        RUN = 0,

        /// <summary>
        /// 暂停中
        /// </summary>
        PAUSE = 1,


        /// <summary>
        /// 不会命中的
        /// </summary>
        NOFIRED = 2

    }


    /// <summary>
    /// 任务运行状态
    /// </summary>
    public enum JobRunStatus
    {
        /// <summary>
        /// 不存在
        /// </summary>
        STATE_NONE = -1,

        /// <summary>
        /// 正常
        /// </summary>
        STATE_NORMAL = 0,

        /// <summary>
        /// 暂停
        /// </summary>
        STATE_PAUSED = 1,

        /// <summary>
        /// 完成
        /// </summary>
        STATE_COMPLETE = 2,

        /// <summary>
        /// 错误
        /// </summary>
        STATE_ERROR = 3,

        /// <summary>
        /// 阻塞
        /// </summary>
        STATE_BLOCKED = 4

    }
}
