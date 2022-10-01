/*********************************************************
* 名    称：ScoreManualTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩查询
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 积分奖惩查询
    /// </summary>
    public class ScoreManualTableQuery : TableQueryBase
    {
        /// <summary>
        /// 奖惩类型
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作开始时间
        /// </summary>
        public DateTime? OperatorStartTime { get; set; }
        /// <summary>
        /// 操作结束时间
        /// </summary>
        public DateTime? OperatorEndTime { get; set; }
        public DateTime? OperatorCompareEndTime
        {
            get
            {
                if (!OperatorEndTime.HasValue)
                {
                    return OperatorEndTime;
                }
                else
                {
                    return OperatorEndTime.Value.AddDays(1);
                }

            }
        }

    }
}
