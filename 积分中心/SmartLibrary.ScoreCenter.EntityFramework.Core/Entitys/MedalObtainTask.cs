using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys
{
    public class MedalObtainTask : BaseEntity<Guid>
    {
        /// <summary>
        /// 勋章名称
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
        /// 勋章图片
        /// </summary>
        [StringLength(200)]
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 应用编码
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
        ///// <summary>
        ///// 触发周期
        ///// </summary>
        //public int TriggerTerm { get; set; }
        ///// <summary>
        ///// 周期触发次数
        ///// </summary>
        //public int TriggerTime { get; set; }
        /// <summary>
        /// 触发方式 0:按次数 1:按天数
        /// </summary>
        public int TriggerWay { get; set; }
        /// <summary>
        /// 达成次数
        /// </summary>
        public int TotalTime { get; set; }
        /// <summary>
        /// 是否必须周期连续
        /// </summary>
        public bool MustContinue { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Pc引导地址
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
        /// 适用Pc
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
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
