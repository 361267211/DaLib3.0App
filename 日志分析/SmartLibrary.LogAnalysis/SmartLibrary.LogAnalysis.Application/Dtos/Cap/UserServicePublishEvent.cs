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

namespace SmartLibrary.LogAnalysis.Application.Dtos.Cap
{
    //发布事件约定类名为自身服务名称+PublishEvent
    public class UserServicePublishEvent : SmartCapPublishEventBase
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        [SmartCapEventBind(typeof(NewPersonMsg))]
        [Description("用户服务发布新增员工消息")]
        public const string NewPerson = "VipSmart.UserService.NewPerson";//公有云.服务名.事件名
        /// <summary>
        /// 事件对应消息
        /// </summary>
        public class NewPersonMsg : SmartCapEventMsgBase
        {
            /// <summary>
            /// 员工Id
            /// </summary>
            [Description("员工Id")]
            public int PersonId { get; set; }
        }
    }


}
