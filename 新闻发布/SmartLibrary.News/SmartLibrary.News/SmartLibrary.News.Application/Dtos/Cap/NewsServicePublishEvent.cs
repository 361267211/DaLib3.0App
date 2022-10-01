using SmartLibrary.Core.Cap;
using System.ComponentModel;

namespace SmartLibrary.News.Application.Dtos.Cap
{
    /// <summary>
    /// 名    称：NewsColumnServicePublishEvent
    /// 作    者：张泽军
    /// 创建时间：2021/9/7 19:36:45
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsServicePublishEvent: SmartCapSubscribeEventBase
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        [SmartCapEventBind(typeof(NewNewsColumnMsg))]
        [Description("新闻服务发布新增新闻栏目消息")]
        public const string NewNewsColumn = "VipSmart.UserService.NewPerson";//公有云.服务名.事件名
        /// <summary>
        /// 事件对应消息
        /// </summary>
        public class NewNewsColumnMsg : SmartCapEventMsgBase
        {
            /// <summary>
            /// 员工Id
            /// </summary>
            [Description("新闻栏目Id")]
            public int ColumnID { get; set; }
        }
    }
}
