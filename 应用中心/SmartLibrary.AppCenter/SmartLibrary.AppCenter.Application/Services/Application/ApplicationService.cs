using EasyCaching.Core;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartLibrary.AppCenter.Application.Dtos.Application;
using SmartLibrary.AppCenter.Application.Services.Common;
using SmartLibrary.AppCenter.Application.Services.Grpc;
using SmartLibrary.AppCenter.Common.BaseService;
using SmartLibrary.AppCenter.Common.Dtos;
using SmartLibrary.AppCenter.Common.Enums;
using SmartLibrary.AppCenter.Common.Utility;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys;
using SmartLibrary.Open;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static SmartLibrary.Open.AppGrpcService;
using static SmartLibrary.User.RpcService.UserGrpcService;

namespace SmartLibrary.AppCenter.Application.Services.Application
{
    /// <summary>
    /// 应用服务
    /// </summary>
    public class ApplicationService : ServiceBase, IApplicationService, IScoped
    {
        private readonly IGrpcClientResolver _GrpcClientResolver;
        private readonly IGeneralService _GeneralService;
        private readonly IRepository<ManagerRole> _RepositoryManagerRole;
        private readonly IRepository<AppManager> _RepositoryAppManager;
        private readonly IRepository<AppUser> _RepositoryAppUser;
        private readonly IRepository<UserRole> _RepositoryUserRole;
        private readonly IRepository<Navigation> _RepositoryNavigation;
        private readonly IRepository<AppNavigation> _RepositoryAppNavigation;
        private readonly IRepository<ThirdApplication> _RepositoryThirdApplication;
        private readonly IDistributedCache _DistributedCache;
        private readonly IRepository<AppColumnInfo> _RepositoryAppColumnInfo;
        private readonly IRepository<AppReName> _RepositoryAppReName;
        private readonly IEasyCachingProvider _EasyCachingProvider;

        public ApplicationService(IRepository<ManagerRole> repositoryManagerRole,
                                  IRepository<AppManager> repositoryAppManager,
                                  IRepository<AppUser> repositoryAppUser,
                                  IGrpcClientResolver grpcClientResolver,
                                  IRepository<UserRole> repositoryUserRole,
                                  IRepository<Navigation> repositoryNavgation,
                                  IRepository<AppNavigation> repositoryAppNavigation,
                                  IRepository<ThirdApplication> repositoryThirdApplication,
                                  IGeneralService generalService,
                                  IDistributedCache distributedCache,
                                  IRepository<AppColumnInfo> repositoryAppColumnInfo,
                                  IRepository<AppReName> repositoryAppReName,
                                  IEasyCachingProvider easyCachingProvider)
        {
            _RepositoryManagerRole = repositoryManagerRole;
            _RepositoryAppManager = repositoryAppManager;
            _RepositoryAppUser = repositoryAppUser;
            _GrpcClientResolver = grpcClientResolver;
            _RepositoryUserRole = repositoryUserRole;
            _RepositoryNavigation = repositoryNavgation;
            _RepositoryAppNavigation = repositoryAppNavigation;
            _RepositoryAppNavigation = repositoryAppNavigation;
            _RepositoryThirdApplication = repositoryThirdApplication;
            _RepositoryThirdApplication = repositoryThirdApplication;
            _GeneralService = generalService;
            _DistributedCache = distributedCache;
            _RepositoryAppColumnInfo = repositoryAppColumnInfo;
            _RepositoryAppReName = repositoryAppReName;
            _EasyCachingProvider = easyCachingProvider;
        }

        #region 基础

        /// <summary>
        /// 获取所有应用
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppListDto>> GetAllApp()
        {
            var result = new List<AppListDto>();

            var cacheApps = await _EasyCachingProvider.GetAsync<List<AppListDto>>(Owner + CacheKey.AllApps);
            if (cacheApps.HasValue)
            {
                result = cacheApps.Value;
                ChangeUrl(result);
                return result;
            }

            var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
            var grpcList = await client.GetAppListAsync(new Google.Protobuf.WellKnownTypes.Empty());
            result = grpcList.AppList.Select(c => new AppListDto
            {
                AppId = c.AppId,
                AppName = c.AppName,
                AppIcon = c.AppIcon,
                FrontUrl = c.FrontUrl,
                BackendUrl = c.BackendUrl,
                AppType = c.AppType,
                BeginDate = c.BeginDate,
                Content = c.Content,
                AppExplain = "",
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
                        EventType = a.EventType
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
            }).ToList();

            //处理三方应用
            var thirdAppList = await GetAllThirdApp();
            if (thirdAppList != null && thirdAppList.Any())
            {
                result.AddRange(thirdAppList);
            }

            // 处理应用改名
            await HandlerAppNameAsync(result);

            //缓存
            await _EasyCachingProvider.SetAsync(Owner + CacheKey.AllApps, result, TimeSpan.FromMinutes(2));

            // 替换跳转2.2的地址
            ChangeUrl(result);

            return result;
        }

