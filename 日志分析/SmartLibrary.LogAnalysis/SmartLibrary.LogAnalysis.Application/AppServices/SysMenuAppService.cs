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

using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using SmartLibrary.LogAnalysis.Application.Services;
using SmartLibrary.LogAnalysis.EntityFramework.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.LogAnalysis.Application.AppServices
{
    /// <summary>
    /// 权限管理接口
    /// </summary>
   // [DatabaseActionFilter]
    public class SysMenuAppService : IDynamicApiController
    {

        public IConfiguration _configuration { get; }

        private readonly ISysMenuService _sysMenuService;
        public SysMenuAppService(IConfiguration configuration,
            ISysMenuService sysMenuService
            )
        {
            _configuration = configuration;
            _sysMenuService = sysMenuService;
        }

        /// <summary>
        /// 获取完整的权限-树型
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [UnitOfWork]
        [AuthorizeMultiplePolicy("Premission2", false)]
        public async Task<PermissionNode> GetAllPermissionTree()
        {
            return await _sysMenuService.GetAllPermissionTree();
        }

        /// <summary>
        /// 获取角色信息  基础信息+权限列表
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        [HttpGet]

        [UnitOfWork]
        public async Task<SysRoleInfoDto> GetSysRoleInfo(Guid roleId)
        {
            return await _sysMenuService.GetSysRoleInfo(roleId);
        }

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="sysRoleInfoDto"></param>
        /// <returns></returns>
        [HttpPost]

        [UnitOfWork]
        public async Task SaveSysRoleInfo(SysRoleInfoDto sysRoleInfoDto)
        {
            if (sysRoleInfoDto.Id == Guid.Empty)
            {
                await _sysMenuService.InsertSysRoleInfo(sysRoleInfoDto);
            }
            else
            {
                await _sysMenuService.UpdateSysRoleInfo(sysRoleInfoDto);
            }
        }
        /// <summary>
        /// 获取登陆用户所有的权限菜单-列表
        /// </summary>
        /// <param name="userKey">用户唯一标识</param>
        /// <returns></returns>
        [HttpGet]

        [UnitOfWork]
        public async Task<List<string>> GetUserPermissionList(Guid userKey)
        {
            return await _sysMenuService.GetUserPermissionList(userKey);
        }

        /// <summary>
        /// 获取登陆用户所有的权限菜单-树型
        /// </summary>
        /// <param name="userKey">用户唯一标识</param>
        /// <returns></returns>
        [HttpGet]

        [UnitOfWork]
        public async Task<PermissionNode> GetUserPermissionTree(Guid userKey)
        {
            PermissionNode permissionNode = await _sysMenuService.GetUserPermissionTree(userKey);
            return permissionNode;
        }


    }
}
