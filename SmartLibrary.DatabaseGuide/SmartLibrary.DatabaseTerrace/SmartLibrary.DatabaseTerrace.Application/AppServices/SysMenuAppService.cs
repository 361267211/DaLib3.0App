
using Furion.DatabaseAccessor;
using Furion.DataEncryption;
using Furion.DynamicApiController;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartLibrary.DatabaseTerrace.Application.Services.RemoteProxy;
using SmartLibrary.DatabaseTerrace.Common.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.ViewModels;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using SmartLibrary.DatabaseTerrace.Application.Services;
using Furion;
using SmartLibrary.Core.GrpcClientHelper;
using Newtonsoft.Json;
using SmartLibrary.DatabaseTerrace.Common.Extensions;
using SmartLibrary.DatabaseTerrace.Common.Const;
//using SmartLibrary.DatabaseTerrace.Application.Filter;

namespace SmartLibrary.DatabaseTerrace.Application
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
        public async Task<List<string>> GetUserPermissionList(string userKey)
        {
            return await _sysMenuService.GetUserPermissionList(new Guid(userKey));
        }

        /// <summary>
        /// 获取登陆用户所有的权限菜单-树型
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [UnitOfWork]
        public async Task<PermissionNode> GetUserPermissionTree()
        {
            PermissionNode permissionNode = await _sysMenuService.GetUserPermissionTree(App.HttpContext.EnsureClaimValue("UserKey"));
            return permissionNode;
        }

        
        [HttpGet]
        public async Task<List<PermissionMenu>> GetUserUnionColumnPermissionTree()
        {
            return await _sysMenuService.GetUserUnionColumnPermissionTree();
        }
    }
}

