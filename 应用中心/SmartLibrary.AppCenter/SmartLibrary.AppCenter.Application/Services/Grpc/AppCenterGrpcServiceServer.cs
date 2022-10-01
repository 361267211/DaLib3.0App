using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter.Application.Services.Application;
using SmartLibrary.AppCenter.Application.Services.Common;
using SmartLibrary.AppCenter.Common.Enums;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys;
using SmartLibrary.Open;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static SmartLibrary.Open.AppGrpcService;
using static SmartLibrary.User.RpcService.UserGrpcService;

namespace SmartLibrary.AppCenter.Application.Services.Grpc
{
    /// <summary>
    /// 应用中心GRPC服务
    /// </summary>
    public class AppCenterGrpcServiceServer : AppCenterGrpcService.AppCenterGrpcServiceBase, IScoped
    {
        private readonly IGrpcClientResolver _GrpcClientResolver;
        private readonly IApplicationService _ApplicationService;
        private readonly IRepository<AppManager> _RepositoryAppManager;
        private readonly IRepository<AppUser> _RepositoryAppUser;
        private readonly IRepository<UserRole> _RepositoryUserRole;
        private readonly IRepository<Navigation> _RepositoryNavigation;
        private readonly IRepository<AppColumnInfo> _RepositoryAppColumnInfo;
        private readonly IGeneralService _GeneralService;
        private readonly IRepository<ManagerRole> _RepositoryManagerRole;

        public AppCenterGrpcServiceServer(IApplicationService applicationService,
                                          IRepository<AppManager> repositoryAppManager,
                                          IRepository<AppUser> repositoryAppUser,
                                          IGrpcClientResolver grpcClientResolver,
                                          IRepository<UserRole> repositoryUserRole,
                                          IRepository<Navigation> repositoryNavigation,
                                          IRepository<AppColumnInfo> repositoryAppColumnInfo,
                                          IGeneralService generalService,
                                          IRepository<ManagerRole> repositoryManagerRole)
        {
            _ApplicationService = applicationService;
            _RepositoryAppManager = repositoryAppManager;
            _RepositoryAppUser = repositoryAppUser;
            _RepositoryUserRole = repositoryUserRole;
            _GrpcClientResolver = grpcClientResolver;
            _RepositoryNavigation = repositoryNavigation;
            _RepositoryAppColumnInfo = repositoryAppColumnInfo;
            _GeneralService = generalService;
            _RepositoryManagerRole = repositoryManagerRole;
        }

