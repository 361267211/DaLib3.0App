using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class UserEventScore : BaseEntity<Guid>
    {

        /// <summary>
        /// 应用编码
        /// </summary>
        [Required]
        [StringLength(20)]
        public string AppCode { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public string EventName { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string EventCode { get; set; }

        /// <summary>
        /// 事件完成编码
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FullEventCode { get; set; }
        /// <summary>
        /// 类型 1:加 -1:减
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int EventScore { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UserKey { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(500)]
        public string Remark { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime TriggerTime { get; set; }
        /// <summary>
        /// 积分获取日期
        /// </summary>
        public DateTime? ScoreObtainDate { get; set; }
        /// <summary>
        /// 积分到期
        /// </summary>
        public DateTime? ScoreExpireDate { get; set; }
    }
}
