/*********************************************************
* 名    称：GoodsListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品列表数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 商品列表数据
    /// </summary>
    public class GoodsListItemDto
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 奖品类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当前库存
        /// </summary>
        public int CurrentCount { get; set; }
        /// <summary>
        /// 总量
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 锁定量
        /// </summary>
        public int FreezeCount { get; set; }
        /// <summary>
        /// 已兑换数量
        /// </summary>
        public int SaleOutCount { get; set; }
        /// <summary>
        /// 所需积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 兑换开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 兑换截止日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}
