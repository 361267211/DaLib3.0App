using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class OrderRecord : BaseEntity<Guid>
    {
        /// <summary>
        /// 商品标识
        /// </summary>
        public Guid GoodsID { get; set; }
        /// <summary>
        /// 兑换码
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ExchangeCode { get; set; }
        /// <summary>
        /// 兑换数量
        /// </summary>
        public int ExchangeCount { get; set; }
        /// <summary>
        /// 消耗积分
        /// </summary>
        public int ExchangeScore { get; set; }
        /// <summary>
        /// 兑换账号
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ExchangeUserKey { get; set; }
        /// <summary>
        /// 兑换名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ExchangeName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 快递名称
        /// </summary>
        [StringLength(200)]
        public string Express { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        [StringLength(200)]
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
        [StringLength(200)]
        public string ObtainTime { get; set; }

        /// <summary>
        /// 邮寄地址
        /// </summary>
        [StringLength(500)]
        public string RecieveAddrss { get; set; }
    }
}
