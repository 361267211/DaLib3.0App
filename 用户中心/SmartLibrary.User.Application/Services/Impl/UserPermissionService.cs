/*********************************************************
* 名    称：UserPermissionService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户权限服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.Permission;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Services;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartLibrary.AppCenter.AppCenterGrpcService;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 用户权限服务
    /// </summary>
    public class UserPermissionService : IUserPermissionService, IScoped
    {
        private readonly IBasicConfigService _basicConfigService;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly ISysMenuService _sysMenuService;
        private readonly IRoleService _roleService;
        private readonly TenantInfo _tenantInfo;
        private readonly IRepository<UserGroup> _userGroupRepository;
        private readonly Base64Crypt _baseEncrypt;
        private readonly IGrpcClientResolver _grpcClientResolver;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="basicConfigService"></param>
        /// <param name="userRepository"></param>
        /// <param name="sysMenuService"></param>
        /// <param name="roleService"></param>
        /// <param name="tenantInfo"></param>
        /// <param name="userGroupRepository"></param>
        /// <param name="grpcClientResolver"></param>
        public UserPermissionService(IBasicConfigService basicConfigService
            , IRepository<EntityFramework.Core.Entitys.User> userRepository
            , ISysMenuService sysMenuService
            , IRoleService roleService
            , TenantInfo tenantInfo
            , IRepository<UserGroup> userGroupRepository
            , IGrpcClientResolver grpcClientResolver)
        {
            var codeTable = FileHelper.ReadFileToText("EncodeTable.cert");
            _baseEncrypt = new Base64Crypt(codeTable);
            _userRepository = userRepository;
            _sysMenuService = sysMenuService;
            _roleService = roleService;
            _tenantInfo = tenantInfo;
            _basicConfigService = basicConfigService;
            _userGroupRepository = userGroupRepository;
            _grpcClientResolver = grpcClientResolver;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<AppUserInfo> GetUserInfo(string userKey)
        {
            //暂时取消缓存
            //var userInfo = await _distributeCache.GetAsync<AppUserInfo>($"{CacheKey.UserBaseInfo}{userKey}");
            //if (userInfo == null || userInfo.UserID == Guid.Empty || string.IsNullOrWhiteSpace(userInfo.UserKey))
            //{
            var userEntity = await _userRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserKey == userKey);
            if (userEntity == null)
            {
                return null;
            }
            var userInfo = userEntity.Adapt<AppUserInfo>();
            //await _distributeCache.SetAsync<AppUserInfo>($"{CacheKey.UserBaseInfo}{userKey}", userInfo, new DistributedCacheEntryOptions
            //{
            //    SlidingExpiration = TimeSpan.FromHours(2)
            //});
            return userInfo;
            //}
            //return userInfo;
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<AppUserPermission> GetUserPermission(string userKey)
        {
            //暂时取消缓存
            //var userInfo = await _distributeCache.GetAsync<AppUserPermission>($"{CacheKey.UserPermisInfo}{userKey}");
            //if (userInfo == null || userInfo.UserID == Guid.Empty || string.IsNullOrWhiteSpace(userInfo.UserKey))
            //{
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var userEntity = await _userRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserKey == userKey);
            if (userEntity == null)
            {
                return null;
            }
            var userInfo = new AppUserPermission
            {
                UserID = userEntity.Id,
                UserKey = userEntity.UserKey,
            };
            var userRoles = await _roleService.GetUserRoles(userInfo.UserID);
            var userPermissionList = new List<string>();
            var userRole = _grpcClientResolver.EnsureClient<AppCenterGrpcServiceClient>().GetUserAppPermissionType(new UserAppPermissionTypeRequest { AppId = SiteGlobalConfig.AppBaseConfig.AppRouteCode });

            var sysMenuPermissions = new SysMenuPermissionDto();
            if (userRole.PermissionType == 1 || userEntity.UserKey == $"{_tenantInfo?.Name ?? ""}_vipsmart00001")
            {
                //管理员
                userPermissionList = await _sysMenuService.GetMGRPermissionList();
            }
            else if (userRoles != null && userRoles.Any())
            {
                //有自己配置的权限
                userPermissionList = await _sysMenuService.GetUserPermissionList(userInfo.UserID);
            }
            else if (userRole.PermissionType == 2)
            {
                //操作员
                userPermissionList = await _sysMenuService.GetOpPermissionList();
            }
            else if (userRole.PermissionType == 3)
            {
                //浏览者
                userPermissionList = await _sysMenuService.GetVisPermissionList();
            }
            else
            {
                userPermissionList = await _sysMenuService.GetUserPermissionList(userInfo.UserID);
            }

            userInfo.PermissionList = userPermissionList;
            userInfo.RoleCodes = userRoles.Select(x => x.Code).ToList();
            userInfo.SensitiveFilter = configSet != null ? configSet.SensitiveFilter : true;
            //await _distributeCache.SetAsync<AppUserPermission>($"{CacheKey.UserPermisInfo}{userKey}", userInfo, new DistributedCacheEntryOptions
            //{
            //    SlidingExpiration = TimeSpan.FromHours(2)
            //});
            return userInfo;
            //}
            //return userInfo;
        }

        /// <summary>
        /// 获取读者领卡权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<AppReaderPermission> GetReaderPermission(string userKey)
        {
            var permissionResult = new AppReaderPermission { UserKey = userKey, HasPermission = false };
            var appCenterGrpcClient = _grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            var userAuthTypeRequest = new UserAppPermissionTypeRequest { AppId = "usermanage" };
            var appFrontPermissionResult = await appCenterGrpcClient.GetUserApppermissionAsync(userAuthTypeRequest);
            var hasPermission = appFrontPermissionResult.IsHasPermission;
            permissionResult.HasPermission = hasPermission > 0;
            return permissionResult;
        }

        private Guid? ConvertToGuid(string val)
        {
            try
            {
                return new Guid(val);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 检查用户是否有修改读者信息权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<bool> CheckModifyReaderInfo(string userKey)
        {
            var updateReaderInfo = false;
            var basicConfig = await _basicConfigService.GetBasicConfigSet();
            if (!basicConfig.UserInfoSupply)
            {
                return updateReaderInfo;
            }

            var userEntity = await _userRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserKey == userKey);
            if (userEntity == null)
            {
                return updateReaderInfo;
            }
            var userGroupIds = await _userGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserID == userEntity.Id).Select(x => x.GroupID).ToListAsync();
            var userType = _baseEncrypt.Decode(userEntity.Type);
            var infoAppendReader = await _basicConfigService.GetInfoAppendReader();
            var permitGroupIds = infoAppendReader.Where(x => x.ReaderType == (int)EnumConfigReaderType.用户组).Select(x => ConvertToGuid(x.RefID)).Where(x => x.HasValue).ToList();
            var permitUserTypes = infoAppendReader.Where(x => x.ReaderType == (int)EnumConfigReaderType.用户类型).Select(x => x.RefID).ToList();

            var matchGroup = userGroupIds.Any(x => permitGroupIds.Contains(x));
            var matchType = permitUserTypes.Contains(userType);
            if (matchGroup || matchType)
            {
                updateReaderInfo = true;
            }
            return updateReaderInfo;
        }

        /// <summary>
        /// 检查用户是否有领取读者卡权限
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<bool> CheckCardClaimPermit(string userKey)
        {
            var cardClaim = false;
            var basicConfig = await _basicConfigService.GetBasicConfigSet();
            if (!basicConfig.CardClaim)
            {
                return cardClaim;
            }

            var userEntity = await _userRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserKey == userKey);
            if (userEntity == null)
            {
                return cardClaim;
            }
            var userGroupIds = await _userGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserID == userEntity.Id).Select(x => x.GroupID).ToListAsync();
            var userType = _baseEncrypt.Decode(userEntity.Type);
            var infoAppendReader = await _basicConfigService.GetCardClaimReader();
            var permitGroupIds = infoAppendReader.Where(x => x.ReaderType == (int)EnumConfigReaderType.用户组).Select(x => ConvertToGuid(x.RefID)).Where(x => x.HasValue).ToList();
            var permitUserTypes = infoAppendReader.Where(x => x.ReaderType == (int)EnumConfigReaderType.用户类型).Select(x => x.RefID).ToList();

            var matchGroup = userGroupIds.Any(x => permitGroupIds.Contains(x));
            var matchType = permitUserTypes.Contains(userType);
            if (matchGroup || matchType)
            {
                cardClaim = true;
            }
            return cardClaim;
        }


    }
}
