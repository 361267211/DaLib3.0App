/*********************************************************
* 名    称：SysMenuAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：菜单权限获取
* 更新历史：
*
* *******************************************************/
using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SmartLibrary.AppCenter;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.Permission;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartLibrary.AppCenter.AppCenterGrpcService;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 菜单权限服务
    /// </summary>
    [Authorize(Policy = PolicyKey.TokenAuth)]
    public class SysMenuAppService : BaseAppService
    {
        private ISysMenuService _sysMenuService;
        private readonly TenantInfo _tenantInfo;
        private IHttpContextAccessor _httpContextAccessor;
        private IGrpcClientResolver _grpcClientResolver;
        private IRoleService _roleService;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="sysMenuService"></param>
        /// <param name="tenantInfo"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="grpcClientResolver"></param>
        /// <param name="roleService"></param>
        public SysMenuAppService(ISysMenuService sysMenuService
            , TenantInfo tenantInfo
            , IHttpContextAccessor httpContextAccessor
            , IGrpcClientResolver grpcClientResolver
            , IRoleService roleService)
        {
            _sysMenuService = sysMenuService;
            _tenantInfo = tenantInfo;
            _httpContextAccessor = httpContextAccessor;
            _grpcClientResolver = grpcClientResolver;
            _roleService = roleService;
        }

        /// <summary>
        /// 获取应用中所有权限列表树
        /// </summary>
        /// <returns></returns>
        public async Task<SysMenuPermissionDto> GetAllPermissionTree()
        {
            var permissionTree = await _sysMenuService.GetAllPermissionTree();
            return permissionTree;
        }

        /// <summary>
        /// 获取用户权限树
        /// </summary>
        /// <returns></returns>
        public async Task<SysMenuPermissionDto> GetUserPermissionTree()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var userIdStr = _httpContextAccessor.HttpContext.User.FindFirst(e => e.Type == "UserKey")?.Value;//调用grpc，获取角色信息，
            var userRole = _grpcClientResolver.EnsureClient<AppCenterGrpcServiceClient>().GetUserAppPermissionType(new UserAppPermissionTypeRequest { AppId = SiteGlobalConfig.AppBaseConfig.AppRouteCode });

            var userRoles = await _roleService.GetUserRoles(CurrentUser.UserID);
            if (userRole.PermissionType == 1 || CurrentUser.UserKey == $"{_tenantInfo?.Name ?? ""}_vipsmart00001")
            {
                //管理员权限
                return await _sysMenuService.GetMGRPermissionTree();
            }
            else if (userRole.PermissionType == 2)
            {
                //操作员
                return await _sysMenuService.GetOpPermissionTree();
            }
            else if (userRole.PermissionType == 3)
            {
                //浏览者
                return await _sysMenuService.GetVisPermissionTree();
            }
            else
            {
                return await _sysMenuService.GetUserPermissoinTree(CurrentUser.UserID);
            }
        }

        /// <summary>
        /// 获取馆员权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetUserPermissionList()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到读者信息");
            }
            var apiList = await _sysMenuService.GetUserPermissionList(CurrentUser.UserID);
            return apiList;
        }

        ///// <summary>
        ///// 获取应用管理员权限
        ///// </summary>
        ///// <returns></returns>
        //public async Task<SysMenuPermissionDto> GetMGRPermissionList()
        //{
        //    var permissionTree = await _sysMenuService.GetMGRPermissionList();
        //    return permissionTree;
        //}

        ///// <summary>
        ///// 获取应用操作员权限
        ///// </summary>
        ///// <returns></returns>
        //public async Task<SysMenuPermissionDto> GetOperatorPermissionList()
        //{
        //    var permissionTree = await _sysMenuService.GetOperatorPermissionList();
        //    return permissionTree;
        //}

        ///// <summary>
        ///// 获取浏览者权限
        ///// </summary>
        ///// <returns></returns>
        //public async Task<SysMenuPermissionDto> GetVisitorPermissionList()
        //{
        //    var permissionTree = await _sysMenuService.GetVisitorsPermissionList();
        //    return permissionTree;
        //}
    }
}
