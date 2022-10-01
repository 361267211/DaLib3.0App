using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter.Application.Services.Common;
using SmartLibrary.AppCenter.Common.BaseService;
using SmartLibrary.AppCenter.EntityFramework.Core.Dto.Permission;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission;
using SmartLibrary.AppCenter.EntityFramework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.Permission
{
    public class SysMenuService : ServiceBase, ISysMenuService, IScoped
    {
        private readonly IRepository<SysRole> _SysRoleRepository;
        private readonly IRepository<SysMenuCategory> _SysMenuCategoryRepository;
        private readonly IRepository<SysRoleMenu> _SysRoleMenuRepository;
        private readonly IRepository<SysUserRole> _SysUserRoleRepository;
        private readonly IRepository<SysMenuPermission> _SysMenuPermissionRepository;
        private readonly IGeneralService _GeneralService;

        public SysMenuService(IRepository<SysRole> sysRoleRepository,
                              IRepository<SysMenuCategory> sysMenuCategoryRepository,
                              IRepository<SysRoleMenu> sysRoleMenuRepository,
                              IRepository<SysMenuPermission> sysMenuPermissionRepository,
                              IRepository<SysUserRole> sysUserRoleRepository,
                              IGeneralService generalService)
        {
            _SysRoleRepository = sysRoleRepository;
            _SysMenuCategoryRepository = sysMenuCategoryRepository;
            _SysRoleMenuRepository = sysRoleMenuRepository;
            _SysMenuPermissionRepository = sysMenuPermissionRepository;
            _SysUserRoleRepository = sysUserRoleRepository;
            _GeneralService = generalService;
        }

        /// <summary>
        /// 获取所有权限树
        /// </summary>
        /// <returns></returns>
        public async Task<PermissionNode> GetAllPermissionTree()
        {
            var sysMenuPermissions = await _SysMenuPermissionRepository.Where(e => !e.DeleteFlag).OrderBy(c => c.Sort).ToListAsync();

            var node = sysMenuPermissions.First(e => e.Pid == string.Empty).Adapt<PermissionNode>();
            node.PermissionNodes = GetPermissionNodes(node, sysMenuPermissions);
            return node;
        }

        /// <summary>
        /// 递归获取权限
        /// </summary>
        /// <param name="permissionNode"></param>
        /// <param name="sysMenuPermissions"></param>
        /// <returns></returns>
        public List<PermissionNode> GetPermissionNodes(PermissionNode permissionNode, List<SysMenuPermission> sysMenuPermissions)
        {
            List<PermissionNode> nodes = sysMenuPermissions.Where(e => e.FullPath == $"{permissionNode.FullPath}-{e.Path}").Adapt<List<PermissionNode>>();
            foreach (var node in nodes)
            {
                node.PermissionNodes = GetPermissionNodes(node, sysMenuPermissions);
            }

            return nodes;
        }

        /// <summary>
        /// 查角色的信息
        /// </summary>
        /// <returns></returns>
        public async Task<PermissionNode> GetUserPermissionTree()
        {
            var permissonType = await _GeneralService.GetPermissionType();

            switch (permissonType)
            {
                case 1: //管理员
                    return await GetMGRPermissionTree();
                case 2: //操作员
                    return await GetOperatorPermissionTree();
                case 3: //浏览者
                    return await GetVisitorsPermissionTree();
                default:
                    return new PermissionNode();
            }
        }

        /// <summary>
        /// 取管理员（默认角色）的权限树
        /// </summary>
        /// <returns></returns>
        public async Task<PermissionNode> GetMGRPermissionTree()
        {
            return await GetAllPermissionTree();
        }

        /// <summary>
        /// 获取管理员API权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetMGRPermissionList()
        {
            var list = await _SysMenuPermissionRepository.Where(c => !c.DeleteFlag && c.Type == (int)PermissionTypeEnum.Api).ToListAsync();
            return list.Select(c => c.Permission).ToList();
        }

        /// <summary>
        /// 取操作员（默认角色）的权限树
        /// </summary>
        /// <returns></returns>
        public async Task<PermissionNode> GetOperatorPermissionTree()
        {
            var permissionList = await _SysMenuPermissionRepository.Where(e => !e.DeleteFlag && !e.IsSysMenu
                                           && e.Type != (int)PermissionTypeEnum.Api).OrderBy(c => c.Sort).ToListAsync();

            var firstNode = permissionList.First(e => e.Pid == string.Empty).Adapt<PermissionNode>();
            firstNode.PermissionNodes = GetPermissionNodes(firstNode, permissionList);
            return firstNode;
        }

        /// <summary>
        /// 获取操作员的API权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetOperatorPermissionList()
        {
            var list = await _SysMenuPermissionRepository.Where(c => !c.DeleteFlag && !c.IsSysMenu && c.Type == (int)PermissionTypeEnum.Api).ToListAsync();
            return list.Select(c => c.Permission).ToList();
        }

        /// <summary>
        /// 取浏览者（默认角色）的权限树
        /// </summary>
        /// <returns></returns>
        public async Task<PermissionNode> GetVisitorsPermissionTree()
        {
            var permissionList = await _SysMenuPermissionRepository.Where(e => !e.DeleteFlag && !e.IsSysMenu && e.Category == "3")
                                        .OrderBy(c => c.Sort).ToListAsync();

            var firstNode = permissionList.First(e => e.Pid == string.Empty).Adapt<PermissionNode>();

            firstNode.PermissionNodes = GetPermissionNodes(firstNode, permissionList);
            return firstNode;
        }

        /// <summary>
        /// 获取浏览者API权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetVisitorsPermissionList()
        {
            var list = await _SysMenuPermissionRepository.Where(c => !c.DeleteFlag && !c.IsSysMenu && c.Category == "3" && c.Type == (int)PermissionTypeEnum.Api).ToListAsync();
            return list.Select(c => c.Permission).ToList();
        }

        /// <summary>
        /// 获取用户API调用权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetUserPermissionList()
        {
            var permissonType = await _GeneralService.GetPermissionType();

            switch (permissonType)
            {
                case 1: //管理员
                    return await GetMGRPermissionList();
                case 2: //操作员
                    return await GetOperatorPermissionList();
                case 3: //浏览者
                    return await GetVisitorsPermissionList();
                default:
                    return Array.Empty<string>().ToList();
            }
        }
    }
}
