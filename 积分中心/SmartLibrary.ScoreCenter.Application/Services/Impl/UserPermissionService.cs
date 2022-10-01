/*********************************************************
* 名    称：UserPermissionService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者权限获取服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.Extensions.Logging;
using SmartLibrary.AppCenter;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Common.Services;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 读者权限获取
    /// </summary>
    public class UserPermissionService : IUserPermissionService, IScoped
    {
        private readonly ITenantDistributedCache _distributeCache;
        private readonly ILogger<UserPermissionService> _logger;
        private readonly IGrpcClientResolver _grpcClientResolver;
        private readonly ISysMenuService _sysMenuService;
        private readonly TenantInfo _tenantInfo;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="grpcClientResolver"></param>
        /// <param name="logger"></param>
        /// <param name="distributedCache"></param>
        /// <param name="tenantInfo"></param>
        /// <param name="sysMenuService"></param>
        public UserPermissionService(IGrpcClientResolver grpcClientResolver
            , ILogger<UserPermissionService> logger
            , ITenantDistributedCache distributedCache
            , TenantInfo tenantInfo
            , ISysMenuService sysMenuService)
        {
            _grpcClientResolver = grpcClientResolver;
            _logger = logger;
            _distributeCache = distributedCache;
            _tenantInfo = tenantInfo;
            _sysMenuService = sysMenuService;
        }



        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<AppUserInfo> GetUserInfo(string userKey)
        {
            var userGrpcClient = _grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            var userData = await userGrpcClient.GetUserByKeyAsync(new Google.Protobuf.WellKnownTypes.StringValue { Value = userKey.ToString() });
            if (userData == null)
            {
                return null;
            }
            var appUserInfo = userData.Adapt<AppUserInfo>();
            appUserInfo.UserKey = userData.Key;
            return appUserInfo;
        }

        /// <summary>
        /// 获取用户后台权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<AppUserPermission> GetUserPermission(string userKey)
        {
            //var userInfo = await _distributeCache.GetAsync<AppUserInfo>(userKey.ToString());
            //if (userInfo == null)
            //{
            try
            {
                var userGrpcClient = _grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>();
                var userData = await userGrpcClient.GetUserByKeyAsync(new Google.Protobuf.WellKnownTypes.StringValue { Value = userKey.ToString() });
                if (userData == null)
                {
                    return null;
                }
                var userInfo = new AppUserPermission
                {
                    UserKey = userData.Key,
                };
                var userPermissionList = new List<string>();
                if (userInfo.UserKey == $"{_tenantInfo?.Name ?? ""}_vipsmart00001")
                {
                    userPermissionList = await _sysMenuService.GetMGRPermissionList();
                }
                else
                {
                    userPermissionList = await _sysMenuService.GetUserPermissionList(userInfo.UserKey);
                }

                userInfo.PermissionList = userPermissionList;
                //await _distributeCache.SetAsync<AppUserInfo>(userKey.ToString(), userInfo, new DistributedCacheEntryOptions
                //{
                //    SlidingExpiration = TimeSpan.FromHours(1)
                //});
                return userInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Rpc远程获取用户信息失败:{ex.Message}");
                throw Oops.Oh("获取用户信息失败");
            }
            //}
            //return userInfo;
        }

        /// <summary>
        /// 获取读者前台权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<AppReaderPermission> GetReaderPermission(string userKey)
        {
            var permissionResult = new AppReaderPermission { UserKey = userKey, HasPermission = false };
            var appCenterGrpcClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            var userAuthTypeRequest = new UserAppPermissionTypeRequest { AppId = "scorecenter" };
            var appFrontPermissionResult = await appCenterGrpcClient.GetUserApppermissionAsync(userAuthTypeRequest);
            var hasPermission = appFrontPermissionResult.IsHasPermission;
            permissionResult.HasPermission = hasPermission > 0;
            return permissionResult;
        }
    }
}
