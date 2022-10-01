/*********************************************************
* 名    称：ISysMenuService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：菜单权限管理服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.Permission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 权限菜单服务
    /// </summary>
    public interface ISysMenuService
    {
        /// <summary>
        /// 获取本应用中所有的权限列表
        /// </summary>
        /// <returns></returns>
        Task<SysMenuPermissionDto> GetAllPermissionTree();
        /// <summary>
        /// 获取用户权限树
        /// </summary>
        /// <returns></returns>
        Task<SysMenuPermissionDto> GetUserPermissoinTree(Guid userId);
        /// <summary>
        /// 获取馆员权限列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetUserPermissionList(Guid userId);
        /// <summary>
        /// 获取应用管理员权限
        /// </summary>
        /// <returns></returns>
        Task<SysMenuPermissionDto> GetMGRPermissionTree();
        /// <summary>
        /// 获取应用管理员权限
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetMGRPermissionList();
        /// <summary>
        /// 获取应用操作员权限
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetOpPermissionList();
        /// <summary>
        /// 获取浏览者权限
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetVisPermissionList();
        /// <summary>
        /// 获取浏览者权限
        /// </summary>
        /// <returns></returns>
        Task<SysMenuPermissionDto> GetVisPermissionTree();
        /// <summary>
        /// 获取操作员权限
        /// </summary>
        /// <returns></returns>
        Task<SysMenuPermissionDto> GetOpPermissionTree();
    }
}
