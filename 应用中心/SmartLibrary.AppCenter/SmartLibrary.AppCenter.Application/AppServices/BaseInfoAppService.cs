/*********************************************************
 * 名    称：BaseInfoAppService
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/12/2 16:49:29
 * 描    述：
 *
 * 更新历史：
 *
 * *******************************************************/
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.AppCenter.Application.Dtos.Application;
using SmartLibrary.AppCenter.Application.Dtos.BaseInfo;
using SmartLibrary.AppCenter.Application.Services.Application;
using SmartLibrary.AppCenter.Application.Services.Common;
using SmartLibrary.AppCenter.Application.Services.UserApplication;
using SmartLibrary.AppCenter.Common.BaseService;
using SmartLibrary.AppCenter.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.AppServices
{
    /// <summary>
    /// 基础信息服务
    /// </summary>
    public class BaseInfoAppService : BaseAppService
    {
        /// <summary>
        /// 应用服务
        /// </summary>
        private readonly IApplicationService _applicationService;
        private readonly IUserAppService _userAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGeneralService _generalService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="applicationService"></param>
        /// <param name="userAppService"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="generalService"></param>
        public BaseInfoAppService(IApplicationService applicationService,
                                  IUserAppService userAppService,
                                  IHttpContextAccessor httpContextAccessor,
                                  IGeneralService generalService)
        {
            _applicationService = applicationService;
            _userAppService = userAppService;
            _httpContextAccessor = httpContextAccessor;
            _generalService = generalService;
        }

        /// <summary>
        /// 获取当前用户机构基础信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseInfo> GetBaseInfo()
        {
            var result = new BaseInfo() { UrlInfo = new List<UrlInfo>() };

            var orgInfoTask = _generalService.GetCurrentOrgInfo();
            var headerFooterInfoTask = _generalService.GetCurrentHeaderFooterInfo();
            var allAppsTaks = _applicationService.GetAllApp();

            await Task.WhenAll(orgInfoTask, headerFooterInfoTask, allAppsTaks);

            result.OrgInfo = orgInfoTask.Result;
            result.HeaderFooterInfo = headerFooterInfoTask.Result;

            result.UrlInfo.Add(new UrlInfo { Code = "index", Path = result.OrgInfo?.PortalUrl });
            if (allAppsTaks.Result != null && allAppsTaks.Result.Any())
            {
                foreach (var item in allAppsTaks.Result)
                {
                    if (!string.IsNullOrWhiteSpace(item.FrontUrl))
                    {
                        var isOK = Uri.TryCreate(item.FrontUrl, UriKind.Absolute, out var uri);
                        result.UrlInfo.Add(new UrlInfo
                        {
                            AppId = item.AppId,
                            Code = item.RouteCode,
                            Path = isOK ? $"{uri.Scheme}://{uri.Host}:{uri.Port}" : item.FrontUrl
                        });
                    }
                    else
                    {
                        result.UrlInfo.Add(new UrlInfo { AppId = item.AppId, Code = item.RouteCode, Path = "" });
                    }
                }
            }

            if (string.IsNullOrEmpty(UserKey)) return result;

            var user = await _userAppService.GetCurrentUserInfo(UserKey);
            result.UserInfo = user;

            return result;
        }

        /// <summary>
        /// 根据userkey获取指定应用的权限类型 1：管理员，2：操作员，3：浏览者，0：无权限
        /// </summary>
        /// <param name="appRouteCode"></param>
        /// <returns>1：管理员，2：操作员，3：浏览者，0：无权限</returns>
        [HttpGet]
        public async Task<int> GetUserAppPermissionType(string appRouteCode)
        {
            if (string.IsNullOrEmpty(UserKey))
            {
                Oops.Oh("请登录").StatusCode((int)HttpStatusCode.Unauthorized);
            }
            var perType = await _userAppService.GetUserAppPermissionType(UserKey, appRouteCode);

            var result = perType.PermissionType;
            return result;
        }

        /// <summary>
        /// 判断用户对指定应用是否有使用权限(前台)
        /// </summary>
        /// <returns>1. 有权限 2.无权限</returns>
        [HttpGet]
        public async Task<int> GetUserApppermission(string appRouteCode)
        {
            if (string.IsNullOrEmpty(UserKey))
            {
                Oops.Oh("请登录").StatusCode((int)HttpStatusCode.Unauthorized);
            }
            var perType = await _userAppService.GetUserAppPermission(UserKey, appRouteCode);

            var result = perType.IsHasPermission;
            return result;
        }

        /// <summary>
        /// 获取对指定应用是否有前后台权限
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AuthInfoEx> GetAuthInfo(string appCode)
        {
            var result = new AuthInfoEx() { CanWeb = true, CanAdmin = false };

            if (string.IsNullOrWhiteSpace(UserKey))
                return result;

            var front = await _userAppService.GetUserAppPermission(UserKey, appCode);
            var back = await _userAppService.GetUserAppPermissionType(UserKey, appCode);

            result.CanWeb = front.IsHasPermission == 1;
            result.CanAdmin = back.PermissionType != 0;

            return result;
        }

        /// <summary>
        /// 根据routecode获取当前应用的名称和版本号
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CurrentAppInfo> GetCurrentAppInfo(string appCode)
        {
            return await _userAppService.GetCurrentAppInfo(appCode);
        }

        /// <summary>
        /// 获取后台头部菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MgrMenu> GetMgrTopMenu()
        {
            if (string.IsNullOrEmpty(UserKey))
            {
                Oops.Oh("请登录").StatusCode((int)HttpStatusCode.Unauthorized);
            }
            var result = new MgrMenu();
            var orgInfo = await _generalService.GetCurrentOrgInfo();
            result.LogoUrl = orgInfo.LogoUrl;
            result.SimpleLogoUrl = orgInfo.SimpleLogoUrl;

            result.AppMenuList = await _userAppService.GetMgrTopMenu(UserKey);

            return result;
        }

        /// <summary>
        /// 获取当前用户收藏应用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PersonalAppCenterViewModel> GetMyCollectionApps()
        {
            if (string.IsNullOrEmpty(UserKey))
            {
                Oops.Oh("请登录").StatusCode((int)HttpStatusCode.Unauthorized);
            }
            var allApps = await _applicationService.GetAllApp();
            var appCenter = await _applicationService.GetAppDetailByCode(SiteGlobalConfig.AppBaseConfig.AppRouteCode);
            var apps = await _userAppService.GetMyCollectionApps();
            var appList = apps.Select(p => new AppMenuListDto
            {
                AppIcon = p.AppIcon,
                AppId = p.AppId,
                AppName = p.AppName,
                FrontUrl = p.FrontUrl
            }).ToList();

            if (appList != null && appList.Any())
            {
                foreach (var item in appList)
                {
                    var tempApp = allApps.FirstOrDefault(c => c.AppId == item.AppId);
                    if (tempApp != null)
                    {
                        switch (tempApp.RouteCode)
                        {
                            case "usermanage": //用户中心
                                item.FrontUrl = item.FrontUrl?.Split('#')[0] + "#/web_library";
                                break;
                            case "appcenter": //应用中心
                                item.FrontUrl = item.FrontUrl?.Split('#')[0] + "#/web_myApps";
                                break;
                            case "databaseguide": //数据库导航
                                item.FrontUrl = item.FrontUrl?.Split('#')[0] + "#/web_myDataBase";
                                break;
                            case "assembly": //文献专题
                                item.FrontUrl = item.FrontUrl?.Split('#')[0] + "#/web_MyTopic";
                                break;
                        }
                    }
                }
            }

            var result = new PersonalAppCenterViewModel
            {
                AppList = appList,
                AppCenterRouteCode = $"{appCenter?.FrontUrl}",
                MyAppRouteCode = appCenter?.FrontUrl.Split('#')[0] + "#/web_myApps"
            };

            return result;
        }
    }
}
