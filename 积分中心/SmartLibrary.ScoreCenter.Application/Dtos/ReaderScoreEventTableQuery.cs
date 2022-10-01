/*********************************************************
* 名    称：ReaderScoreEventTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者积分明细查询
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 读者积分明细查询
    /// </summary>
    public class ReaderScoreEventTableQuery : TableQueryBase
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 变更方式
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 事件发生时间
        /// </summary>
        public DateTime? TriggerStartTime { get; set; }
        /// <summary>
        /// 事件发生时间
        /// </summary>
        public DateTime? TriggerEndTime { get; set; }
        /// <summary>
        /// 实际比较时间
        /// </summary>
        public DateTime? TriggerCompareEndTime
        {
            get
            {
                if (TriggerEndTime.HasValue)
                {
                    return TriggerEndTime.Value.AddDays(1);
                }
                return TriggerEndTime;
            }
        }

    }
}
