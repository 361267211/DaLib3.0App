/*********************************************************
* 名    称：ScoreObtainTaskDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分获取任务
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 积分任务获取
    /// </summary>
    public class ScoreObtainTaskDto
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 事件触发应用
        /// </summary>
        public string AppCode { get; set; }
        /// <summary>
        /// 事件编码
        /// </summary>
        public string EventCode { get; set; }

        /// <summary>
        /// 获取积分
        /// </summary>
        public int ObtainScore { get; set; }
        /// <summary>
        /// 宣传图地址
        /// </summary>
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// pc引导地址
        /// </summary>
        public string PcLink { get; set; }
        /// <summary>
        /// App引导地址
        /// </summary>
        public string AppLink { get; set; }
        /// <summary>
        /// 小程序引导地址
        /// </summary>
        public string MicroAppLink { get; set; }

        /// <summary>
        /// H5引导地址
        /// </summary>
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
