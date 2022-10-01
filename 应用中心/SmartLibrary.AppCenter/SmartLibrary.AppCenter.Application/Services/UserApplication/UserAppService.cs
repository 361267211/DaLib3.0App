using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter.Application.Dtos.Application;
using SmartLibrary.AppCenter.Application.Dtos.BaseInfo;
using SmartLibrary.AppCenter.Application.Dtos.UserApplication;
using SmartLibrary.AppCenter.Application.Services.Application;
using SmartLibrary.AppCenter.Application.Services.Grpc;
using SmartLibrary.AppCenter.Common.BaseService;
using SmartLibrary.AppCenter.Common.Enums;
using SmartLibrary.AppCenter.Common.Utility;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartLibrary.User.RpcService.UserGrpcService;

namespace SmartLibrary.AppCenter.Application.Services.UserApplication
{
    /// <summary>
    /// 个人应用中心
    /// </summary>
    public class UserAppService : ServiceBase, IUserAppService, IScoped
    {
        private readonly IRepository<AppCollection> _RepositoryAppCollection;
        private readonly IGrpcClientResolver _GrpcClientResolver;
        private readonly IApplicationService _ApplicationService;
        private readonly IRepository<AppManager> _RepositoryAppManager;
        private readonly IRepository<AppUser> _RepositoryAppUser;
        private readonly IRepository<UserRole> _RepositoryUserRole;
        private readonly IRepository<Navigation> _RepositoryNavigation;

        public UserAppService(IRepository<AppCollection> repositoryAppCollection,
                              IGrpcClientResolver grpcClientResolver,
                              IApplicationService applicationService,
                              IRepository<AppManager> repositoryAppManager,
                              IRepository<AppUser> repositoryAppUser,
                              IRepository<UserRole> repositoryUserRole,
                              IRepository<Navigation> repositoryNavigation)
        {
            _RepositoryAppCollection = repositoryAppCollection;
            _GrpcClientResolver = grpcClientResolver;
            _ApplicationService = applicationService;
            _RepositoryAppManager = repositoryAppManager;
            _RepositoryAppUser = repositoryAppUser;
            _RepositoryUserRole = repositoryUserRole;
            _RepositoryNavigation = repositoryNavigation;
        }

        /// <summary>
        /// 收藏应用
        /// </summary>
        /// <param name="id">应用ID</param>
        /// <returns></returns>
        public async Task<bool> CollectionApp(string id)
        {
            CheckIsLogin();

            var current = await _RepositoryAppCollection.FirstOrDefaultAsync(c => !c.DeleteFlag && c.AppId == id && c.UserKey == UserKey);
            if (current == null)
            {
                await _RepositoryAppCollection.InsertNowAsync(new AppCollection
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTimeOffset.Now,
                    UpdatedTime = DateTimeOffset.Now,
                    DeleteFlag = false,
                    AppId = id,
                    UserKey = UserKey
                });
            }

            return true;
        }

        /// <summary>
        /// 删除已经收藏的应用
        /// </summary>
        /// <param name="id">应用ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteMyApp(string id)
        {
            CheckIsLogin();

            var temp = await _RepositoryAppCollection.FirstOrDefaultAsync(c => c.UserKey == UserKey && c.AppId == id && !c.DeleteFlag);
            if (temp != null)
            {
                temp.DeleteFlag = true;
                temp.UpdatedTime = DateTimeOffset.Now;

                await _RepositoryAppCollection.UpdateIncludeNowAsync(temp, new[] { nameof(temp.DeleteFlag), nameof(temp.UpdatedTime) });
            }

            return true;
        }

        /// <summary>
        /// 获取所有前台应用,包含用户收藏标识，用于前台我的应用中心
        /// </summary>
        /// <returns></returns>
        public async Task<List<AllAppDto>> GetAllApps()
        {
            var result = new List<AllAppDto>();

            var allAppList = await _ApplicationService.GetAllApp();
            var collection = await _RepositoryAppCollection.DetachedEntities.Where(c => c.UserKey == UserKey && !c.DeleteFlag)
                                .Select(c => c.AppId)
                                .ToListAsync();

            var appCenter = allAppList.FirstOrDefault(c => c.RouteCode == "appcenter");
            //应用中心地址前面半截
            var appCenterPreUrl = appCenter?.FrontUrl.Split('#')[0];

            allAppList = allAppList.FindAll(c => c.AppEntranceList != null && c.AppEntranceList.Any(x => x.IsDefault && x.IsSystem && x.UseScene == 1));
            allAppList.ForEach(app =>
            {
                result.Add(new AllAppDto
                {
                    AppId = app.AppId,
                    AppName = app.AppNewName,
                    AppIcon = app.AppIcon,
                    Content = app.Content,
                    FrontUrl = app.Terminal == ((int)TerminalEnum.H5).ToString() ? $"{appCenterPreUrl}#/web_appsDetails?id={app.AppId}" : app.FrontUrl,
                    AppType = app.AppType,
                    RouteCode = app.RouteCode,
                    IsCollection = collection != null && collection.Any(c => c == app.AppId)
                });
            });

            return result;
        }

