/*********************************************************
* 名    称：ConsumeScoreInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分消费实践
* 更新历史：
*
* *******************************************************/using System;
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 积分消费事件
    /// </summary>
    public class ConsumeScoreInput
    {
        /// <summary>
        /// 租户
        /// </summary>
        [Description("租户")]
        public string Tenant { get; set; }
        /// <summary>
        /// 应用编码
        /// </summary>
        [Description("应用编码")]
        public string AppCode { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        [Description("应用名称")]
        public string AppName { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        [Description("用户标识")]
        public string UserKey { get; set; }
        /// <summary>
        /// 事件编码
        /// </summary>
        [Description("事件编码")]
        public string EventCode { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        [Description("事件名称")]
        public string EventName { get; set; }
        /// <summary>
        /// 事件ID
        /// </summary>
        [Description("消费事件ID")]
        public Guid EventID { get; set; }
    }
}
