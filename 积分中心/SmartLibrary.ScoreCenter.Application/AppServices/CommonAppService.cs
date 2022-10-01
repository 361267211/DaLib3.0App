/*********************************************************
* 名    称：CommonAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：数据迁移初始化操作
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SmartLibrary.DtmClient.Dtm.Tcc;
using SmartLibrary.ScoreCenter.Application.AppServices;
using SmartLibrary.ScoreCenter.Application.Dtos.Common;
using SmartLibrary.ScoreCenter.Application.Services.Enum;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.EntityFramework.Core.DbContexts;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application
{
    /// <summary>
    /// 数据库迁移的专用接口示例
    /// </summary>
    [Authorize(Policy = PolicyKey.TokenAuth)]
    public class CommonAppService : BaseAppService
    {
        private readonly ScoreCenterDbContext _dbContext;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly TenantInfo _tenantInfo;
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly TccGlobalTransaction _globalTransaction;
        private readonly IAppGatewayEnsureService _appGatewayEnsureService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CommonAppService(ScoreCenterDbContext dbContext
            , IRepository<SysMenuPermission> sysMenuPermissionRepository
            , TenantInfo tenantInfo
            , IDistributedIDGenerator idGenerator
            , TccGlobalTransaction globalTransaction
            , IAppGatewayEnsureService appGatewayEnsureService
            , IHttpContextAccessor httpContextAccessor
            )
        {
            this._dbContext = dbContext;
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
            _tenantInfo = tenantInfo;
            _idGenerator = idGenerator;
            _globalTransaction = globalTransaction;
            _appGatewayEnsureService = appGatewayEnsureService;
            _httpContextAccessor = httpContextAccessor;

        }

        /// <summary>
        /// 租户数据迁移
        /// </summary>
        /// <returns></returns>
        public async Task UpdateDatabaseSchema()
        {
            await _dbContext.Database.MigrateAsync();
            //初始化权限配置
            await InitPermissionMenu();
        }

        #region 数据初始化
        private async Task InitPermissionMenu()
        {
            var fullMenu = new List<SysMenuPermission>
            {
                //Path同一层级内重新编码
                new SysMenuPermission{Id=new Guid(),Name="顶级节点",Remark="虚拟节点",Pid="",Type=(int)EnumPermissionType.Dir,Path="0",FullPath=".0."},
                #region 积分配置
                new SysMenuPermission{Id=new Guid("0E3F7EAB-5812-28B8-89A5-72082C65D10F"), Name="积分配置",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_integralWork",Component="",IsSysMenu=false,Path="1",FullPath=".0.1."},

                new SysMenuPermission{Id=new Guid("93A8F8F6-500C-9168-9640-903B72FE4A42"), Name="积分任务列表",Pid=".0.1.",Type=(int)EnumPermissionType.Query,Permission="config:scoreObtainTaskList",IsSysMenu=false,Path="1",FullPath=".0.1.1."},
                new SysMenuPermission{Id=new Guid("ED76BE56-0051-D625-85CA-74CA78A75207"), Name="积分任务初始化数据",Pid=".0.1.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-obtain-task:init-data", Path="1",FullPath=".0.1.1.1."},
                new SysMenuPermission{Id=new Guid("0FAE459C-2B67-CD2D-072D-E50ED8703FCB"), Name="积分任务列表数据",Pid=".0.1.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-obtain-task:list-data", Path="2",FullPath=".0.1.1.2."},

                new SysMenuPermission{Id=new Guid("B0936A1E-D9FC-3203-F24D-176902415CCF"), Name="积分任务新增",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreObtainTaskAdd",IsSysMenu=false,Path="2",FullPath=".0.1.2."},
                new SysMenuPermission{Id=new Guid("EE0B7DE3-6435-1870-A2AF-DEBA1BA90909"), Name="积分任务初始化数据",Pid=".0.1.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-obtain-task:init-data", Path="1",FullPath=".0.1.2.1."},
                new SysMenuPermission{Id=new Guid("46DF3C10-D854-1A92-2DA7-88C49AC391F3"), Name="通过应用编码获取积分事件",Pid=".0.1.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-obtain-task:score-events-by-app-code:*", Path="2",FullPath=".0.1.2.2."},
                new SysMenuPermission{Id=new Guid("5C1F949F-44A9-C0BD-FDBF-D66142BB1D4B"), Name="积分任务创建",Pid=".0.1.2.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-obtain-task", Path="3",FullPath=".0.1.2.3."},

                new SysMenuPermission{Id=new Guid("44AB177D-7418-9EF0-EF28-186A12B22BAE"), Name="积分任务编辑",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreObtainTaskEdit",IsSysMenu=false,Path="3",FullPath=".0.1.3."},
                new SysMenuPermission{Id=new Guid("402EC28B-31A0-6792-AD2C-7E0B906D8256"), Name="积分任务初始化数据",Pid=".0.1.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-obtain-task:init-data", Path="1",FullPath=".0.1.3.1."},
                new SysMenuPermission{Id=new Guid("49AD20D5-1878-09E4-D86C-09262AFD2F6E"), Name="通过应用编码获取积分事件",Pid=".0.1.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-obtain-task:score-events-by-app-code:*", Path="2",FullPath=".0.1.3.2."},
                new SysMenuPermission{Id=new Guid("3FD3C4DD-F5ED-C49E-D1F5-A4C68E6AF682"), Name="积分任务详情",Pid=".0.1.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-obtain-task:by-id:*", Path="3",FullPath=".0.1.3.3."},
                new SysMenuPermission{Id=new Guid("D74F6AF0-CDE4-F369-EA6B-B842A7D30C07"), Name="积分任务更新",Pid=".0.1.3.",Type=(int)EnumPermissionType.Api,Permission="put-api:score-obtain-task", Path="4",FullPath=".0.1.3.4."},

                new SysMenuPermission{Id=new Guid("E57E2D1E-F0C8-DF60-3ACD-94B7E1C56874"), Name="积分任务删除",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreObtainTaskDelete",IsSysMenu=false,Path="4",FullPath=".0.1.4."},
                new SysMenuPermission{Id=new Guid("6A3870FF-CBC9-6113-93A4-DA532A1A80AD"), Name="积分任务删除",Pid=".0.1.4.",Type=(int)EnumPermissionType.Api,Permission="delete-api:score-obtain-task:*", Path="1",FullPath=".0.1.4.1."},

                new SysMenuPermission{Id=new Guid("844E2EDF-419C-2DF6-8596-A4BD93461069"), Name="积分任务状态设置",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreObtainTaskStatus",IsSysMenu=false,Path="5",FullPath=".0.1.5."},
                new SysMenuPermission{Id=new Guid("8C8A4B8E-4704-413B-1F2C-205A8E60863A"), Name="积分任务状态设置",Pid=".0.1.5.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-obtain-task:set-active-status", Path="1",FullPath=".0.1.5.1."},

                new SysMenuPermission{Id=new Guid("123F0D23-BE3A-2380-9C44-57E4FF604C75"), Name="积分消耗列表",Pid=".0.1.",Type=(int)EnumPermissionType.Query,Permission="config:scoreConsumeTaskList",IsSysMenu=false,Path="6",FullPath=".0.1.6."},
                new SysMenuPermission{Id=new Guid("1197B280-78B3-BF50-B232-4B3AE1787A55"), Name="积分消耗初始化数据",Pid=".0.1.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-consume-task:init-data", Path="1",FullPath=".0.1.6.1."},
                new SysMenuPermission{Id=new Guid("FB30B386-0A4F-BF73-8AF5-FC3D6677C92F"), Name="积分消耗列表数据",Pid=".0.1.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-consume-task:list-data", Path="2",FullPath=".0.1.6.2."},

                new SysMenuPermission{Id=new Guid("6411B960-718D-D17E-0A9E-392D03DF7743"), Name="积分消耗新增",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreConsumeTaskAdd",IsSysMenu=false,Path="7",FullPath=".0.1.7."},
                new SysMenuPermission{Id=new Guid("6AFD088B-4E1B-03B7-C472-22D811DF25FF"), Name="积分消耗初始化数据",Pid=".0.1.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-consume-task:init-data", Path="1",FullPath=".0.1.7.1."},
                new SysMenuPermission{Id=new Guid("47AF88DA-5F0A-77FF-0B38-38595BACC7F2"), Name="通过应用编码获取积分事件",Pid=".0.1.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-consume-task:consume-events-by-app-code:*", Path="2",FullPath=".0.1.7.2."},
                new SysMenuPermission{Id=new Guid("E002A2DF-1153-7B8B-B71A-42D397A70EA5"), Name="积分消耗创建",Pid=".0.1.7.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-consume-task", Path="3",FullPath=".0.1.7.3."},

                new SysMenuPermission{Id=new Guid("7E2CC977-80C0-17CC-5A5E-BD12BC52CE33"), Name="积分消耗编辑",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreConsumeTaskEdit",IsSysMenu=false,Path="8",FullPath=".0.1.8."},
                new SysMenuPermission{Id=new Guid("2F18349C-524B-9BC6-7FEA-5AE8C386ED7C"), Name="积分消耗初始化数据",Pid=".0.1.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-consume-task:init-data", Path="1",FullPath=".0.1.8.1."},
                new SysMenuPermission{Id=new Guid("3CD90C00-1E62-E1EB-5C30-88C0B6BDB97B"), Name="通过应用编码获取积分事件",Pid=".0.1.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-consume-task:consume-events-by-app-code:*", Path="2",FullPath=".0.1.8.2."},
                new SysMenuPermission{Id=new Guid("AA6C0EB7-0D87-7487-7C32-DBDC5833F420"), Name="积分消耗详情",Pid=".0.1.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-consume-task:by-id:*", Path="3",FullPath=".0.1.8.3."},
                new SysMenuPermission{Id=new Guid("2E9FF2D2-CF47-F009-B3AA-20D7B4BA8145"), Name="积分消耗更新",Pid=".0.1.8.",Type=(int)EnumPermissionType.Api,Permission="put-api:score-consume-task", Path="4",FullPath=".0.1.8.4."},

                new SysMenuPermission{Id=new Guid("A8824370-C211-5366-E0DB-BF0F1BDE1BAF"), Name="积分消耗删除",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreConsumeTaskDelete",IsSysMenu=false,Path="9",FullPath=".0.1.9."},
                new SysMenuPermission{Id=new Guid("F11779AD-9705-F456-BB8A-FF8CDCC0E37A"), Name="积分消耗删除",Pid=".0.1.9.",Type=(int)EnumPermissionType.Api,Permission="delete-api:score-consume-task:*", Path="1",FullPath=".0.1.9.1."},

                new SysMenuPermission{Id=new Guid("C8C1CEEF-5C1D-576F-3F2F-17079173130F"), Name="积分消耗状态设置",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreConsumeTaskStatus",IsSysMenu=false,Path="10",FullPath=".0.1.10."},
                new SysMenuPermission{Id=new Guid("FB40A310-0B7C-099B-5D84-8340C915E196"), Name="积分消耗状态设置",Pid=".0.1.10.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-consume-task:set-active-status", Path="1",FullPath=".0.1.10.1."},

                new SysMenuPermission{Id=new Guid("DC977F84-238E-A8CD-00C1-B0FE69954B1F"), Name="积分等级列表",Pid=".0.1.",Type=(int)EnumPermissionType.Query,Permission="config:scoreLevelList",IsSysMenu=false,Path="11",FullPath=".0.1.11."},
                new SysMenuPermission{Id=new Guid("0444561F-EB1C-705B-F2F0-9FC0C8C91014"), Name="积分等级初始化数据",Pid=".0.1.11.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-level:init-data", Path="1",FullPath=".0.1.11.1."},
                new SysMenuPermission{Id=new Guid("08F778F8-D822-4B87-ECB8-7047F917BC81"), Name="积分等级列表数据",Pid=".0.1.11.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-level:list-data", Path="2",FullPath=".0.1.11.2."},

                new SysMenuPermission{Id=new Guid("D5838565-C513-E045-7CEB-7A2C20F17FD2"), Name="积分等级新增",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreLevelAdd",IsSysMenu=false,Path="12",FullPath=".0.1.12."},
                new SysMenuPermission{Id=new Guid("F910AEBE-C17B-DA6D-3305-20354B7EDCCC"), Name="积分等级初始化数据",Pid=".0.1.12.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-level:init-data", Path="1",FullPath=".0.1.12.1."},
                new SysMenuPermission{Id=new Guid("033B6B92-88F5-DAC1-DB0D-50D9A56B5FA7"), Name="积分等级创建",Pid=".0.1.12.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-level", Path="2",FullPath=".0.1.12.2."},

                new SysMenuPermission{Id=new Guid("6E6EE1F7-E3DC-9EE7-A938-27DA0ECF8CD9"), Name="积分等级编辑",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreLevelEdit",IsSysMenu=false,Path="13",FullPath=".0.1.13."},
                new SysMenuPermission{Id=new Guid("3E11F36D-2955-3E64-40A1-C179D08C1506"), Name="积分等级初始化数据",Pid=".0.1.13.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-level:init-data", Path="1",FullPath=".0.1.13.1."},
                new SysMenuPermission{Id=new Guid("676543A8-71E6-E639-2D6E-DEB17ADDFBD0"), Name="积分等级更新",Pid=".0.1.13.",Type=(int)EnumPermissionType.Api,Permission="put-api:score-level", Path="2",FullPath=".0.1.13.2."},

                new SysMenuPermission{Id=new Guid("DADEB02C-31C0-A6F7-972A-EF78B0AF5827"), Name="积分等级删除",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreLevelDelete",IsSysMenu=false,Path="14",FullPath=".0.1.14."},
                new SysMenuPermission{Id=new Guid("2250C127-FF00-0501-7E0C-92FEA82F127D"), Name="积分等级删除",Pid=".0.1.14.",Type=(int)EnumPermissionType.Api,Permission="delete-api:score-level:*", Path="1",FullPath=".0.1.14.1."},

                new SysMenuPermission{Id=new Guid("02C27FE1-851B-DCD6-509B-F286AEE6E5CC"), Name="积分等级是否显示设置",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreLevelStatus",IsSysMenu=false,Path="15",FullPath=".0.1.15."},
                new SysMenuPermission{Id=new Guid("08F8671A-B4B4-4B62-D0CE-A30585F9CB7C"), Name="积分等级是否显示设置",Pid=".0.1.15.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-level:set-level-show-status", Path="1",FullPath=".0.1.15.1."},

                new SysMenuPermission{Id=new Guid("BF3F5C7C-B057-84FE-1183-A2B136CE6F97"), Name="勋章任务列表",Pid=".0.1.",Type=(int)EnumPermissionType.Query,Permission="config:medalObtainTaskList",IsSysMenu=false,Path="16",FullPath=".0.1.16."},
                new SysMenuPermission{Id=new Guid("555F3F7C-ED41-B717-ADC8-45C10A3492C2"), Name="勋章任务初始化数据",Pid=".0.1.16.",Type=(int)EnumPermissionType.Api,Permission="get-api:medal-obtain-task:init-data", Path="1",FullPath=".0.1.16.1."},
                new SysMenuPermission{Id=new Guid("B7B291B9-965C-A2A2-9EB8-BBB5721A8D9D"), Name="勋章任务列表数据",Pid=".0.1.16.",Type=(int)EnumPermissionType.Api,Permission="get-api:medal-obtain-task:list-data", Path="2",FullPath=".0.1.16.2."},

                new SysMenuPermission{Id=new Guid("F23D47A9-98AF-14DB-F01D-6C0AA8DD9D2A"), Name="勋章任务新增",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:medalObtainTaskAdd",IsSysMenu=false,Path="17",FullPath=".0.1.17."},
                new SysMenuPermission{Id=new Guid("3203B4E4-691B-A97C-D4C5-2DF74C0CFD2B"), Name="勋章任务初始化数据",Pid=".0.1.17.",Type=(int)EnumPermissionType.Api,Permission="get-api:medal-obtain-task:init-data", Path="1",FullPath=".0.1.17.1."},
                new SysMenuPermission{Id=new Guid("2DB97DDC-C48D-D71F-0E79-47B3FFE4C19D"), Name="通过应用编码获取勋章事件",Pid=".0.1.17.",Type=(int)EnumPermissionType.Api,Permission="get-api:medal-obtain-task:medal-events-by-app-code:*", Path="2",FullPath=".0.1.17.2."},
                new SysMenuPermission{Id=new Guid("E05C3525-2FB2-7347-F198-C8359D54EC46"), Name="勋章任务创建",Pid=".0.1.17.",Type=(int)EnumPermissionType.Api,Permission="post-api:medal-obtain-task", Path="3",FullPath=".0.1.17.3."},

                new SysMenuPermission{Id=new Guid("B8D31AC5-F6B0-32BB-BE98-585C21CE9D32"), Name="勋章任务编辑",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:medalObtainTaskEdit",IsSysMenu=false,Path="18",FullPath=".0.1.18."},
                new SysMenuPermission{Id=new Guid("A344BD23-19AC-BA3A-CCBD-9E98DDACDF62"), Name="勋章任务初始化数据",Pid=".0.1.18.",Type=(int)EnumPermissionType.Api,Permission="get-api:medal-obtain-task:init-data", Path="1",FullPath=".0.1.18.1."},
                new SysMenuPermission{Id=new Guid("5D0B6B22-6072-9A34-696D-D6398C978628"), Name="通过应用编码获取积分事件",Pid=".0.1.18.",Type=(int)EnumPermissionType.Api,Permission="get-api:medal-obtain-task:medal-events-by-app-code:*", Path="2",FullPath=".0.1.18.2."},
                new SysMenuPermission{Id=new Guid("AF7AABC0-21FF-0EE7-932C-D5BDC0ED5C01"), Name="勋章任务数据",Pid=".0.1.18.",Type=(int)EnumPermissionType.Api,Permission="get-api:medal-obtain-task:by-id:*", Path="3",FullPath=".0.1.18.3."},
                new SysMenuPermission{Id=new Guid("EBA1CD60-E6A5-9090-5D23-1871EA6A5048"), Name="勋章任务更新",Pid=".0.1.18.",Type=(int)EnumPermissionType.Api,Permission="put-api:medal-obtain-task", Path="4",FullPath=".0.1.18.4."},

                new SysMenuPermission{Id=new Guid("2ED811BE-C1F1-0DF1-E05D-575AEA23910E"), Name="勋章任务删除",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:medalObtainTaskDelete",IsSysMenu=false,Path="19",FullPath=".0.1.19."},
                new SysMenuPermission{Id=new Guid("3D1CFA6F-3A39-C359-3BBB-9E2CB32A9E73"), Name="勋章任务删除",Pid=".0.1.19.",Type=(int)EnumPermissionType.Api,Permission="delete-api:medal-obtain-task:*", Path="1",FullPath=".0.1.19.1."},

                new SysMenuPermission{Id=new Guid("BADB0769-09A9-17BB-8853-86F17BB3E4E4"), Name="勋章任务状态设置",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:medalObtainTaskStatus",IsSysMenu=false,Path="20",FullPath=".0.1.20."},
                new SysMenuPermission{Id=new Guid("C0C28687-0F1C-5A9C-5A7F-5AA3F00D4A73"), Name="勋章任务状态设置",Pid=".0.1.20.",Type=(int)EnumPermissionType.Api,Permission="post-api:medal-obtain-task:set-active-status", Path="1",FullPath=".0.1.20.1."},

                new SysMenuPermission{Id=new Guid("3ACF975A-3C11-F8BA-02C9-C7F747EE8FDC"), Name="积分奖惩列表",Pid=".0.1.",Type=(int)EnumPermissionType.Query,Permission="config:scoreManualList",IsSysMenu=false,Path="21",FullPath=".0.1.21."},
                new SysMenuPermission{Id=new Guid("9DA9B281-874B-076B-A864-60159B9820C3"), Name="积分奖惩初始化数据",Pid=".0.1.21.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-manual:init-data", Path="1",FullPath=".0.1.21.1."},
                new SysMenuPermission{Id=new Guid("3456BFDF-17B9-25FF-8DD1-C50141ED2788"), Name="积分奖惩列表数据",Pid=".0.1.21.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-manual:table-data", Path="2",FullPath=".0.1.21.2."},

                new SysMenuPermission{Id=new Guid("D440DAA6-0C6A-553A-831D-640B4635F85E"), Name="积分奖惩新增",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:scoreManualAdd",IsSysMenu=false,Path="22",FullPath=".0.1.22."},
                new SysMenuPermission{Id=new Guid("853F2F6D-6701-1372-4918-922BE7CE8C16"), Name="积分奖惩初始化数据",Pid=".0.1.22.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-manual:init-data", Path="1",FullPath=".0.1.22.1."},
                new SysMenuPermission{Id=new Guid("A690A722-C2D6-CD98-24F1-FC4998B13C7E"), Name="通过用户组获取用户列表",Pid=".0.1.22.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-manual:user-list-by-groups", Path="2",FullPath=".0.1.22.2."},
                new SysMenuPermission{Id=new Guid("945F3203-2D2E-06C0-A4C4-8DD88D1F2818"), Name="通过用户类型获取用户列表",Pid=".0.1.22.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-manual:user-list-by-user-type", Path="3",FullPath=".0.1.22.3."},
                new SysMenuPermission{Id=new Guid("96E66E8B-4117-5F3A-324A-307EB94068D7"), Name="通过用户条件获取用户列表",Pid=".0.1.22.",Type=(int)EnumPermissionType.Api,Permission="get-api:score-manual:user-list-by-conditions", Path="4",FullPath=".0.1.22.4."},
                new SysMenuPermission{Id=new Guid("1A21FC64-0D24-5C03-0720-F55EEB1C0CF9"), Name="用户数据导入模板下载",Pid=".0.1.22.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-manual:download-user-import-template", Path="5",FullPath=".0.1.22.5."},
                new SysMenuPermission{Id=new Guid("900F8BF0-D460-F0C2-F828-77D7B7EBE0FE"), Name="用户数据导入",Pid=".0.1.22.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-manual:import-group-user", Path="6",FullPath=".0.1.22.6."},
                new SysMenuPermission{Id=new Guid("06AFF075-7FDE-30C3-C70E-9675B29A14BB"), Name="积分奖惩创建",Pid=".0.1.22.",Type=(int)EnumPermissionType.Api,Permission="post-api:score-manual", Path="7",FullPath=".0.1.22.7."},

                new SysMenuPermission{Id=new Guid("96547FA3-BED3-FEFA-8EFA-CB1519F32010"), Name="积分规则详情",Pid=".0.1.",Type=(int)EnumPermissionType.Query,Permission="config:basicConfig",IsSysMenu=false,Path="23",FullPath=".0.1.23."},
                new SysMenuPermission{Id=new Guid("675A97DD-D7A7-1AB2-9EA1-40534E45EB6C"), Name="积分规则详情数据",Pid=".0.1.23.",Type=(int)EnumPermissionType.Api,Permission="get-api:basic-config:config-set", Path="1",FullPath=".0.1.23.1."},

                new SysMenuPermission{Id=new Guid("AD18AF49-9311-281D-41AB-578DC4C6E431"), Name="积分规则保存",Pid=".0.1.",Type=(int)EnumPermissionType.Operate,Permission="config:basicConfigSave",IsSysMenu=false,Path="24",FullPath=".0.1.24."},
                new SysMenuPermission{Id=new Guid("1EADA217-CA43-B733-6BBD-0C6E121C4854"), Name="积分奖惩创建",Pid=".0.1.24.",Type=(int)EnumPermissionType.Api,Permission="post-api:basic-config:save-config-set", Path="1",FullPath=".0.1.24.1."},

                #endregion
                #region 积分商城管理
                new SysMenuPermission{Id=new Guid("CE608EEC-DA98-7421-2E78-588683D9C4F7"), Name="积分商城",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_integralShop",Component="",IsSysMenu=false,Path="2",FullPath=".0.2."},

                new SysMenuPermission{Id=new Guid("6B08D285-6DF0-0A7F-3DF1-BA95FEDB43F2"), Name="商品列表",Pid=".0.2.",Type=(int)EnumPermissionType.Query,Permission="goods:goodsManageList",IsSysMenu=false,Path="1",FullPath=".0.2.1."},
                new SysMenuPermission{Id=new Guid("9E2FD9D2-2E60-012B-1882-B83C13CA2A05"), Name="商品初始化数据",Pid=".0.2.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:goods-manage:init-data", Path="1",FullPath=".0.2.1.1."},
                new SysMenuPermission{Id=new Guid("13B50C25-6C4B-9417-890C-C9CE67847D9F"), Name="商品列表数据",Pid=".0.2.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:goods-manage:table-data", Path="2",FullPath=".0.2.1.2."},

                new SysMenuPermission{Id=new Guid("B2F9F0A5-2EBD-8CE7-AF27-F3109B58FFEF"), Name="商品新增",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="goods:goodsAdd",IsSysMenu=false,Path="2",FullPath=".0.2.2."},
                new SysMenuPermission{Id=new Guid("A83FD4E3-BFE7-8B14-3E62-8B4559DB24F5"), Name="商品初始化数据",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:goods-manage:init-data", Path="1",FullPath=".0.2.2.1."},
                new SysMenuPermission{Id=new Guid("1E4B29FF-FFE5-A12A-E860-0444DA78F507"), Name="商品新增保存",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="post-api:goods-manage", Path="2",FullPath=".0.2.2.2."},

                new SysMenuPermission{Id=new Guid("516A7470-4C2E-AD19-0ED7-DB6FEAF6DA6A"), Name="商品编辑",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="goods:goodsEdit",IsSysMenu=false,Path="3",FullPath=".0.2.3."},
                new SysMenuPermission{Id=new Guid("13DD3A20-5944-C6F1-5B78-EA83EEA8289B"), Name="商品初始化数据",Pid=".0.2.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:goods-manage:init-data", Path="1",FullPath=".0.2.3.1."},
                new SysMenuPermission{Id=new Guid("259E4BF8-48F7-51CC-63A1-C26D4DE36A98"), Name="商品详情数据",Pid=".0.2.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:goods-manage:by-id:*", Path="2",FullPath=".0.2.3.2."},
                new SysMenuPermission{Id=new Guid("5EF6B9FF-ED28-054D-EAD0-D81F48160A5F"), Name="商品新增保存",Pid=".0.2.3.",Type=(int)EnumPermissionType.Api,Permission="put-api:goods-manage", Path="3",FullPath=".0.2.3.3."},

                new SysMenuPermission{Id=new Guid("DFDDE869-F67D-AA6E-ABFC-94413B431CEE"), Name="商品上下架管理",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="goods:goodsStatusSet",IsSysMenu=false,Path="4",FullPath=".0.2.4."},
                new SysMenuPermission{Id=new Guid("E582359B-2F8B-71E2-734D-A9BF264117B4"), Name="商品批量上下架",Pid=".0.2.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:goods-manage:batch-set-goods-status", Path="1",FullPath=".0.2.4.1."},
                new SysMenuPermission{Id=new Guid("83445E58-0B65-0972-70ED-16722B9674D8"), Name="商品状态设置",Pid=".0.2.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:goods-manage:set-goods-status", Path="2",FullPath=".0.2.4.2."},

                new SysMenuPermission{Id=new Guid("E62E0EEB-5129-8A9C-7511-2EE9B6C61F37"), Name="商品库存量管理",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="goods:goodsCountSet",IsSysMenu=false,Path="5",FullPath=".0.2.5."},
                new SysMenuPermission{Id=new Guid("2460D830-300E-26CC-6C3B-72D22BA05565"), Name="修改商品库存量",Pid=".0.2.5.",Type=(int)EnumPermissionType.Api,Permission="post-api:goods-manage:set-goods-store-count", Path="1",FullPath=".0.2.5.1."},

                new SysMenuPermission{Id=new Guid("EE00404C-A649-19AE-58BA-841C34AE7787"), Name="删除商品",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="goods:goodsDelete",IsSysMenu=false,Path="6",FullPath=".0.2.6."},
                new SysMenuPermission{Id=new Guid("A268DDE2-5BBD-C468-CB43-0D33150DB9C0"), Name="商品删除",Pid=".0.2.6.",Type=(int)EnumPermissionType.Api,Permission="delete-api:goods-manage:*", Path="1",FullPath=".0.2.6.1."},

                new SysMenuPermission{Id=new Guid("13D92E77-86F6-6972-E1F3-766FDA59C1E3"), Name="商品导入",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="goods:goodsImport",IsSysMenu=false,Path="7",FullPath=".0.2.7."},
                new SysMenuPermission{Id=new Guid("B16BA978-0BCC-204B-E0B8-6BEA420211BF"), Name="导入模板下载",Pid=".0.2.7.",Type=(int)EnumPermissionType.Api,Permission="post-api:goods-manage:download-goods-import-template", Path="1",FullPath=".0.2.7.1."},
                new SysMenuPermission{Id=new Guid("48665664-5C1B-0332-0720-1949910F13CC"), Name="商品数据导入",Pid=".0.2.7.",Type=(int)EnumPermissionType.Api,Permission="post-api:goods-manage:import-goods", Path="2",FullPath=".0.2.7.2."},
                new SysMenuPermission{Id=new Guid("5E0F2784-0E51-F178-C589-BD688647EAF0"), Name="查询待导入数据",Pid=".0.2.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:goods-manage:import-temp-goods-data", Path="3",FullPath=".0.2.7.3."},
                new SysMenuPermission{Id=new Guid("C3599F47-9966-BA9F-8901-90DCADF57FB4"), Name="商品数据导入确认",Pid=".0.2.7.",Type=(int)EnumPermissionType.Api,Permission="post-api:goods-manage:import-goods-confirm", Path="4",FullPath=".0.2.7.4."},
                #endregion
                #region 兑换管理
                new SysMenuPermission{Id=new Guid("A548B30F-76D4-5453-F33C-CC44E3DAAC80"), Name="兑换管理",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_exchangeManagement",Component="",IsSysMenu=false,Path="3",FullPath=".0.3."},

                new SysMenuPermission{Id=new Guid("CDFC05C8-7196-BD30-DEB3-EC8A61BCC7B4"), Name="订单列表",Pid=".0.3.",Type=(int)EnumPermissionType.Query,Permission="order:orderManageList",IsSysMenu=false,Path="1",FullPath=".0.3.1."},
                new SysMenuPermission{Id=new Guid("163F95B1-6060-A1CF-456B-BFFD81235C71"), Name="订单初始化数据",Pid=".0.3.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:order-manage:init-data", Path="1",FullPath=".0.3.1.1."},
                new SysMenuPermission{Id=new Guid("A09C7B20-E15C-B308-0C61-44581851EC44"), Name="订单列表数据",Pid=".0.3.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:order-manage:table-data", Path="2",FullPath=".0.3.1.2."},

                new SysMenuPermission{Id=new Guid("7B49FE01-506F-699C-310D-A97053AC3544"), Name="订单详情",Pid=".0.3.",Type=(int)EnumPermissionType.Query,Permission="order:orderManageDetail",IsSysMenu=false,Path="2",FullPath=".0.3.2."},
                new SysMenuPermission{Id=new Guid("8C78ABDC-3687-29A0-F03C-10BA9A3395DF"), Name="订单初始化数据",Pid=".0.3.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:order-manage:init-data", Path="1",FullPath=".0.3.2.1."},
                new SysMenuPermission{Id=new Guid("BDACC294-EEB2-CDAE-EDF6-8787D3FC32A7"), Name="订单详情数据",Pid=".0.3.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:order-manage:by-id:*", Path="2",FullPath=".0.3.2.2."},

                new SysMenuPermission{Id=new Guid("9B2CAE30-0227-23FC-604C-CE29121E77ED"), Name="商品兑换",Pid=".0.3.",Type=(int)EnumPermissionType.Operate,Permission="order:orderManageExchange",IsSysMenu=false,Path="3",FullPath=".0.3.3."},
                new SysMenuPermission{Id=new Guid("1B140368-80E0-C857-B203-82EE79D59860"), Name="商品订单兑换",Pid=".0.3.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:order-manage:exchange-goods", Path="1",FullPath=".0.3.3.1."},
                #endregion
            };
            //需要新增的菜单
            if (!await _sysMenuPermissionRepository.AnyAsync())
            {
                //系统菜单及权限
                await _dbContext.BulkInsertAsync(fullMenu);
            }
        }
        #endregion

        #region 测试接口
        /// <summary>
        /// 测试积分消费
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestScoreConsume()
        {
            var svcUrlResult = await _appGatewayEnsureService.GetAppServiceAddress("scorecenter");
            var cts = new CancellationTokenSource();
            var requestBody = new ConsumeScoreInput
            {
                Tenant = _tenantInfo.Name,
                AppCode = "articlesearch",
                AppName = "检索应用",
                UserKey = CurrentUser?.UserKey,
                EventCode = "ArticleSearch_Download",
                EventName = "文献下载",
                EventID = _idGenerator.CreateGuid()
            };
            var requestHeader = new Dictionary<string, string>();
            var token = this._httpContextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization];
            requestHeader.Add(HeaderNames.Authorization, token);
            var dtmResult = await _globalTransaction.Excecute(async (tcc) =>
            {
                ////调用两次只是为了测试下，实际上是调用不同的业务分支逻辑
                //if (svcUrlResult.InUse)
                //{
                //    requestBody.EventID = _idGenerator.CreateGuid();
                //    var svcUrl = "http://192.168.21.43:9011/api";//svcUrlResult.Gateway;
                //    await tcc.CallBranch(requestBody, svcUrl + "/consume-score/try", svcUrl + "/consume-score/confirm", svcUrl + "/consume-score/cancel", headers: requestHeader);
                //}
                if (svcUrlResult.InUse)
                {
                    requestBody.EventID = _idGenerator.CreateGuid();
                    var svcUrl = svcUrlResult.Gateway;
                    await tcc.CallBranch(requestBody, svcUrl + "/consume-score/try", svcUrl + "/consume-score/confirm", svcUrl + "/consume-score/cancel", headers: requestHeader);
                }
            }, requestHeader, cts.Token);
            if (!dtmResult.IsSuccess)
            {
                throw Oops.Oh(dtmResult.ErrMsg);
            }
            return true;
        }
        #endregion

    }
}
