using Furion.DependencyInjection;
using Furion.FriendlyException;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SmartLibrary.Identity.Application.Dtos;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.Common.Services;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services.Impl
{
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
    }
}
