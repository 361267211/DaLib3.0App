/*********************************************************
 * 名    称：AppManageService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/10/12 17:19:12
 * 描    述：开放平台应用管理Grpc服务
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.DependencyInjection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SmartLibrary.Open.Common.Enums;
using SmartLibrary.Open.Common.Utility;
using Mapster;
using SmartLibrary.Open.Common;
using SmartLibrary.Open.Services;
using SmartLibrary.Open.Services.AppManagement;
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Services.Dtos.Application;
using SmartLibrary.Open.Services.Dtos.Infomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartLibrary.Open.AppGrpcService;
using Furion;
using Furion.FriendlyException;
using System.Security.Claims;
using SmartLibrary.Open.EntityFramework.Core.Entitys;

namespace SmartLibrary.Open.Application.GrpcAppServices.Service
{
    /// <summary>
    /// 开放平台应用管理Grpc服务
    /// </summary>
    public class AppManageGrpcService : AppGrpcServiceBase, IAppManageGrpcService, IScoped
    {
        /// <summary>
        /// 应用分支服务
        /// </summary>
        private IAppBranchService _appBranchService { get; set; }
        /// <summary>
        /// 应用服务
        /// </summary>
        private IApplicationService _applicationService { get; set; }
        /// <summary>
        /// 应用组件服务
        /// </summary>
        private IAppWidgetService _appWidgetService { get; set; }
        /// <summary>
        /// 应用入口服务
        /// </summary>
        private IAppBranchEntryPointService _appBranchEntryPointService { get; set; }
        /// <summary>
        /// 机构客户服务
        /// </summary>
        private ICustomerService _customerService { get; set; }
        /// <summary>
        /// 应用路由服务
        /// </summary>
        private IAppRouteService _appRouteService { get; set; }
        /// <summary>
        /// 订单服务
        /// </summary>
        private IOrderService _orderService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appBranchService"></param>
        /// <param name="applicationService"></param>
        /// <param name="appWidgetService"></param>
        /// <param name="appBranchEntryPointService"></param>
        /// <param name="appRouteService"></param>
        /// <param name="customerService"></param>
        /// <param name="orderService"></param>
        public AppManageGrpcService(IAppBranchService appBranchService,
                                    IApplicationService applicationService,
                                    IAppWidgetService appWidgetService,
                                    IAppBranchEntryPointService appBranchEntryPointService,
                                    IAppRouteService appRouteService,
                                    ICustomerService customerService,
                                    IOrderService orderService)
        {
            _appBranchService = appBranchService;
            _applicationService = applicationService;
            _appWidgetService = appWidgetService;
            _appBranchEntryPointService = appBranchEntryPointService;
            _appRouteService = appRouteService;
            _customerService = customerService;
            _orderService = orderService;
        }

        /// <summary>
        /// 应用列表检索
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppListReply> GetAppList(Empty request, ServerCallContext context)
        {
            var query = new AppSearchQuery
            {
                CustomerId = context.GetHttpContext().User?.FindFirstValue("OrgCode"),
            };
            var list = await _applicationService.GetAppList(query);

            var result = new AppListReply
            {
                TotalCount = list.TotalCount
            };
            try
            {
                result.AppList.AddRange(list.Items.Select(p => new AppListSingle
                {
                    AppIcon = p.Icon,
                    AppName = p.Name,
                    BackendUrl = p.BackendUrl,
                    BeginDate = p.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    ExpireDate = p.ExpireTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    FrontUrl = p.FrontUrl,
                    ApiHost = p.ApiHost,
                    UpdateTime = p.UpdateTime.HasValue ? p.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    CurrentVersion = p.Version ?? "v3.0.0",
                    Terminal = p.Terminal,
                    AppId = p.AppID,
                    AppType = p.ServiceType,
                    Content = p.Intro,
                    Developer = p.DeveloperName,
                    Price = $"{p.AdvisePrice:N2}",
                    PurchaseType = p.PurcaseType,
                    PurchaseTypeName = p.PurcaseTypeName,
                    SceneType = p.UseScene,
                    Status = p.Status.ToString(),
                    RouteCode = p.RouteCode,
                    ShowStatus = EnumTools.GetName((AppStatusEnum)p.Status)
                }));
                foreach (var app in result.AppList)
                {
                    app.AppWidgetList.AddRange(list.Items.FirstOrDefault(p => p.AppID.ToString() == app.AppId)
                        ?.AppWidgets.Select(p => new AppWidgetSingle
                        {
                            AppId = p.AppId,
                            Id = p.Id.ToString(),
                            AvailableConfig = p.AvailableConfig,
                            MaxTopCount = p.MaxTopCount,
                            Name = p.Name,
                            Target = p.Target,
                            Cover = p.Cover,
                            TopCountInterval = p.TopCountInterval,
                            Width = p.Width,
                            Height = p.Height,
                            CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            UpdateTime = p.UpdateTime.HasValue ? p.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                            SceneType = p.SceneType,
                            WidgetCode = p.Target.Trim('/').Replace('/', '_'),
                        }));
                    app.AppEntranceList.AddRange(list.Items.FirstOrDefault(p => p.AppID.ToString() == app.AppId)
                        ?.AppEntrances.Select(p => new AppEntranceSingle
                        {
                            Id = p.Id.ToString(),
                            Code = p.Code,
                            IsDefault = p.IsDefault,
                            IsSystem = p.IsSystem,
                            Name = p.Name,
                            UseScene = p.UseScene,
                            VisitUrl = p.VisitUrl,
                            BusinessType = p.BusinessType ?? ""
                        }));
                    app.AppAvailibleSortFieldList.AddRange(list.Items.FirstOrDefault(p => p.AppID.ToString() == app.AppId)
                        ?.AppAvailibleSortFields.Select(p => new AppAvailibleSortFieldSingle
                        {
                            Id = p.Id.ToString(),
                            AppId = p.AppId,
                            SortFieldName = p.SortFieldName,
                            SortFieldValue = p.SortFieldValue
                        }));
                    foreach (var entry in app.AppEntranceList)
                    {
                        entry.AppEventList.AddRange(list.Items.FirstOrDefault(p => p.AppID.ToString() == app.AppId)
                            .AppEntrances.FirstOrDefault(p => p.Id.ToString() == entry.Id)?.AppEvents.Select(e => new AppEventSingle
                            {
                                EventCode = e.EventCode,
                                EventName = e.EventName,
                                EventType = e.EventType
                            }));
                    }
                }
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 获取机构详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<OrgInfoReply> GetOrgInfo(Empty request, ServerCallContext context)
        {
            var orgCode = App.User.FindFirst(x => x.Type == "OrgCode")?.Value;
            var detail = await _customerService.GetByCode(orgCode);
            var result = new OrgInfoReply
            {
                FileUrl = detail.FileUrl,
                LoginUrl = detail.LoginUrl,
                ManageUrl = detail.ManageUrl,
                MgrLoginUrl = detail.MgrLoginUrl,
                OrgCode = detail.Owner,
                OrgName = detail.Name,
                PortalUrl = detail.PortalUrl,
                LogoUrl = detail.LogoUrl,
                SimpleLogoUrl = detail.SimpleLogoUrl
            };
            return result;
        }

        /// <summary>
        /// 按类型获取字典
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<DictionaryReply> GetDictionaryByType(DictionaryRequest request, ServerCallContext context)
        {
            var list = await _applicationService.GetDictionaryByType(request.DicType);

            var result = new DictionaryReply();
            result.DictionaryList.AddRange(list.Select(p => new DictionarySingle
            {
                Id = p.Key,
                Name = p.Key,
                Value = p.Value
            }));
            return result;
        }

        /// <summary>
        /// 获取应用日志
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppLogReply> GetAppLog(AppLogRequest request, ServerCallContext context)
        {
            var query = new AppLogQuery
            {
                CustomerId = context.GetHttpContext().User.Claims.FirstOrDefault(x => x.Type == "OrgCode").Value,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                InfoType = request.InfoType,
                SortField = "PublishDate"
            };
            var list = await _applicationService.GetAppLog(query);

            var result = new AppLogReply() { TotalCount = list.TotalCount };
            result.AppLogList.AddRange(list.Items.Select(p => new AppLogSingle
            {
                Id = p.InfoID.ToString(),
                Title = p.Title,
                ReleaseTime = p.PublishDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Content = p.Content,
                AppId = p.AppID.ToString(),
                Version = p.Version
            }));

            return result;
        }

        /// <summary>
        /// 获取应用日志详情
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppLogDetailReply> GetAppLogDetail(AppLogDetailRequest request, ServerCallContext context)
        {
            var detail = await _applicationService.GetAppLogDetail(request.Id);

            var result = new AppLogDetailReply
            {
                Id = detail.InfoID.ToString(),
                Title = detail.Title,
                UpdateTime = detail.PublishDate.ToString("yyyy-MM-dd HH:mm:ss"),
                AppIcon = detail.AppIcon,
                AppTitle = detail.AppName,
                Content = detail.Content,
                Version = detail.Version
            };
            return result;
        }

        /// <summary>
        /// 付费应用推荐
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<PayAppReply> GetPayAppList(PayAppRequest request, ServerCallContext context)
        {
            var payAppQueryFilter = new PayAppTableQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                ServiceType = request.AppType,
                Owner = context.GetHttpContext().User?.FindFirstValue("OrgCode")
            };
            var pagedList = await _applicationService.GetPayAppList(payAppQueryFilter);
            var payAppResult = new PayAppReply { TotalCount = pagedList.TotalCount };
            foreach (var item in pagedList.Items)
            {
                var payAppSingle = item.Adapt<PayAppSingle>();
                payAppResult.PayAppList.Add(payAppSingle);
            }
            return payAppResult;
        }

        /// <summary>
        /// 应用订单列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<OrderListReply> GetOrderList(OrderListRequest request, ServerCallContext context)
        {
            var orderQueryFilter = new OrderTableQuery
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Status = request.Status,
                Keyword = request.SearchKey,
                CustomerOwner = context.GetHttpContext().User?.FindFirstValue("OrgCode")
            };
            var pagedList = await _orderService.QueryTableData(orderQueryFilter);
            var orderResult = new OrderListReply
            {
                TotalCount = pagedList.TotalCount
            };
            foreach (var item in pagedList.Items)
            {
                orderResult.OrderList.Add(new OrderListSingle
                {
                    Id = item.ID.ToString(),
                    AppName = item.AppName ?? "",
                    AuthType = item.AuthType.ToString(),
                    OpenType = item.OpenType.ToString(),
                    Phone = item.Phone ?? "",
                    Remark = item.Remark ?? "",
                    Status = item.Status.ToString(),
                    Contacts = item.ContactMan ?? "",
                    Developer = item.DevName ?? "",
                    ShowAuthType = item.AuthTypeDisp ?? "",
                    ShowOpenType = item.OpenTypeDisp ?? "",
                    ShowStatus = item.StatusDisp ?? "",
                    CommitDate = item.CommitDate.ToString("yyyy-MM-dd"),
                    ExpireDate = item.ExpireDate.ToString("yyyy-MM-dd")
                });
            }

            return orderResult;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<CancelOrderReply> CancelOder(CancelOrderRequest request, ServerCallContext context)
        {
            var orderIds = new List<Guid>() { new Guid(request.Id) };
            var cancelResult = new CancelOrderReply
            {
                ErrorMsg = "",
                IsSuccess = true,
            };
            try
            {
                var result = await _orderService.Cancel(orderIds);
                cancelResult.IsSuccess = result;
            }
            catch (Exception ex)
            {
                cancelResult.IsSuccess = false;
                cancelResult.ErrorMsg = ex.Message;
            }
            return cancelResult;
        }

        /// <summary>
        /// 操作类型 1=续订，2=延期，3=免费试用，4=预约采购，5=启用，6=停用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task<AppActionReply> AppAction(AppActionReuqest request, ServerCallContext context)
        {
            var appActionResult = new AppActionReply { ErrorMsg = "", IsSuccess = true, };

            var owner = App.User?.FindFirstValue("OrgCode");
            if (string.IsNullOrWhiteSpace(owner))
            {
                appActionResult.IsSuccess = false;
                appActionResult.ErrorMsg = "未找到客户";
                return appActionResult;
            }
            var customer = await _customerService.GetByCode(owner);
            if (customer is null)
            {
                appActionResult.IsSuccess = false;
                appActionResult.ErrorMsg = "未找到客户";
                return appActionResult;
            }
            var orderDto = new OrderDto
            {
                Id = Guid.Empty,
                AppID = new Guid(request.AppId),
                CustomerID = customer.ID,
                AuthType = (int)EnumOrderAuthType.正式授权,
                OpenType = (int)EnumOrderOpenType.首次授权,
                Way = (int)EnumOrderWay.客户申请,
                BeginDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddMonths(request.TimeNum),
                ContactMan = request.Contacts,
                ContactPhone = request.Phone,
            };
            var appInfo = await _applicationService.GetById(orderDto.AppID);
            if (appInfo is null)
            {
                appActionResult.IsSuccess = false;
                appActionResult.ErrorMsg = "未找到应用";
                return appActionResult;
            }
            orderDto.AppName = appInfo.Name;
            switch (request.ActionType)
            {
                case 1: //续订
                    orderDto.OpenType = (int)EnumOrderOpenType.续费授权;
                    break;
                case 2: //延期
                    orderDto.OpenType = (int)EnumOrderOpenType.试用延期;
                    break;
                case 3: // 免费试用
                    orderDto.AuthType = (int)EnumOrderAuthType.试用授权;
                    break;
                case 5: //启用
                case 6: //停用
                    return appActionResult;
            }
            try
            {
                if (await _orderService.IsExist(orderDto))
                {
                    appActionResult.ErrorMsg = "已存在类似订单，请勿重复提交";
                    return appActionResult;
                }
                var result = await _orderService.Create(orderDto);
                appActionResult.IsSuccess = true;
            }
            catch (Exception ex)
            {
                appActionResult.IsSuccess = false;
                appActionResult.ErrorMsg = ex.Message;
            }

            return appActionResult;
        }
    }
}
