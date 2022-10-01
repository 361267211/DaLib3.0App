/*********************************************************
* 名    称：UserServiceSubscribeEvent.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210831
* 描    述：Cap事件描述类，该类用于定义当前应用所有需要消费的事件
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Core.Cap;
using System.ComponentModel;

namespace SmartLibrary.ScoreCenter.Application.Dtos.Cap
{
    //针对被订阅服务定义监听事件名称及接收参数类型
    //约定为被订阅服务名称+SubscribeEvent
    //这里的内容和PublishEvent内容类似，但是由于事件订阅服务和事件发布服务通常是两个独立的服务，
    //可能由不同机构开发，所以不能共用类型，事件订阅方参考发布方提供的文档实现自己的订阅类型
    public class SubscribeEvent : SmartCapSubscribeEventBase
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        [SmartCapEventBind(typeof(ActionEventMsg))]
        [Description("监听通用行为事件")]
        public const string ActionEvent = "VipSmart.Common.ActionEvent";//公有云.服务名.事件名(公有云即维普智图环境VipSmart)
        /// <summary>
        /// 消息接收类型
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
