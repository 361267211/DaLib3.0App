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
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using SmartLibrary.Navigation.EntityFramework.Core.Dto.Permission;
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;
using SmartLibrary.Navigation.EntityFramework.Core.Enum;
using SmartLibrary.Navigation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.Services
{
    public class SysMenuService : ISysMenuService, IScoped
    {
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysMenuCategory> _sysMenuCategoryRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;
        private readonly INavigationSettingsService _settingsService;
        private readonly int UserRole = 3;//1 管理者，2 操作者，3 浏览者
        private TenantInfo _tenantInfo;

        public SysMenuService(

            IRepository<SysRole> sysRoleRepository,
            IRepository<SysMenuCategory> sysMenuCategoryRepository,
            IRepository<SysRoleMenu> sysRoleMenuRepository,
            IRepository<SysMenuPermission> sysMenuPermissionRepository,
            IRepository<SysUserRole> sysUserRoleRepository,
            INavigationSettingsService settingsService,
            TenantInfo tenantInfo
            )
        {
            _sysRoleRepository = sysRoleRepository;
            _sysMenuCategoryRepository = sysMenuCategoryRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
            _settingsService = settingsService;
            _tenantInfo = tenantInfo;
            //UserRoleReply userRole = new UserRoleReply();
            ////调用grpc，获取角色信息，
            //userRole = App.GetService<IGrpcClientResolver>().EnsureClient<UserRoleGrpcService.UserRoleGrpcServiceClient>().GetUserRole(new UserRoleRequest {  UserKey = _tenantInfo.UserKey.ToString() });
            //UserRole = userRole.UserRole;

            //模拟假数据TODO：GRPC调用正常后删除
            UserRole = 1;
        }

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<PermissionNode> GetAllPermissionTree()
        {
            List<SysMenuPermission> sysMenuPermissions = _sysMenuPermissionRepository.Where(e => e.Type != (int)PermissionTypeEnum.Api).ToList();
            var node = sysMenuPermissions.First(e => e.Pid == string.Empty).Adapt<PermissionNode>();
            node.PermissionNodes = GetPermissionNodes(node, sysMenuPermissions);
            return node;
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
        public async Task<List<string>> GetUserPermissionList(string userId)
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
        public async Task<PermissionNode> GetUserPermissionTree(string userId)
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
        public async Task<PermissionNode> GetMGRPermissionTree(Guid userId)
        {
            return await this.GetAllPermissionTree();

        }

        /// <summary>
        /// 取操作员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<PermissionNode> GetOperatorPermissionTree(Guid userId)
        {
            var permissionList = _sysMenuPermissionRepository.Where(e => !e.IsSysMenu && e.Type != (int)PermissionTypeEnum.Api).ToList();
            var firstNode = permissionList.First(e => e.Pid == string.Empty).Adapt<PermissionNode>();
            firstNode.PermissionNodes = this.GetPermissionNodes(firstNode, permissionList);
            return firstNode;
        }

        /// <summary>
        /// 取浏览者（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<PermissionNode> GetVisitorsPermissionTree(Guid userId)
        {
            var permissionList = _sysMenuPermissionRepository.Where(e => !e.IsSysMenu && e.Type != (int)PermissionTypeEnum.Api && e.Type == (int)PermissionTypeEnum.Menu || e.Type == (int)PermissionTypeEnum.Query).ToList();
            var firstNode = permissionList.First(e => e.Pid == string.Empty).Adapt<PermissionNode>();
            firstNode.PermissionNodes = this.GetPermissionNodes(firstNode, permissionList);
            return firstNode;
        }


        /// <summary>
        /// 取管理员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetMGRPermissionList()
        {
            return _sysMenuPermissionRepository.Where(e => e.Type == (int)PermissionTypeEnum.Api).ProjectToType<SysMenuPermissionDto>().ToList();

        }

        /// <summary>
        /// 取操作员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>  GetOperatorPermissionList
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetOperatorPermissionList()
        {
            return _sysMenuPermissionRepository.Where(e => !e.IsSysMenu && e.Type == (int)PermissionTypeEnum.Api).ProjectToType<SysMenuPermissionDto>().ToList();
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

            return premissionQuery.ToList().Adapt<List<SysMenuPermissionDto>>();
        }

        /// <summary>
        /// 取操作员（栏目权限设置）的权限树
        /// </summary>
        /// <param name="userId"></param>  
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetOperatorPermissionListByColumnPerimission()
        {
            var columnKV = await _settingsService.GetColumnKVByUserKey(_tenantInfo.UserKey);
            var columnIDs = columnKV.Select(d => d.Key).ToList();
            //动态SQL
            Expression<Func<SysMenuPermission, bool>> pre = s => !s.DeleteFlag && !s.IsSysMenu && s.Type == (int)PermissionTypeEnum.Api && s.Permission.Length- s.Permission.Replace(":","").Length==2;
            Expression<Func<SysMenuPermission, bool>> preCol = s => 1 == 1;
            int i = 0;
            foreach (var columnID in columnIDs)
            {
                if (i == 0)
                    preCol = s => string.IsNullOrEmpty(s.Permission) ? true : s.Permission.EndsWith(columnID);
                else
                    preCol = preCol.Or(s => string.IsNullOrEmpty(s.Permission) ? true : s.Permission.EndsWith(columnID));
                i++;
            }
            preCol = preCol.And(s => !s.IsSysMenu);
            pre = pre.Or(preCol);
            return _sysMenuPermissionRepository.Where(pre).ProjectToType<SysMenuPermissionDto>().ToList();
        }

        /// <summary>
        /// 按用户角色以及栏目赋权获取用户权限树
        /// </summary>
        /// <returns></returns>
        public async Task<List<PermissionMenu>> GetUserUnionColumnPermissionTree()
        {
            int userRole = UserRole;
            IQueryable<SysMenuPermission> premissionQuery = null;
            if (userRole == 1)
            {
                premissionQuery = _sysMenuPermissionRepository.Where(c => !c.DeleteFlag && c.Type != (int)PermissionTypeEnum.Api);
            }
            else if (userRole == 2)
            {
                var columnKV = await _settingsService.GetColumnKVByUserKey(_tenantInfo.UserKey);
                var columnIDs = columnKV.Select(d => d.Key).ToList();
                //动态SQL
                Expression<Func<SysMenuPermission, bool>> pre = c => !c.DeleteFlag && c.Type != (int)PermissionTypeEnum.Api && !c.IsSysMenu && c.Visible;
                Expression<Func<SysMenuPermission, bool>> preCol = s => 1 == 1;
                int i = 0;
                foreach (var columnID in columnIDs)
                {
                    if (i == 0)
                        preCol = s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : true);
                    else
                        preCol = preCol.Or(s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : true));
                    i++;
                }
                pre = pre.And(preCol);
                premissionQuery = _sysMenuPermissionRepository.Where(pre);
            }
            else if (userRole == 3)
            {
                premissionQuery = _sysMenuPermissionRepository.Where(c => !c.DeleteFlag && (c.Type == (int)PermissionTypeEnum.Menu || c.Type == (int)PermissionTypeEnum.Query) && !c.IsSysMenu && c.Visible);
            }

            List<SysMenuPermission> sysMenus = premissionQuery.ToList();
            List<PermissionMenu> newList = sysMenus.Where(e => e.Pid == "1" && e.Visible).Adapt<List<PermissionMenu>>();
            newList.ForEach(d =>
            {
                List<PermissionMenu> permissions = sysMenus.Where(e => e.FullPath == $"{d.FullPath}-{e.Path}" && e.Type!= ((int)PermissionTypeEnum.Menu)).Adapt<List<PermissionMenu>>();
                d.ListPermission = permissions.Select(d => d.Permission).ToList();
                List<PermissionMenu> nodes = sysMenus.Where(e => e.FullPath == $"{d.FullPath}-{e.Path}" && e.Type == ((int)PermissionTypeEnum.Menu)).Adapt<List<PermissionMenu>>();
                foreach (var node in nodes)
                {
                    if (node.Type == ((int)PermissionTypeEnum.Menu))
                    {
                        List<PermissionMenu> childNodes = sysMenus.Where(e => e.FullPath == $"{node.FullPath}-{e.Path}").Adapt<List<PermissionMenu>>();
                        node.ListPermission = childNodes.Select(d => d.Permission).ToList();
                    }
                }
                d.ChildMenu = nodes;
            });
            return newList.OrderBy(d => d.Sort).ToList();
        }
    }
}
