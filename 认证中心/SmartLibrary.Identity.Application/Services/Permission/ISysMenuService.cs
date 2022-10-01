
using SmartLibrary.Identity.EntityFramework.Core.Dtos;
using SmartLibrary.Identity.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Services
{
    /// <summary>
    /// 用户权限服务
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
        Task<SysMenuPermissionDto> GetUserPermissoinTree(string userKey);
        /// <summary>
        /// 获取馆员权限列表
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<List<string>> GetUserPermissionList(string userKey);
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
        Task<SysMenuPermissionDto> GetOpPermissionTree();
        /// <summary>
        /// 获取应用操作员权限
        /// </summary>
        /// <returns></returns>
        Task<SysMenuPermissionDto> GetOpPermissionList();
        /// <summary>
        /// 获取浏览者权限
        /// </summary>
        /// <returns></returns>
        Task<SysMenuPermissionDto> GetVisPermissionTree();
        /// <summary>
        /// 获取浏览者权限
        /// </summary>
        /// <returns></returns>
        Task<SysMenuPermissionDto> GetVisPermissionList();

    }
}