        /// <summary>
        /// 获取所有三方应用
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppListDto>> GetAllThirdApp()
        {
            var result = new List<AppListDto>();

            var thirdApps = await _RepositoryThirdApplication.DetachedEntities.Where(c => !c.DeleteFlag).ToListAsync();

            if (thirdApps != null && thirdApps.Any())
            {
                foreach (var item in thirdApps)
                {
                    result.Add(new AppListDto
                    {
                        AppId = item.Id.ToString(),
                        AppName = item.AppName,
                        AppIcon = item.AppIcon,
                        FrontUrl = item.FrontUrl,
                        BackendUrl = "",
                        AppType = item.AppType,
                        BeginDate = item.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        Content = item.AppIntroduction,
                        AppExplain = item.AppExplain,
                        CreateTime = item.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        CurrentVersion = "",
                        Developer = item.Developer,
                        ExpireDate = "",
                        Price = "",
                        PurchaseType = "1",
                        PurchaseTypeName = "正式",
                        SceneType = "1",
                        ShowStatus = item.Status == 1 ? "启用" : "停用",
                        Status = item.Status.ToString(),
                        Terminal = item.Terminal,
                        UpdateTime = item.UpdatedTime.GetValueOrDefault(DateTimeOffset.Now).ToString("yyyy-MM-dd HH:mm:ss"),
                        ApiHost = "",
                        RouteCode = "",
                        IsThirdApp = true,
                        AppEntranceList = new List<AppEntrance>(),
                        AppWidgetList = new List<AppWidget>(),
                        AppAvailibleSortFieldList = new List<AppAvailibleSortField>()
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 不同的人转换为不同跳转2.2的地址
        /// </summary>
        /// <param name="apps"></param>
        private void ChangeUrl(List<AppListDto> apps)
        {
            //临时删除掉馆员工作台
            //apps.RemoveAll(c => c.RouteCode == "workbench");
            var constApps = new List<string> { "reference", "readrecord", "bookborrow", "leaveschool", "spaceorder",
                                               "activity", "questionorder", "approvalmgr","repairmgr","erms","checkrepeat",
                                               "bigdata","apimgr","dataauthmgr","datamonitorlog","resourcecenter","loganalysis",
                                               "documentdelivery","coursecenter","readreport","questionnaire","scientific","checkcitation"};
            if (apps != null && apps.Any())
            {
                var appCenter = apps.FirstOrDefault(c => c.RouteCode == "appcenter");
                //应用中心地址前面半截
                var appCenterPreUrl = appCenter?.FrontUrl.Split('#')[0];

                apps.ForEach(c =>
                {
                    if (constApps.Contains(c.RouteCode))
                    {
                        c.FrontUrl = GetFrontUrl(c.AppEntranceList?.FirstOrDefault(x => x.IsSystem && x.IsDefault && x.UseScene == 1)?.VisitUrl);
                        c.BackendUrl = GetBackUrl(c.AppEntranceList?.FirstOrDefault(x => x.IsSystem && x.IsDefault && x.UseScene == 2)?.VisitUrl);
                    }
                    //如果是三方应用，并且没得访问地址，则访问地址为前台应用详情页
                    if (c.IsThirdApp && string.IsNullOrWhiteSpace(c.FrontUrl))
                    {
                        c.FrontUrl = $"{appCenterPreUrl}#/web_appsDetails?id={c.AppId}";
                    }
                });
            }
        }

        /// <summary>
        /// 获取2.2前台跳转地址
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        private string GetFrontUrl(string redirectUrl)
        {
            if (string.IsNullOrWhiteSpace(redirectUrl)) return "";

            var code = MD5.Encode($"{SiteGlobalConfig.OldSite.Aid}{SiteGlobalConfig.OldSite.Secret}{UserKey}");
            return $"{SiteGlobalConfig.OldSite.WebUrl}/caslogin/appredirect?aid={SiteGlobalConfig.OldSite.Aid}&code={code}&readerId={UserKey}&redirect={HttpUtility.UrlEncode(redirectUrl)}";
        }

        /// <summary>
        /// 获取2.2后台跳转地址
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        private string GetBackUrl(string redirectUrl)
        {
            if (string.IsNullOrWhiteSpace(redirectUrl))
                return "";
            var encodeString = MD5.Encode($"{SiteGlobalConfig.OldSite.Owner}@{SiteGlobalConfig.OldSite.SiteId}#{UserKey}&{DateTime.Today:yyyyMMdd}");
            var token = $"{TripleDES.TripleDESEncrypting($"{encodeString}|{UserKey}")}";
            redirectUrl = HttpUtility.UrlEncode(redirectUrl);
            var result = $"{SiteGlobalConfig.OldSite.MgrUrl}/core/account/quicklogin?token={token}&returnUrl={redirectUrl}";
            return result;
        }

        /// <summary>
        /// 处理应用名称，改名则用新名称，没改名则用原来的名称
        /// </summary>
        /// <param name="apps"></param>
        private async Task HandlerAppNameAsync(List<AppListDto> apps)
        {
            if (apps != null && apps.Any())
            {
                var allReNames = await _RepositoryAppReName.DetachedEntities.Where(c => !c.DeleteFlag).ToListAsync();
                if (allReNames != null && allReNames.Any())
                {
                    apps.ForEach(c =>
                    {
                        var temp = allReNames.FirstOrDefault(x => x.AppId == c.AppId);
                        c.AppNewName = temp != null ? temp.AppNewName : c.AppName;
                    });
                }
                else
                {
                    apps.ForEach(c =>
                    {
                        c.AppNewName = c.AppName;
                    });
                }
            }
        }

        #endregion


        #region 角色

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RoleDto> GetRole(Guid id)
        {
            var role = await _RepositoryManagerRole.DetachedEntities.FirstOrDefaultAsync(c => c.Id == id);

            var res = new RoleDto
            {
                Id = role.Id.ToString(),
                Name = role.Name,
                Remark = role.Remark,
                AppAuths = new List<AppAuth>()
            };

            var managerList = await _RepositoryAppManager.DetachedEntities.Where(c => c.ManageRoleId == id.ToString() && !c.DeleteFlag).ToListAsync();
            if (managerList != null && managerList.Any())
            {
                res.AppAuths.AddRange(managerList.Select(c => new AppAuth { AppId = c.AppId, ManagerType = c.ManagerType }));
            }

            return res;
        }

        /// <summary>
        /// 分页获取角色列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<RoleListDto>> GetRoleList(int pageIndex, int pageSize)
        {
            var allApps = await GetAllApp();
            var list = await _RepositoryManagerRole.DetachedEntities.Where(c => !c.DeleteFlag)
                                                   .Select(c => new RoleListDto
                                                   {
                                                       Id = c.Id,
                                                       Name = c.Name,
                                                       CreateTime = c.CreatedTime.ToString("yyyy-MM-dd"),
                                                       AuthApps = ""
                                                   }).ToPagedListAsync(pageIndex, pageSize);

            if (list != null && list.Items.Any())
            {
                foreach (var item in list.Items)
                {
                    var authApps = await _RepositoryAppManager.DetachedEntities.Where(c => !c.DeleteFlag && c.ManageRoleId == item.Id.ToString())
                                        .Select(c => c.AppId)
                                        .ToListAsync();

                    item.AuthApps = string.Join(';', allApps.Where(c => authApps.Contains(c.AppId)).Select(c => c.AppName));
                }
            }

            return list;
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleListDto>> GetAllRole()
        {
            var list = await _RepositoryManagerRole.DetachedEntities.Where(c => !c.DeleteFlag)
                                                   .Select(c => new RoleListDto
                                                   {
                                                       Id = c.Id,
                                                       Name = c.Name,
                                                       CreateTime = c.CreatedTime.ToString("yyyy-MM-dd"),
                                                       AuthApps = ""
                                                   }).ToListAsync();

            return list;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> InsertRole(RoleDto dto)
        {
            var exist = await _RepositoryManagerRole.DetachedEntities.AnyAsync(c => c.Name == dto.Name);
            if (exist) throw Oops.Oh("角色名称不能重复").StatusCode(499);

            var entity = new ManagerRole
            {
                Id = Guid.NewGuid(),
                CreatedTime = DateTimeOffset.Now,
                UpdatedTime = DateTimeOffset.Now,
                Name = dto.Name,
                Remark = dto.Remark,
                DeleteFlag = false
            };

            var newEntity = await _RepositoryManagerRole.InsertNowAsync(entity);

            var list = new List<AppManager>();
            foreach (var item in dto.AppAuths)
            {
                list.Add(new AppManager
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTimeOffset.Now,
                    UpdatedTime = DateTimeOffset.Now,
                    DeleteFlag = false,
                    ManageRoleId = newEntity.Entity.Id.ToString(),
                    AppId = item.AppId,
                    ManagerType = item.ManagerType
                });
            }

            await _RepositoryAppManager.InsertNowAsync(list);

            return true;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateRole(RoleDto dto)
        {
            //判断名称是否重复
            var exist = await _RepositoryManagerRole.DetachedEntities.AnyAsync(c => c.Name == dto.Name && c.Id.ToString() != dto.Id);
            if (exist) throw Oops.Oh("角色名称不能重复").StatusCode(499);

            var entity = await _RepositoryManagerRole.FirstOrDefaultAsync(c => c.Id.ToString() == dto.Id);

            entity.Name = dto.Name;
            entity.Remark = dto.Remark;
            entity.UpdatedTime = DateTimeOffset.Now;

            var newEntity = await _RepositoryManagerRole.UpdateNowAsync(entity);

            var managerList = _RepositoryAppManager.Where(c => c.ManageRoleId == newEntity.Entity.Id.ToString() && !c.DeleteFlag).ToList();

            managerList.ForEach(c =>
            {
                var temp = dto.AppAuths.FirstOrDefault(a => a.AppId == c.AppId);
                if (temp != null)
                {
                    c.ManagerType = temp.ManagerType;
                    c.UpdatedTime = DateTimeOffset.Now;
                }
                else
                {
                    c.DeleteFlag = true;
                    c.UpdatedTime = DateTimeOffset.Now;
                }
            });

            await _RepositoryAppManager.UpdateNowAsync(managerList);

            //新增的
            var list = new List<AppManager>();
            foreach (var item in dto.AppAuths)
            {
                if (!managerList.Any(c => c.AppId == item.AppId))
                {
                    list.Add(new AppManager
                    {
                        Id = Guid.NewGuid(),
                        CreatedTime = DateTimeOffset.Now,
                        UpdatedTime = DateTimeOffset.Now,
                        DeleteFlag = false,
                        ManageRoleId = newEntity.Entity.Id.ToString(),
                        AppId = item.AppId,
                        ManagerType = item.ManagerType
                    });
                }
            }

            await _RepositoryAppManager.InsertNowAsync(list);

            return true;
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRole(List<string> ids)
        {
            var roles = await _RepositoryManagerRole.Where(c => ids.Contains(c.Id.ToString())).ToListAsync();
            roles.ForEach(c =>
            {
                c.DeleteFlag = true;
                c.UpdatedTime = DateTimeOffset.Now;
            });
            await _RepositoryManagerRole.UpdateNowAsync(roles);

            return true;
        }

        #endregion


        #region 应用日志

        /// <summary>
        /// 获取日志更新详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AppLogDetailDto> GetAppLogDetail(string id)
        {
            var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
            var log = await client.GetAppLogDetailAsync(new AppLogDetailRequest { Id = id });

            var result = new AppLogDetailDto()
            {
                Id = log.Id,
                AppIcon = log.AppIcon,
                AppTilte = log.AppTitle,
                Content = log.Content,
                Title = log.Title,
                UpdateTime = log.UpdateTime,
                Version = log.Version,
            };

            return result;
        }

        /// <summary>
        /// 分页获取应用更新日志
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<AppLogDto>> GetAppLogs(int pageIndex, int pageSize)
        {
            var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
            var list = await client.GetAppLogAsync(new Open.AppLogRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                InfoType = 1
            });

            var result = new PagedList<AppLogDto> { TotalCount = 0 };

            if (list != null)
            {
                result.TotalCount = list.TotalCount;
                result.Items = list.AppLogList.Select(item => new AppLogDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    ReleaseTime = item.ReleaseTime,
                    Content = item.Content
                });
            }

            return result;
        }


        #endregion

        #region 内部方法

        /// <summary>
        /// 当前登录用户是否是超级管理员
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsSuperManagerAsync()
        {
            var userRole = await _RepositoryUserRole.DetachedEntities.FirstOrDefaultAsync(c => !c.DeleteFlag && c.UserKey == UserKey);
            if (userRole == null) return false;

            return userRole.IsSuper;
        }

        /// <summary>
        /// 获取当前登录用户所有有权限的应用
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetCurrentUserAllAppsAsync()
        {
            var result = new List<string>();

            var userRole = await _RepositoryUserRole.DetachedEntities.FirstOrDefaultAsync(c => !c.DeleteFlag && c.UserKey == UserKey);
            if (userRole == null) return result;
            if (userRole.ManagerRoleIds.IsEmptyOrWhiteSpace()) return result;

            var roleArray = userRole.ManagerRoleIds.Split(',');
            foreach (var item in roleArray)
            {
                var tempAppManagers = await _RepositoryAppManager.DetachedEntities.Where(c => !c.DeleteFlag && c.ManageRoleId.ToString() == item)
                                                                            .Select(c => c.AppId).ToListAsync();
                if (tempAppManagers != null)
                {
                    result.AddRange(tempAppManagers);
                }
            }

            return result.Distinct().ToList();
        }

        #endregion


        #region 应用列表，详情

        /// <summary>
        /// 获取应用详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onlyAppInfo"></param>
        /// <returns></returns>
        public async Task<AppDetailDto> GetAppDetail(string id, bool onlyAppInfo = false)
        {
            var result = new AppDetailDto();

            var appListsTask = GetAllApp();
            var terminalTypeTask = _GeneralService.GetTerminalType();
            var appTypeTask = _GeneralService.GetAppType();

            await Task.WhenAll(appListsTask, terminalTypeTask, appTypeTask);

            var appLists = appListsTask.Result;
            var detailInfo = appLists.FirstOrDefault(c => c.AppId == id);

            if (detailInfo != null)
            {
                result = new AppDetailDto
                {
                    Id = detailInfo.AppId,
                    AppName = detailInfo.AppName,
                    AppNewName = detailInfo.AppNewName,
                    CurrentVersion = detailInfo.CurrentVersion,
                    AppType = detailInfo.AppType,
                    BackendUrl = detailInfo.BackendUrl,
                    Content = detailInfo.Content,
                    AppExplain = detailInfo.AppExplain,
                    Developer = detailInfo.Developer,
                    ExpireDate = detailInfo.ExpireDate.ToShortDateTimeString(),
                    FrontUrl = detailInfo.FrontUrl,
                    Price = detailInfo.Price,
                    PurchaseType = detailInfo.PurchaseType,
                    ShowPurchaseType = detailInfo.PurchaseTypeName,
                    SceneType = detailInfo.SceneType,
                    ShowStatus = detailInfo.ShowStatus,
                    Status = detailInfo.Status,
                    IsThirdApp = detailInfo.IsThirdApp,
                    Terminal = detailInfo.Terminal,
                    AppIcon = detailInfo.AppIcon
                };
                var terminalType = terminalTypeTask.Result;
                var appType = appTypeTask.Result;
                if (result.AppType.IsNotEmptyOrWhiteSpace())
                {
                    result.AppType = string.Join(';', appType.Where(c => result.AppType.Split(',').Contains(c.Id)).Select(c => c.Name));
                }
                if (result.Terminal.IsNotEmptyOrWhiteSpace())
                {
                    result.Terminal = string.Join(';', terminalType.Where(c => result.Terminal.Split(',').Contains(c.Id)).Select(c => c.Name));
                }
            }
            //只要应用详情，不需要权限信息
            if (onlyAppInfo) return result;

            //管理权限
            var appManagerList = await _RepositoryAppManager.DetachedEntities.Where(c => c.AppId == id && !c.DeleteFlag)
                                    .Distinct().Select(c => c.ManageRoleId).ToListAsync();
            var managerRoleList = await _RepositoryManagerRole.DetachedEntities.Where(c => appManagerList.Contains(c.Id.ToString()) && !c.DeleteFlag)
                                    .ToListAsync();
            result.ManagerAuth = managerRoleList.Select(c => new AuthInfo { AuthType = 1, AuthName = c.Name }).ToList();

            //使用权限
            var appUserList = await _RepositoryAppUser.DetachedEntities.Where(c => c.AppId == id && !c.DeleteFlag)
                                                .Select(c => new { c.UserSetId, c.UserSetType }).ToListAsync();
            var userTypeTask = _GeneralService.GetUserTypeList();
            var userGroupTask = _GeneralService.GetUserGroupList();
            await Task.WhenAll(userTypeTask, userGroupTask);

            result.UseAuth = new List<AuthInfo>();
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
                        result.UseAuth.Add(new AuthInfo { AuthType = 2, AuthName = authName });
                    }
                }
            }
            result.UseAuth = result.UseAuth.Distinct().ToList();

            return result;
        }

        /// <summary>
        /// 获取应用详情，通过routecode
        /// </summary>
        /// <param name="routeCode"></param>
        /// <returns></returns>
        public async Task<AppDetailDto> GetAppDetailByCode(string routeCode)
        {
            var appLists = await GetAllApp();
            var detailInfo = appLists.FirstOrDefault(c => c.RouteCode == routeCode);

            if (detailInfo == null)
            {
                return new AppDetailDto();
            }

            var result = new AppDetailDto
            {
                Id = detailInfo.AppId,
                AppName = detailInfo.AppName,
                AppNewName = detailInfo.AppNewName,
                CurrentVersion = detailInfo.CurrentVersion,
                AppType = detailInfo.AppType,
                AppIcon = detailInfo.AppIcon,
                BackendUrl = detailInfo.BackendUrl,
                Content = detailInfo.Content,
                AppExplain = detailInfo.AppExplain,
                Developer = detailInfo.Developer,
                ExpireDate = detailInfo.ExpireDate.ToShortDateTimeString(),
                FrontUrl = detailInfo.FrontUrl,
                Price = detailInfo.Price,
                PurchaseType = detailInfo.PurchaseType,
                SceneType = detailInfo.SceneType,
                ShowStatus = detailInfo.ShowStatus,
                Status = detailInfo.Status,
                Terminal = detailInfo.Terminal,
                RouteCode = detailInfo.RouteCode,
            };

            return result;
        }

        /// <summary>
        /// 分页获取应用列表
        /// </summary>
        /// <param name="key">检索关键词</param>
        /// <param name="appType">应用类型</param>
        /// <param name="purchaseType">采购类型</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<AppListDto>> GetAppSearchList(string key, string appType, string purchaseType, int pageIndex, int pageSize)
        {
            //获取所有应用
            var list = await GetAllApp();

            var result = new PagedList<AppListDto>();

            if (!await IsSuperManagerAsync())
            {
                var myApps = await GetCurrentUserAllAppsAsync();
                list = list.Where(c => myApps.Contains(c.AppId)).ToList();
            }

            if (key.IsNotEmptyOrWhiteSpace())
            {
                list = list.Where(c => c.AppName.Contains(key) || c.Developer.Contains(key) || c.Content.Contains(key)).ToList();
            }

            if (appType.IsNotEmptyOrWhiteSpace())
            {
                list = list.Where(c => c.AppType.Split(',').Contains(appType)).ToList();
            }

            if (purchaseType.IsNotEmptyOrWhiteSpace())
            {
                list = list.Where(c => c.PurchaseType == purchaseType).ToList();
            }

            result.TotalCount = list.Count;
            var tempList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var resultList = new List<AppListDto>();
            foreach (var item in tempList)
            {
                var isFrontApp = !item.IsThirdApp && item.AppEntranceList != null && item.AppEntranceList.Any(c => c.IsSystem && c.IsDefault && c.UseScene == 1);
                var isBackApp = !item.IsThirdApp && item.AppEntranceList != null && item.AppEntranceList.Any(c => c.IsSystem && c.IsDefault && c.UseScene == 2);

                resultList.Add(new AppListDto
                {
                    AppId = item.AppId,
                    AppName = item.AppNewName,
                    AppIcon = item.AppIcon,
                    FrontUrl = isFrontApp ? item.FrontUrl : (item.IsThirdApp ? item.FrontUrl : ""),
                    BackendUrl = isBackApp ? item.BackendUrl : "",
                    IsThirdApp = item.IsThirdApp,
                    Terminal = item.Terminal
                });
            }

            result.Items = resultList;

            return result;
        }

        /// <summary>
        /// 分页获取即将到期应用
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<AppExpireDto>> GetAppExpire(int pageIndex, int pageSize)
        {
            var list = await GetAllApp();
            var result = new PagedList<AppExpireDto>();

            //到期时间小于等于3个月
            var expireList = list.Where(c => DateTimeHelper.ToResult(c.ExpireDate, DateTime.Now.ToString(), DiffResultFormat.mm)[0] <= 3);
            result.TotalCount = expireList.Count();

            var tempList = expireList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            result.Items = tempList.Select(c => new AppExpireDto
            {
                AppId = c.AppId,
                AppName = c.AppNewName,
                AppIcon = c.AppIcon,
                ExpireDate = c.ExpireDate,
                IsMyApp = HasAuthorityAsync(c.AppId).Result
            });

            return result;
        }

        /// <summary>
        /// 当前登录用户对指定应用是否有权限
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        private async Task<bool> HasAuthorityAsync(string appId)
        {
            var userRole = await _RepositoryUserRole.DetachedEntities.FirstOrDefaultAsync(c => !c.DeleteFlag && c.UserKey == UserKey);
            if (userRole == null) return false;

            if (userRole.IsSuper) return true;

            if (userRole.ManagerRoleIds.IsEmptyOrWhiteSpace()) return false;

            var roleArray = userRole.ManagerRoleIds.Split(',');
            var result = await _RepositoryAppManager.DetachedEntities.AnyAsync(c => !c.DeleteFlag && roleArray.Contains(c.ManageRoleId) && c.AppId == appId);

            return result;
        }


        #endregion


        #region 付费应用推荐

        /// <summary>
        /// 分页获取付费推荐应用
        /// </summary>
        /// <param name="appType">应用类型</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<AppPayDto>> GetPayAppList(string appType, int pageIndex, int pageSize)
        {
            var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
            var list = await client.GetPayAppListAsync(new PayAppRequest
            {
                AppType = appType ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize
            });

            var result = new PagedList<AppPayDto>
            {
                TotalCount = list.TotalCount,
                Items = list.PayAppList.Select(c => new AppPayDto
                {
                    Id = c.Id,
                    AppName = c.AppName,
                    IsFreeTry = c.IsFreeTry,
                    AppIcon = c.AppIcon,
                    Star = c.Star,
                    Content = c.Content,
                    Price = c.Price.ToString(),
                    Developer = c.Developer
                })
            };

            return result;
        }

        /// <summary>
        /// 应用信息管理，获取付费推荐应用
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppPayDto>> GetPayAppForIndex()
        {
            var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
            var list = await client.GetPayAppListAsync(new PayAppRequest
            {
                AppType = "",
                PageIndex = 1,
                PageSize = 100
            });
            var result = new List<AppPayDto>();
            if (list != null && list.PayAppList != null)
            {
                foreach (var c in list.PayAppList.Where(c => c.IsFreeTry))
                {
                    if (result.Count == 3)
                    {
                        break;
                    }
                    result.Add(new AppPayDto
                    {
                        Id = c.Id,
                        AppName = c.AppName,
                        IsFreeTry = c.IsFreeTry,
                        AppIcon = c.AppIcon,
                        Star = c.Star,
                        Content = c.Content,
                        Price = c.Price.ToString(),
                        Developer = c.Developer
                    });
                }
            }

            return result;
        }

        #endregion



        #region 应用订单

        /// <summary>
        /// 分页获取应用订单
        /// </summary>
        /// <param name="key"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<OrderInfoDto>> GetOrderList(string key, string status, int pageIndex, int pageSize)
        {
            var client = _GrpcClientResolver.EnsureClient<SmartLibrary.Open.AppGrpcService.AppGrpcServiceClient>();
            var list = await client.GetOrderListAsync(new OrderListRequest
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                SearchKey = key ?? "",
                Status = status.ToInt(),
            });

            var result = new PagedList<OrderInfoDto>
            {
                TotalCount = list.TotalCount,
                Items = list.OrderList.Select(x => new OrderInfoDto
                {
                    Id = x.Id,
                    AppName = x.AppName,
                    Developer = x.Developer,
                    Contacts = x.Contacts,
                    CommitDate = x.CommitDate,
                    ExpireDate = x.ExpireDate,
                    Phone = x.Phone,
                    Remark = x.Remark,
                    AuthType = x.AuthType,
                    ShowAuthType = x.ShowAuthType,
                    OpenType = x.OpenType,
                    ShowOpenType = x.ShowOpenType,
                    Status = x.Status,
                    ShowStatus = x.ShowStatus,
                })
            };

            return result;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CancelOrder(string id)
        {
            var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
            var result = await client.CancelOderAsync(new CancelOrderRequest { Id = id });

            if (result.ErrorMsg.IsNotEmptyOrWhiteSpace())
            {
                throw Oops.Oh(result.ErrorMsg).StatusCode(499);
            }

            return result.IsSuccess;
        }

        /// <summary>
        /// 应用续订，延期，免费试用，预约采购
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> AppAction(AppActionRequest dto)
        {
            bool result;
            var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
            var grpcResult = await client.AppActionAsync(new AppActionReuqest
            {
                ActionType = dto.ActionType,
                AppId = dto.AppId,
                Contacts = dto.Contacts ?? "",
                Phone = dto.Phone ?? "",
                TimeNum = dto.TimeNum,
            });
            if (grpcResult.ErrorMsg.IsNotEmptyOrWhiteSpace())
            {
                throw Oops.Oh(grpcResult.ErrorMsg).StatusCode(499);
            }
            result = grpcResult.IsSuccess;

            return result;
        }

        /// <summary>
        /// 应用启用和停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> AppChangeStatus(AppStatusChangeDto dto)
        {
            var result = "";
            if (dto.IsThirdApp)
            {
                var appInfo = await _RepositoryThirdApplication.FirstOrDefaultAsync(c => c.Id.ToString() == dto.AppId);
                if (appInfo != null)
                {
                    appInfo.Status = dto.ActionType == 1 ? 1 : 0;
                    appInfo.UpdatedTime = DateTimeOffset.Now;
                    await _RepositoryThirdApplication.UpdateNowAsync(appInfo);
                }
            }
            else
            {
                var client = _GrpcClientResolver.EnsureClient<AppGrpcServiceClient>();
                var grpcResult = await client.AppActionAsync(new AppActionReuqest
                {
                    ActionType = dto.ActionType == 1 ? 5 : 6,
                    AppId = dto.AppId,
                    Contacts = "",
                    Phone = "",
                    TimeNum = 0,
                });
                result = grpcResult.ErrorMsg;
            }

            return result;
        }

        /// <summary>
        /// 应用修改名称
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> AppReName(AppReNameDto dto)
        {
            var result = true;
            var info = await _RepositoryAppReName.FirstOrDefaultAsync(c => !c.DeleteFlag && c.AppId == dto.AppId);
            if (info != null)
            {
                if (string.IsNullOrWhiteSpace(dto.NewName))
                {
                    info.DeleteFlag = true;
                    info.UpdatedTime = DateTimeOffset.Now;
                    await _RepositoryAppReName.UpdateIncludeNowAsync(info, new string[] { nameof(info.AppNewName), nameof(info.UpdatedTime) });
                    return result;
                }
                //修改改名记录
                info.AppNewName = dto.NewName;
                info.UpdatedTime = DateTimeOffset.Now;
                await _RepositoryAppReName.UpdateIncludeNowAsync(info, new string[] { nameof(info.AppNewName), nameof(info.UpdatedTime) });
            }
            else
            {
                //新增改名记录
                if (!string.IsNullOrWhiteSpace(dto.NewName))
                {
                    await _RepositoryAppReName.InsertNowAsync(new AppReName
                    {
                        Id = Guid.NewGuid(),
                        CreatedTime = DateTimeOffset.Now,
                        UpdatedTime = DateTimeOffset.Now,
                        DeleteFlag = false,
                        AppId = dto.AppId,
                        AppNewName = dto.NewName,
                        UserKey = UserKey
                    });
                }
            }

            await _DistributedCache.RemoveAsync((Owner ?? "") + CacheKey.AllApps);
            return result;
        }

        #endregion



        #region 三方应用

        /// <summary>
        /// 分页获取三方应用列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<ThirdAppInfoDto>> GetThirdAppList(string key, int pageIndex, int pageSize)
        {
            var list = await _RepositoryThirdApplication.Where(c => !c.DeleteFlag &&
                        (!key.IsNotEmptyOrWhiteSpace() || c.AppName.Contains(key) || c.Developer.Contains(key)))
                        .OrderByDescending(c => c.CreatedTime)
                        .Select(c => new ThirdAppInfoDto
                        {
                            Id = c.Id.ToString(),
                            AppName = c.AppName,
                            CreateTime = c.CreatedTime.ToString("yyyy-MM-dd"),
                            Developer = c.Developer,
                            Terminal = c.Terminal,
                            AppType = c.AppType
                        })
                        .ToPagedListAsync(pageIndex, pageSize);

            var appTypeList = await _GeneralService.GetAppType();

            foreach (var item in list.Items)
            {
                if (!string.IsNullOrWhiteSpace(item.Terminal))
                {
                    item.Terminal = string.Join(',', item.Terminal.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                                  .Select(c => EnumTools.GetName(EnumTools.GetEnum<TerminalEnum>(c))));
                }
                if (!string.IsNullOrWhiteSpace(item.AppType))
                {
                    item.AppType = string.Join(',', item.AppType.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                                .Select(c => appTypeList.FirstOrDefault(x => x.Id == c)?.Name));
                }

            }

            return list;
        }

        /// <summary>
        /// 批量删除三方应用
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteThirdApp(List<string> ids)
        {
            var apps = await _RepositoryThirdApplication.Where(c => ids.Contains(c.Id.ToString())).ToListAsync();
            apps.ForEach(c =>
            {
                c.DeleteFlag = true;
                c.UpdatedTime = DateTimeOffset.Now;
            });
            await _RepositoryThirdApplication.UpdateNowAsync(apps);
            return true;
        }

        /// <summary>
        /// 获取三方应用详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ThirdAppDetailDto> GetThirdAppDetail(string id)
        {
            var result = new ThirdAppDetailDto();

            var userTypeListTask = _GeneralService.GetUserTypeList();
            var userGroupListTask = _GeneralService.GetUserGroupList();

            await Task.WhenAll(userTypeListTask, userGroupListTask);

            var info = await _RepositoryThirdApplication.DetachedEntities.FirstOrDefaultAsync(c => c.Id.ToString() == id);
            if (info != null)
            {
                var userTypeList = userTypeListTask.Result;
                var userGroupList = userGroupListTask.Result;

                result = new ThirdAppDetailDto
                {
                    Id = info.Id.ToString(),
                    AppName = info.AppName,
                    AppIcon = info.AppIcon,
                    AppIntroduction = info.AppIntroduction,
                    AppExplain = info.AppExplain,
                    Contacts = info.Contacts,
                    Developer = info.Developer,
                    FrontUrl = info.FrontUrl,
                    Terminal = info.Terminal?.Split(',').ToList(),
                    AppType = info.AppType?.Split(',').ToList(),
                    AuthInfos = new List<ThirdAppDetailAuthInfo>()
                };

                var appUser = await _RepositoryAppUser.DetachedEntities.Where(c => c.AppId == id && !c.DeleteFlag).ToListAsync();
                appUser?.ForEach(c =>
                {
                    result.AuthInfos.Add(new ThirdAppDetailAuthInfo
                    {
                        Id = c.UserSetId,
                        UserSetType = c.UserSetType,
                        UserSetId = c.UserSetId,
                        UserSetName = c.UserSetType == (int)UserSetTypeEnum.用户类型
                                                        ? userTypeList.FirstOrDefault(x => x.Id == c.UserSetId)?.Name
                                                        : userGroupList.FirstOrDefault(x => x.Id == c.UserSetId)?.Name
                    });
                });
            }

            return result;
        }

        /// <summary>
        /// 新增/修改三方应用信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateThirdAppInfo(ThirdAppInfoUpdateDto dto)
        {
            var appInfo = await _RepositoryThirdApplication.FirstOrDefaultAsync(c => c.Id.ToString() == dto.Id && !c.DeleteFlag);
            if (appInfo is null)
            {
                appInfo = new ThirdApplication
                {
                    Id = Guid.NewGuid(),
                    DeleteFlag = false,
                    CreatedTime = DateTimeOffset.Now,
                    UpdatedTime = DateTimeOffset.Now,
                    AppName = dto.AppName,
                    AppIcon = dto.AppIcon,
                    AppIntroduction = dto.AppIntroduction,
                    AppExplain = dto.AppExplain,
                    Contacts = dto.Contacts,
                    Developer = dto.Developer,
                    FrontUrl = dto.FrontUrl,
                    AppType = string.Join(',', dto.AppType),
                    Terminal = string.Join(',', dto.Terminal),
                    Status = (int)AppStatusEnum.Normal
                };
                await _RepositoryThirdApplication.InsertAsync(appInfo);
            }
            else
            {
                //修改
                appInfo.UpdatedTime = DateTimeOffset.Now;
                appInfo.AppName = dto.AppName;
                appInfo.AppIcon = dto.AppIcon;
                appInfo.AppIntroduction = dto.AppIntroduction;
                appInfo.AppExplain = dto.AppExplain;
                appInfo.Contacts = dto.Contacts;
                appInfo.Developer = dto.Developer;
                appInfo.FrontUrl = dto.FrontUrl;
                appInfo.AppType = string.Join(',', dto.AppType);
                appInfo.Terminal = string.Join(',', dto.Terminal);

                await _RepositoryThirdApplication.UpdateAsync(appInfo);

                //删除授权信息
                var existAppUsers = await _RepositoryAppUser.Where(c => c.AppId == appInfo.Id.ToString() && !c.DeleteFlag).ToListAsync();
                if (existAppUsers != null && existAppUsers.Any())
                {
                    existAppUsers.ForEach(c =>
                    {
                        c.DeleteFlag = true;
                        c.UpdatedTime = DateTimeOffset.Now;
                    });
                    await _RepositoryAppUser.UpdateAsync(existAppUsers);
                }
            }
            //添加授权信息
            var appUsers = new List<AppUser>();
            if (dto.AuthInfos != null && dto.AuthInfos.Any())
            {
                foreach (var item in dto.AuthInfos)
                {
                    appUsers.Add(new AppUser
                    {
                        Id = Guid.NewGuid(),
                        DeleteFlag = false,
                        CreatedTime = DateTimeOffset.Now,
                        UpdatedTime = DateTimeOffset.Now,
                        AppId = appInfo.Id.ToString(),
                        UserSetType = item.UserSetType,
                        UserSetId = item.Id,
                    });
                }
            }
            if (appUsers.Any())
            {
                await _RepositoryAppUser.InsertAsync(appUsers);
            }
            //删除缓存
            await _EasyCachingProvider.RemoveAsync(Owner + CacheKey.AllApps);

            return true;
        }

        #endregion


        #region 导航栏目

        /// <summary>
        /// 分页获取导航栏目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<NavgationItemDto>> GetNavgationItems(int pageIndex, int pageSize)
        {
            var allApps = await GetAllApp();

            var list = await _RepositoryNavigation.Where(c => !c.DeleteFlag).OrderByDescending(c => c.CreatedTime)
                        .Select(c => new NavgationItemDto
                        {
                            NavId = c.Id.ToString(),
                            NavName = c.Name,
                            CreateTime = c.CreatedTime.ToString("yyyy-MM-dd"),
                        }).ToPagedListAsync(pageIndex, pageSize);

            if (list != null && list.Items != null)
            {
                foreach (var item in list.Items)
                {
                    var appNavs = await _RepositoryAppNavigation.Where(c => c.NavigationId == item.NavId && c.DeleteFlag == false).ToListAsync();
                    if (appNavs != null && appNavs.Any())
                    {
                        item.RelationAppCount = appNavs.Count;
                        //取应用名称
                        var appNavIds = appNavs.Select(c => c.AppId);
                        var appNames = allApps.Where(c => appNavIds.Contains(c.AppId)).Select(c => c.AppNewName);
                        item.RelationApp = string.Join(',', appNames);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 删除导航栏目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteNavgation(Guid id)
        {
            var info = await _RepositoryNavigation.FirstOrDefaultAsync(c => c.Id == id);
            if (info == null) throw Oops.Oh("该栏目不存在").StatusCode(499);

            info.DeleteFlag = true;
            info.UpdatedTime = DateTimeOffset.Now;
            // 删除栏目
            await _RepositoryNavigation.UpdateIncludeAsync(info, new string[] { nameof(info.DeleteFlag), nameof(info.UpdatedTime) });

            //删除栏目应用
            var appNavs = await _RepositoryAppNavigation.Where(c => c.AppId == id.ToString()).ToListAsync();
            appNavs?.ForEach(async c =>
            {
                c.DeleteFlag = true;
                c.UpdatedTime = DateTimeOffset.Now;
                await _RepositoryAppNavigation.UpdateIncludeAsync(c, new string[] { nameof(c.DeleteFlag), nameof(c.UpdatedTime) });
            });

            //删除 AppColumnInfo
            var appColumnInfo = await _RepositoryAppColumnInfo.FirstOrDefaultAsync(c => c.ColumnId == id.ToString() && c.DeleteFlag == false && c.AppRouteCode == "appcenter");
            if (appColumnInfo != null)
            {
                appColumnInfo.DeleteFlag = true;
                appColumnInfo.UpdatedTime = DateTimeOffset.Now;
                await _RepositoryAppColumnInfo.UpdateIncludeAsync(appColumnInfo, new string[] { nameof(appColumnInfo.DeleteFlag), nameof(appColumnInfo.UpdatedTime) });
            }
            return true;
        }

        /// <summary>
        /// 获取导航栏目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NavigationItemInfoDto> GetNavgationDetail(Guid id)
        {
            var result = new NavigationItemInfoDto();

            var item = await _RepositoryNavigation.FirstOrDefaultAsync(c => c.Id == id);
            if (item != null)
            {
                result.NavId = item.Id.ToString();
                result.NavName = item.Name;
                result.IsPrivateFirst = item.PrivateFirst;
            }

            result.AppInfos = new List<NavigationItemInfoAppInfo>();

            var navApps = await _RepositoryAppNavigation.Where(c => c.NavigationId == id.ToString() && !c.DeleteFlag)
                                                        .OrderBy(c => c.OrderIndex).ToListAsync();

            navApps?.ForEach(c =>
            {
                var appInfo = GetAppDetail(c.AppId.ToString(), true).Result;
                result.AppInfos.Add(new NavigationItemInfoAppInfo
                {
                    AppId = c.AppId.ToString(),
                    AppName = appInfo?.AppNewName,
                    AppType = appInfo?.AppType,
                    Developer = appInfo?.Developer,
                    OrderIndex = c.OrderIndex
                });
            });

            return result;
        }

        /// <summary>
        /// 修改导航栏目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateNavigationItem(NavigationItemUpdateDto dto)
        {
            if (dto == null) return false;

            var allApps = await GetAllApp();

            //修改
            if (dto.NavId.IsNotEmptyOrWhiteSpace())
            {
                var navInfo = await _RepositoryNavigation.FirstOrDefaultAsync(c => c.Id.ToString() == dto.NavId && c.DeleteFlag == false);
                if (navInfo == null) return false;

                navInfo.UpdatedTime = DateTimeOffset.Now;
                navInfo.Name = dto.NavName;
                navInfo.PrivateFirst = dto.IsPrivateFirst;
                await _RepositoryNavigation.UpdateIncludeAsync(navInfo, new[] { nameof(navInfo.UpdatedTime), nameof(navInfo.Name), nameof(navInfo.PrivateFirst) });

                //修改关联应用
                var navApps = await _RepositoryAppNavigation.Where(c => c.NavigationId == navInfo.Id.ToString()).ToListAsync();
                navApps?.ForEach(async c =>
                {
                    c.UpdatedTime = DateTimeOffset.Now;
                    c.DeleteFlag = true;
                    await _RepositoryAppNavigation.UpdateIncludeAsync(c, new[] { nameof(c.UpdatedTime), nameof(c.DeleteFlag) });
                });
                dto.AppInfos?.ForEach(c =>
                {
                    _RepositoryAppNavigation.InsertAsync(new AppNavigation
                    {
                        Id = Guid.NewGuid(),
                        CreatedTime = DateTimeOffset.Now,
                        UpdatedTime = DateTimeOffset.Now,
                        DeleteFlag = false,
                        AppId = c.AppId,
                        NavigationId = navInfo.Id.ToString(),
                        OrderIndex = c.OrderIndex
                    });
                });

                // 修改AppColumnInfo表
                var appColumnInfo = await _RepositoryAppColumnInfo.FirstOrDefaultAsync(c => c.AppRouteCode == "appcenter" && c.DeleteFlag == false && c.ColumnId == dto.NavId);
                if (appColumnInfo != null)
                {
                    appColumnInfo.UpdatedTime = DateTimeOffset.Now;
                    appColumnInfo.ColumnName = dto.NavName;
                    await _RepositoryAppColumnInfo.UpdateIncludeAsync(appColumnInfo, new[] { nameof(appColumnInfo.UpdatedTime), nameof(appColumnInfo.ColumnName) });
                }
            }
            else
            {
                //新增
                var newNav = await _RepositoryNavigation.InsertNowAsync(new Navigation
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTimeOffset.Now,
                    UpdatedTime = DateTimeOffset.Now,
                    DeleteFlag = false,
                    PrivateFirst = dto.IsPrivateFirst,
                    Name = dto.NavName,
                });
                dto.AppInfos?.ForEach(async c =>
                {
                    await _RepositoryAppNavigation.InsertAsync(new AppNavigation
                    {
                        Id = Guid.NewGuid(),
                        CreatedTime = DateTimeOffset.Now,
                        UpdatedTime = DateTimeOffset.Now,
                        DeleteFlag = false,
                        AppId = c.AppId,
                        NavigationId = newNav.Entity.Id.ToString(),
                        OrderIndex = c.OrderIndex
                    });
                });

                // 添加到AppColumnInfo表
                var appCenterApp = allApps.FirstOrDefault(c => c.RouteCode == "appcenter");
                if (appCenterApp != null)
                {
                    await _RepositoryAppColumnInfo.InsertAsync(new AppColumnInfo
                    {
                        Id = Guid.NewGuid(),
                        CreatedTime = DateTimeOffset.Now,
                        UpdatedTime = DateTimeOffset.Now,
                        DeleteFlag = false,
                        AppRouteCode = "appcenter",
                        AppId = appCenterApp.AppId,
                        ColumnName = dto.NavName,
                        ColumnId = newNav.Entity.Id.ToString(),
                        ColumnCreateTime = newNav.Entity.CreatedTime,
                        VisitUrl = "/admin_directoryMessage"
                    });
                }
            }

            return true;
        }

        /// <summary>
        /// 导航栏目设置选择应用
        /// </summary>
        /// <returns></returns>
        public async Task<List<AppListDto>> GetNavigationApp()
        {
            var result = new List<AppListDto>();

            var allApps = await GetAllApp();
            if (allApps != null && allApps.Any())
            {
                //查找所有前台应用和三方应用
                result.AddRange(allApps.FindAll(c => (c.AppEntranceList != null && c.AppEntranceList.Any(x => x.IsDefault && x.IsSystem && x.UseScene == 1)) || c.IsThirdApp));
            }

            return result;
        }

        #endregion


        #region 管理权限设置

        /// <summary>
        /// 分页获取管理员应用权限列表
        /// </summary>
        /// <param name="key">姓名/卡号</param>
        /// <param name="status">账号状态</param>
        /// <param name="type">账号类型</param>
        /// <param name="role">授权角色</param>
        /// <param name="expire">有效期截止时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<ManagerInfoDto>> GetManagerList(string key, string status, string type, string role, string expire, int pageIndex, int pageSize)
        {
            var client = _GrpcClientResolver.EnsureClient<UserGrpcServiceClient>();
            var list = await client.GetManagerListAsync(new Google.Protobuf.WellKnownTypes.Empty());

            var result = new PagedList<ManagerInfoDto> { TotalCount = 0, Items = new List<ManagerInfoDto>() };
            if (list != null && list.Items != null)
            {
                var userKeys = list.Items.Select(c => c.Key);
                var userRoles = await _RepositoryUserRole.DetachedEntities.Where(c => userKeys.Contains(c.UserKey) && !c.DeleteFlag).ToListAsync();
                var allRole = await GetAllRole();

                var tempResultList = new List<ManagerInfoDto>();
                foreach (var item in list.Items)
                {
                    var userRole = userRoles.FirstOrDefault(c => c.UserKey == item.Key);
                    var names = new List<string>();
                    if (userRole != null && !string.IsNullOrWhiteSpace(userRole.ManagerRoleIds))
                    {
                        names = allRole.Where(c => userRole.ManagerRoleIds.Split(',').Contains(c.Id.ToString())).Select(c => c.Name).ToList();
                    }

                    tempResultList.Add(new ManagerInfoDto
                    {
                        UserKey = item.Key,
                        UserName = item.Name,
                        LoginName = item.CardNo,
                        AccountType = item.Usage.ToString(),
                        ShowAccountType = item.Usage == 1 ? "临时" : "馆员",
                        ExpireDate = item.CardExpireDate?.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        LastLoginDate = item.LastLoginTime?.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        Card = item.CardNo,
                        ShowAccountStatus = EnumTools.GetName(EnumTools.GetEnum<EnumCardStatus>(item.CardStatus.GetValueOrDefault(1).ToString())),
                        AccountStatus = item.CardStatus.GetValueOrDefault(1),
                        ManagerRoleString = string.Join(',', names),
                        ManagerRoleIds = userRole?.ManagerRoleIds
                    });
                }

                result.Items = tempResultList;
            }

            #region 条件过滤

            //姓名，卡号
            if (!string.IsNullOrWhiteSpace(key))
            {
                result.Items = result.Items.Where(c => c.UserName.Contains(key) || c.LoginName.Contains(key) || c.Card.Contains(key));
            }
            //状态
            if (!string.IsNullOrWhiteSpace(status))
            {
                result.Items = result.Items.Where(c => c.AccountStatus.ToString() == status);
            }
            //类型
            if (!string.IsNullOrWhiteSpace(type))
            {
                result.Items = result.Items.Where(c => c.AccountType.ToString() == type);
            }
            //授权角色
            if (!string.IsNullOrWhiteSpace(role))
            {
                result.Items = result.Items.Where(c => !string.IsNullOrWhiteSpace(c.ManagerRoleIds) && c.ManagerRoleIds.Split(',').Contains(role));
            }
            //有效截止时间
            if (!string.IsNullOrWhiteSpace(expire))
            {
                result.Items = result.Items.Where(c => c.ExpireDate.ToDateTime() > expire.ToDateTime());
            }

            #endregion

            //分页
            result.TotalCount = result.Items.Count();
            result.Items = result.Items.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return result;
        }

        /// <summary>
        /// 获取管理员详情
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<ManagerInfoDetailDto> GetManagerInfoDetail(string userKey)
        {
            var client = _GrpcClientResolver.EnsureClient<UserGrpcServiceClient>();
            var userInfo = await client.GetUserByKeyAsync(new Google.Protobuf.WellKnownTypes.StringValue() { Value = userKey });

            var result = new ManagerInfoDetailDto
            {
                UserKey = userInfo.Key,
                UserName = userInfo.Name,
                LoginName = userInfo.CardNo,
                AccountType = userInfo.Type,
                ShowAccountType = userInfo.TypeName,
                ExpireDate = userInfo.CardExpireDate?.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss"),
            };

            var temp = await _RepositoryUserRole.DetachedEntities.FirstOrDefaultAsync(c => c.UserKey == userKey && !c.DeleteFlag);
            if (temp != null)
            {
                result.IsSuper = temp.IsSuper;
                result.CheckedRoleIds = temp.ManagerRoleIds.Split(',').ToList();
            }

            return result;
        }

        /// <summary>
        /// 修改管理员权限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> GrantManager(ManagerInfoDetailUpdateDto dto)
        {
            var temp = await _RepositoryUserRole.FirstOrDefaultAsync(c => c.UserKey == dto.UserKey && c.DeleteFlag == false);

            if (temp is null)
            {
                await _RepositoryUserRole.InsertNowAsync(new UserRole
                {
                    Id = Guid.NewGuid(),
                    CreatedTime = DateTimeOffset.Now,
                    UpdatedTime = DateTimeOffset.Now,
                    DeleteFlag = false,
                    IsSuper = dto.IsSuper,
                    UserKey = dto.UserKey,
                    ManagerRoleIds = string.Join(',', dto.CheckedRoleIds)
                });
            }
            else
            {
                temp.IsSuper = dto.IsSuper;
                temp.ManagerRoleIds = string.Join(',', dto.CheckedRoleIds);
                temp.UpdatedTime = DateTimeOffset.Now;

                await _RepositoryUserRole.UpdateNowAsync(temp);
            }

            return true;
        }

        #endregion


        #region 用户权限设置

        /// <summary>
        /// 分页获取用户权限列表-按应用
        /// </summary>
        /// <param name="key">应用名称</param>
        /// <param name="authType">已授权/未授权</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<AuthAppDto>> GetAppUserByApp(string key, int authType, int pageIndex, int pageSize)
        {
            //应用列表
            var appListTask = GetAllApp();
            var userTypeListTask = _GeneralService.GetUserTypeList();
            var userGroupListTask = _GeneralService.GetUserGroupList();

            await Task.WhenAll(appListTask, userTypeListTask, userGroupListTask);

            var appList = appListTask.Result;
            var userTypeList = userTypeListTask.Result;
            var userGroupList = userGroupListTask.Result;

            if (key.IsNotEmptyOrWhiteSpace())
            {
                appList = appList.Where(c => c.AppName.Contains(key)).ToList();
            }
            if (authType > 0)
            {
                var appUserList = await _RepositoryAppUser.DetachedEntities.Select(c => c.AppId.ToString()).ToListAsync();
                //已授权
                if (authType == 1)
                {
                    appList = appList.Where(c => appUserList.Contains(c.AppId)).ToList();
                }
                else
                {
                    appList = appList.Where(c => !appUserList.Contains(c.AppId)).ToList();
                }
            }

            var result = new PagedList<AuthAppDto>
            {
                TotalCount = appList.Count
            };

            appList = appList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var tempList = new List<AuthAppDto>();
            foreach (var item in appList)
            {
                string authUsers = string.Empty;
                List<string> typeIds = new List<string>();
                List<string> groupIds = new List<string>();

                var tempAppUser = await _RepositoryAppUser.DetachedEntities.Where(c => c.AppId == item.AppId && !c.DeleteFlag).ToListAsync();
                if (tempAppUser != null && tempAppUser.Any())
                {
                    var typeList = tempAppUser.Where(c => c.UserSetType == (int)UserSetTypeEnum.用户类型).ToList();
                    var groupList = tempAppUser.Where(c => c.UserSetType == (int)UserSetTypeEnum.用户分组).ToList();

                    typeIds = typeList?.Select(c => c.UserSetId).Distinct().ToList() ?? new List<string>();
                    groupIds = groupList?.Select(c => c.UserSetId).Distinct().ToList() ?? new List<string>();

                    var typeNames = userTypeList.Where(c => typeIds.Contains(c.Id)).Select(c => c.Name);
                    var groupNames = userGroupList.Where(c => groupIds.Contains(c.Id)).Select(c => c.Name);

                    if (typeNames != null && typeNames.Any())
                    {
                        authUsers = string.Join(',', typeNames);
                    }
                    if (groupNames != null && groupNames.Any())
                    {
                        authUsers += string.Join(',', groupNames);
                    }
                }
                tempList.Add(new AuthAppDto
                {
                    AppId = item.AppId,
                    AppName = item.AppNewName,
                    AuthUserIdsByType = typeIds,
                    AuthUserIdsByGroup = groupIds,
                    AuthUsers = authUsers
                });
            }
            result.Items = tempList;

            return result;
        }

        /// <summary>
        /// 分页获取用户权限列表-按用户
        /// </summary>
        /// <param name="key">应用名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedList<AuthUserDto>> GetAppUserByUser(string key, int pageIndex, int pageSize)
        {
            var appListTask = GetAllApp();
            var userTypeListTask = _GeneralService.GetUserTypeList();
            var userGroupListTask = _GeneralService.GetUserGroupList();
            await Task.WhenAll(appListTask, userTypeListTask, userGroupListTask);

            var appList = appListTask.Result;
            var userTypeList = userTypeListTask.Result;
            var userGroupList = userGroupListTask.Result;

            var result = new PagedList<AuthUserDto>();
            var tempResultList = new List<AuthUserDto>();
            //用户类型
            if (userTypeList != null)
            {
                result.TotalCount += userTypeList.Count;
                var typeIdArray = userTypeList.Select(c => c.Id);
                var appUserList = await _RepositoryAppUser.DetachedEntities.Where(c => !c.DeleteFlag && c.UserSetType == (int)UserSetTypeEnum.用户类型 && typeIdArray.Contains(c.UserSetId)).ToListAsync();
                foreach (var item in userTypeList)
                {
                    string authApps = string.Empty;
                    List<string> authAppIds = new();
                    var tempAppUserList = appUserList.FindAll(c => c.UserSetId == item.Id);
                    if (tempAppUserList != null && tempAppUserList.Any())
                    {
                        authAppIds = tempAppUserList.Select(c => c.AppId.ToString()).Distinct().ToList();
                        var tempAppList = appList.Where(c => authAppIds.Contains(c.AppId)).ToList();
                        if (tempAppList != null && tempAppList.Any())
                        {
                            authApps = string.Join(',', tempAppList.Select(c => c.AppNewName));
                        }
                    }
                    tempResultList.Add(new AuthUserDto
                    {
                        UserSetId = item.Id,
                        UserSetName = item.Name,
                        UserSetType = (int)UserSetTypeEnum.用户类型,
                        AuthAppIds = authAppIds,
                        AuthApps = authApps
                    });
                }
            }
            //用户分组
            if (userGroupList != null)
            {
                result.TotalCount += userGroupList.Count;
                var groupIdArray = userGroupList.Select(c => c.Id);
                var appUserList = await _RepositoryAppUser.DetachedEntities.Where(c => !c.DeleteFlag && c.UserSetType == (int)UserSetTypeEnum.用户分组 && groupIdArray.Contains(c.UserSetId)).ToListAsync();
                foreach (var item in userGroupList)
                {
                    List<string> authAppIds = new();
                    string authApps = string.Empty;
                    var tempAppUserList = appUserList.FindAll(c => c.UserSetId == item.Id);
                    if (tempAppUserList != null && tempAppUserList.Any())
                    {
                        authAppIds = tempAppUserList.Select(c => c.AppId.ToString()).Distinct().ToList();
                        var tempAppList = appList.Where(c => authAppIds.Contains(c.AppId)).ToList();
                        if (tempAppList != null && tempAppList.Any())
                        {
                            authApps = string.Join(',', tempAppList.Select(c => c.AppNewName));
                        }
                    }
                    tempResultList.Add(new AuthUserDto
                    {
                        UserSetId = item.Id,
                        UserSetName = item.Name,
                        UserSetType = (int)UserSetTypeEnum.用户分组,
                        AuthAppIds = authAppIds,
                        AuthApps = authApps
                    });
                }
            }
            //应用名称过滤
            if (key.IsNotEmptyOrWhiteSpace())
            {
                tempResultList = tempResultList.Where(c => c.AuthApps.Contains(key)).ToList();
            }

            tempResultList = tempResultList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            result.Items = tempResultList;

            return result;
        }

        /// <summary>
        /// 用户应用授权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAppUser(AppAuthUpdateDto dto)
        {
            if (dto.AppIds is null || !dto.AppIds.Any())
            {
                return false;
            }

            if (dto.OperationType == 1)
            {
                var temp = await _RepositoryAppUser.Where(c => c.UserSetId == dto.UserSetIds.First() && c.UserSetType == dto.UserSetType && c.DeleteFlag == false)
                                                   .ToListAsync();
                if (temp is null)
                {
                    var newList = new List<AppUser>();
                    newList.AddRange(dto.AppIds.Select(c => new AppUser
                    {
                        Id = Guid.NewGuid(),
                        AppId = c,
                        CreatedTime = DateTimeOffset.Now,
                        UpdatedTime = DateTimeOffset.Now,
                        DeleteFlag = false,
                        UserSetType = dto.UserSetType,
                        UserSetId = dto.UserSetIds.First()
                    }));
                    await _RepositoryAppUser.InsertAsync(newList);
                }
                else
                {
                    foreach (var item in dto.AppIds)
                    {
                        var tempOne = temp.FirstOrDefault(c => c.AppId == item);
                        if (tempOne != null)
                        {
                            tempOne.UpdatedTime = DateTimeOffset.Now;
                            await _RepositoryAppUser.UpdateIncludeAsync(tempOne, new[] { nameof(tempOne.UpdatedTime) });
                        }
                        else
                        {
                            await _RepositoryAppUser.InsertAsync(new AppUser
                            {
                                Id = Guid.NewGuid(),
                                AppId = item,
                                CreatedTime = DateTimeOffset.Now,
                                UpdatedTime = DateTimeOffset.Now,
                                DeleteFlag = false,
                                UserSetType = dto.UserSetType,
                                UserSetId = dto.UserSetIds.First()
                            });
                        }
                    }

                    //删除没有勾选的
                    var deleteList = temp.Where(c => !dto.AppIds.Contains(c.AppId)).ToList();
                    if (deleteList != null && deleteList.Any())
                    {
                        deleteList.ForEach(async c =>
                        {
                            c.DeleteFlag = true;
                            c.UpdatedTime = DateTimeOffset.Now;
                            await _RepositoryAppUser.UpdateIncludeAsync(c, new[] { nameof(c.DeleteFlag), nameof(c.UpdatedTime) });
                        });
                    }
                }
            }
            else
            {
                foreach (var item in dto.AppIds)
                {
                    //已经授权集合
                    var tempList = await _RepositoryAppUser.Where(c => c.AppId == item && c.UserSetType == dto.UserSetType && !c.DeleteFlag)
                                                           .ToListAsync();
                    if (tempList is null)
                    {
                        foreach (var info in dto.UserSetIds)
                        {
                            await _RepositoryAppUser.InsertAsync(new AppUser
                            {
                                Id = Guid.NewGuid(),
                                AppId = item,
                                CreatedTime = DateTimeOffset.Now,
                                UpdatedTime = DateTimeOffset.Now,
                                DeleteFlag = false,
                                UserSetType = dto.UserSetType,
                                UserSetId = info
                            });
                        }
                    }
                    else
                    {
                        foreach (var info in dto.UserSetIds)
                        {
                            var one = tempList.FirstOrDefault(c => c.UserSetId == info);
                            if (one is null)
                            {
                                await _RepositoryAppUser.InsertAsync(new AppUser
                                {
                                    Id = Guid.NewGuid(),
                                    AppId = item,
                                    CreatedTime = DateTimeOffset.Now,
                                    UpdatedTime = DateTimeOffset.Now,
                                    DeleteFlag = false,
                                    UserSetType = dto.UserSetType,
                                    UserSetId = info
                                });
                            }
                            else
                            {
                                one.UpdatedTime = DateTimeOffset.Now;
                                await _RepositoryAppUser.UpdateIncludeAsync(one, new[] { nameof(one.UpdatedTime) });
                            }
                        }

                        //删除没有勾选的
                        var deleteList = tempList.Where(c => !dto.UserSetIds.Contains(c.UserSetId)).ToList();
                        if (deleteList != null && deleteList.Any())
                        {
                            deleteList.ForEach(async c =>
                            {
                                c.DeleteFlag = true;
                                c.UpdatedTime = DateTimeOffset.Now;
                                await _RepositoryAppUser.UpdateIncludeAsync(c, new[] { nameof(c.DeleteFlag), nameof(c.UpdatedTime) });
                            });
                        }
                    }
                }
            }

            return true;
        }



        #endregion


    }
}
