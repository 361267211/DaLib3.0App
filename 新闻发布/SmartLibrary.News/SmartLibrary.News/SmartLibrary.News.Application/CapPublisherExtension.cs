using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.CAP;
using SmartLibrary.News.Common;

namespace SmartLibrary.Search.Service
{
    internal static class CapPublisherExtension
    {
        private const string AppCode = "news";
        public const string AppEventCapName = nameof(AppEventCapName);
        private const string EventCodeHeaderName = "EventCode";
        private const string OwnerHeaderName = nameof(OwnerHeaderName);
        /// <summary>
        /// 发布AppEvent事件
        /// </summary>
        /// <param name="capPublisher"></param>
        /// <param name="owner">发送事件的机构</param>
        /// <param name="eventCode">事件代码，对应open.public.AppEvent.EventCode</param>
        /// <param name="contentObj">事件内容</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Task PublishAppEventAsync(this ICapPublisher capPublisher, string owner, string eventCode, AppEventSimpleMsgModel contentObj)
        {
            if (capPublisher == null) throw new ArgumentNullException(nameof(capPublisher));
            return capPublisher.PublishAsync(AppEventCapName, contentObj, new Dictionary<string, string> { { nameof(AppCode), AppCode }, { EventCodeHeaderName, eventCode }, { OwnerHeaderName, owner } });
        }
    }

    public class AppEventSimpleMsgModel
    {

        public string Id { get; set; }
        public string ParentId { get; set; }
        /// <summary>
        /// 当前对象的id
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// 当前对象的名称
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// 父对象id
        /// </summary>
        public string ParentObjectId { get; set; }
        /// <summary>
        /// 父对象名
        /// </summary>
        public string ParentObjectName { get; set; }

    }
}