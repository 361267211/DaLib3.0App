using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Filter
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
