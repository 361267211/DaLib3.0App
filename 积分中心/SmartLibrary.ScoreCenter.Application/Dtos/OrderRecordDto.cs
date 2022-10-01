/*********************************************************
* 名    称：OrderRecordDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品订单管理
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 商品订单数据
    /// </summary>
    public class OrderRecordDto
    {
        /// <summary>
        /// 商品标识
        /// </summary>
        public Guid GoodsID { get; set; }
        /// <summary>
        /// 兑换码
        /// </summary>
        public string ExchangeCode { get; set; }
        /// <summary>
        /// 兑换数量
        /// </summary>
        public int ExchangeCount { get; set; }
        /// <summary>
        /// 兑换账号
        /// </summary>
        public string ExchangeUserKey { get; set; }
        /// <summary>
        /// 兑换名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 快递名称
        /// </summary>
        public string Express { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }
        /// <summary>
        /// 兑换方式
        /// </summary>
        public int ObtainWay { get; set; }
        /// <summary>
        /// 兑换时间
        /// </summary>
        public DateTime ExchangeTime { get; set; }
        /// <summary>
        /// 领取时间
        /// </summary>
        public string ObtainTime { get; set; }

        /// <summary>
        /// 邮寄地址
        /// </summary>
        public string RecieveAddrss { get; set; }
    }
}
