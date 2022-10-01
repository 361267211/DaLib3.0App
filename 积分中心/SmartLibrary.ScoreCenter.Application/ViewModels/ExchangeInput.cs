/*********************************************************
* 名    称：ExchangeInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分兑换
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分兑换
    /// </summary>
    public  class ExchangeInput
    {
        /// <summary>
        /// 订单标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 操作标识
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        public string Express { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }
    }
}
