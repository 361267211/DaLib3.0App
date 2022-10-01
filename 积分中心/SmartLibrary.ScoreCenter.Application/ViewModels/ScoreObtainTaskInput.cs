/*********************************************************
* 名    称：ScoreObtainTaskInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分任务创建对象
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分任务创建对象
    /// </summary>
    public class ScoreObtainTaskInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        [Required(ErrorMessage = "请填写任务名称")]
        [MaxLength(20, ErrorMessage = "任务名称最多输入20个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "任务描述最多输入200个字符")]
        public string Desc { get; set; }
        /// <summary>
        /// 事件触发应用
        /// </summary>
        [Required(ErrorMessage = "应用编码必填")]
        [StringLength(20, ErrorMessage = "应用编码最多输入20个字符")]
        public string AppCode { get; set; }
        /// <summary>
        /// 事件编码
        /// </summary>
        [Required(ErrorMessage = "事件编码必填")]
        [StringLength(50, ErrorMessage = "事件编码最多输入50个字符")]
        public string EventCode { get; set; }
        /// <summary>
        /// 获取积分
        /// </summary>
        public int ObtainScore { get; set; }
        /// <summary>
        /// 宣传图地址
        /// </summary>
        [StringLength(200, ErrorMessage = "宣传图地址最多输入200个字符")]
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// pc引导地址
        /// </summary>
        [StringLength(200, ErrorMessage = "链接地址最多输入200个字符")]
        public string PcLink { get; set; }
        /// <summary>
        /// App引导地址
        /// </summary>
        [StringLength(200, ErrorMessage = "链接地址最多输入200个字符")]
        public string AppLink { get; set; }
        /// <summary>
        /// 小程序引导地址
        /// </summary>
        [StringLength(200, ErrorMessage = "链接地址最多输入200个字符")]
        public string MicroAppLink { get; set; }

        /// <summary>
        /// H5引导地址
        /// </summary>
        [StringLength(200, ErrorMessage = "链接地址最多输入200个字符")]
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
