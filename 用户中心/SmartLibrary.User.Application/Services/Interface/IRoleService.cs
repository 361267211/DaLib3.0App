/*********************************************************
* 名    称：IRoleService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：角色服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.Role;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 角色服务
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 角色列表获取
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<RoleListItemDto>> QueryRoleTableData(TableQueryBase queryFilter);
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="roleData"></param>
        /// <returns></returns>
        Task<Guid> CreateRole(RoleEditDto roleData);
        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="roleData"></param>
        /// <returns></returns>
        Task<Guid> UpdateRole(RoleEditDto roleData);
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<RoleEditDto> GetRoleData(Guid roleId);

        /// <summary>
        /// 通过编码获取角色信息
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        Task<RoleEditDto> GetRoleDataByCode(string roleCode);
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<bool> DeleteRole(Guid roleId);
        /// <summary>
        /// 设置角色用户
        /// </summary>
        /// <param name="roleUserSet"></param>
        /// <returns></returns>
        Task<bool> AddRoleUsers(RoleUserSetDto roleUserSet);
        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="userRoleInfo"></param>
        /// <returns></returns>
        Task<bool> DeleteUserRole(UserRoleDeleteDto userRoleInfo);
        /// <summary>
        /// 馆员列表获取
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<StaffListItemDto>> QueryStaffTableData(StaffRoleTableQuery queryFilter);
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userRoleSet"></param>
        /// <returns></returns>
        Task<bool> SetUserRoles(UserRoleSetDto userRoleSet);
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<RoleListItemDto>> GetUserRoles(Guid userId);

    }
}
