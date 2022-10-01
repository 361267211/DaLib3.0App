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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.News.EntityFramework.Core.Dtos;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using SmartLibrary.News.Application.Enums;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using SmartLibrary.News.Application.Dto.Permission;
using System.Linq.Expressions;
using SmartLibrary.News.Utility;
using SmartLibrary.News.Application.Interceptors;
using SmartLibrary.AppCenter;

namespace SmartLibrary.News.Application.Services
{
    public class SysMenuService : ISysMenuService, IScoped
    {
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysMenuCategory> _sysMenuCategoryRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;
        private readonly INewsSettingsService _settingsService;
        private readonly int UserRole =2;//1 管理者，2 操作者，3 浏览者
        private TenantInfo _tenantInfo;

        public SysMenuService(

            IRepository<SysRole> sysRoleRepository,
            IRepository<SysMenuCategory> sysMenuCategoryRepository,
            IRepository<SysRoleMenu> sysRoleMenuRepository,
            IRepository<SysMenuPermission> sysMenuPermissionRepository,
            IRepository<SysUserRole> sysUserRoleRepository,
            INewsSettingsService settingsService,
            TenantInfo tenantInfo)
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
        public async Task<PermissionNode> GetMGRPermissionTree(string userId)
        {
            return await this.GetAllPermissionTree();

        }

        /// <summary>
        /// 取操作员（默认角色）的权限树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<PermissionNode> GetOperatorPermissionTree(string userId)
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
        public async Task<PermissionNode> GetVisitorsPermissionTree(string userId)
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
        /// <param name="userId"></param> 
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
        /// 获取默认栏目及其下属浏览操作权限
        /// </summary>
        /// <returns></returns>
        public async Task<PermissionNode> GetDefaultColumnTempleteMenuPermission()
        {
            var permissionList = _sysMenuPermissionRepository.Where(e => !e.IsSysMenu && e.Type != (int)PermissionTypeEnum.Api && e.FullPath.StartsWith("1-3")).ToList();
            var firstNode = permissionList.First(e => e.Type == ((int)PermissionTypeEnum.Menu)).Adapt<PermissionNode>();
            firstNode.PermissionNodes = this.GetPermissionNodes(firstNode, permissionList);
            return await Task.FromResult(firstNode);
        }

        /// <summary>
        /// 取操作员（栏目权限设置）的权限树
        /// </summary>
        /// <param name="userId"></param>  
        /// <returns></returns>
        public async Task<List<SysMenuPermissionDto>> GetOperatorPermissionListByColumnPerimission()
        {
            var columnIDs = await _settingsService.GetColumnIDsByUserKey();
            columnIDs = columnIDs.Distinct().ToList();
            //动态SQL
            Expression<Func<SysMenuPermission, bool>> pre = s => !s.DeleteFlag && !s.IsSysMenu && s.Type == (int)PermissionTypeEnum.Api && s.Permission.Length - s.Permission.Replace(":", "").Length == 2;
            Expression<Func<SysMenuPermission, bool>> preCol = s => 1==1;
            int i = 0;
            foreach (var columnID in columnIDs)
            {
                var colAuditPower = await _settingsService.GetColumnAuditPowerListByUserKey(columnID);
                if (colAuditPower.Contains(AuditPowerEunm.Manage) || (colAuditPower.Contains(AuditPowerEunm.Edit) && !colAuditPower.Contains(AuditPowerEunm.Manage) && colAuditPower.Count>1)) //包含管理权限 或者 包含编辑并且包含任一审核权限
                {
                    if (i == 0)
                        preCol = s => string.IsNullOrEmpty(s.Permission) ? true : s.Permission.EndsWith(columnID);
                    else
                        preCol = preCol.Or(s => string.IsNullOrEmpty(s.Permission) ? true : s.Permission.EndsWith(columnID));
                }
                else 
                {
                    if (colAuditPower.Contains(AuditPowerEunm.Edit))//编辑权限排除审核API
                    {
                        if (i == 0)
                            preCol = s => string.IsNullOrEmpty(s.Permission) ? true : s.Permission.EndsWith(columnID) && !s.Permission.Contains("news-content-update-audit-status");
                        else
                            preCol = preCol.Or(s => string.IsNullOrEmpty(s.Permission) ? true : s.Permission.EndsWith(columnID) && !s.Permission.Contains("news-content-update-audit-status"));
                    }
                    if (!colAuditPower.Contains(AuditPowerEunm.Edit))//审核权限只获取审核API
                    {
                        if (i == 0)
                            preCol = s => string.IsNullOrEmpty(s.Permission) ? true : s.Permission.EndsWith($"news-content-update-audit-status:{columnID}");
                        else
                            preCol = preCol.Or(s => string.IsNullOrEmpty(s.Permission) ? true : s.Permission.EndsWith($"news-content-update-audit-status:{columnID}"));
                    }
                }
                i++;
            }
            preCol = preCol.And(s => !s.IsSysMenu);
            pre = pre.Or(preCol);
            return _sysMenuPermissionRepository.Where(pre).ProjectToType<SysMenuPermissionDto>().ToList();
        }

