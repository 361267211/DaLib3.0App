/*********************************************************
* 名    称：MedalObtainTaskDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：勋章任务
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.ScoreCenter.Application.Dtos
{
    /// <summary>
    /// 勋章任务
    /// </summary>
    public class MedalObtainTaskDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 勋章名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 勋章图片
        /// </summary>
        public string IntroPicUrl { get; set; }
        /// <summary>
        /// 应用编码
        /// </summary>
        public string AppCode { get; set; }
        /// <summary>
        /// 事件编码
        /// </summary>
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
        /// 获取方式 0:按次数 1:按天
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
