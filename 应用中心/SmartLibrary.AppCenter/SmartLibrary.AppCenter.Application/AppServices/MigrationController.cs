using Furion.DatabaseAccessor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter.Application.Services.Application;
using SmartLibrary.AppCenter.EntityFramework.Core.DbContexts;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission;
using SmartLibrary.AppCenter.EntityFramework.Core.Enum;
using SmartLibrary.Search.EsSearchProxy.Core.Dto;
using SmartLibrary.Search.EsSearchProxy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartLibrary.AppCenter.Common.Utility;
using TinyPinyin;
using System.Collections;

namespace SmartLibrary.AppCenter.Application.AppServices
{
    /// <summary>
    /// 数据库迁移的专用接口示例
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MigrationController : ControllerBase
    {
        private readonly AppCenterDbContext _StoreDbContext;
        private readonly IRepository<AppCenterSettings> _RepositoryAppCenterSettings;
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysMenuCategory> _sysMenuCategoryRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;
        private readonly IEsProxyService _EsProxyService;
        private readonly IApplicationService _ApplicationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="storeDbContext"></param>
        /// <param name="repositoryAppCenterSettings"></param>
        /// <param name="sysRoleRepository"></param>
        /// <param name="sysMenuCategoryRepository"></param>
        /// <param name="sysRoleMenuRepository"></param>
        /// <param name="sysMenuPermissionRepository"></param>
        /// <param name="sysUserRoleRepository"></param>
        /// <param name="esProxyService"></param>
        /// <param name="applicationService"></param>
        public MigrationController(AppCenterDbContext storeDbContext,
                                   IRepository<AppCenterSettings> repositoryAppCenterSettings,
                                   IRepository<SysRole> sysRoleRepository,
                                   IRepository<SysMenuCategory> sysMenuCategoryRepository,
                                   IRepository<SysRoleMenu> sysRoleMenuRepository,
                                   IRepository<SysMenuPermission> sysMenuPermissionRepository,
                                   IRepository<SysUserRole> sysUserRoleRepository,
                                   IEsProxyService esProxyService,
                                   IApplicationService applicationService)
        {
            _StoreDbContext = storeDbContext;
            _RepositoryAppCenterSettings = repositoryAppCenterSettings;
            _sysRoleRepository = sysRoleRepository;
            _sysMenuCategoryRepository = sysMenuCategoryRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
            _EsProxyService = esProxyService;
            _ApplicationService = applicationService;
        }


        /// <summary>
        /// 升级数据库结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<bool> UpdateDatabaseSchema()
        {
            _StoreDbContext.Database.Migrate();
            return Task.FromResult(true);
        }

        /// <summary>
        /// 初始话数据，本接口示范  向基础表插入基础数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [UnitOfWork]
        public async Task<int> InitializeBasicData()
        {
            #region 插入基础数据
            var sysMenuPermisson = new List<SysMenuPermission>()
            {
                new SysMenuPermission{Id=Guid.NewGuid(),Name="顶级节点",Remark="虚拟节点",Pid="",Type=(int)PermissionTypeEnum.Dir,Path="0",FullPath="0" },

                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用信息管理",Type=(int)PermissionTypeEnum.Menu,Router="/admin_appInfo",IsSysMenu=false,Pid="0",Path="1",FullPath="0-1",Sort=1,Category="3" },
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用信息管理",Type=(int)PermissionTypeEnum.Query,Permission="appinfomanage",IsSysMenu=false,Pid="1",Path="1",FullPath="0-1-1",Sort=1,Category="3"},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="所有应用",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getallapp",Pid="1",Path="2",FullPath="0-1-2",Sort=1,Category="3"},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="更新日志",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getapplogs",Pid="1",Path="3",FullPath="0-1-3",Sort=1,Category="3"},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="日志详情",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getapplogdetail",Pid="1",Path="4",FullPath="0-1-4",Sort=1,Category="3"},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用到期列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getappexpire",Pid="1",Path="5",FullPath="0-1-5",Sort=1,Category="3"},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getappsearchlist",Pid="1",Path="6",FullPath="0-1-6",Sort=1,Category="3"},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用详情",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getappdetail",Pid="1",Path="7",FullPath="0-1-7",Sort=1,Category="3"},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="付费推荐应用",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getpayapplist",Pid="1",Path="8",FullPath="0-1-8",Sort=1,Category="3"},

                new SysMenuPermission{Id=Guid.NewGuid(),Name="管理权限设置",Type=(int)PermissionTypeEnum.Menu,Router="/admin_privilegeSet",IsSysMenu=false,Pid="0",Path="2",FullPath="0-2",Sort=2 },
                new SysMenuPermission{Id=Guid.NewGuid(),Name="管理员授权列表",Type=(int)PermissionTypeEnum.Query,Permission="authlist",IsSysMenu=false,Pid="2",Path="1",FullPath="0-2-1",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="角色列表",Type=(int)PermissionTypeEnum.Query,Permission="rolelist",IsSysMenu=false,Pid="2",Path="2",FullPath="0-2-2",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="角色新增",Type=(int)PermissionTypeEnum.Operate,Permission="add",IsSysMenu=false,Pid="2",Path="3",FullPath="0-2-3",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="角色编辑",Type=(int)PermissionTypeEnum.Operate,Permission="edit",IsSysMenu=false,Pid="2",Path="4",FullPath="0-2-4",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="角色删除",Type=(int)PermissionTypeEnum.Operate,Permission="delete",IsSysMenu=false,Pid="2",Path="5",FullPath="0-2-5",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="权限设置",Type=(int)PermissionTypeEnum.Operate,Permission="set",IsSysMenu=false,Pid="2",Path="6",FullPath="0-2-6",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="管理权限列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getmanagerlist",Pid="2",Path="7",FullPath="0-2-7",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="管理权限详情",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getmanagerinfodetail",Pid="8",Path="5",FullPath="0-2-8",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="管理员授权",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:grantmanager",Pid="2",Path="9",FullPath="0-2-9",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="角色新增",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:insertrole",Pid="2",Path="10",FullPath="0-2-10",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="角色编辑",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:updaterole",Pid="2",Path="11",FullPath="0-2-11",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="角色删除",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:deleterole",Pid="2",Path="12",FullPath="0-2-12",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="角色详情",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getroledetail",Pid="2",Path="13",FullPath="0-2-13",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="角色列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getrolelists",Pid="2",Path="14",FullPath="0-2-14",Sort=2},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="全部角色",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getallrole",Pid="2",Path="15",FullPath="0-2-15",Sort=2},

                new SysMenuPermission{Id=Guid.NewGuid(),Name="用户权限设置",Type=(int)PermissionTypeEnum.Menu,Router="/admin_permissionSet",IsSysMenu=false,Pid="0",Path="3",FullPath="0-3",Sort=3 },
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用授权列表",Type=(int)PermissionTypeEnum.Query,Permission="appauthlist",IsSysMenu=false,Pid="3",Path="1",FullPath="0-3-1",Sort=3},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="读者授权列表",Type=(int)PermissionTypeEnum.Query,Permission="userauthlist",IsSysMenu=false,Pid="3",Path="2",FullPath="0-3-2",Sort=3},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用授权",Type=(int)PermissionTypeEnum.Operate,Permission="appauth",IsSysMenu=false,Pid="3",Path="3",FullPath="0-3-3",Sort=3},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="用户授权",Type=(int)PermissionTypeEnum.Operate,Permission="userauth",IsSysMenu=false,Pid="3",Path="4",FullPath="0-3-4",Sort=3},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用授权列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getappuserbyapp",Pid="3",Path="5",FullPath="0-3-5",Sort=3},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="读者授权列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getappuserbyuser",Pid="3",Path="6",FullPath="0-3-6",Sort=3},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="权限设置",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:updateappuser",Pid="3",Path="7",FullPath="0-3-7",Sort=3},

                new SysMenuPermission{Id=Guid.NewGuid(),Name="付费应用推荐",Type=(int)PermissionTypeEnum.Menu,Router="/admin_recommend",IsSysMenu=true,Pid="0",Path="4",FullPath="0-4",Sort=5 },
                new SysMenuPermission{Id=Guid.NewGuid(),Name="付费应用推荐列表",Type=(int)PermissionTypeEnum.Query,Permission="payapplist",IsSysMenu=false,Pid="4",Path="1",FullPath="0-4-1",Sort=5},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="付费应用列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getpayapplist",Pid="4",Path="2",FullPath="0-4-2",Sort=5},

                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用订单管理",Type=(int)PermissionTypeEnum.Menu,Router="/admin_orderManage",IsSysMenu=true,Pid="0",Path="5",FullPath="0-5",Sort=6 },
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用订单列表",Type=(int)PermissionTypeEnum.Query,Permission="orderlist",IsSysMenu=false,Pid="5",Path="1",FullPath="0-5-1",Sort=6},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="订单列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getorderlist",Pid="5",Path="2",FullPath="0-5-2",Sort=6},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="订单取消",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:cancelorder",Pid="5",Path="3",FullPath="0-5-3",Sort=6},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="订单操作",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:appaction",Pid="5",Path="4",FullPath="0-5-4",Sort=6},

                new SysMenuPermission{Id=Guid.NewGuid(),Name="导航栏目管理",Type=(int)PermissionTypeEnum.Menu,Router="/admin_directoryMessage",IsSysMenu=true,Pid="0",Path="6",FullPath="0-6",Sort=8 },
                new SysMenuPermission{Id=Guid.NewGuid(),Name="导航栏目列表",Type=(int)PermissionTypeEnum.Query,Permission="columnlist",IsSysMenu=false,Pid="6",Path="1",FullPath="0-6-1",Sort=8},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="栏目新增",Type=(int)PermissionTypeEnum.Operate,Permission="add",IsSysMenu=false,Pid="6",Path="2",FullPath="0-6-2",Sort=8},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="栏目编辑",Type=(int)PermissionTypeEnum.Operate,Permission="edit",IsSysMenu=false,Pid="6",Path="3",FullPath="0-6-3",Sort=8},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="栏目删除",Type=(int)PermissionTypeEnum.Operate,Permission="delete",IsSysMenu=false,Pid="6",Path="4",FullPath="0-6-4",Sort=8},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="栏目列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getnavgationitems",Pid="6",Path="5",FullPath="0-6-5",Sort=8},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="栏目删除",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:deletenavgation",Pid="6",Path="6",FullPath="0-6-6",Sort=8},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="栏目详情",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getnavgationdetail",Pid="6",Path="7",FullPath="0-6-7",Sort=8},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="栏目编辑",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:updatenavigationitem",Pid="6",Path="8",FullPath="0-6-8",Sort=8},

                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用注册",Type=(int)PermissionTypeEnum.Menu,Router="/admin_thirdparty",IsSysMenu=false,Pid="0",Path="7",FullPath="0-7",Sort=4 },
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用列表",Type=(int)PermissionTypeEnum.Query,Permission="thirdapplist",IsSysMenu=false,Pid="7",Path="1",FullPath="0-7-1",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用新增",Type=(int)PermissionTypeEnum.Operate,Permission="add",IsSysMenu=false,Pid="7",Path="2",FullPath="0-7-2",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用修改",Type=(int)PermissionTypeEnum.Operate,Permission="edit",IsSysMenu=false,Pid="7",Path="3",FullPath="0-7-3",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用删除",Type=(int)PermissionTypeEnum.Operate,Permission="delete",IsSysMenu=false,Pid="7",Path="4",FullPath="0-7-4",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用列表",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getthirdapplist",Pid="7",Path="5",FullPath="0-7-5",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用获取应用类型",Type=(int)PermissionTypeEnum.Api,Permission="get-api:general:getapptype",Pid="7",Path="6",FullPath="0-7-6",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用获取终端类型",Type=(int)PermissionTypeEnum.Api,Permission="get-api:general:getterminaltype",Pid="7",Path="7",FullPath="0-7-7",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用获取推荐图标",Type=(int)PermissionTypeEnum.Api,Permission="get-api:general:getrecommendicons",Pid="7",Path="8",FullPath="0-7-8",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用详情",Type=(int)PermissionTypeEnum.Api,Permission="get-api:application:getthirdappdetail",Pid="7",Path="9",FullPath="0-7-9",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用编辑",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:updatethirdappinfo",Pid="7",Path="10",FullPath="0-7-10",Sort=4},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="三方应用删除",Type=(int)PermissionTypeEnum.Api,Permission="post-api:application:deletethirdapp",Pid="7",Path="11",FullPath="0-7-11",Sort=4},

                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用中心设置",Type=(int)PermissionTypeEnum.Menu,Router="/admin_appsSet",IsSysMenu=true,Pid="0",Path="8",FullPath="0-8",Sort=7 },
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用中心设置",Type=(int)PermissionTypeEnum.Query,Permission="appcenterset",IsSysMenu=false,Pid="8",Path="1",FullPath="0-8-1",Sort=7},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="应用中心设置编辑",Type=(int)PermissionTypeEnum.Operate,Permission="edit",IsSysMenu=false,Pid="8",Path="2",FullPath="0-8-2",Sort=7},
                new SysMenuPermission{Id=Guid.NewGuid(),Name="获取设置信息",Type=(int)PermissionTypeEnum.Api,Permission="get-api:applicationsetting:getapplicationsetting",Pid="8",Path="3",FullPath="0-8-3",Sort=7},
            };

            //系统菜单及权限
            if (!_sysMenuPermissionRepository.Any())
            {
                await _sysMenuPermissionRepository.Context.BulkInsertAsync(sysMenuPermisson);
            }

            #endregion
            if (!_RepositoryAppCenterSettings.Any())
            {
                await _StoreDbContext.BulkInsertAsync(
                 new[] {
                    new AppCenterSettings {
                        Id = Guid.NewGuid(), ItemKey = "IsShowChargeApp", ItemName = "BaseConfig", ItemValue = "1",
                        DeleteFlag = false, CreatedTime = DateTimeOffset.Now, UpdatedTime = DateTimeOffset.Now },
                    new AppCenterSettings
                    {
                       Id=Guid.NewGuid(),ItemKey = "IsNeedLogin", ItemName = "BaseConfig", ItemValue = "1",
                        DeleteFlag = false, CreatedTime = DateTimeOffset.Now, UpdatedTime = DateTimeOffset.Now
                    }
                 });
            }

            return 1;
        }

        /// <summary>
        /// 向ES插入应用信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> InsertEsAppInfo(string owner)
        {
            var allApps = await _ApplicationService.GetAllApp();

            allApps = allApps.FindAll(c => c.IsThirdApp || (c.AppEntranceList != null && c.AppEntranceList.Any(x => x.IsDefault && x.IsSystem && x.UseScene == 1)));

            List<UpsertOwnerNewsRequestParameter> list = new();

            foreach (var item in allApps)
            {
                var urlArray = item.FrontUrl.Split('#');
                list.Add(new UpsertOwnerNewsRequestParameter
                {
                    app_id = item.RouteCode.IsEmptyOrWhiteSpace() ? PinyinHelper.GetPinyin(item.AppName, "").ToLower() : item.RouteCode,
                    app_type = Search.EsSearchProxy.Core.Models.OrganNewsType.Service,
                    click_count = 20,
                    docId = "app_" + item.AppId.Replace("-", "_"),
                    fulltext = item.Content,
                    keyword = new string[] { item.AppName },
                    owner = owner,
                    pub_time = item.CreateTime.ToDateTimeOffset(),
                    summary = item.Content,
                    title = item.AppName,
                    update_time = item.UpdateTime.ToDateTimeOffset(),
                    url = item.IsThirdApp ? item.FrontUrl : (urlArray.Length == 1 ? item.FrontUrl : "/#/" + item.FrontUrl.Split('#')[1].TrimStart('/'))
                });
            }

            list.ForEach(async c =>
            {
                var temp = await _EsProxyService.UpsertOrganNewsAsync(c);
            });

            return true;
        }
    }
}
