using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class ScoreObtainTask : BaseEntity<Guid>
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
        /// 事件触发应用
        /// </summary>
        [Required]
        [StringLength(20)]
        public string AppCode { get; set; }
        /// <summary>
        /// 事件编码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EventCode { get; set; }

        /// <summary>
        /// 事件编码全称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string FullEventCode { get; set; }
        /// <summary>
        /// 获取积分
        /// </summary>
        public int ObtainScore { get; set; }
        /// <summary>
        /// 宣传图地址
        /// </summary>
        [StringLength(200)]
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// pc引导地址
        /// </summary>
        [StringLength(200)]
        public string PcLink { get; set; }
        /// <summary>
        /// App引导地址
        /// </summary>
        [StringLength(200)]
        public string AppLink { get; set; }
        /// <summary>
        /// 小程序引导地址
        /// </summary>
        [StringLength(200)]
        public string MicroAppLink { get; set; }

        /// <summary>
        /// H5引导地址
        /// </summary>
        [StringLength(200)]
        public string H5Link { get; set; }
        /// <summary>
        /// 适用PC
        /// </summary>
        public bool ForPc { get; set; }
        /// <summary>
        /// 适用App
        /// </summary>
        public bool ForApp { get; set; }
        /// <summary>
        /// 适用小程序
        /// </summary>
        public bool ForMicroApp { get; set; }
        /// <summary>
        /// 适用H5
        /// </summary>
        public bool ForH5 { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int ValidTerm { get; set; }
        /// <summary>
        /// 任务截止日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 触发周期,0:固定1:每日2:每周3:每月4:每年
        /// </summary>
        public int TriggerTerm { get; set; }
        /// <summary>
        /// 周期内可重复触发次数
        /// </summary>
        public int TriggerTime { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
