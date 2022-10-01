
using Furion.DatabaseAccessor;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLibrary.LogAnalysis.Common.Dtos;
using SmartLibrary.LogAnalysis.EntityFramework.Core.DbContexts;
using SmartLibrary.LogAnalysis.EntityFramework.Core.Dtos;
using SmartLibrary.LogAnalysis.EntityFramework.Core.Entitys;
using SmartLibrary.LogAnalysis.EntityFramework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.LogAnalysis.Application
{
    /// <summary>
    /// 数据库迁移的专用接口示例
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MigrationController : ControllerBase
    {
        private readonly AssetDbContext storeDbContext;
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysMenuCategory> _sysMenuCategoryRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;

        public MigrationController(
            AssetDbContext storeDbContext,
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
        /// 初始话数据，本接口示范  向基础表插入基础数据
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
                          //菜单
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
  new SysMenuPermission{Id=new Guid("D5AFFE66-6D8E-7A37-1F9D-CBFA125CA071"), Name="数据库删除接口",Remark="数据库删除",Pid="1-1-4",Type=(int)PermissionTypeEnum.Api,Permission="api:database-terrace:batch-delete-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path="1",FullPath="1-1-4-1"},




  //new SysMenuPermission{Name="数据库推荐接口",Remark="推荐数据库",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:batch-recommend-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="数据库恢复接口",Remark="恢复数据库",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:batch-recover-database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="数据库查询接口",Remark="查询数据库",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-terrace",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查初始模型接口",Remark="查初始模型",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-initialized-model",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查学科分类接口",Remark="查学科分类",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:domain-escs",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="存学科分类接口",Remark="存学科分类",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-domain-esc-dtos",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="所有数据库栏目接口",Remark="查所有数据库栏目",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-columns",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="数据库栏目接口",Remark="查数据库栏目",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-column",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="存数据库栏目接口",Remark="存数据库栏目",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-database-column",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查栏目预览接口",Remark="查栏目预览",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-column-preview",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查应用设置接口",Remark="查应用设置",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-settings",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="存应用设置接口",Remark="存应用设置",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-database-settings",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查自定义标签接口",Remark="查自定义标签",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-database-settings",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="删自定义标签接口",Remark="删自定义标签",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:batch-delete-coustom-label",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="存自定义标签接口",Remark="存自定义标签",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-custom-labels",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},  
  //new SysMenuPermission{Name="查链接名称接口",Remark="查链接名称",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:acess-url-name",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="删链接名称接口",Remark="删链接名称",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:batch-delete-acess-url-name",Router="路由地址",Component="组件地址",IsSysMenu=false,Path=2,FullPath="1;2;9"},

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

        [HttpPost]
        public async Task InitializeBasicData([FromBody] SysRoleDto sysRoleDto)
        {

        }
    }
}
