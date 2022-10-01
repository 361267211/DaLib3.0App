
using Furion.DatabaseAccessor;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.DbContexts;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Application
{
    /// <summary>
    /// 数据库迁移的专用接口示例
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MigrationController : ControllerBase
    {
        private readonly DatabaseTerraceDbContext storeDbContext;
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysMenuCategory> _sysMenuCategoryRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;

        public MigrationController(
            DatabaseTerraceDbContext storeDbContext,
            IRepository<SysRole> sysRoleRepository,
            IRepository<SysMenuCategory> sysMenuCategoryRepository,
            IRepository<SysRoleMenu> sysRoleMenuRepository,
            IRepository<SysMenuPermission> sysMenuPermissionRepository,
            IRepository<SysUserRole> sysUserRoleRepository

            )
        {
            this.storeDbContext = storeDbContext;
   
            this.storeDbContext.Database.Migrate();
            _sysRoleRepository = sysRoleRepository;
            _sysMenuCategoryRepository = sysMenuCategoryRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
        }

        /// <summary>
        /// 多租户测试 查询多条Asset数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        public async Task InitializeBasicData(string name)
        {
            #region 插入基础数据
            var gu = Guid.NewGuid();
            if (!_sysMenuPermissionRepository.Any())
                //系统菜单及权限
                _sysMenuPermissionRepository.Context.BulkInsert(
                      new[]
                      {
/*                          //菜单
  new SysMenuPermission{Id=new Guid("784FBA89-42CB-0D4C-2601-D7BE7DC6A4BA"), Name="虚拟头部",  Remark="虚拟头部",Pid="",Type=(int)PermissionTypeEnum.Menu,Permission="api:database-terrace:database-terrace-list",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1" },
  new SysMenuPermission{Id=new Guid("34122400-7AEB-4BFD-4EE8-008FA6683450"), Name="数据库管理",Remark="数据库管理",Pid="1",Type=(int)PermissionTypeEnum.Menu,Permission="api:database-terrace:database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1"},

                        //btn
  new SysMenuPermission{Id=new Guid("C5E150FF-4490-357E-7CD1-3E1ED43D0E58"), Name="数据库列表",Remark="数据库列表",Pid="1-1",Type=(int)PermissionTypeEnum.Query,Permission="database-terrace:createdatabase",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-1-2"},
  new SysMenuPermission{Id=new Guid("41AC3AA0-2A1E-58C5-0511-CD91A8744880"), Name="数据库新增",Remark="数据库新增",Pid="1-1",Type=(int)PermissionTypeEnum.Operate ,Permission="database-terrace:editdatabase",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="3",FullPath="1-1-3"},
  new SysMenuPermission{Id=new Guid("509FF994-4601-539B-3B89-F2ABDA27B4BC"), Name="数据库删除",Remark="数据库删除",Pid="1-1",Type=(int)PermissionTypeEnum.Operate ,Permission="database-terrace:deletedatabase",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="4",FullPath="1-1-4"},
  new SysMenuPermission{Id=new Guid("680D5D0B-6652-D87F-B43C-95EF627A4F1A"), Name="数据库编辑",Remark="数据库编辑",Pid="1-1",Type=(int)PermissionTypeEnum.Operate,Permission="database-terrace:editdatabase",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="5",FullPath="1-1-5"},

                        //API
  new SysMenuPermission{Id=new Guid("35BDCAA0-4ADC-1B62-23D7-D9AEC95E2638"), Name="数据库列表接口",Remark="查询总览表",Pid="1-1-2",Type=(int)PermissionTypeEnum.Api,Permission="api:database-terrace:database-terrace-list",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-2-1"},
  new SysMenuPermission{Id=new Guid("F18B97BA-24FC-892F-E69C-774DDE2B7B7B"), Name="数据库保存接口",Remark="用于新增时",Pid="1-1-3",Type=(int)PermissionTypeEnum.Api,Permission="api:database-terrace:save-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-3-1"},
  new SysMenuPermission{Id=new Guid("BDB5C6B4-2B8D-900E-6312-5B73520A5F86"), Name="数据库保存接口",Remark="用于编辑时",Pid="1-1-3",Type=(int)PermissionTypeEnum.Api,Permission="api:database-terrace:save-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-1-3-2"},
  new SysMenuPermission{Id=new Guid("D5AFFE66-6D8E-7A37-1F9D-CBFA125CA071"), Name="数据库删除接口",Remark="数据库删除",Pid="1-1-4",Type=(int)PermissionTypeEnum.Api,Permission="api:database-terrace:batch-delete-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-4-1"},*/

  new SysMenuPermission{Id=Guid.NewGuid(), Name="应用设置查询",  Remark=" ",Pid="1-4",Type=3,Permission="AppSettings:Query",Router="路由地址",Component="组件地址",IsSysMenu=true,Path="1",FullPath="1-4-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库批量推荐",  Remark=" ",Pid="1-1-5",Type=5,Permission="api:database-terrace:batch-recommend-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-5-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:模型字段获取(编辑)",  Remark=" ",Pid="1-1-2",Type=5,Permission="api:database-terrace:database-initialized-model",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="3",FullPath="1-1-2-3" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航栏目列表",  Remark=" ",Pid="1-2",Type=3,Permission="DatabaseColumn:Query",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-2-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="数据库推荐",  Remark=" ",Pid="1-1",Type=4,Permission="Database:Recommend",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="5",FullPath="1-1-5" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="数据库恢复",  Remark=" ",Pid="1-1",Type=4,Permission="Database:Recover",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="6",FullPath="1-1-6" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="数据库删除",  Remark=" ",Pid="1-1",Type=4,Permission="Database:Delete",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="4",FullPath="1-1-4" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="数据库排序",  Remark=" ",Pid="1-1",Type=4,Permission="Database:Sort",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="7",FullPath="1-1-7" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航栏目删除",  Remark=" ",Pid="1-2",Type=4,Permission="DatabaseColumn:Delete",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-2-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库保存接口",  Remark=" ",Pid="1-1-3",Type=5,Permission="api:database-terrace:save-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-3-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航属性学科分类列表",  Remark=" ",Pid="1-3",Type=3,Permission="NavAttribute:DomainQuery",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-3-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航栏目新增",  Remark=" ",Pid="1-2",Type=4,Permission="DatabaseColumn:Add",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="3",FullPath="1-2-3" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航栏目编辑",  Remark=" ",Pid="1-2",Type=4,Permission="DatabaseColumn:Edit",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="4",FullPath="1-2-4" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航属性学科分类编辑",  Remark=" ",Pid="1-3",Type=4,Permission="NavAttribute:DomainEdit",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-3-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:模型字段获取(新增)",  Remark=" ",Pid="1-1-3",Type=5,Permission="api:database-terrace:database-initialized-model",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="3",FullPath="1-1-3-3" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库删除接口",  Remark=" ",Pid="1-1-4",Type=5,Permission="api:database-terrace:batch-delete-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-4-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库列表接口",  Remark=" ",Pid="1-1-2",Type=5,Permission="api:database-terrace:database-terrace-list",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-2-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="数据库新增",  Remark=" ",Pid="1-1",Type=4,Permission="Database:Add",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="3",FullPath="1-1-3" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库保存接口",  Remark=" ",Pid="1-1-3",Type=5,Permission="api:database-terrace:save-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="4",FullPath="1-1-3-4" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库批量删除",  Remark=" ",Pid="1-1-4",Type=5,Permission="api:database-terrace:batch-delete-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-1-4-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航属性访问地址列表",  Remark=" ",Pid="1-3",Type=3,Permission="NavAttribute:UrlQuery",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="5",FullPath="1-3-5" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航属性访问地址删除",  Remark=" ",Pid="1-3",Type=4,Permission="NavAttribute:UrlDelete",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="6",FullPath="1-3-6" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="应用设置编辑",  Remark=" ",Pid="1-4",Type=4,Permission="AppSettings:Edit",Router="路由地址",Component="组件地址",IsSysMenu=true,Path="2",FullPath="1-4-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库批量恢复",  Remark=" ",Pid="1-1-6",Type=5,Permission="api:database-terrace:batch-recover-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-6-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库拖拽排序",  Remark=" ",Pid="1-1-7",Type=5,Permission="api:database-terrace:sort-database-by-drag",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-7-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库弹窗排序",  Remark=" ",Pid="1-1-7",Type=5,Permission="api:database-terrace:sort-source-from-import-by-destination",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-1-7-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航栏目列表",  Remark=" ",Pid="1-2-1",Type=5,Permission="api:database-terrace:database-columns",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-2-1-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库保存(新增)",  Remark=" ",Pid="1-1-3",Type=5,Permission="api:database-terrace:save-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-1-3-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="虚拟头部",  Remark=" ",Pid="",Type=1,Permission="api:database-terrace:database-terrace-list",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="数据库列表",  Remark=" ",Pid="1-1",Type=3,Permission="Database:Query",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航属性标签列表",  Remark=" ",Pid="1-3",Type=3,Permission="NavAttribute:LabelQuery",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="3",FullPath="1-3-3" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="数据库编辑",  Remark=" ",Pid="1-1",Type=4,Permission="Database:Edit",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-1-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航属性标签删除",  Remark=" ",Pid="1-3",Type=4,Permission="NavAttribute:LabelDelete",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="4",FullPath="1-3-4" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:查数据库列表",  Remark=" ",Pid="1-1-1",Type=5,Permission="api:database-terrace:database-terrace-list",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-1-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库保存(编辑)",  Remark=" ",Pid="1-1-2",Type=5,Permission="api:database-terrace:save-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-1-2-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航栏目删除",  Remark=" ",Pid="1-2-2",Type=5,Permission="api:database-terrace:batch-delete-database-column",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-2-2-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航栏目保存(新增)",  Remark=" ",Pid="1-2-3",Type=5,Permission="api:database-terrace:save-database-column",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-2-3-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:数据库详情",  Remark=" ",Pid="1-1-2",Type=5,Permission="api:database-terrace:database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="4",FullPath="1-1-2-4" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航栏目预览(新增)",  Remark=" ",Pid="1-2-3",Type=5,Permission="api:database-terrace:database-column-preview",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-2-3-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航栏目查找",  Remark=" ",Pid="1-2-4",Type=5,Permission="api:database-terrace:database-column",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-2-4-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航栏目保存(编辑)",  Remark=" ",Pid="1-2-4",Type=5,Permission="api:database-terrace:save-database-column",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-2-4-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航属性管理",  Remark=" ",Pid="1",Type=1,Permission="NavAttribute:ManageMenu",Router="/admin_databaseAttr",Component="组件地址",IsSysMenu=false,Path="3",FullPath="1-3" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="总导航管理",  Remark=" ",Pid="1",Type=1,Permission="Database:ManageMenu",Router="/admin_databaseNav",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航属性学科分类树查询",  Remark=" ",Pid="1-3-1",Type=5,Permission="api:database-terrace:domain-esc-tree",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-3-1-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航属性学科分类保存",  Remark=" ",Pid="1-3-2",Type=5,Permission="api:database-terrace:save-domain-esc-dtos",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-3-2-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航属性标签列表查看",  Remark=" ",Pid="1-3-3",Type=5,Permission="api:database-terrace:coustom-labels",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-3-3-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航属性标签删除",  Remark=" ",Pid="1-3-4",Type=5,Permission="api:database-terrace:batch-delete-coustom-label",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-3-4-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航属性地址名称列表查看",  Remark=" ",Pid="1-3-5",Type=5,Permission="api:database-terrace:acess-url-name",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-3-5-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航属性地址名称删除",  Remark=" ",Pid="1-3-6",Type=5,Permission="api:database-terrace:batch-delete-acess-url-name",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-3-6-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:应用设置查询",  Remark=" ",Pid="1-4-1",Type=5,Permission="api:database-terrace:database-settings",Router="路由地址",Component="组件地址",IsSysMenu=true,Path="1",FullPath="1-4-1-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:应用设置保存",  Remark=" ",Pid="1-4-2",Type=5,Permission="api:database-terrace:save-database-settings",Router="路由地址",Component="组件地址",IsSysMenu=true,Path="1",FullPath="1-4-2-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航栏目预览(编辑)",  Remark=" ",Pid="1-2-4",Type=5,Permission="api:database-terrace:database-column-preview",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="3",FullPath="1-2-4-3" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航属性本馆在用学科分类查询",  Remark=" ",Pid="1-3-1",Type=5,Permission="api:database-terrace:domain-escs",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-3-1-2" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航属性标签编辑",  Remark=" ",Pid="1-3",Type=4,Permission="NavAttribute:LabelEdit",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="7",FullPath="1-3-7" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:导航属性标签编辑",  Remark=" ",Pid="1-3-7",Type=5,Permission="api:database-terrace:save-custom-labels",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-3-7-1" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:获取中心站数据库模型(快速新增)",  Remark=" ",Pid="1-1-3",Type=5,Permission="api:database-terrace:database-from-center-as-model",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="5",FullPath="1-1-3-5" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:获取用户分组列表",  Remark=" ",Pid="1-1-3",Type=5,Permission="api:database-terrace:user-group-infos",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="6",FullPath="1-1-3-6" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:获取专辑信息",  Remark=" ",Pid="1-1-3",Type=5,Permission="api:database-terrace:album-from-center",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="7",FullPath="1-1-3-7" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="Api:获取中心站数据库列表",  Remark=" ",Pid="1-1-3",Type=5,Permission="api:database-terrace:database-from-center",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="8",FullPath="1-1-3-8" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="应用设置",  Remark=" ",Pid="1",Type=1,Permission="AppSettings:ManageMenu",Router="/admin_databaseSet",Component="组件地址",IsSysMenu=true,Path="4",FullPath="1-4" },
  new SysMenuPermission{Id=Guid.NewGuid(), Name="导航栏目管理",  Remark=" ",Pid="1",Type=1,Permission="DatabaseColumn:ManageMenu",Router="/admin_databaseNavigation",Component="组件地址",IsSysMenu=false,Path="2",FullPath="1-2" },
                       });
            //系统角色
            var roleId = Guid.NewGuid();
            if (!_sysRoleRepository.Any())
                _sysRoleRepository.Context.BulkInsert
                     (new[]{
                       new SysRole{ Name="测试员",Remark="测试员",Id=roleId,Code="sys_tester_role" },

                     });
            //角色-用户关系
            if (!_sysUserRoleRepository.Any())
                _sysRoleRepository.Context.BulkInsert
                     (new[]{
                       new SysUserRole{ Id=Guid.NewGuid(),RoleID=roleId,UserID=new Guid("d4c5aaba-1a53-4e5c-a4a5-d0661f1b4567") },

                     });
            //角色-菜单权限 关系
            if (!_sysRoleMenuRepository.Any())
                _sysRoleRepository.Context.BulkInsert
                     (new[]{

                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("784FBA89-42CB-0D4C-2601-D7BE7DC6A4BA") },
                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("34122400-7AEB-4BFD-4EE8-008FA6683450") },
                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("C5E150FF-4490-357E-7CD1-3E1ED43D0E58") },
                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("41AC3AA0-2A1E-58C5-0511-CD91A8744880") },
                       new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("509FF994-4601-539B-3B89-F2ABDA27B4BC") },
                     });

            #endregion

        }



    }
}
