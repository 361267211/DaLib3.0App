using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class GoodsRecord : BaseEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 类型 0:虚拟 1:实物
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 总量
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 当前库存量
        /// </summary>
        public int CurrentCount { get; set; }
        /// <summary>
        /// 锁定量
        /// </summary>
        public int FreezeCount { get; set; }
        /// <summary>
        /// 兑换量
        /// </summary>
        public int SaleOutCount { get; set; }
        /// <summary>
        /// 所需积分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 商品图片
        /// </summary>
        [StringLength(200)]
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 领取方式
        /// </summary>
        public int ObtainWay { get; set; }
        /// <summary>
        /// 领取地址
        /// </summary>
        [StringLength(500)]
        public string ObtainAddress { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        [StringLength(200)]
        public string ObtainContact { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string DetailInfo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 获取时间
        /// </summary>
        public string ObtainTime { get; set; }
    }
}
