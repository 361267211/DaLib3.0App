/*********************************************************
* 名    称：ReaderGoodsListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者商品列表
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 读者商品列表
    /// </summary>
    public class ReaderGoodsListItemOutput
    {
        /// <summary>
        /// 商品标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图片地址
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
        /// 兑换数量
        /// </summary>
        public int ExchangeCount { get; set; }
        /// <summary>
        /// 是否喜欢
        /// </summary>
        public bool Like { get; set; }
    }
}
