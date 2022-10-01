/*********************************************************
* 名    称：EnumScheduleJobStatus.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：任务状态枚举
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Services.Enum
{
    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum EnumScheduleJobStatus
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
    public enum EnumScheduleJobRunStatus
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