        /// <summary>
        /// 获取我已经收藏的应用
        /// </summary>
        /// <returns></returns>
        public async Task<List<MyCollectionAppDto>> GetMyCollectionApps()
        {
            CheckIsLogin();

            var allAppList = await _ApplicationService.GetAllApp();
            var myApps = await _RepositoryAppCollection.DetachedEntities.Where(c => c.UserKey == UserKey && !c.DeleteFlag).ToListAsync();

            var appCenter = allAppList.FirstOrDefault(c => c.RouteCode == "appcenter");
            //应用中心地址前面半截
            var appCenterPreUrl = appCenter?.FrontUrl.Split('#')[0];

            var result = new List<MyCollectionAppDto>();
            if (myApps != null && myApps.Any())
            {
                foreach (var item in myApps)
                {
                    var appInfo = allAppList.FirstOrDefault(x => x.AppId == item.AppId);
                    if (appInfo != null)
                    {
                        result.Add(new MyCollectionAppDto
                        {
                            AppId = item.AppId,
                            RouteCode = appInfo.RouteCode,
                            AppName = appInfo.AppNewName,
                            AppIcon = appInfo.AppIcon,
                            FrontUrl = appInfo.Terminal == ((int)TerminalEnum.H5).ToString() ? $"{appCenterPreUrl}#/web_appsDetails?id={appInfo.AppId}" : appInfo.FrontUrl,
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取推荐应用
        /// </summary>
        /// <returns></returns>
        public async Task<List<RecommendAppDto>> GetRecommendApps()
        {
            var result = new List<RecommendAppDto>();
            var allAppList = await _ApplicationService.GetAllApp();

            var appCenter = allAppList.FirstOrDefault(c => c.RouteCode == "appcenter");
            //应用中心地址前面半截
            var appCenterPreUrl = appCenter?.FrontUrl.Split('#')[0];

            allAppList.ForEach(c =>
            {
                // 要默认入口 并且使用场景 =1
                if (c.AppEntranceList != null && c.AppEntranceList.Any(x => x.IsDefault && x.IsSystem && x.UseScene == 1))
                {
                    result.Add(new RecommendAppDto
                    {
                        AppIcon = c.AppIcon,
                        AppId = c.AppId,
                        AppName = c.AppNewName,
                        FrontUrl = c.Terminal == ((int)TerminalEnum.H5).ToString() ? $"{appCenterPreUrl}#/web_appsDetails?id={c.AppId}" : c.FrontUrl,
                        Content = c.Content,
                        ShowType = "热门",
                        IsCollection = !string.IsNullOrWhiteSpace(UserKey) && _RepositoryAppCollection.DetachedEntities.Any(x => x.AppId == c.AppId && x.UserKey == UserKey && !x.DeleteFlag)
                    });
                }
            });

            return result;
        }

        /// <summary>
        /// 获取推荐应用，带应用中心跳转地址
        /// </summary>
        /// <returns></returns>
        public async Task<RecommendAppMoreDto> GetRecommendAppMore()
        {
            var result = new RecommendAppMoreDto() { RecommendApps = new List<RecommendAppDto>() };

            var allApps = await _ApplicationService.GetAllApp();
            var appCenter = allApps.FirstOrDefault(c => c.RouteCode == "appcenter");
            //应用中心地址前面半截
            var appCenterPreUrl = appCenter?.FrontUrl.Split('#')[0];
            result.MoreUrl = appCenterPreUrl + "#/web_appsCenter";

            allApps.ForEach(c =>
            {
                // 要默认入口 并且使用场景 =1
                if (c.AppEntranceList != null && c.AppEntranceList.Any(x => x.IsDefault && x.IsSystem && x.UseScene == 1))
                {
                    result.RecommendApps.Add(new RecommendAppDto
                    {
                        AppIcon = c.AppIcon,
                        AppId = c.AppId,
                        AppName = c.AppNewName,
                        FrontUrl = c.Terminal == ((int)TerminalEnum.H5).ToString() ? $"{appCenterPreUrl}#/web_appsDetails?id={c.AppId}" : c.FrontUrl,
                        Content = c.Content,
                        ShowType = "热门",
                        IsCollection = !string.IsNullOrWhiteSpace(UserKey) && _RepositoryAppCollection.DetachedEntities.Any(x => x.AppId == c.AppId && x.UserKey == UserKey && !x.DeleteFlag)
                    });
                }
            });

            return result;
        }

        /// <summary>
        /// 根据userkey获取指定应用的权限类型
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="appRouteCode"></param>
        /// <returns></returns>
        public async Task<UserAppPermissionTypeReply> GetUserAppPermissionType(string userKey, string appRouteCode)
        {
            var result = new UserAppPermissionTypeReply();
            result.PermissionType = 0;

            //获取userkey
            if (string.IsNullOrWhiteSpace(userKey) || string.IsNullOrWhiteSpace(appRouteCode))
            {
                return result;
            }
            var apps = await _ApplicationService.GetAllApp();
            var appId = apps.FirstOrDefault(p => p.RouteCode == appRouteCode)?.AppId;
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
                    var appManagers = await _RepositoryAppManager.DetachedEntities.Where(c => roleArray.Contains(c.ManageRoleId) && !c.DeleteFlag)
                                                                 .ToListAsync();
                    if (appManagers != null && appManagers.Any())
                    {
                        //获取传入应用角色信息
                        var currentAppManagers = appManagers.Where(c => c.AppId == appId).ToList();
                        if (currentAppManagers != null && currentAppManagers.Any())
                        {
                            //如果多个角色都有此应用，则取授权类型范围最大的返回
                            result.PermissionType = currentAppManagers.Min(c => c.ManagerType);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 判断用户对指定应用是否有使用权限(前台)
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="appRouteCode"></param>
        /// <returns></returns>
        public async Task<UserAppPermissionReply> GetUserAppPermission(string userKey, string appRouteCode)
        {
            var result = new UserAppPermissionReply();
            result.IsHasPermission = 0;

            //获取userkey
            if (string.IsNullOrWhiteSpace(userKey) || string.IsNullOrWhiteSpace(appRouteCode))
            {
                return result;
            }
            var apps = await _ApplicationService.GetAllApp();
            var appId = apps.FirstOrDefault(p => p.RouteCode == appRouteCode)?.AppId;

            //获取当前用户的分组和类型
            var client = _GrpcClientResolver.EnsureClient<UserGrpcServiceClient>();
            var userInfo = await client.GetUserByKeyAsync(new StringValue() { Value = userKey });

            if (userInfo != null)
            {
                //用户类型判断
                bool isPermission = false;
                var userType = userInfo.Type;
                isPermission = await _RepositoryAppUser.DetachedEntities.AnyAsync(c => c.UserSetId == userType && c.AppId == appId
                                                                && c.UserSetType == 1 && !c.DeleteFlag);
                if (isPermission)
                {
                    result.IsHasPermission = 1;
                    return result;
                }

                //用户组判断
                var userGroups = userInfo.GroupIds.ToList();
                isPermission = await _RepositoryAppUser.DetachedEntities.AnyAsync(c => userGroups.Contains(c.UserSetId) && c.AppId == appId
                                                                 && c.UserSetType == 2 && !c.DeleteFlag);
                result.IsHasPermission = isPermission ? 1 : 0;
            }

            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetCurrentUserInfo(string userKey)
        {
            //获取userkey
            if (string.IsNullOrWhiteSpace(userKey))
            {
                return new UserInfo();
            }

            //获取当前用户的分组和类型
            var client = _GrpcClientResolver.EnsureClient<UserGrpcServiceClient>();
            var user = await client.GetUserByKeyAsync(new StringValue() { Value = userKey });
            var result = new UserInfo
            {
                UserKey = user.Key,
                Depart = user.Depart,
                DepartName = user.DepartName,
                Edu = user.Edu,
                Gender = user.Gender,
                Grade = user.Grade,
                GroupIds = user.GroupIds.ToList(),
                Name = user.Name,
                NickName = user.NickName,
                Photo = user.Photo,
                ShowStatus = user.ShowStatus,
                Status = user.Status,
                StudentNo = user.StudentNo,
                Type = user.Type,
                TypeName = user.TypeName
            };
            return result;
        }

        /// <summary>
        /// 获取顶部菜单
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<List<AppMenuListDto>> GetMgrTopMenu(string userKey)
        {
            var result = new List<AppMenuListDto>();

            var dicMgrTopMenu = new Dictionary<string, string>
            {
                //{ "workbench", "工作台" },
                { "appcenter", "应用中心" },
                { "scenemanage", "场景管理" },
                { "usermanage", "用户管理" },
                { "resourcecenter", "数据管理" },
                { "loganalysis", "运行统计" },
            };

            foreach (var item in dicMgrTopMenu)
            {
                var permission = await GetUserAppPermissionType(userKey, item.Key);
                if (permission.PermissionType == 0 && item.Key != "workbench")
                {
                    continue;
                }
                var app = await _ApplicationService.GetAppDetailByCode(item.Key);
                result.Add(new AppMenuListDto
                {
                    AppIcon = app.AppIcon,
                    AppId = app.Id,
                    AppName = item.Value,
                    BackendUrl = app.BackendUrl
                });
            }

            return result;
        }

        /// <summary>
        /// 获取应用详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AppDetailDto> GetAppDetail(string id)
        {
            return await _ApplicationService.GetAppDetail(id);
        }

        /// <summary>
        /// 根据routecode获取当前应用的名称和版本号
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public async Task<CurrentAppInfo> GetCurrentAppInfo(string appCode)
        {
            var result = new CurrentAppInfo() { AppName = "", AppVersion = "", AppIcon = "" };
            if (appCode.IsEmptyOrWhiteSpace())
            {
                return result;
            }
            var allApps = await _ApplicationService.GetAllApp();
            var currentApp = allApps.FirstOrDefault(c => c.RouteCode == appCode.ToLower());
            if (currentApp != null)
            {
                result.AppName = currentApp.AppNewName;
                result.AppVersion = currentApp.CurrentVersion;
                result.AppIcon = currentApp.AppIcon;
            }

            return result;
        }
    }
}
