/*********************************************************
* 名    称：RoleAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：角色及权限管理
* 更新历史：
*
* *******************************************************/
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.Role;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 角色服务
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class RoleAppService : BaseAppService
    {
        private IRoleService _roleService;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="roleService"></param>
        public RoleAppService(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 获取角色表格数据
        /// </summary>
        /// <returns></returns>
        public async Task<PagedList<RoleOutput>> QueryRoleTableData([FromQuery] TableQueryBase queryFilter)
        {
            var pagedList = await _roleService.QueryRoleTableData(queryFilter);
            var targetPagedList = pagedList.Adapt<PagedList<RoleOutput>>();
            return targetPagedList;
        }

        /// <summary>
        /// 查询馆员数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<StaffListItemOutput>> QueryStaffTableData([FromQuery] StaffRoleTableQuery queryFilter)
        {
            var pagedList = await _roleService.QueryStaffTableData(queryFilter);
            var targetPageList = pagedList.Adapt<PagedList<StaffListItemOutput>>();
            return targetPageList;
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<RoleEditDto> GetRoleData(Guid roleId)
        {
            var roleData = await _roleService.GetRoleData(roleId);
            return roleData;
        }

        /// <summary>
        /// 通过编码获取角色
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public async Task<RoleEditDto> GetRoleDataByCode(string roleCode)
        {
            var roleData = await _roleService.GetRoleDataByCode(roleCode);
            return roleData;
        }

        /// <summary>
        /// 创建角色数据
        /// </summary>
        /// <param name="roleData"></param>
        /// <returns></returns>
        public async Task<Guid> Create([FromBody] RoleInput roleData)
        {
            var roleEditData = roleData.Adapt<RoleEditDto>();
            var roleId = await _roleService.CreateRole(roleEditData);
            return roleId;
        }

        /// <summary>
        /// 编辑角色数据
        /// </summary>
        /// <param name="roleData"></param>
        /// <returns></returns>
        public async Task<Guid> Update([FromBody] RoleInput roleData)
        {
            var roleEditData = roleData.Adapt<RoleEditDto>();
            var roleId = await _roleService.UpdateRole(roleEditData);
            return roleId;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid roleId)
        {
            var result = await _roleService.DeleteRole(roleId);
            return result;
        }

        /// <summary>
        /// 添加角色馆员
        /// </summary>
        /// <param name="roleUserSet"></param>
        /// <returns></returns>
        public async Task<bool> AddRoleUsers([FromBody] RoleUserSetDto roleUserSet)
        {
            var result = await _roleService.AddRoleUsers(roleUserSet);
            return result;
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="UserRoleInfo"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserRole([FromQuery] UserRoleDeleteDto UserRoleInfo)
        {
            var result = await _roleService.DeleteUserRole(UserRoleInfo);
            return result;
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userRoleSet"></param>
        /// <returns></returns>
        public async Task<bool> SetUserRoles(UserRoleSetDto userRoleSet)
        {
            var result = await _roleService.SetUserRoles(userRoleSet);
            return result;
        }

    }
}
