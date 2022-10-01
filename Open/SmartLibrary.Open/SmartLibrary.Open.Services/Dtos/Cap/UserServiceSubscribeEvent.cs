/*********************************************************
* 名    称：UserServiceSubscribeEvent.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210831
* 描    述：Cap事件描述类，该类用于定义当前应用所有需要消费的事件
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Core.Cap;
using System.ComponentModel;

namespace SmartLibrary.Open.Services.Dtos.Cap
{
    //针对被订阅服务定义监听事件名称及接收参数类型
    //约定为被订阅服务名称+SubscribeEvent
    //这里的内容和PublishEvent内容类似，但是由于事件订阅服务和事件发布服务通常是两个独立的服务，
    //可能由不同机构开发，所以不能共用类型，事件订阅方参考发布方提供的文档实现自己的订阅类型
    public class UserServiceSubscribeEvent : SmartCapSubscribeEventBase
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        [SmartCapEventBind(typeof(NewPersonMsg))]
        [Description("用户服务发布新增员工消息")]
        public const string NewPerson = "VipSmart.UserService.NewPerson";//公有云.服务名.事件名
        /// <summary>
        /// 消息接收类型
        /// </summary>
        public class NewPersonMsg : SmartCapEventMsgBase
        {
            [Description("员工Id")]
            public int PersonId { get; set; }
        }
    }
}
