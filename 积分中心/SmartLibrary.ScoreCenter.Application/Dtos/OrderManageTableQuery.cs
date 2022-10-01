/*********************************************************
* 名    称：OrderManageTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：订单管理查询
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 订单管理查询
    /// </summary>
    public class OrderManageTableQuery : TableQueryBase
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 奖品类型
        /// </summary>
        public int? GoodsType { get; set; }
        /// <summary>
        /// 兑换状态
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// 兑换方式
        /// </summary>
        public int? ObtainWay { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 读者名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 兑换开始时间
        /// </summary>
        public DateTime? TriggerStartTime { get; set; }
        /// <summary>
        /// 兑换结束时间
        /// </summary>
        public DateTime? TriggerEndTime { get; set; }
        /// <summary>
        /// 实际用于对比时间
        /// </summary>
        public DateTime? TriggerEndCompareTime
        {
            get
            {
                if (!TriggerEndTime.HasValue)
                {
                    return TriggerEndTime;
                }
                return TriggerEndTime.Value.AddDays(1);
            }
        }
    }
}