        /// <summary>
        /// 获取管理权限
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserAppAuthReply> GetUserAppAuthList(UserAppAuthRequest request, ServerCallContext context)
        {
            var result = new UserAppAuthReply();

            var allApps = await _ApplicationService.GetAllApp();
            var userRole = await _RepositoryUserRole.FirstOrDefaultAsync(c => c.UserKey == request.UserId && !c.DeleteFlag);
            if (userRole != null)
            {
                if (userRole.IsSuper)
                {
                    //超级管理员，返回全部
                    result.UserAppAuthList.AddRange(allApps.Select(c => new UserAppAuthSingle
                    {
                        AppId = c.AppId,
                        AppName = c.AppNewName,
                        Icon = c.AppIcon
                    }));
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(userRole.ManagerRoleIds))
                    {
                        var managerRoleIdArray = userRole.ManagerRoleIds.Split(',');
                        var appManagers = await _RepositoryAppManager.Where(c => managerRoleIdArray.Contains(c.ManageRoleId) && !c.DeleteFlag).ToListAsync();
                        if (appManagers != null && appManagers.Any())
                        {
                            foreach (var item in appManagers)
                            {
                                var tempApp = allApps.FirstOrDefault(c => c.AppId == item.AppId);
                                if (tempApp != null)
                                {
                                    result.UserAppAuthList.Add(new UserAppAuthSingle
                                    {
                                        AppId = tempApp.AppId,
                                        AppName = tempApp.AppNewName,
                                        Icon = tempApp.AppIcon
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 根据用户类型和用户分组获取有权限的应用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserAppAuthReply> GetUserAppAuthListByType(UserAppAuthByTypeRequest request, ServerCallContext context)
        {
            var result = new UserAppAuthReply();

            var allApps = await _ApplicationService.GetAllApp();
            //用户类型取
            var appUsersByType = await _RepositoryAppUser.DetachedEntities.Where(c => c.UserSetId == request.UserTypeId
                                                          && c.UserSetType == 1 && !c.DeleteFlag)
                                                    .ToListAsync();
            if (appUsersByType != null && appUsersByType.Any())
            {
                foreach (var item in appUsersByType)
                {
                    var tempApp = allApps.FirstOrDefault(c => c.AppId == item.AppId);
                    if (tempApp != null)
                    {
                        result.UserAppAuthList.Add(new UserAppAuthSingle
                        {
                            AppId = tempApp.AppId,
                            AppName = tempApp.AppNewName,
                            Icon = tempApp.AppIcon
                        });
                    }
                }
            }

            //用户分组取
            if (request.UserGroupIds != null && request.UserGroupIds.Any())
            {
                var appUsersByGroup = await _RepositoryAppUser.Where(c => request.UserGroupIds.Contains(c.UserSetId)
                                                             && c.UserSetType == 2 && !c.DeleteFlag).ToListAsync();
                if (appUsersByGroup != null && appUsersByGroup.Any())
                {
                    foreach (var item in appUsersByGroup)
                    {
                        var tempApp = allApps.FirstOrDefault(c => c.AppId == item.AppId);
                        if (tempApp != null)
                        {
                            result.UserAppAuthList.Add(new UserAppAuthSingle
                            {
                                AppId = tempApp.AppId,
                                AppName = tempApp.AppNewName,
                                Icon = tempApp.AppIcon
                            });
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取指定应用授权给哪些馆员
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppUserAuthReply> GetAppUserAuthList(AppUserAuthRequest request, ServerCallContext context)
        {
            var result = new AppUserAuthReply();

            Dictionary<string, int> dicUser = new();

            //先查找应用绑定到哪些角色
            var appManagers = await _RepositoryAppManager.DetachedEntities.Where(c => c.AppId == request.AppId && !c.DeleteFlag).ToListAsync();
            if (appManagers != null && appManagers.Any())
            {
                //再找出绑定了这些角色的管理员
                foreach (var item in appManagers)
                {
                    var userRoles = await _RepositoryUserRole.DetachedEntities.Where(c => c.ManagerRoleIds.Contains(item.ManageRoleId)
                                                         && !c.DeleteFlag).ToListAsync();
                    if (userRoles != null && userRoles.Any())
                    {
                        foreach (var one in userRoles)
                        {
                            if (dicUser.ContainsKey(one.UserKey) && dicUser[one.UserKey] > item.ManagerType)
                            {
                                dicUser[one.UserKey] = item.ManagerType;
                            }
                            else
                            {
                                dicUser.Add(one.UserKey, item.ManagerType);
                            }
                        }
                    }
                }
            }

            //查找超级管理员
            var superUsers = await _RepositoryUserRole.DetachedEntities.Where(c => c.IsSuper && !c.DeleteFlag).ToListAsync();
            if (superUsers != null && superUsers.Any())
            {
                foreach (var item in superUsers)
                {
                    if (dicUser.ContainsKey(item.UserKey))
                    {
                        dicUser[item.UserKey] = 1;
                    }
                    else
                    {
                        dicUser.Add(item.UserKey, 1);
                    }
                }
            }
            //组装返回结果
            foreach (var item in dicUser)
            {
                result.AppUserAuthList.Add(new AppUserAuthSingle
                {
                    UserId = item.Key,
                    PermissionType = item.Value
                });
            }

            return result;
        }

        /// <summary>
        /// 获取应用地址
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppBaseUriReply> GetAppBaseUri(AppBaseUriRequest request, ServerCallContext context)
        {
            var result = new AppBaseUriReply();

            var allApps = await _ApplicationService.GetAllApp();

            var currentApp = allApps.FirstOrDefault(c => c.RouteCode == request.AppRouteCode);
            if (currentApp != null)
            {
                result.FrontUrl = currentApp.FrontUrl.Split('#')[0].TrimEnd('/');
                result.BackUrl = currentApp.BackendUrl.Split('#')[0].TrimEnd('/');
            }

            return result;
        }

        /// <summary>
        /// 批量获取应用地址
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppBaseUriBatchReply> GetAppBaseUriBatch(AppBaseUriBatchRequest request, ServerCallContext context)
        {
            var result = new AppBaseUriBatchReply();

            var allApps = await _ApplicationService.GetAllApp();

            foreach (var item in request.AppRouteCode)
            {
                var temp = allApps.FirstOrDefault(c => c.RouteCode == item);

                result.AppBaseUriBatchReplys.Add(new AppBaseUriBatchReplySingle
                {
                    RouteCode = item,
                    FrontUrl = temp?.FrontUrl.Split('#')[0].TrimEnd('/'),
                    BackUrl = temp?.BackendUrl.Split('#')[0].TrimEnd('/')
                });
            }

            return result;
        }

        /// <summary>
        /// 根据userkey获取指定应用的权限类型,后台
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserAppPermissionTypeReply> GetUserAppPermissionType(UserAppPermissionTypeRequest request, ServerCallContext context)
        {
            var result = new UserAppPermissionTypeReply { PermissionType = 0 };

            //获取userkey
            var userKey = context.GetHttpContext().User?.FindFirstValue("UserKey");
            if (string.IsNullOrWhiteSpace(userKey) || string.IsNullOrWhiteSpace(request.AppId))
                return result;

            //获取管理员分配的角色信息
            var userRole = await _RepositoryUserRole.DetachedEntities.FirstOrDefaultAsync(c => c.UserKey == userKey && !c.DeleteFlag);
            if (userRole != null)
            {
                //是超级管理员
                if (userRole.IsSuper)
                {
                    result.PermissionType = 1;
                    return result;
                }
                else if (!string.IsNullOrWhiteSpace(userRole.ManagerRoleIds))
                {
                    var roleArray = userRole.ManagerRoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    //获取角色绑定的应用
                    var appManagers = await _RepositoryAppManager.DetachedEntities.Where(c => roleArray.Contains(c.ManageRoleId) && !c.DeleteFlag).ToListAsync();
                    if (appManagers != null && appManagers.Any())
                    {
                        var allApps = await _ApplicationService.GetAllApp();
                        var appId = allApps.FirstOrDefault(c => c.RouteCode == request.AppId || c.AppId == request.AppId);
                        if (appId != null)
                        {
                            //获取传入应用角色信息
                            var currentAppManagers = appManagers.Where(c => c.AppId == appId.AppId).ToList();
                            if (currentAppManagers != null && currentAppManagers.Any())
                            {
                                //如果多个角色都有此应用，则取授权类型范围最大的返回
                                result.PermissionType = currentAppManagers.Min(c => c.ManagerType);
                            }
                        }
                    }
                }
            }


            return result;
        }

        /// <summary>
        /// 判断用户对指定应用是否有使用权限(前台)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserAppPermissionReply> GetUserApppermission(UserAppPermissionTypeRequest request, ServerCallContext context)
        {
            var result = new UserAppPermissionReply { IsHasPermission = 0 };

            //获取userkey
            var userKey = context.GetHttpContext().User?.FindFirstValue("UserKey");
            if (string.IsNullOrWhiteSpace(userKey) || string.IsNullOrWhiteSpace(request.AppId))
                return result;
            if (!Guid.TryParse(request.AppId, out var guid))
            {
                var allApps = await _ApplicationService.GetAllApp();
                var current = allApps.FirstOrDefault(c => c.RouteCode == request.AppId);
                if (current != null)
                {
                    request.AppId = current.AppId;
                }
            }

            //获取当前用户的分组和类型
            var client = _GrpcClientResolver.EnsureClient<UserGrpcServiceClient>();
            var userInfo = await client.GetUserByKeyAsync(new StringValue() { Value = userKey });

            if (userInfo != null)
            {
                //用户类型判断
                bool isPermission = false;
                var userType = userInfo.Type;
                isPermission = await _RepositoryAppUser.DetachedEntities.AnyAsync(c => c.UserSetId == userType && c.AppId == request.AppId
                                                                && c.UserSetType == 1 && !c.DeleteFlag);
                if (isPermission)
                {
                    result.IsHasPermission = 1;
                    return result;
                }

                //用户组判断
                var userGroups = userInfo.GroupIds.ToList();
                isPermission = await _RepositoryAppUser.DetachedEntities.AnyAsync(c => userGroups.Contains(c.UserSetId) && c.AppId == request.AppId
                                                                 && c.UserSetType == 2 && !c.DeleteFlag);
                result.IsHasPermission = isPermission ? 1 : 0;
            }

            return result;
        }

        /// <summary>
        /// 通过事件类型查询当前机构的使用的应用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GetAppByEventTypeReply> GetAppByEventType(GetAppByEventTypeRequest request, ServerCallContext context)
        {
            var result = new GetAppByEventTypeReply();

            var allApps = await _ApplicationService.GetAllApp();

            var filterApps = allApps.Where(c => c.AppEntranceList != null
                                             && c.AppEntranceList.Any(x => x.AppEventList != null
                                                                        && x.AppEventList.Any(z => request.EventType == 0 || z.EventType.Split(';').Contains(request.EventType.ToString())))).ToList();

            if (filterApps != null && filterApps.Any())
            {
                filterApps.ForEach(c =>
                {
                    result.GetAppByEventTypeList.Add(new GetAppByEventTypeSingle
                    {
                        AppCode = c.RouteCode,
                        AppName = c.AppNewName
                    });
                });
            }

            return result;
        }

        /// <summary>
        /// 通过类型查询当前机构某个应用下的事件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GetAppEventByCodeReply> GetAppEventByCode(GetAppEventByCodeRequest request, ServerCallContext context)
        {
            var result = new GetAppEventByCodeReply();

            //获取所有应用
            var allApps = await _ApplicationService.GetAllApp();

            //获取指定应用
            var currentApp = allApps.First(c => c.RouteCode == request.AppCode);

            if (currentApp != null && currentApp.AppEntranceList != null && currentApp.AppEntranceList.Any())
            {
                foreach (var item in currentApp.AppEntranceList)
                {
                    if (item.AppEventList != null && item.AppEventList.Any())
                    {
                        foreach (var item1 in item.AppEventList)
                        {
                            if (request.EventType == 0 || item1.EventType.Split(';').Contains(request.EventType.ToString()))
                            {
                                result.GetAppEventByCodeList.Add(new GetAppEventByCodeSingle
                                {
                                    EventCode = item1.EventCode,
                                    EventName = item1.EventName
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 应用栏目操作，新增、修改、删除
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppColumnOperationReply> AppColumnOperation(AppColumnOperationRequest request, ServerCallContext context)
        {
            var result = new AppColumnOperationReply { IsSuccess = false, ErrorMsg = "" };

            switch (request.OperationType)
            {
                case 1: //新增
                    var allApps = await _ApplicationService.GetAllApp();
                    var currentApp = allApps.FirstOrDefault(c => c.RouteCode.ToLower() == request.AppRouteCode.ToLower());
                    if (currentApp != null)
                    {
                        var isSuccess = DateTimeOffset.TryParse(request.CreateTime, out var columnCreateTime);
                        var newInfo = new AppColumnInfo
                        {
                            Id = Guid.NewGuid(),
                            DeleteFlag = false,
                            CreatedTime = DateTimeOffset.Now,
                            UpdatedTime = DateTimeOffset.Now,
                            ColumnName = request.ColumnName,
                            ColumnId = request.ColumnId,
                            ColumnCreateTime = isSuccess ? columnCreateTime : DateTimeOffset.Now,
                            AppRouteCode = request.AppRouteCode,
                            VisitUrl = request.VisitUrl,
                            AppId = currentApp.AppId,
                        };
                        await _RepositoryAppColumnInfo.InsertNowAsync(newInfo);
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.ErrorMsg = "当前应用不存在";
                    }

                    break;

                case 2: //修改
                    var editColumnInfo = await _RepositoryAppColumnInfo.FirstOrDefaultAsync(c => c.AppRouteCode.ToLower() == request.AppRouteCode.ToLower()
                                                                                         && c.ColumnId == request.ColumnId && c.DeleteFlag == false);
                    if (editColumnInfo != null)
                    {
                        editColumnInfo.ColumnName = request.ColumnName;
                        editColumnInfo.VisitUrl = request.VisitUrl;
                        editColumnInfo.UpdatedTime = DateTimeOffset.Now;

                        await _RepositoryAppColumnInfo.UpdateIncludeNowAsync(editColumnInfo, new string[] { nameof(editColumnInfo.ColumnName),
                                                                                                         nameof(editColumnInfo.VisitUrl),
                                                                                                         nameof(editColumnInfo.UpdatedTime)});
                        result.IsSuccess = true;
                    }
                    else
                    {
                        var allAppInfos = await _ApplicationService.GetAllApp();
                        var currentAppInfo = allAppInfos.FirstOrDefault(c => c.RouteCode.ToLower() == request.AppRouteCode.ToLower());
                        var isSuccess = DateTimeOffset.TryParse(request.CreateTime, out var columnCreateTime);
                        if (currentAppInfo != null)
                        {
                            var newInfo = new AppColumnInfo
                            {
                                Id = Guid.NewGuid(),
                                DeleteFlag = false,
                                CreatedTime = DateTimeOffset.Now,
                                UpdatedTime = DateTimeOffset.Now,
                                ColumnName = request.ColumnName,
                                ColumnId = request.ColumnId,
                                ColumnCreateTime = isSuccess ? columnCreateTime : DateTimeOffset.Now,
                                AppRouteCode = request.AppRouteCode,
                                VisitUrl = request.VisitUrl,
                                AppId = currentAppInfo.AppId,
                            };
                            await _RepositoryAppColumnInfo.InsertNowAsync(newInfo);
                            result.IsSuccess = true;
                        }
                        else
                        {
                            result.ErrorMsg = "修改新增-当前应用不存在";
                        }
                    }
                    break;

                case 3: //删除
                    var ids = request.ColumnId.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    var delColumnInfoList = await _RepositoryAppColumnInfo.Where(c => c.AppRouteCode.ToLower() == request.AppRouteCode.ToLower()
                                                        && ids.Contains(c.ColumnId) && c.DeleteFlag == false).ToListAsync();
                    if (delColumnInfoList != null && delColumnInfoList.Any())
                    {
                        foreach (var item in delColumnInfoList)
                        {
                            item.DeleteFlag = true;
                            item.UpdatedTime = DateTimeOffset.Now;
                            await _RepositoryAppColumnInfo.UpdateIncludeNowAsync(item, new string[] { nameof(item.DeleteFlag),
                                                                                                        nameof(item.UpdatedTime) });
                        }
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.ErrorMsg = "当前删除栏目不存在";
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取服务类型列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ServiceTypeReply> GetServiceType(Empty request, ServerCallContext context)
        {
            var result = new ServiceTypeReply();

            var allTypes = await _GeneralService.GetAppType();

            if (allTypes != null && allTypes.Any())
            {
                result.ServiceTypeList.AddRange(allTypes.Select(c => new ServiceTypeSingle
                {
                    Key = c.Id,
                    Value = c.Name
                }));
            }

            return result;
        }

        /// <summary>
        /// 获取应用列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppListReply> GetAppList(AppListRequest request, ServerCallContext context)
        {
            var result = new AppListReply();

            var allApps = await _ApplicationService.GetAllApp();

            if (allApps != null && allApps.Any())
            {
                if (!string.IsNullOrWhiteSpace(request.AppRouteCodes))
                {
                    allApps = allApps.FindAll(c => request.AppRouteCodes.Split(',').Contains(c.RouteCode) || request.AppRouteCodes.Split(',').Contains(c.AppId));
                }
                if (!string.IsNullOrWhiteSpace(request.AppServiceType))
                {
                    allApps = allApps.FindAll(c => c.AppType == request.AppServiceType);
                }
                if (request.TerminalType > 0)
                {
                    allApps = allApps.FindAll(c => c.Terminal.Contains(request.TerminalType.ToString()));
                }
                allApps = allApps.FindAll(c => c.AppWidgetList != null && c.AppWidgetList.Any()
                                            && (c.AppWidgetList.Any(a => a.SceneType == request.SceneType) || request.SceneType == 0));
                if (request.UseScene > 0)
                {
                    allApps = allApps.FindAll(c => c.AppEntranceList.Any(x => x.UseScene == request.UseScene));
                }
                if (request.HasPermission)
                {
                    // 调用用户中心grpc获取用户类型和分组
                    var userKey = context.GetHttpContext().User?.FindFirstValue("UserKey");
                    if (!string.IsNullOrWhiteSpace(userKey))
                    {
                        var client = _GrpcClientResolver.EnsureClient<UserGrpcServiceClient>();
                        var userInfo = await client.GetUserByKeyAsync(new StringValue() { Value = userKey });
                        if (userInfo != null)
                        {
                            var appUserList = new List<AppUser>();
                            if (!string.IsNullOrWhiteSpace(userInfo.Type))
                            {
                                appUserList.AddRange(await _RepositoryAppUser.Where(c => c.DeleteFlag == false && c.UserSetId == userInfo.Type && c.UserSetType == 1).ToListAsync());
                            }
                            if (userInfo.GroupIds != null && userInfo.GroupIds.Any())
                            {
                                appUserList.AddRange(await _RepositoryAppUser.Where(c => c.DeleteFlag == false && userInfo.GroupIds.Contains(c.UserSetId) && c.UserSetType == 2).ToListAsync());
                            }
                            if (appUserList.Any())
                            {
                                var appIds = appUserList.Select(c => c.AppId);
                                allApps = allApps.FindAll(c => appIds.Contains(c.AppId));
                            }
                        }
                    }
                }

                result.AppList.AddRange(allApps.Select(c => new AppListSingle
                {
                    AppId = c.AppId,
                    Name = c.AppNewName,
                    Icon = c.AppIcon,
                    RouteCode = c.RouteCode,
                    FrontUrl = c.FrontUrl,
                    BackUrl = c.BackendUrl,
                    ServiceType = c.AppType
                }));
            }

            return result;
        }

        /// <summary>
        /// 获取应用组件列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppWidgetListReply> GetAppWidgetList(AppWidgetListRequest request, ServerCallContext context)
        {
            var result = new AppWidgetListReply();

            var allApps = await _ApplicationService.GetAllApp();

            var currentApp = allApps.FirstOrDefault(c => c.AppId == request.AppId || c.RouteCode == request.AppId);

            if (currentApp != null && currentApp.AppWidgetList != null && currentApp.AppWidgetList.Any())
            {
                var appWidgetList = currentApp.AppWidgetList.FindAll(c => request.SceneType == 0 || c.SceneType == request.SceneType);

                if (appWidgetList != null)
                {
                    foreach (var item in appWidgetList)
                    {
                        var single = new AppWidgetSingle
                        {
                            Id = item.Id,
                            AppId = item.AppId,
                            Name = item.Name,
                            WidgetCode = item.WidgetCode,
                            Target = item.Target,
                            AvailableConfig = item.AvailableConfig,
                            Cover = item.Cover,
                            Width = item.Width,
                            Height = item.Height,
                            CreateTime = item.CreateTime,
                            UpdateTime = item.UpdateTime,
                            SceneType = request.SceneType,
                        };
                        if (currentApp.AppAvailibleSortFieldList != null)
                        {
                            single.SortList.AddRange(currentApp.AppAvailibleSortFieldList.Select(c => new StringSysDictSingle
                            {
                                Key = c.SortFieldName,
                                Value = c.SortFieldValue
                            }));
                        }
                        if (item.MaxTopCount > 0)
                        {
                            for (int i = 1; i <= item.MaxTopCount; i++)
                            {
                                single.TopCountList.Add(new IntSysDictSingle
                                {
                                    Key = (i * item.TopCountInterval).ToString(),
                                    Value = i * item.TopCountInterval
                                });
                            }
                        }
                        result.AppWidgetList.Add(single);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取应用栏目列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppColumnListReply> GetAppColumnList(AppColumnListRequest request, ServerCallContext context)
        {
            var result = new AppColumnListReply();

            var columnList = await _RepositoryAppColumnInfo.Where(c => c.DeleteFlag == false && (c.AppId == request.AppId || c.AppRouteCode == request.AppId))
                                                           .ToListAsync();
            if (columnList != null && columnList.Any())
            {
                result.AppColumnList.AddRange(columnList.Select(c => new AppColumnListSingle
                {
                    Name = c.ColumnName,
                    ColumnId = c.ColumnId,
                    CreateTime = c.ColumnCreateTime.ToString(),
                    VisitUrl = c.VisitUrl,
                }));
            }

            return result;
        }

        /// <summary>
        /// 通过应用编码获取网关地址，如果学校未启用该应用，则返回为空，否则返回网关地址
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GetAppGateHostByCodeReply> GetAppGateHostByCode(GetAppGateHostByCodeRequest request, ServerCallContext context)
        {
            var result = new GetAppGateHostByCodeReply() { GateHost = "" };

            var allApps = await _ApplicationService.GetAllApp();
            var currentApp = allApps.FirstOrDefault(c => c.RouteCode.ToLower() == request.RouteCode.ToLower());
            if (currentApp != null)
            {
                result.GateHost = string.IsNullOrWhiteSpace(currentApp.ApiHost) ? "" : currentApp.ApiHost.TrimEnd('/') + $"/{request.RouteCode}";
            }

            return result;
        }

        /// <summary>
        /// 批量通过应用编码获取网关地址，如果学校未启用该应用，则返回为空，否则返回网关地址
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GetAppGateHostByCodeBatchReply> GetAppGateHostByCodeBatch(GetAppGateHostByCodeBatchRequest request, ServerCallContext context)
        {
            var result = new GetAppGateHostByCodeBatchReply();

            var allApps = await _ApplicationService.GetAllApp();

            foreach (var item in request.RouteCode)
            {
                var temp = allApps.FirstOrDefault(c => c.RouteCode == item);

                result.Results.Add(new GetAppGateHostByCodeBacthReplySingle
                {
                    RouteCode = item,
                    GateHost = string.IsNullOrWhiteSpace(temp?.ApiHost) ? "" : temp.ApiHost.TrimEnd('/') + $"/{item}"
                });
            }

            return result;
        }

        /// <summary>
        /// 获取应用事件列表（for 馆员工作台）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppEventReply> GetAppEvents(Empty request, ServerCallContext context)
        {
            var result = new AppEventReply();

            var allAppsTask = _ApplicationService.GetAllApp();
            var allAppTypeTask = _GeneralService.GetAppType();

            await Task.WhenAll(allAppsTask, allAppTypeTask);

            var allApps = allAppsTask.Result;
            var allAppType = allAppTypeTask.Result;

            if (allApps != null && allApps.Any())
            {
                foreach (var item in allApps)
                {
                    //判断是否有权限
                    var hasPermission = await GetUserAppPermissionType(new UserAppPermissionTypeRequest { AppId = item.AppId, }, context);
                    if (item.AppEntranceList != null && item.AppEntranceList.Any() && hasPermission.PermissionType > 0)
                    {
                        foreach (var entrance in item.AppEntranceList)
                        {
                            if (entrance.AppEventList != null && entrance.AppEventList.Any())
                            {
                                foreach (var eventItem in entrance.AppEventList)
                                {
                                    if (eventItem.EventType.Split(',').Contains(((int)EventTypeEnum.待办项目).ToString()))
                                    {
                                        string visitUrl;
                                        if (entrance.UseScene == 1)
                                        {
                                            visitUrl = item.FrontUrl.Split('#')[0].TrimEnd('/') + entrance.VisitUrl;
                                        }
                                        else
                                        {
                                            visitUrl = item.BackendUrl.Split('#')[0].TrimEnd('/') + entrance.VisitUrl;
                                        }
                                        result.AppEvents.Add(new AppEventSingle
                                        {
                                            AppID = item.AppId,
                                            AppName = item.AppNewName,
                                            IconPath = item.AppIcon,
                                            ParentObjID = "",
                                            EventID = eventItem.EventCode,
                                            EventName = eventItem.EventName,
                                            VisitUrl = visitUrl,
                                            AppType = item.AppType,
                                            AppTypeName = string.Join(',', allAppType.FindAll(c => item.AppType.Contains(c.Value)).Select(c => c.Name))
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取我有权限应用的业务列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<BusinessListReply> GetBusinessList(Empty request, ServerCallContext context)
        {
            var result = new BusinessListReply();

            var allAppTask = _ApplicationService.GetAllApp();
            var allBusinessTypeTask = _GeneralService.GetBusinessType();

            await Task.WhenAll(allAppTask, allBusinessTypeTask);

            var allApps = allAppTask.Result;
            var allBusinessType = allBusinessTypeTask.Result;

            if (allBusinessType != null && allBusinessType.Any())
            {
                foreach (var item in allBusinessType)
                {
                    var businessItem = new BusinessListItem
                    {
                        GroupKey = item.Value,
                        GroupName = item.Name,
                    };
                    //找到属于这种类型下的应用入口信息
                    var tempList = allApps.FindAll(c => c.AppEntranceList != null && c.AppEntranceList.Any(x => x.UseScene == 2 && x.BusinessType == item.Value));
                    foreach (var app in tempList)
                    {
                        //判断是否有权限
                        var hasPermission = await GetUserAppPermissionType(new UserAppPermissionTypeRequest { AppId = app.AppId, }, context);
                        if (hasPermission.PermissionType > 0 && app.AppEntranceList != null && app.AppEntranceList.Any())
                        {
                            var tempEntranceList = app.AppEntranceList.FindAll(c => c.UseScene == 2 && !c.IsDefault);
                            foreach (var entrance in tempEntranceList)
                            {
                                //新闻发布
                                if (app.RouteCode == "news" && entrance.Name == "新闻管理")
                                {
                                    var columnList = await _RepositoryAppColumnInfo.DetachedEntities.Where(c => !c.DeleteFlag && c.AppRouteCode == app.RouteCode).ToListAsync();
                                    if (columnList != null && columnList.Any())
                                    {
                                        foreach (var news in columnList)
                                        {
                                            businessItem.BusinessInfoList.Add(new BusinessInfo
                                            {
                                                Id = entrance.Id,
                                                AppId = app.AppId,
                                                RouteCode = app.RouteCode,
                                                IconPath = app.AppIcon,
                                                BusinessName = news.ColumnName + "管理",
                                                VisitUrl = string.Format(app.BackendUrl.Split('#')[0].TrimEnd('/') + entrance.VisitUrl, news.ColumnId)
                                            });
                                        }
                                    }
                                }
                                //信息导航
                                else if (app.RouteCode == "navigation" && entrance.Name == "栏目管理")
                                {
                                    var columnList = await _RepositoryAppColumnInfo.DetachedEntities.Where(c => !c.DeleteFlag && c.AppRouteCode == app.RouteCode).ToListAsync();
                                    if (columnList != null && columnList.Any())
                                    {
                                        foreach (var nav in columnList)
                                        {
                                            businessItem.BusinessInfoList.Add(new BusinessInfo
                                            {
                                                Id = entrance.Id,
                                                AppId = app.AppId,
                                                RouteCode = app.RouteCode,
                                                IconPath = app.AppIcon,
                                                BusinessName = nav.ColumnName + "管理",
                                                VisitUrl = string.Format(app.BackendUrl.Split('#')[0].TrimEnd('/') + entrance.VisitUrl, nav.ColumnId, nav.ColumnName)
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    var businessInfo = new BusinessInfo
                                    {
                                        Id = entrance.Id,
                                        AppId = app.AppId,
                                        RouteCode = app.RouteCode,
                                        IconPath = app.AppIcon,
                                        VisitUrl = app.BackendUrl.Split('#')[0].TrimEnd('/') + entrance.VisitUrl,
                                        BusinessName = entrance.Name
                                    };
                                    businessItem.BusinessInfoList.Add(businessInfo);
                                }
                            }
                        }
                    }

                    result.BusinessList.Add(businessItem);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取所有应用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<GetAllAppsReply> GetAllApps(Empty request, ServerCallContext context)
        {
            var result = new GetAllAppsReply();

            var appList = await _ApplicationService.GetAllApp();

            if (appList != null && appList.Any())
            {
                result.AppList.AddRange(appList.Select(c => new GetAllAppsSingle
                {
                    AppId = c.AppId,
                    Icon = c.AppIcon,
                    Name = c.AppNewName,
                    RouteCode = c.RouteCode,
                    FrontUrl = c.FrontUrl,
                    BackUrl = c.BackendUrl
                }));
            }

            return result;
        }

        /// <summary>
        /// 根据路由编码获取应用权限信息，管理权限和使用权限
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppAuthInfoReply> GetAppAuthInfo(AppAuthInfoRequest request, ServerCallContext context)
        {
            var result = new AppAuthInfoReply();

            var allApps = await _ApplicationService.GetAllApp();
            var currentApp = allApps.FirstOrDefault(c => c.RouteCode == request.AppRouteCode);
            if (currentApp != null)
            {
                //管理权限
                var appManagerList = await _RepositoryAppManager.DetachedEntities.Where(c => c.AppId == currentApp.AppId && !c.DeleteFlag)
                                    .Distinct().Select(c => c.ManageRoleId).ToListAsync();
                var managerRoleList = await _RepositoryManagerRole.DetachedEntities.Where(c => appManagerList.Contains(c.Id.ToString()) && !c.DeleteFlag)
                                        .ToListAsync();
                result.AppAuthInfo.AddRange(managerRoleList.Select(c => new AppAuthInfoSingle
                {
                    AuthType = 1,
                    UserSetType = 0,
                    AuthId = c.Id.ToString(),
                    AuthName = c.Name
                }));
                //使用权限
                var appUserList = await _RepositoryAppUser.DetachedEntities.Where(c => c.AppId == currentApp.AppId && !c.DeleteFlag).ToListAsync();
                var userTypeTask = _GeneralService.GetUserTypeList();
                var userGroupTask = _GeneralService.GetUserGroupList();
                await Task.WhenAll(userTypeTask, userGroupTask);

                var tempList = new List<AppAuthInfoSingle>();
                if (appUserList != null && appUserList.Any())
                {
                    foreach (var item in appUserList)
                    {
                        string authName;
                        if (item.UserSetType == 1)
                        {
                            authName = userTypeTask.Result.FirstOrDefault(c => c.Id == item.UserSetId)?.Name;
                        }
                        else
                        {
                            authName = userGroupTask.Result.FirstOrDefault(c => c.Id == item.UserSetId)?.Name;
                        }
                        if (!string.IsNullOrWhiteSpace(authName))
                        {
                            tempList.Add(new AppAuthInfoSingle
                            {
                                AuthType = 2,
                                UserSetType = item.UserSetType,
                                AuthId = item.UserSetId,
                                AuthName = authName
                            });
                        }
                    }
                    result.AppAuthInfo.AddRange(tempList.Distinct());
                }
            }

            return result;
        }

        /// <summary>
        /// 根据userkey获取用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserInfoReply> GetUserInfoByKey(UserInfoRequest request, ServerCallContext context)
        {
            var result = new UserInfoReply();

            var client = _GrpcClientResolver.EnsureClient<UserGrpcServiceClient>();
            var userInfo = await client.GetUserByKeyAsync(new StringValue { Value = request.UserKey });

            if (userInfo != null)
            {
                result.UserType = userInfo.Type;
                result.UserGroup = string.Join(';', userInfo.GroupIds);
                var userRole = await _RepositoryUserRole.FirstOrDefaultAsync(c => c.UserKey == request.UserKey && !c.DeleteFlag);
                result.IsSuper = userRole != null && userRole.IsSuper;
            }

            return result;
        }

        /// <summary>
        /// 获取应用更新日志
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppInfoReply> GetAppLogs(AppInfoRequest request, ServerCallContext context)
        {
            var result = new AppInfoReply();

            var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
            var grpcResult = await client.GetAppLogAsync(new Open.AppLogRequest
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                InfoType = 1
            });
            var appCenter = await _ApplicationService.GetAppDetailByCode("appcenter");
            var visitUrl = appCenter.FrontUrl.Split('#')[0] + "#/admin_appsLogDetails?id={0}";
            result.AppLogList.AddRange(grpcResult.AppLogList?.Select(c => new AppInfoSingle
            {
                Id = c.Id,
                ReleaseTime = c.ReleaseTime,
                Title = c.Title,
                VisitUrl = string.Format(visitUrl, c.Id)
            }));

            return result;
        }

        /// <summary>
        /// 获取使用帮助
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<AppUseHelpReply> GetAppUseHelpList(AppInfoRequest request, ServerCallContext context)
        {
            var result = new AppUseHelpReply();

            var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
            var grpcResult = await client.GetAppLogAsync(new Open.AppLogRequest
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                InfoType = 3
            });

            if (grpcResult != null)
            {
                var allApps = await _ApplicationService.GetAllApp();
                result.TotalCount = grpcResult.TotalCount;
                result.AppUseHelpList.AddRange(grpcResult.AppLogList.Select(c => new AppUseHelpSingle
                {
                    Id = c.Id,
                    Title = c.Title,
                    ReleaseTime = c.ReleaseTime,
                    Version = c.Version,
                    Content = c.Content,
                    AppName = allApps.FirstOrDefault(x => x.AppId == c.AppId)?.AppNewName
                }));
            }

            return result;
        }

        /// <summary>
        /// 获取对指定应用有管理权限的馆员列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<ManagerListReply> GetManagerListByCode(ManagerListRequest request, ServerCallContext context)
        {
            var result = new ManagerListReply();

            var allApps = await _ApplicationService.GetAllApp();
            var currentApp = allApps.FirstOrDefault(c => c.RouteCode == request.RouteCode);
            if (currentApp != null)
            {
                var client = _GrpcClientResolver.EnsureClient<UserGrpcServiceClient>();
                var list = await client.GetManagerListAsync(new Google.Protobuf.WellKnownTypes.Empty());
                if (list != null)
                {
                    var userRoles = await _RepositoryUserRole.DetachedEntities.Where(c => !c.DeleteFlag).ToListAsync();
                    var appManager = await _RepositoryAppManager.DetachedEntities.Where(c => !c.DeleteFlag && c.ManagerType < (int)ManagerTypeEnum.浏览者).ToListAsync();

                    foreach (var item in list.Items)
                    {
                        var hasPermission = false;
                        hasPermission = userRoles.Any(c => c.UserKey == item.Key && c.IsSuper);
                        if (hasPermission)
                        {
                            result.ManagerList.Add(item.Adapt<UserListItemSingle>());
                            continue;
                        }
                        var tempUserRole = userRoles.FirstOrDefault(c => c.UserKey == item.Key);
                        if (tempUserRole != null && !string.IsNullOrWhiteSpace(tempUserRole.ManagerRoleIds))
                        {
                            var idsArray = tempUserRole.ManagerRoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
                            hasPermission = appManager.Any(c => c.AppId == currentApp.AppId && idsArray.Contains(c.ManageRoleId));
                        }
                        if (hasPermission)
                        {
                            result.ManagerList.Add(item.Adapt<UserListItemSingle>());
                        }
                    }
                    result.TotalCount = result.ManagerList.Count;
                }
            }

            return result;
        }

    }
}
