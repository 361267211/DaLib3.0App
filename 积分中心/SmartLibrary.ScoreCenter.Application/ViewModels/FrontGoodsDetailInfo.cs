/*********************************************************
* 名    称：FrontGoodsDetailInfo.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：前台商品详情信息
* 更新历史：
*
* *******************************************************/
using System;
namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 前台商品详情信息
    /// </summary>
    public class FrontGoodsDetailInfo
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 所需积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 当前库存量
        /// </summary>
        public int CurrentCount { get; set; }

        /// <summary>
        /// 兑换时间
        /// </summary>
        public DateTime? ExchangeStartTime { get; set; }
        /// <summary>
        /// 兑换截止时间
        /// </summary>
        public DateTime? ExchangeEndTime { get; set; }
        /// <summary>
        /// 获取时间，描述信息
        /// </summary>
        public string ObtainTime { get; set; }
        /// <summary>
        /// 详情信息
        /// </summary>
        public string DetailInfo { get; set; }
    }
}
