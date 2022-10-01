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

using Furion;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Identity.Application.Services.Enum;
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
    public class SysMenuService : ISysMenuService, IScoped
    {
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;


        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="sysMenuPermissionRepository"></param>
        public SysMenuService(IRepository<SysMenuPermission> sysMenuPermissionRepository)
        {
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
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
            var sysMenuPermissions = await _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api).ProjectToType<SysMenuPermissionDto>().ToListAsync();
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
            var sysMenuPermissions = await _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api && x.Type != (int)EnumPermissionType.Operate && !x.IsSysMenu).ProjectToType<SysMenuPermissionDto>().ToListAsync();
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
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUserPermissionList(string userKey)
        {
            var menuFullPathQuery = from menu in _sysMenuPermissionRepository.DetachedEntities.Where(mp => !mp.DeleteFlag && mp.Type == (int)EnumPermissionType.Menu)
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
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<SysMenuPermissionDto> GetUserPermissoinTree(string userKey)
        {
            var premissionQuery = from menu in _sysMenuPermissionRepository.DetachedEntities.Where(mp => !mp.DeleteFlag && mp.Type == (int)EnumPermissionType.Menu)
                                  select menu;

            var userSysMenus = await premissionQuery.ProjectToType<SysMenuPermissionDto>().ToListAsync();

            var sysMenuPermissions = await _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api).ProjectToType<SysMenuPermissionDto>().ToListAsync();
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
            var menuFullPathQuery = _sysMenuPermissionRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Type != (int)EnumPermissionType.Api).Where(x => x.Pid == "").Select(x => x.FullPath);
            var permissionQuery = from api in _sysMenuPermissionRepository.DetachedEntities.Where(mp => !mp.DeleteFlag && mp.Type == (int)EnumPermissionType.Api)
                                  where menuFullPathQuery.Any(menuPath => api.FullPath.StartsWith(menuPath))
                                  select api.Permission;
            var premissionList = await permissionQuery.ToListAsync();
            return premissionList;
        }

        public Task<SysMenuPermissionDto> GetOpPermissionList()
        {
            throw new NotImplementedException();
        }

        public Task<SysMenuPermissionDto> GetVisPermissionList()
        {
            throw new NotImplementedException();
        }

    }
}
