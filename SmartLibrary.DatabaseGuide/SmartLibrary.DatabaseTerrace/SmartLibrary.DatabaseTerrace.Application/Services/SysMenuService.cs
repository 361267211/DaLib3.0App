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
using Furion.LinqBuilder;
using Mapster;
using SmartLibrary.AppCenter;
using SmartLibrary.Assembly.EntityFramework.Core.Enum;
using SmartLibrary.DatabaseTerrace.Application.Interceptors;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Application.Services
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
        public async Task<List<SysMenuPermissionDto>> GetMGRApiPermissionList()
        {
            return _sysMenuPermissionRepository.Where(e => e.Type == (int)PermissionTypeEnum.Api).ProjectToType<SysMenuPermissionDto>().ToList();

        }

        /// <summary>
        /// 取管理员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetMGRPermissionList()
        {
            return _sysMenuPermissionRepository.Where(e => e.Type != (int)PermissionTypeEnum.Api).ProjectToType<SysMenuPermissionDto>().ToList();

        }

        /// <summary>
        /// 取操作员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>  GetOperatorPermissionList
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetOperatorApiPermissionList()
        {
            var premissionQuery = from c in _sysMenuPermissionRepository.AsQueryable(e => !e.IsSysMenu && (e.Type == (int)PermissionTypeEnum.Operate|| e.Type == (int)PermissionTypeEnum.Query) )
                                  join d in _sysMenuPermissionRepository.AsQueryable(e => !e.IsSysMenu )
                                  on 1 equals 1
                                  where d.Type == (int)PermissionTypeEnum.Api
                                  where d.FullPath == (c.FullPath + "-" + d.Path)
                                  select d;

            return premissionQuery.ToList().Adapt<List<SysMenuPermissionDto>>();
        }

        /// <summary>
        /// 取操作员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>  GetOperatorPermissionList
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetOperatorPermissionList()
        {
            var premissionQuery = from c in _sysMenuPermissionRepository.AsQueryable(e => !e.IsSysMenu && (e.Type == (int)PermissionTypeEnum.Operate || e.Type == (int)PermissionTypeEnum.Query || e.Type == (int)PermissionTypeEnum.Menu))
                                  join d in _sysMenuPermissionRepository.AsQueryable(e => !e.IsSysMenu && (e.Type == (int)PermissionTypeEnum.Operate || e.Type == (int)PermissionTypeEnum.Query || e.Type == (int)PermissionTypeEnum.Menu)) 
                                  on 1 equals 1
                                  where d.Type != (int)PermissionTypeEnum.Api
                                  where d.FullPath == (c.FullPath + "-" + d.Path)
                                  select d;

            return premissionQuery.ToList().Adapt<List<SysMenuPermissionDto>>();
        }

        /// <summary>
        /// 取浏览者（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetVisitorsApiPermissionList()
        {
            var premissionQuery = from c in _sysMenuPermissionRepository.AsQueryable(e => !e.IsSysMenu && e.Type == (int)PermissionTypeEnum.Query)
                                  join d in _sysMenuPermissionRepository.AsQueryable() on 1 equals 1
                                  where d.Type == (int)PermissionTypeEnum.Api
                                  where d.FullPath == (c.FullPath + "-" + d.Path)
                                  select d;

            return premissionQuery.ToList().Adapt<List<SysMenuPermissionDto>>();
        }

        /// <summary>
        /// 取浏览者（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetVisitorsPermissionList()
        {
            var premissionQuery = from c in _sysMenuPermissionRepository.AsQueryable(e => !e.IsSysMenu &&( e.Type == (int)PermissionTypeEnum.Query || e.Type == (int)PermissionTypeEnum.Menu ))
                                  join d in _sysMenuPermissionRepository.AsQueryable(e => !e.IsSysMenu && (e.Type == (int)PermissionTypeEnum.Query || e.Type == (int)PermissionTypeEnum.Menu)) on 1 equals 1
                                  where d.Type != (int)PermissionTypeEnum.Api
                                  where d.FullPath == (c.FullPath + "-" + d.Path)
                                  select d;

            return premissionQuery.ToList().Adapt<List<SysMenuPermissionDto>>();
        }


        /// <summary>
        /// 按用户角色以及栏目赋权获取用户权限树
        /// </summary>
        /// <returns></returns>
        public async Task<List<PermissionMenu>> GetUserUnionColumnPermissionTree()
        {
            //模拟操作员
            int userRole = 3;
            IQueryable<SysMenuPermission> premissionQuery = null;
            if (userRole == 1)
            {
                premissionQuery = _sysMenuPermissionRepository.Where(c => !c.DeleteFlag && c.Type != (int)PermissionTypeEnum.Api);
            }
            else if (userRole == 2)
            {

                var pre = LinqExpression.Create<SysMenuPermission>(c => !c.DeleteFlag && c.Type != (int)PermissionTypeEnum.Api && !c.IsSysMenu && c.Visible);
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
                List<PermissionMenu> permissions = sysMenus.Where(e => e.FullPath == $"{d.FullPath}-{e.Path}").Adapt<List<PermissionMenu>>();
                d.ListPermission = permissions.Select(d => d.Permission).ToList();
            });
            return newList.OrderBy(d => d.Sort).ToList();
        }

        public async Task<PermissionNode> GetUserPermissionTree(string guid)
        {
            //1.根据用户的身份信息查询拥有的权限

            //2.根据用户信息取他的通用角色  1管理员/2操作员/3浏览者  TODO:暂时伪造数据
            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            UserAppPermissionTypeRequest request1 = new UserAppPermissionTypeRequest { AppId = "databaseguide" };
            UserAppPermissionTypeReply reply1 = new UserAppPermissionTypeReply();
            try
            {
                reply1 = await grpcClient1.GetUserAppPermissionTypeAsync(request1);
            }
            catch (Exception)
            {
                reply1 = new UserAppPermissionTypeReply { PermissionType = 1 };
            }


            UserRoleEnum userRoleType = (UserRoleEnum)reply1.PermissionType;

            //3.根据角色类型取用户的权限列表
            List<SysMenuPermissionDto> rolePermissionDtoList = new List<SysMenuPermissionDto>();
            switch (userRoleType)
            {
                case UserRoleEnum.Manager:
                    rolePermissionDtoList = await this.GetMGRPermissionList();
                    break;
                case UserRoleEnum.Operator:
                    rolePermissionDtoList = await this.GetOperatorPermissionList();
                    break;
                case UserRoleEnum.Visitor:
                    rolePermissionDtoList = await this.GetVisitorsPermissionList();
                    break;
                default:
                    break;
            }

            //4.根据colpermission表取当前用户的栏目权限
            List<SysMenuPermissionDto> colPermissionDtoList = new List<SysMenuPermissionDto>();
            //colPermissionDtoList = await this.GetUserColumnPermissionList(userKey);


            //5.合并两类权限形成总集
            List<SysMenuPermissionDto> totalPermissionDtoList = new List<SysMenuPermissionDto>();
            totalPermissionDtoList.AddRange(rolePermissionDtoList);
            totalPermissionDtoList.AddRange(colPermissionDtoList);


            //6.组装成树
            PermissionNode firstNode = await this.GetFirstNode();
            firstNode.PermissionNodes = this.GetPermissionNodes(firstNode, totalPermissionDtoList.Adapt<List<SysMenuPermission>>());

            //7.菜单节点排序
            firstNode.PermissionNodes = firstNode.PermissionNodes.OrderBy(e => e.Sort).ToList();

            return firstNode;
        }


        private async Task<PermissionNode> GetFirstNode()
        {
            var entity = await _sysMenuPermissionRepository.FirstOrDefaultAsync(e => e.Pid == "");
            return entity.Adapt<PermissionNode>();
        }

    }
}
