/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.Identity.Application.Dtos.Common;
using SmartLibrary.Identity.Application.Services;
using SmartLibrary.Identity.EntityFramework.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.AppServices
{
    /// <summary>
    /// 菜单权限服务
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class SysMenuAppService : BaseAppService
    {
        private ISysMenuService _sysMenuService;
        private readonly TenantInfo _tenantInfo;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="sysMenuService"></param>
        /// <param name="tenantInfo"></param>
        public SysMenuAppService(ISysMenuService sysMenuService
            , TenantInfo tenantInfo)
        {
            _sysMenuService = sysMenuService;
            _tenantInfo = tenantInfo;
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
            if (CurrentUser.UserKey == $"{_tenantInfo?.Name ?? ""}_vipsmart00001")
            {
                return await _sysMenuService.GetMGRPermissionTree();
            }
            else
            {
                return await _sysMenuService.GetUserPermissoinTree(CurrentUser.UserKey);
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
            var apiList = await _sysMenuService.GetUserPermissionList(CurrentUser.UserKey);
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
