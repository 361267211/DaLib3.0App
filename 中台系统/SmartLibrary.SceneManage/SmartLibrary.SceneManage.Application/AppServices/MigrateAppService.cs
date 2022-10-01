
using Furion.DatabaseAccessor;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLibrary.SceneManage.Common.AssemblyBase;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.EntityFramework.Core.DbContexts;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;
using SmartLibrary.SceneManage.EntityFramework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Application
{
    /// <summary>
    /// 数据库迁移的专用接口示例
    /// </summary>
    public class MigrateAppService : BaseAppService
    {
        private readonly SceneManageDbContext _storeDbContext;
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysMenuCategory> _sysMenuCategoryRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;
        private readonly IRepository<Template> _templateRepository;

        public MigrateAppService(
            SceneManageDbContext storeDbContext,
            IRepository<SysRole> sysRoleRepository,
            IRepository<SysMenuCategory> sysMenuCategoryRepository,
            IRepository<SysRoleMenu> sysRoleMenuRepository,
            IRepository<SysMenuPermission> sysMenuPermissionRepository,
            IRepository<SysUserRole> sysUserRoleRepository,
            IRepository<Template> templateRepository
            )
        {
            _storeDbContext = storeDbContext;
            _sysRoleRepository = sysRoleRepository;
            _sysMenuCategoryRepository = sysMenuCategoryRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
            _templateRepository = templateRepository;
        }


        /// <summary>
        /// 升级数据库结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> UpdateDatabaseSchema()
        {
            _storeDbContext.Database.Migrate();
            return Task.FromResult(true);
        }

        /// <summary>
        /// 初始话数据，本接口示范  向基础表插入基础数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task InitializeBasicData(string name)
        {
            #region 插入基础数据
            var gu = Guid.NewGuid();
            //if (!_sysMenuPermissionRepository.Any())
                //系统菜单及权限
                _sysMenuPermissionRepository.Context.BulkInsert(
                      new[]
                      {
                          //菜单
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="场景管理",  Remark="场景管理",Pid="",Path="1",FullPath="1",Type=(int)PermissionTypeEnum.Menu,Permission="",Router="/caseShow",Component="",IsSysMenu=false,Sort=1 },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="场景总览",  Remark="场景总览",Pid="1",Path="1",FullPath="1-1",Type=(int)PermissionTypeEnum.Query,Permission="scene-manage_list",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="获取场景总览API",  Remark="获取场景总览API",Pid="1-1",Path="1",FullPath="1-1-1",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:scene-overview",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="获取场景字典API",  Remark="获取场景字典API",Pid="1-1",Path="2",FullPath="1-1-2",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:dictionary",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="获取模板列表API",  Remark="获取模板列表API",Pid="1-1",Path="3",FullPath="1-1-3",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:template-list",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="按终端获取场景列表API",  Remark="按终端获取场景列表API",Pid="1-1",Path="4",FullPath="1-1-4",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:scene-list-by-terminal-id",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="按服务类型获取应用列表API",  Remark="按服务类型获取应用列表API",Pid="1-1",Path="5",FullPath="1-1-5",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:app-list-by-service-type",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="获取应用栏目列表API",  Remark="获取应用栏目列表API",Pid="1-1",Path="6",FullPath="1-1-6",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:app-plate-list-by-app-id",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="获取应用组件列表API",  Remark="获取应用组件列表API",Pid="1-1",Path="7",FullPath="1-1-7",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:app-widget-list-by-app-id",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="按类型获取场景字典API",  Remark="按类型获取场景字典API",Pid="1-1",Path="8",FullPath="1-1-8",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:dictionary-by-type",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="获取当前场景访问地址API",  Remark="获取当前场景访问地址API",Pid="1-1",Path="9",FullPath="1-1-9",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:scene-url-by-id",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="新建场景",  Remark="新建场景",Pid="1",Path="2",FullPath="1-2",Type=(int)PermissionTypeEnum.Operate,Permission="scene-manage_add",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="新建场景API",  Remark="新建场景API",Pid="1-2",Path="1",FullPath="1-2-1",Type=(int)PermissionTypeEnum.Api,Permission="post|api:scene-manage:scene",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="编辑场景",  Remark="编辑场景",Pid="1",Path="3",FullPath="1-3",Type=(int)PermissionTypeEnum.Operate,Permission="scene-manage_edit",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="编辑场景API",  Remark="编辑场景API",Pid="1-3",Path="1",FullPath="1-3-1",Type=(int)PermissionTypeEnum.Api,Permission="put|api:scene-manage:scene",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="删除场景",  Remark="删除场景",Pid="1",Path="4",FullPath="1-4",Type=(int)PermissionTypeEnum.Operate,Permission="scene-manage_delete",Router="",Component="",IsSysMenu=true },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="删除场景API",  Remark="删除场景API",Pid="1-4",Path="1",FullPath="1-4-1",Type=(int)PermissionTypeEnum.Api,Permission="delete|api:scene-manage:scene",Router="",Component="",IsSysMenu=true },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="启用禁用场景",  Remark="启用禁用场景",Pid="1",Path="5",FullPath="1-5",Type=(int)PermissionTypeEnum.Operate,Permission="scene-manage_disable",Router="",Component="",IsSysMenu=true },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="启用禁用场景API",  Remark="启用禁用场景API",Pid="1-5",Path="1",FullPath="1-5-1",Type=(int)PermissionTypeEnum.Api,Permission="put|api:scene-manage:change-scene-status",Router="",Component="",IsSysMenu=true },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="预览场景",  Remark="预览场景",Pid="1",Path="5",FullPath="1-5",Type=(int)PermissionTypeEnum.Query,Permission="scene-manage_preview",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="预览场景API",  Remark="预览场景API",Pid="1-6",Path="1",FullPath="1-6-1",Type=(int)PermissionTypeEnum.Menu,Permission="get|api:scene-manage:scene-detail",Router="",Component="",IsSysMenu=false },

                          new SysMenuPermission{Id=Guid.NewGuid(), Name="栏目管理",  Remark="栏目管理",Pid="",Path="2",FullPath="2",Type=(int)PermissionTypeEnum.Menu,Permission="",Router="/programManage",Component="",IsSysMenu=false ,Sort=2},
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="栏目列表",  Remark="栏目列表",Pid="2",Path="1",FullPath="2-1",Type=(int)PermissionTypeEnum.Query,Permission="scene-manage_columnlist",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="栏目列表API",  Remark="栏目列表API",Pid="2-1",Path="1",FullPath="2-1-1",Type=(int)PermissionTypeEnum.Api,Permission="get|api:scene-manage:app-plate-list-by-scene-id",Router="",Component="",IsSysMenu=false },

                          new SysMenuPermission{Id=Guid.NewGuid(), Name="终端管理",  Remark="终端管理",Pid="",Path="3",FullPath="3",Type=(int)PermissionTypeEnum.Menu,Permission="",Router="/terminalManage",Component="",IsSysMenu=false ,Sort=3},
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="终端列表",  Remark="终端列表",Pid="3",Path="1",FullPath="3-1",Type=(int)PermissionTypeEnum.Query,Permission="terminal_list",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="终端列表API",  Remark="终端列表API",Pid="3-1",Path="1",FullPath="3-1-1",Type=(int)PermissionTypeEnum.Api,Permission="get|api:terminal:terminal-instance-list",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="获取终端实例详情API",  Remark="获取终端实例详情API",Pid="3-1",Path="2",FullPath="3-1-2",Type=(int)PermissionTypeEnum.Api,Permission="get|api:terminal:terminal-instance-detail",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="获取下拉框字典API",  Remark="获取下拉框字典API",Pid="3-1",Path="3",FullPath="3-1-3",Type=(int)PermissionTypeEnum.Api,Permission="get|api:terminal:dictionary",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="添加终端实例",  Remark="添加终端实例",Pid="3",Path="2",FullPath="3-2",Type=(int)PermissionTypeEnum.Operate,Permission="terminal_add",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="添加终端实例API",  Remark="添加终端实例API",Pid="3-2",Path="1",FullPath="3-2-1",Type=(int)PermissionTypeEnum.Api,Permission="post|api:terminal:terminal-instance",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="编辑终端实例",  Remark="编辑终端实例",Pid="3",Path="3",FullPath="3-3",Type=(int)PermissionTypeEnum.Operate,Permission="terminal_edit",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="编辑终端实例API",  Remark="编辑终端实例API",Pid="3",Path="1",FullPath="3-3-1",Type=(int)PermissionTypeEnum.Api,Permission="put|api:terminal:terminal-instance",Router="",Component="",IsSysMenu=false },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="删除终端实例",  Remark="删除终端实例",Pid="3",Path="4",FullPath="3-4",Type=(int)PermissionTypeEnum.Operate,Permission="terminal_delete",Router="",Component="",IsSysMenu=true },
                          new SysMenuPermission{Id=Guid.NewGuid(), Name="删除终端实例API",  Remark="删除终端实例API",Pid="3",Path="1",FullPath="3-4-1",Type=(int)PermissionTypeEnum.Api,Permission="delete|api:terminal:terminal-instance",Router="",Component="",IsSysMenu=true },

  
  
  
  //new SysMenuPermission{Id=new Guid("34122400-7AEB-4BFD-4EE8-008FA6683450"), Name="数据库管理",Remark="数据库管理",Pid="1",Type=(int)PermissionTypeEnum.Menu,Permission="api:database-terrace:database-terrace",Router="",Component="",IsSysMenu=false,Path="1",FullPath="1-1"},

  //                      //btn
  //new SysMenuPermission{Id=new Guid("C5E150FF-4490-357E-7CD1-3E1ED43D0E58"), Name="数据库列表",Remark="数据库列表",Pid="1-1",Type=(int)PermissionTypeEnum.Query,Permission="database-terrace:createdatabase",Router="",Component="",IsSysMenu=false,Path="2",FullPath="1-1-2"},
  //new SysMenuPermission{Id=new Guid("41AC3AA0-2A1E-58C5-0511-CD91A8744880"), Name="数据库新增",Remark="数据库新增",Pid="1-1",Type=(int)PermissionTypeEnum.Operate ,Permission="database-terrace:editdatabase",Router="",Component="",IsSysMenu=false,Path="3",FullPath="1-1-3"},
  //new SysMenuPermission{Id=new Guid("509FF994-4601-539B-3B89-F2ABDA27B4BC"), Name="数据库删除",Remark="数据库删除",Pid="1-1",Type=(int)PermissionTypeEnum.Operate ,Permission="database-terrace:deletedatabase",Router="",Component="",IsSysMenu=false,Path="4",FullPath="1-1-4"},
  //new SysMenuPermission{Id=new Guid("680D5D0B-6652-D87F-B43C-95EF627A4F1A"), Name="数据库编辑",Remark="数据库编辑",Pid="1-1",Type=(int)PermissionTypeEnum.Operate,Permission="database-terrace:editdatabase",Router="",Component="",IsSysMenu=false,Path="5",FullPath="1-1-5"},

  //                      //API
  //new SysMenuPermission{Id=new Guid("35BDCAA0-4ADC-1B62-23D7-D9AEC95E2638"), Name="数据库列表接口",Remark="查询总览表",Pid="1-1-2",Type=(int)PermissionTypeEnum.Api,Permission="api:database-terrace:database-terrace-list",Router="",Component="",IsSysMenu=false,Path="1",FullPath="1-1-2-1"},
  //new SysMenuPermission{Id=new Guid("F18B97BA-24FC-892F-E69C-774DDE2B7B7B"), Name="数据库保存接口",Remark="用于新增时",Pid="1-1-3",Type=(int)PermissionTypeEnum.Api,Permission="api:database-terrace:save-database-terrace",Router="",Component="",IsSysMenu=false,Path="1",FullPath="1-1-3-1"},
  //new SysMenuPermission{Id=new Guid("BDB5C6B4-2B8D-900E-6312-5B73520A5F86"), Name="数据库保存接口",Remark="用于编辑时",Pid="1-1-3",Type=(int)PermissionTypeEnum.Api,Permission="api:database-terrace:save-database-terrace",Router="",Component="",IsSysMenu=false,Path="2",FullPath="1-1-3-2"},
  //new SysMenuPermission{Id=new Guid("D5AFFE66-6D8E-7A37-1F9D-CBFA125CA071"), Name="数据库删除接口",Remark="数据库删除",Pid="1-1-4",Type=(int)PermissionTypeEnum.Api,Permission="api:database-terrace:batch-delete-database-terrace",Router="",Component="",IsSysMenu=false,Path="1",FullPath="1-1-4-1"},




  //new SysMenuPermission{Name="数据库推荐接口",Remark="推荐数据库",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:batch-recommend-database-terrace",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="数据库恢复接口",Remark="恢复数据库",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:batch-recover-database-terrace",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="数据库查询接口",Remark="查询数据库",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-terrace",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查初始模型接口",Remark="查初始模型",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-initialized-model",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查学科分类接口",Remark="查学科分类",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:domain-escs",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="存学科分类接口",Remark="存学科分类",Pid="",Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-domain-esc-dtos",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="所有数据库栏目接口",Remark="查所有数据库栏目",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-columns",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="数据库栏目接口",Remark="查数据库栏目",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-column",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="存数据库栏目接口",Remark="存数据库栏目",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-database-column",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查栏目预览接口",Remark="查栏目预览",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-column-preview",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查应用设置接口",Remark="查应用设置",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:database-settings",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="存应用设置接口",Remark="存应用设置",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-database-settings",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="查自定义标签接口",Remark="查自定义标签",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-database-settings",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="删自定义标签接口",Remark="删自定义标签",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:batch-delete-coustom-label",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="存自定义标签接口",Remark="存自定义标签",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:save-custom-labels",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},  
  //new SysMenuPermission{Name="查链接名称接口",Remark="查链接名称",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:acess-url-name",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},
  //new SysMenuPermission{Name="删链接名称接口",Remark="删链接名称",Pid=2,Id=9,Type=(int)PremissionTypeEnum.Api,Permission="api:database-terrace:batch-delete-acess-url-name",Router="",Component="",IsSysMenu=false,Path=2,FullPath="1;2;9"},

                       });
            //系统角色
            //var roleId = Guid.NewGuid();
            //if (!_sysRoleRepository.Any())
            //    _sysRoleRepository.Context.BulkInsert
            //         (new[]{
            //           new SysRole{ Name="测试员",Remark="测试员",Id=roleId,Code="sys_tester_role" },

            //         });
            ////角色-用户关系
            //if (!_sysUserRoleRepository.Any())
            //    _sysRoleRepository.Context.BulkInsert
            //         (new[]{
            //           new SysUserRole{ Id=Guid.NewGuid(),RoleID=roleId,UserID=new Guid("d4c5aaba-1a53-4e5c-a4a5-d0661f1b4567") },

            //         });
            ////角色-菜单权限 关系
            //if (!_sysRoleMenuRepository.Any())
            //    _sysRoleRepository.Context.BulkInsert
            //         (new[]{

            //           new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("784FBA89-42CB-0D4C-2601-D7BE7DC6A4BA") },
            //           //new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("34122400-7AEB-4BFD-4EE8-008FA6683450") },
            //           //new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("C5E150FF-4490-357E-7CD1-3E1ED43D0E58") },
            //           //new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("41AC3AA0-2A1E-58C5-0511-CD91A8744880") },
            //           //new SysRoleMenu{ Id=Guid.NewGuid(),RoleID=roleId,MenuPermissionID=new Guid("509FF994-4601-539B-3B89-F2ABDA27B4BC") },
            //         });
            #endregion

        }


        /// <summary>
        /// 初始化测试数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task InitializeTestData()
        {
            _templateRepository.Context.BulkInsert(
                  new[]
                  {
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="头部一",Router="/header_sys/temp1",Type=2},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="头部二",Router="/header_sys/temp2",Type=2},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="底部一",Router="/footer_sys/temp1",Type=3},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="底部二",Router="/footer_sys/temp2",Type=3},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="通屏模板一",Router="/",Type=1, ScreenCount=1,IsLock=false,ColumnCount=1},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="通屏模板二",Router="/",Type=1, ScreenCount=1,IsLock=true,ColumnCount=2},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="分屏模板一",Router="/",Type=1, ScreenCount=1,IsLock=false,ColumnCount=1},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="分屏模板二",Router="/",Type=1, ScreenCount=3,IsLock=true,ColumnCount=2},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="通屏定宽模板一",Router="/",Type=1, ScreenCount=1,IsLock=false,ColumnCount=1},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="通屏定宽模板二",Router="/",Type=1, ScreenCount=1,IsLock=true,ColumnCount=2},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="分屏定宽模板一",Router="/",Type=1, ScreenCount=1,IsLock=false,ColumnCount=1},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="分屏定宽模板二",Router="/",Type=1, ScreenCount=3,IsLock=true,ColumnCount=2},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="头部三",Router="/header_sys/temp3",Type=2},
                         //new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="1440-浅灰",Router="/",Type=1, ScreenCount=1,IsLock=true,ColumnCount=2,BackgroundColor="#f5f5f5",Width=1440},
                         new Template{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Name="头部四",Router="/cqu/header_sys/temp1",Type=2},

                  });

        }




    }
}