        /// <summary>
        /// 按用户角色以及新闻栏目赋权获取用户权限树
        /// </summary>
        /// <returns></returns>
        public async Task<List<PermissionMenu>> GetUserUnionColumnPermissionTreeOld()
        {
            var userRoles = _sysUserRoleRepository.Where(d => d.UserID == _tenantInfo.UserKey).ToList();
            Guid roleID = Guid.Empty;
            if (userRoles.Count() == 0)
            {
                roleID = _sysRoleRepository.FirstOrDefault(d => !d.DeleteFlag && d.Name == "栏目操作人员").Id;
            }
            else
            {
                roleID = userRoles.FirstOrDefault().RoleID;
            }

            IQueryable<SysMenuPermission> premissionQuery = from b in _sysRoleMenuRepository.AsQueryable(d=>d.RoleID== roleID) 
                                                            join c in _sysMenuPermissionRepository.AsQueryable() on b.MenuPermissionID equals c.Id
                                                            where c.Type != (int)PermissionTypeEnum.Api
                                                            select c;

            List<SysMenuPermission> sysMenus = premissionQuery.ToList();
            List<PermissionMenu> newList= sysMenus.Where(e => e.Pid == "1" && e.Visible).Adapt<List<PermissionMenu>>();
            newList.ForEach(d =>
            {
                List<PermissionMenu> nodes = sysMenus.Where(e => e.FullPath == $"{d.FullPath}-{e.Path}").Adapt<List<PermissionMenu>>();
                d.ListPermission = nodes.Select(d => d.Permission).ToList();
                if (d.Name == "应用设置")
                {
                    d.Component = "/newsSet";
                    d.Sort = 99;
                }
                if (d.Name == "栏目管理")
                {
                    d.Component = "/newsProgram";
                    d.Sort = 1;
                }
            });
            var node = sysMenus.FirstOrDefault(e => e.FullPath == "1-3").Adapt<PermissionNode>();
            var nodeChildNodes = GetPermissionNodes(node, sysMenus);
            bool isDefaultRole = false;
            if (_sysRoleRepository.FirstOrDefault(d => d.Id == roleID).Code == "sys_default_role")
                isDefaultRole = true;
            var columnKV = _settingsService.GetColumnKVBuUserKey(isDefaultRole?"":_tenantInfo.UserKey).Result;
            int i = 1;
            foreach (var item in columnKV)
            {
                i++;
                var nodeTemp = sysMenus.FirstOrDefault(e => e.FullPath == "1-3").Adapt<PermissionMenu>(); 
                nodeTemp.Name = item.Value;
                nodeTemp.Component = $"/newsInfo?id={item.Key}";
                nodeTemp.Sort = i;
                if (isDefaultRole)
                {
                    nodeTemp.ListPermission = nodeChildNodes.Select(d => d.Permission).ToList();
                    newList.Add(nodeTemp);
                    continue;
                }

                var colAuditPower = await _settingsService.GetColumnAuditPowerListByUserKey(item.Key);
                if (colAuditPower.Contains(AuditPowerEunm.Manage) || colAuditPower.Contains(AuditPowerEunm.Edit))
                {
                    nodeTemp.ListPermission = nodeChildNodes.Select(d => d.Permission).ToList();
                }
                //栏目审核权限
                var listTemp = nodeChildNodes.Where(d => d.Permission == "audit" || d.Permission == "query").ToList();
                foreach (var power in colAuditPower)
                {
                    switch (power)
                    {
                        case AuditPowerEunm.Manage://前面已优先处理
                            break;
                        case AuditPowerEunm.Edit:
                            break;
                        case AuditPowerEunm.PreliminaryAudit:
                            nodeTemp.ListPermission.AddRange(listTemp.Select(d => d.Permission).ToList());
                            break;
                        case AuditPowerEunm.PreliminaryCheck:
                            nodeTemp.ListPermission.AddRange(listTemp.Select(d => d.Permission).ToList());
                            break;
                        case AuditPowerEunm.SecondAudit:
                            nodeTemp.ListPermission.AddRange(listTemp.Select(d => d.Permission).ToList());
                            break;
                        case AuditPowerEunm.SecondCheck:
                            nodeTemp.ListPermission.AddRange(listTemp.Select(d => d.Permission).ToList());
                            break;
                        case AuditPowerEunm.FinallyAudit:
                            nodeTemp.ListPermission.AddRange(listTemp.Select(d => d.Permission).ToList());
                            break;
                        case AuditPowerEunm.FinallyCheck:
                            nodeTemp.ListPermission.AddRange(listTemp.Select(d => d.Permission).ToList());
                            break;
                        case AuditPowerEunm.Publish:
                            nodeTemp.ListPermission.AddRange(listTemp.Select(d => d.Permission).ToList());
                            break;
                        default:
                            break;
                    }
                }
                
                nodeTemp.ListPermission = nodeTemp.ListPermission.Distinct().ToList();
                newList.Add(nodeTemp);
            }
            return newList.OrderBy(d=>d.Sort).ToList();
        }

