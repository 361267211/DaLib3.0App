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
using SmartLibrary.SceneManage.EntityFramework.Core.Dtos;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;
using SmartLibrary.SceneManage.EntityFramework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Services
{
    public class SysMenuService : ISysMenuService, IScoped
    {
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysMenuCategory> _sysMenuCategoryRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;

        public SysMenuService(

            IRepository<SysRole> sysRoleRepository,
            IRepository<SysMenuCategory> sysMenuCategoryRepository,
            IRepository<SysRoleMenu> sysRoleMenuRepository,
            IRepository<SysMenuPermission> sysMenuPermissionRepository,
            IRepository<SysUserRole> sysUserRoleRepository
            )
        {
            _sysRoleRepository = sysRoleRepository;
            _sysMenuCategoryRepository = sysMenuCategoryRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
        }

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<PermissionNode>> GetAllPermissionTree()
        {
            List<SysMenuPermission> sysMenuPermissions = _sysMenuPermissionRepository.Where(e => e.Type != (int)PermissionTypeEnum.Api).ToList();
            var nodeList = sysMenuPermissions.Where(e => e.Pid == string.Empty).OrderBy(p => p.Sort).Adapt<List<PermissionNode>>();
            nodeList.ForEach(node=> { node.PermissionNodes = GetPermissionNodes(node, sysMenuPermissions); });
            return nodeList;
        }

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
        /// 获取登陆用户的所有可用API权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUserPermissionList(Guid userId)
        {
            var premissionQuery = from a in _sysUserRoleRepository.AsQueryable(e => e.UserID == userId)
                                  join b in _sysRoleMenuRepository.AsQueryable() on a.RoleID equals b.RoleID
                                  join c in _sysMenuPermissionRepository.AsQueryable() on b.MenuPermissionID equals c.Id
                                  join d in _sysMenuPermissionRepository.AsQueryable() on 1 equals 1
                                  where d.FullPath == (c.FullPath + "-" + d.Path)
                                  where d.Type == (int)PermissionTypeEnum.Api
                                  select new String(d.Permission);
            var premissionList = premissionQuery.ToList();
            return premissionList;
        }

        /// <summary>
        /// 保存角色-菜单权限关系
        /// </summary>
        /// <param name="sysRoleMenuDtos"></param>
        /// <returns></returns>
        public async Task SaveRoleMenuList(List<SysRoleMenuDto> sysRoleMenuDtos)
        {
            if (sysRoleMenuDtos.Count == 0)
                return;
            var sysRoleMenu = sysRoleMenuDtos.Adapt<List<SysRoleMenu>>();
            var roleId = sysRoleMenu.FirstOrDefault()?.Id;
            _sysRoleMenuRepository.Context.DeleteRange<SysRoleMenu>(e => e.RoleID == roleId);
            _sysRoleMenuRepository.Context.BulkInsert(sysRoleMenu);
        }

        /// <summary>
        /// 新增角色基础信息
        /// </summary>
        /// <param name="sysRoleInfoDto"></param>
        /// <returns></returns>
        public async Task InsertSysRoleInfo(SysRoleInfoDto sysRoleInfoDto)
        {
            var sysRoleMenu = sysRoleInfoDto.Adapt<SysRole>();
            sysRoleMenu.Id = Guid.NewGuid();
            _sysRoleRepository.Insert(sysRoleMenu);
        }

        /// <summary>
        /// 修改角色基础信息
        /// </summary>
        /// <param name="sysRoleInfoDto"></param>
        /// <returns></returns>
        public async Task UpdateSysRoleInfo(SysRoleInfoDto sysRoleInfoDto)
        {
            var sysRoleMenu = sysRoleInfoDto.Adapt<SysRole>();

            _sysRoleRepository.Update(sysRoleMenu);
        }

        /// <summary>
        /// 修改角色基础信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<SysRoleDto> GetSysRoleBaseInfo(Guid roleId)
        {
            var entity = _sysRoleRepository.Find(roleId);
            //_sysRoleRepository.First(e=>e.Id==roleId).ProjectToType<List<SysRoleDto>>();
            var sysRoleDto = entity.Adapt<SysRoleDto>();
            return sysRoleDto;
        }

        /// <summary>
        /// 查角色-权限绑定关系
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<SysRoleMenu>> GetRoleMenu(Guid roleId)
        {
            var test = _sysRoleMenuRepository.Where(e => e.RoleID == roleId).ToList();
            return _sysRoleMenuRepository.Where(e => e.RoleID == roleId).ToList();
        }

        /// <summary>
        /// 查角色的信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<SysRoleInfoDto> GetSysRoleInfo(Guid roleId)
        {
            var role = await GetSysRoleBaseInfo(roleId);
            var sysRoleInfoDto = role.Adapt<SysRoleInfoDto>();
            var roleMenu = await GetRoleMenu(roleId);
            sysRoleInfoDto.Permissions = roleMenu.Adapt<List<SysRoleMenuDto>>();

            return sysRoleInfoDto;
        }

        /// <summary>
        /// 查角色的信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<PermissionNode> GetUserPermissionTree(Guid userId)
        {
            IQueryable<SysMenuPermission> premissionQuery = from a in _sysUserRoleRepository.AsQueryable(e => e.UserID == userId)
                                                            join b in _sysRoleMenuRepository.AsQueryable() on a.RoleID equals b.RoleID
                                                            join c in _sysMenuPermissionRepository.AsQueryable() on b.MenuPermissionID equals c.Id
                                                            where c.Type != (int)PermissionTypeEnum.Api
                                                            select c;

            List<SysMenuPermission> sysMenus = premissionQuery.ToList();
            var node = sysMenus.First(e => e.Pid == string.Empty).Adapt<PermissionNode>();

            List<PermissionNode> newList = new List<PermissionNode>();

            node.PermissionNodes = GetPermissionNodes(node, sysMenus);
            return node;

        }

        /// <summary>
        /// 取管理员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<PermissionNode>> GetMGRPermissionTree()
        {
            return await this.GetAllPermissionTree();

        }

        /// <summary>
        /// 取操作员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<PermissionNode>> GetOperatorPermissionTree()
        {
            var permissionList = await _sysMenuPermissionRepository.Where(e => !e.IsSysMenu && e.Type != (int)PermissionTypeEnum.Api).ToPagedListAsync();
            var nodeList = permissionList.Items.Where(e => e.Pid == string.Empty).OrderBy(p => p.Sort).Adapt<List<PermissionNode>>();
            nodeList.ForEach(node => { node.PermissionNodes = GetPermissionNodes(node, permissionList.Items.ToList()); });
            return nodeList;
        }

        /// <summary>
        /// 取浏览者（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<PermissionNode>> GetVisitorsPermissionTree()
        {
            var permissionList = await _sysMenuPermissionRepository.Where(e => !e.IsSysMenu && e.Type != (int)PermissionTypeEnum.Api && e.Type == (int)PermissionTypeEnum.Menu || e.Type == (int)PermissionTypeEnum.Query).ToPagedListAsync();
            var nodeList = permissionList.Items.Where(e => e.Pid == string.Empty).OrderBy(p => p.Sort).Adapt<List<PermissionNode>>();
            nodeList.ForEach(node => { node.PermissionNodes = GetPermissionNodes(node, permissionList.Items.ToList()); });
            return nodeList;
        }


        /// <summary>
        /// 取管理员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetMGRPermissionList()
        {
            return _sysMenuPermissionRepository.Where(e => e.Type == (int)PermissionTypeEnum.Api).ProjectToType<SysMenuPermissionDto>().OrderBy(p=>p.Sort).ToList();

        }

        /// <summary>
        /// 取操作员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>  GetOperatorPermissionList
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetOperatorPermissionList()
        {
            return _sysMenuPermissionRepository.Where(e => !e.IsSysMenu && e.Type == (int)PermissionTypeEnum.Api).ProjectToType<SysMenuPermissionDto>().OrderBy(p => p.Sort).ToList();
        }

        /// <summary>
        /// 取浏览者（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetVisitorsPermissionList()
        {

            var premissionQuery = from c in _sysMenuPermissionRepository.AsQueryable(e => !e.IsSysMenu && e.Type == (int)PermissionTypeEnum.Query)
                                  join d in _sysMenuPermissionRepository.AsQueryable() on 1 equals 1
                                  where d.Type == (int)PermissionTypeEnum.Api
                                  where d.FullPath == (c.FullPath + "-" + d.Path)
                                  select d;

            return premissionQuery.OrderBy(p => p.Sort).ToList().Adapt<List<SysMenuPermissionDto>>();
        }

    }
}
