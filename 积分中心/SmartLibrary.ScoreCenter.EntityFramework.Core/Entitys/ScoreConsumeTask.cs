using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class ScoreConsumeTask : BaseEntity<Guid>
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [StringLength(200)]
        public string Desc { get; set; }
        /// <summary>
        /// 应用编码
        /// </summary>
        [Required]
        [StringLength(20)]
        public string AppCode { get; set; }
        /// <summary>
        /// 触发事件编码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EventCode { get; set; }
        /// <summary>
        /// 消耗积分
        /// </summary>
        public int ConsumeScore { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