        /// <summary>
        /// 按用户角色以及栏目赋权获取用户权限树
        /// </summary>
        /// <returns></returns>
        public async Task<List<PermissionMenu>> GetUserUnionColumnPermissionTree()
        {
            //1.根据用户的身份信息查询拥有的权限

            //2.根据用户信息取他的通用角色  1管理员/2操作员/3浏览者  TODO:暂时伪造数据
            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            UserAppPermissionTypeRequest request1 = new UserAppPermissionTypeRequest { AppId = "news" };
            UserAppPermissionTypeReply reply1 = new UserAppPermissionTypeReply();
            try
            {
                reply1 = await grpcClient1.GetUserAppPermissionTypeAsync(request1);
            }
            catch (Exception)
            {
                // throw Oops.Oh("grpc调用异常");
                reply1 = new UserAppPermissionTypeReply { PermissionType = 1 };
            }


            int userRole = reply1.PermissionType;
            IQueryable<SysMenuPermission> premissionQuery = null;
            if (userRole == 1)
            {
                premissionQuery = _sysMenuPermissionRepository.Where(c => !c.DeleteFlag && c.Type != (int)PermissionTypeEnum.Api);
            }
            else if (userRole == 2)
            {
                var columnIDs = await _settingsService.GetColumnIDsByUserKey();
                columnIDs = columnIDs.Distinct().ToList();
                //动态SQL
                Expression<Func<SysMenuPermission, bool>> pre = c => !c.DeleteFlag && c.Type != (int)PermissionTypeEnum.Api && !c.IsSysMenu && c.Visible;
                Expression<Func<SysMenuPermission, bool>> preCol = s => 1 == 1;
                int i = 0;
                foreach (var columnID in columnIDs)
                {
                    //if (i == 0)
                    //    preCol = s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : true);
                    //else
                    //    preCol = preCol.Or(s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : true));

                    var colAuditPower = await _settingsService.GetColumnAuditPowerListByUserKey(columnID);
                    if (colAuditPower.Contains(AuditPowerEunm.Manage) || (colAuditPower.Contains(AuditPowerEunm.Edit) && !colAuditPower.Contains(AuditPowerEunm.Manage) && colAuditPower.Count > 1)) //包含管理权限 或者 包含编辑并且包含任一审核权限
                    {
                        if (i == 0)
                            preCol = s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : true);
                        else
                            preCol = preCol.Or(s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : true));
                    }
                    else
                    {
                        if (colAuditPower.Contains(AuditPowerEunm.Edit))//编辑权限排除审核操作
                        {
                            if (i == 0)
                                preCol = s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : !s.Permission.Contains("audit"));
                            else
                                preCol = preCol.Or(s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : !s.Permission.Contains("audit")));
                        }
                        if (!colAuditPower.Contains(AuditPowerEunm.Edit))//审核权限只获取审核(及浏览)操作
                        {
                            if (i == 0)
                                preCol = s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : (s.Permission.Contains("audit") || s.Permission.Contains("query")));
                            else
                                preCol = preCol.Or(s => string.IsNullOrEmpty(s.Permission) ? true : (s.Permission.Contains(":") ? s.Permission.EndsWith(columnID) : (s.Permission.Contains("audit") || s.Permission.Contains("query"))));
                        }
                    }
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
                List<PermissionMenu> permissions = sysMenus.Where(e => e.FullPath == $"{d.FullPath}-{e.Path}").Adapt<List<PermissionMenu>>();
                d.ListPermission = permissions.Select(d => d.Permission).ToList();
            });
            return newList.OrderBy(d => d.Sort).ThenBy(e=>e.Name).ToList();
        }
    }
}
