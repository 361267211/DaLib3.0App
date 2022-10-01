/*********************************************************
* 名    称：CapActionEventAttribute.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：配合CapActionEventFilter，用于投递简单的行为事件
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using System;

namespace SmartLibrary.User.Application.Filter
{
    /// <summary>
    /// 事件发布描述
    /// </summary>
    [SuppressSniffer, AttributeUsage(AttributeTargets.Method)]
    public class CapActionEventAttribute : Attribute
    {
        /// <summary>
        /// 事件编码
        /// </summary>
        public string EventCode { get; set; } = "";
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; } = "";
        /// <summary>
        /// 是否启用事务
        /// </summary>
        public bool UnitOfWork { get; set; } = false;
        /// <summary>
        /// 确保事务可用
        /// <para>此方法为了解决静态类方式操作数据库的问题</para>
        /// </summary>
        public bool EnsureTransaction { get; set; } = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CapActionEventAttribute(string eventCode, string eventName)
        {
            EventCode = eventCode;
            EventName = eventName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CapActionEventAttribute(string eventCode, string eventName, bool unitOfWork)
        {
            EventCode = eventCode;
            EventName = EventName;
            UnitOfWork = unitOfWork;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CapActionEventAttribute(string eventCode, string eventName, bool unitOfWork, bool ensureTransaction)
        {
            EventCode = eventCode;
            EventName = eventName;
            UnitOfWork = unitOfWork;
            EnsureTransaction = ensureTransaction;
        }
    }
}
