/*********************************************************
* 名    称：SysMenuService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：菜单权限管理服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos.Permission;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 菜单管理服务
    /// </summary>
    public class SysMenuService : ISysMenuService, IScoped
    {
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;


        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="sysMenuPermissionRepository"></param>
        /// <param name="sysUserRoleRepository"></param>
        /// <param name="sysRoleMenuRepository"></param>
        public SysMenuService(IRepository<SysMenuPermission> sysMenuPermissionRepository
            , IRepository<SysUserRole> sysUserRoleRepository
            , IRepository<SysRoleMenu> sysRoleMenuRepository)
        {
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
        }

        /// <summary>
        /// 递归获取权限树
        /// </summary>
        /// <param name="permissionNode"></param>
        /// <param name="sysMenuPermissions"></param>
        /// <returns></returns>
        public List<SysMenuPermissionDto> GetPermissionNodes(SysMenuPermissionDto permissionNode, List<SysMenuPermissionDto> sysMenuPermissions)
        {
            var nodes = sysMenuPermissions.Where(e => e.Pid == permissionNode.FullPath).Adapt<List<SysMenuPermissionDto>>();
            foreach (var node in nodes)
            {
                node.PermissionNodes = GetPermissionNodes(node, sysMenuPermissions);
            }
            return nodes;
        }


        /// <summary>
        /// 递归获取权限树，必须包含子节点
        /// </summary>
        /// <param name="permissionNode"></param>
        /// <param name="sysMenuPermissions"></param>
        /// <param name="childNodes"></param>
        /// <returns></returns>
        public List<SysMenuPermissionDto> GetPermissionNodesWithChildNodes(SysMenuPermissionDto permissionNode, List<SysMenuPermissionDto> sysMenuPermissions, List<SysMenuPermissionDto> childNodes)
        {
            var nodes = sysMenuPermissions.Where(e => e.Pid == permissionNode.FullPath && childNodes.Any(c => c.FullPath.StartsWith(e.FullPath))).Adapt<List<SysMenuPermissionDto>>();
            foreach (var node in nodes)
            {
                node.PermissionNodes = GetPermissionNodesWithChildNodes(node, sysMenuPermissions, childNodes);
            }
            return nodes;
        }


        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <returns></returns>
        public async Task<SysMenuPermissionDto> GetAllPermissionTree()
        {
            var sysMenuPermissions = await _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api).OrderBy(x => x.CreateTime).ProjectToType<SysMenuPermissionDto>().ToListAsync();
            var topNode = sysMenuPermissions.FirstOrDefault(x => x.Pid == string.Empty);
            if (topNode == null)
            {
                return null;
            }
            topNode.PermissionNodes = GetPermissionNodes(topNode, sysMenuPermissions);
            return topNode;
        }


        /// <summary>
        /// 获取操作人，用户管理降级为浏览者
        /// </summary>
        /// <returns></returns>
        private async Task<SysMenuPermissionDto> GetOperatorPermissionTree()
        {
            var sysMenuPermissions = await _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api && !x.IsSysMenu).ProjectToType<SysMenuPermissionDto>().ToListAsync();
            var topNode = sysMenuPermissions.FirstOrDefault(x => x.Pid == string.Empty);
            if (topNode == null)
            {
                return null;
            }
            topNode.PermissionNodes = GetPermissionNodes(topNode, sysMenuPermissions);
            return topNode;

        }

        private async Task<SysMenuPermissionDto> GetVisitorsPermissionTree()
        {
            var sysMenuPermissions = await _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api && x.Type != (int)EnumPermissionType.Operate && !x.IsSysMenu).ProjectToType<SysMenuPermissionDto>().ToListAsync();
            var topNode = sysMenuPermissions.FirstOrDefault(x => x.Pid == string.Empty);
            if (topNode == null)
            {
                return null;
            }
            topNode.PermissionNodes = GetPermissionNodes(topNode, sysMenuPermissions);
            return topNode;
        }

        /// <summary>
        /// 获取登录用户Api权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUserPermissionList(Guid userId)
        {
            var menuFullPathQuery = from userRole in _sysUserRoleRepository.DetachedEntities.Where(ur => !ur.DeleteFlag && ur.UserID == userId)
                                    join roleMenu in _sysRoleMenuRepository.DetachedEntities.Where(rm => !rm.DeleteFlag) on userRole.RoleID equals roleMenu.RoleID into roleMenus
                                    from roleMenu in roleMenus
                                    join menu in _sysMenuPermissionRepository.DetachedEntities.Where(mp => !mp.DeleteFlag) on roleMenu.MenuPermissionID equals menu.Id into menus
                                    from menu in menus
                                    select menu.FullPath;
            var permissionQuery = from api in _sysMenuPermissionRepository.DetachedEntities.Where(mp => !mp.DeleteFlag && mp.Type == (int)EnumPermissionType.Api)
                                  where menuFullPathQuery.Any(menuPath => api.FullPath.StartsWith(menuPath))
                                  select api.Permission;
            var premissionList = await permissionQuery.ToListAsync();
            return premissionList;
        }

        /// <summary>
        /// 获取用户权限菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<SysMenuPermissionDto> GetUserPermissoinTree(Guid userId)
        {
            var premissionQuery = from userRole in _sysUserRoleRepository.DetachedEntities.Where(e => !e.DeleteFlag && e.UserID == userId)
                                  join roleMenu in _sysRoleMenuRepository.DetachedEntities.Where(e => !e.DeleteFlag) on userRole.RoleID equals roleMenu.RoleID into roleMenus
                                  from roleMenu in roleMenus
                                  join menu in _sysMenuPermissionRepository.DetachedEntities.Where(e => !e.DeleteFlag && e.Type != (int)EnumPermissionType.Api) on roleMenu.MenuPermissionID equals menu.Id into menus
                                  from menu in menus
                                  select menu;

            var userSysMenus = await premissionQuery.ProjectToType<SysMenuPermissionDto>().ToListAsync();

            var sysMenuPermissions = await _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api).OrderBy(x => x.CreateTime).ProjectToType<SysMenuPermissionDto>().ToListAsync();
            var topNode = sysMenuPermissions.FirstOrDefault(x => x.Pid == string.Empty);
            if (topNode == null)
            {
                return null;
            }
            topNode.PermissionNodes = GetPermissionNodesWithChildNodes(topNode, sysMenuPermissions, userSysMenus);
            return topNode;
        }

        /// <summary>
        /// 获取默认管理员权限
        /// </summary>
        /// <returns></returns>
        public async Task<SysMenuPermissionDto> GetMGRPermissionTree()
        {
            var topNode = await this.GetAllPermissionTree();
            return topNode;
        }

        /// <summary>
        /// 获取操作者权限
        /// </summary>
        /// <returns></returns>
        public async Task<SysMenuPermissionDto> GetOpPermissionTree()
        {
            var topNode = await this.GetOperatorPermissionTree();
            return topNode;
        }

        /// <summary>
        /// 获取浏览者权限
        /// </summary>
        /// <returns></returns>
        public async Task<SysMenuPermissionDto> GetVisPermissionTree()
        {
            var topNode = await this.GetVisitorsPermissionTree();
            return topNode;
        }

        public async Task<List<string>> GetMGRPermissionList()
        {
            var menuFullPathQuery = _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api).OrderBy(x => x.CreateTime).Where(x => x.Pid == "").Select(x => x.FullPath);
            var permissionQuery = from api in _sysMenuPermissionRepository.DetachedEntities.Where(mp => !mp.DeleteFlag && mp.Type == (int)EnumPermissionType.Api)
                                  where menuFullPathQuery.Any(menuPath => api.FullPath.StartsWith(menuPath))
                                  select api.Permission;
            var premissionList = await permissionQuery.ToListAsync();
            return premissionList;
        }

        public async Task<List<string>> GetOpPermissionList()
        {
            var menuFullPathQuery = _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api && !x.IsSysMenu).Where(x => x.Pid == "").Select(x => x.FullPath);
            var permissionQuery = from api in _sysMenuPermissionRepository.DetachedEntities.Where(mp => !mp.DeleteFlag && mp.Type == (int)EnumPermissionType.Api)
                                  where menuFullPathQuery.Any(menuPath => api.FullPath.StartsWith(menuPath))
                                  select api.Permission;
            var premissionList = await permissionQuery.Where(x=>!string.IsNullOrWhiteSpace(x)).ToListAsync();
            return premissionList;
        }

        public async Task<List<string>> GetVisPermissionList()
        {
            var menuFullPathQuery = _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api && x.Type != (int)EnumPermissionType.Operate && !x.IsSysMenu).OrderBy(x => x.CreateTime).Where(x => x.Pid == "").Select(x => x.FullPath);
            var permissionQuery = from api in _sysMenuPermissionRepository.DetachedEntities.Where(mp => !mp.DeleteFlag && mp.Type == (int)EnumPermissionType.Api)
                                  where menuFullPathQuery.Any(menuPath => api.FullPath.StartsWith(menuPath))
                                  select api.Permission;
            var premissionList = await permissionQuery.ToListAsync();
            return premissionList;
        }
    }
}
