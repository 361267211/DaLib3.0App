/*********************************************************
* 名    称：UserServicePublishEvent.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210831
* 描    述：Cap事件描述类，该类用于定义当前应用所有需要投递的事件
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Core.Cap;
using System.ComponentModel;

namespace SmartLibrary.Identity.Application.Dtos.Cap
{
    //发布事件约定类名为自身服务名称+PublishEvent
    public class PublishEvent : SmartCapPublishEventBase
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        [SmartCapEventBind(typeof(ActionEventMsg))]
        [Description("发布通用行为事件")]
        public const string ActionEvent = "VipSmart.Common.ActionEvent";//公有云.服务名.事件名，(公有云即维普智图:VipSmart)
        /// <summary>
        /// 事件对应消息
        /// </summary>
        public class ActionEventMsg : SmartCapEventMsgBase
        {
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
        }
    }


}
