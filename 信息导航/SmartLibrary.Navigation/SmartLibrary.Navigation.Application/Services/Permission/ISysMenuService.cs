
using SmartLibrary.Navigation.EntityFramework.Core.Dto.Permission;
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    public interface ISysMenuService
    {
        /// <summary>
        /// 获取本应用中所有的权限列表
        /// </summary>
        /// <returns></returns>
        Task<PermissionNode> GetAllPermissionTree();
        /// <summary>
        /// 获取当前登陆用户的所有权限列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetUserPermissionList(string userId);
        /// <summary>
        /// 保存用户的权限列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SaveRoleMenuList(List<SysRoleMenuDto> sysRoleMenuDtos);
        /// <summary>
        /// 新增角色信息
        /// </summary>
        /// <param name="sysRoleInfoDto"></param>
        /// <returns></returns>
        Task InsertSysRoleInfo(SysRoleInfoDto sysRoleInfoDto);
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="sysRoleInfoDto"></param>
        /// <returns></returns>
        Task UpdateSysRoleInfo(SysRoleInfoDto sysRoleInfoDto);
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<SysRoleDto> GetSysRoleBaseInfo(Guid roleId);
        /// <summary>
        /// 查角色-权限关联
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<SysRoleMenu>> GetRoleMenu(Guid roleId);
        /// <summary>
        /// 查角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<SysRoleInfoDto> GetSysRoleInfo(Guid roleId);
        Task<PermissionNode> GetUserPermissionTree(string guid);

        Task<List<SysMenuPermissionDto>> GetMGRPermissionList();
        Task<List<SysMenuPermissionDto>> GetOperatorPermissionList();
        Task<List<SysMenuPermissionDto>> GetOperatorPermissionListByColumnPerimission();
        Task<List<SysMenuPermissionDto>> GetVisitorsPermissionList();

        Task<List<PermissionMenu>> GetUserUnionColumnPermissionTree();

    }
}
