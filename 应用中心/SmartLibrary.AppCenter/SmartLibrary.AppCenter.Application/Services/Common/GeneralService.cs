using EasyCaching.Core;
using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SmartLibrary.AppCenter.Application.Dtos.Application;
using SmartLibrary.AppCenter.Application.Dtos.BaseInfo;
using SmartLibrary.AppCenter.Application.Services.Grpc;
using SmartLibrary.AppCenter.Common.BaseService;
using SmartLibrary.AppCenter.Common.Dtos;
using SmartLibrary.AppCenter.Common.Enums;
using SmartLibrary.AppCenter.Common.Utility;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys;
using SmartLibrary.Open;
using SmartLibrary.SceneManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SmartLibrary.AppCenter.Application.Services.Common
{
    /// <summary>
    /// 通用服务
    /// </summary>
    public class GeneralService : ServiceBase, IGeneralService, IScoped
    {
        private readonly IGrpcClientResolver _GrpcClientResolver;
        private readonly IRepository<AppManager> _RepositoryAppManager;
        private readonly IRepository<AppUser> _RepositoryAppUser;
        private readonly IRepository<UserRole> _RepositoryUserRole;
        private readonly IDistributedCache _DistributedCache;
        private readonly IEasyCachingProvider _EasyCachingProvider;

        public GeneralService(IGrpcClientResolver grpcClientResolver,
                              IRepository<AppManager> repositoryAppManager,
                              IRepository<AppUser> repositoryAppUser,
                              IRepository<UserRole> repositoryUserRole,
                              IDistributedCache distributedCache,
                              IEasyCachingProvider easyCachingProvider)
        {
            _GrpcClientResolver = grpcClientResolver;
            _RepositoryAppManager = repositoryAppManager;
            _RepositoryAppUser = repositoryAppUser;
            _RepositoryUserRole = repositoryUserRole;
            _DistributedCache = distributedCache;
            _EasyCachingProvider = easyCachingProvider;
        }


        /// <summary>
        /// 终端类型
        /// </summary>
        /// <returns></returns>
        public Task<List<DictionaryDto>> GetTerminalType()
        {
            var result = new List<DictionaryDto>();

            var tempList = EnumTools.EnumToList<TerminalEnum>();

            foreach (var item in tempList)
            {
                result.Add(new DictionaryDto
                {
                    Id = item.Value.ToString(),
                    Name = item.Key,
                    Value = item.Value.ToString()
                });
            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// 应用类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictionaryDto>> GetAppType()
        {
            var client = _GrpcClientResolver.EnsureClient<SmartLibrary.Open.AppGrpcService.AppGrpcServiceClient>();
            var list = await client.GetDictionaryByTypeAsync(new DictionaryRequest { DicType = "AppServiceType" });

            var result = new List<DictionaryDto>();
            foreach (var item in list.DictionaryList)
            {
                result.Add(new DictionaryDto
                {
                    Id = item.Value,
                    Name = item.Name,
                    Value = item.Value
                });
            }

            return result;
        }

        /// <summary>
        /// 获取业务类型 馆员工作台
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictionaryDto>> GetBusinessType()
        {
            var client = _GrpcClientResolver.EnsureClient<SmartLibrary.Open.AppGrpcService.AppGrpcServiceClient>();
            var list = await client.GetDictionaryByTypeAsync(new DictionaryRequest { DicType = "BusinessType" });

            var result = new List<DictionaryDto>();
            foreach (var item in list.DictionaryList)
            {
                result.Add(new DictionaryDto
                {
                    Id = item.Value,
                    Name = item.Name,
                    Value = item.Value
                });
            }

            return result;
        }


        /// <summary>
        /// 采购类型
        /// </summary>
        /// <returns></returns>
        public Task<List<DictionaryDto>> GetPurchaseType()
        {
            var result = new List<DictionaryDto>();

            var tempList = EnumTools.EnumToList<PurchaseTypeEnum>();

            foreach (var item in tempList)
            {
                result.Add(new DictionaryDto
                {
                    Id = item.Value.ToString(),
                    Name = item.Key,
                    Value = item.Value.ToString()
                });
            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictionaryDto>> GetUserTypeList()
        {
            var result = new List<DictionaryDto>();

            var cacheList = await _EasyCachingProvider.GetAsync<List<DictionaryDto>>(Owner + CacheKey.AllUserType);
            if (cacheList.HasValue)
            {
                result = cacheList.Value;
                return result;
            }

            var client = _GrpcClientResolver.EnsureClient<SmartLibrary.User.RpcService.UserGrpcService.UserGrpcServiceClient>();
            var list = await client.GetUserTypeListAsync(new User.RpcService.SimpleTableQuery
            {
                PageIndex = 1,
                PageSize = 100,
                KeyWord = string.Empty
            });

            result.AddRange(list.Items.Select(c => new DictionaryDto
            {
                Id = c.Value,
                Name = c.Key,
                Value = c.Value
            }));

            await _EasyCachingProvider.SetAsync(Owner + CacheKey.AllUserType, result, TimeSpan.FromMinutes(2));

            return result;
        }

        /// <summary>
        /// 用户分组
        /// </summary>
        /// <returns></returns>
        public async Task<List<DictionaryDto>> GetUserGroupList()
        {
            var result = new List<DictionaryDto>();

            var cacheList = await _EasyCachingProvider.GetAsync<List<DictionaryDto>>(Owner + CacheKey.AllUserGroup);
            if (cacheList.HasValue)
            {
                result = cacheList.Value;
                return result;
            }

            var client = _GrpcClientResolver.EnsureClient<SmartLibrary.User.RpcService.UserGrpcService.UserGrpcServiceClient>();
            var list = await client.GetUserGroupListAsync(new User.RpcService.SimpleTableQuery
            {
                PageIndex = 1,
                PageSize = 100,
                KeyWord = string.Empty
            });

            result.AddRange(list.Items.Select(c => new DictionaryDto
            {
                Id = c.Value,
                Name = c.Key,
                Value = c.Value
            }));

            //缓存
            await _EasyCachingProvider.SetAsync(Owner + CacheKey.AllUserGroup, result, TimeSpan.FromMinutes(2));

            return result;
        }

        /// <summary>
        /// 获取当前机构信息
        /// </summary>
        /// <returns></returns>
        public async Task<OrgInfo> GetCurrentOrgInfo()
        {
            var client = _GrpcClientResolver.EnsureClient<SmartLibrary.Open.AppGrpcService.AppGrpcServiceClient>();
            var org = await client.GetOrgInfoAsync(new Google.Protobuf.WellKnownTypes.Empty());

            var result = new OrgInfo
            {
                FileUrl = org.FileUrl,
                LoginUrl = org.LoginUrl,
                ManageUrl = org.ManageUrl,
                MgrLoginUrl = org.MgrLoginUrl,
                OrgCode = org.OrgCode,
                OrgName = org.OrgName,
                PortalUrl = org.PortalUrl,
                LogoUrl = org.LogoUrl,
                SimpleLogoUrl = org.SimpleLogoUrl
            };

            return result;
        }

        /// <summary>
        /// 获取当前头部底部信息
        /// </summary>
        /// <returns></returns>
        public async Task<HeaderFooterReply> GetCurrentHeaderFooterInfo()
        {
            var client = _GrpcClientResolver.EnsureClient<SmartLibrary.SceneManage.SceneManageGrpcService.SceneManageGrpcServiceClient>();
            var result = await client.GetDefaultHeaderFooterAsync(new Google.Protobuf.WellKnownTypes.Empty());

            return result;
        }

        /// <summary>
        /// 获取当前用户对应【应用中心】权限类型（后台）
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetPermissionType()
        {
            var result = 0;

            //获取userkey
            var userKey = App.User?.FindFirstValue("UserKey");
            if (string.IsNullOrWhiteSpace(userKey))
            {
                return result;
            }

            //获取管理员分配的角色信息
            var userRole = await _RepositoryUserRole.DetachedEntities.FirstOrDefaultAsync(c => c.UserKey == userKey && !c.DeleteFlag);
            if (userRole != null)
            {
                //是超级管理员
                if (userRole.IsSuper)
                {
                    result = 1;
                    return result;
                }
                else if (!string.IsNullOrWhiteSpace(userRole.ManagerRoleIds))
                {
                    var roleArray = userRole.ManagerRoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    //获取角色绑定的应用
                    var appManagers = await _RepositoryAppManager.DetachedEntities.Where(c => roleArray.Contains(c.ManageRoleId.ToString()) && !c.DeleteFlag)
                                                                 .ToListAsync();
                    if (appManagers != null && appManagers.Any())
                    {
                        var allApps = await GetAllApp();
                        var appInfo = allApps.FirstOrDefault(c => c.RouteCode.ToLower() == "appcenter");
                        if (appInfo != null)
                        {
                            //获取传入应用角色信息
                            var currentAppManagers = appManagers.Where(c => c.AppId.ToString() == appInfo.AppId).ToList();
                            if (currentAppManagers != null && currentAppManagers.Any())
                            {
                                //如果多个角色都有此应用，则取授权类型范围最大的返回
                                result = currentAppManagers.Min(c => c.ManagerType);
                            }
                        }
                    }
                }
            }


            return result;
        }

        /// <summary>
        /// 获取所有应用
        /// </summary>
        /// <returns></returns>
        private async Task<List<AppListDto>> GetAllApp()
        {
            var client = _GrpcClientResolver.EnsureClient<SmartLibrary.Open.AppGrpcService.AppGrpcServiceClient>();
            var list = await client.GetAppListAsync(new Google.Protobuf.WellKnownTypes.Empty());

            var result = new List<AppListDto>();

            result.AddRange(list.AppList.Select(c => new AppListDto
            {
                AppId = c.AppId,
                AppName = c.AppName,
                AppIcon = c.AppIcon,
                FrontUrl = c.FrontUrl,
                BackendUrl = c.BackendUrl,
                AppType = c.AppType,
                BeginDate = c.BeginDate,
                Content = c.Content,
                CreateTime = c.CreateTime,
                CurrentVersion = c.CurrentVersion,
                Developer = c.Developer,
                ExpireDate = c.ExpireDate,
                Price = c.Price,
                PurchaseType = c.PurchaseType,
                PurchaseTypeName = c.PurchaseTypeName,
                SceneType = c.SceneType,
                ShowStatus = c.ShowStatus,
                Status = c.Status,
                Terminal = c.Terminal,
                UpdateTime = c.UpdateTime,
                ApiHost = c.ApiHost,
                RouteCode = c.RouteCode,
                IsThirdApp = false,
                AppEntranceList = c.AppEntranceList.Select(x => new AppEntrance
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    UseScene = x.UseScene,
                    VisitUrl = x.VisitUrl,
                    IsSystem = x.IsSystem,
                    IsDefault = x.IsDefault,
                    BusinessType = x.BusinessType,
                    AppEventList = x.AppEventList.Select(a => new AppEvent
                    {
                        EventCode = a.EventCode,
                        EventName = a.EventName,
                        EventType = a.EventType,
                    }).ToList()
                }).ToList(),
                AppWidgetList = c.AppWidgetList.Select(x => new AppWidget
                {
                    Id = x.Id,
                    AppId = x.AppId,
                    Name = x.Name,
                    Target = x.Target,
                    AvailableConfig = x.AvailableConfig,
                    MaxTopCount = x.MaxTopCount,
                    TopCountInterval = x.TopCountInterval,
                    Cover = x.Cover,
                    WidgetCode = x.WidgetCode,
                    Width = x.Width,
                    Height = x.Height,
                    CreateTime = x.CreateTime,
                    UpdateTime = x.UpdateTime,
                    SceneType = x.SceneType,
                }).ToList(),
                AppAvailibleSortFieldList = c.AppAvailibleSortFieldList.Select(x => new AppAvailibleSortField
                {
                    Id = x.Id,
                    AppId = x.AppId,
                    SortFieldName = x.SortFieldName,
                    SortFieldValue = x.SortFieldValue,
                }).ToList()
            }));

            return result;
        }


    }
}
