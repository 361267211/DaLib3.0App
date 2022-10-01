using DotNetCore.CAP;
using Furion.DependencyInjection;
using SmartLibrary.AppCenter.Application.Services.Application;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services
{
    /// <summary>
    /// 所有的应用事件有这里进行内容分发
    /// </summary>
    public class AppEventCapDistributeService : ICapSubscribe, IScoped
    {
        private readonly ICapPublisher _capPublisher;
        private readonly IApplicationService _applicationService;
        public class AppEventSimpleMsgModel
        {
            public string Id { get; set; }
            public string ParentId { get; set; }
            public string ObjectId { get; set; }
            public string ObjectName { get; set; }
            public string ParentObjectId { get; set; }
            public string ParentObjectName { get; set; }
        }

        private const string AppEventCapName = nameof(AppEventCapName);
        private const string AppEventFire2SubscriberName = nameof(AppEventFire2SubscriberName);
        private const string AppCodeHeaderName = "AppCode";
        private const string EventCodeHeaderName = "EventCode";
        private const string OwnerHeaderName = nameof(OwnerHeaderName);

        public AppEventCapDistributeService(ICapPublisher capPublisher, IApplicationService applicationService)
        {
            _capPublisher = capPublisher;
            _applicationService = applicationService;
        }
        [CapSubscribe(AppEventCapName)]
        public async Task SubscribeSubAppEventAsync(AppEventSimpleMsgModel contentObj, [FromCap] CapHeader header)
        {
            if (!header.TryGetValue(AppCodeHeaderName, out var appCode)
                ||
                !header.TryGetValue(EventCodeHeaderName, out var eventCode)
                ||
                !header.TryGetValue(OwnerHeaderName, out var owner)
                ) return;
            var allApp = await this._applicationService.GetAllApp();
            var temp = allApp?.Find(x => x.RouteCode == appCode);
            if (temp == null) return;
            var result = new
            {
                AppId = temp.AppId ?? String.Empty,
                Name = temp.AppName ?? String.Empty,
                EventId = eventCode,
                IconPath = temp.AppIcon ?? String.Empty,
                ParentId = contentObj.ParentId ?? String.Empty,
                EventName = temp.AppEntranceList?.SelectMany(x => x.AppEventList?.Select(y => y)).FirstOrDefault(x => x.EventCode == eventCode)?.EventName ?? String.Empty,
                VisitUrl = temp.BackendUrl ?? String.Empty,
                contentObj.ParentObjectId,
                contentObj.ParentObjectName,
                contentObj.ObjectId,
                contentObj.ObjectName
            };
            await this._capPublisher.PublishAsync(AppEventFire2SubscriberName, result, header
                .Where(x => new[] { AppCodeHeaderName, EventCodeHeaderName, OwnerHeaderName }.Contains(x.Key))
                .ToDictionary(x => x.Key, x => x.Value));
        }
    }
}
