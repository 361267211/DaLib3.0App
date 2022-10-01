/*********************************************************
* 名    称：CommonAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：辅助操作Api
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.DbContexts;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vive.Crypto;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 公用接口
    /// </summary>
    [Authorize(Policy = PolicyKey.TokenAuth)]
    public class CommonAppService : BaseAppService
    {
        private readonly UserDbContext _dbContext;
        private readonly IRepository<BasicConfigSet> _basicConfigSetRepository;
        private readonly IRepository<SysRole> _sysRoleRepository;
        private readonly IRepository<SysRoleMenu> _sysRoleMenuRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;
        private readonly IRepository<SysUserRole> _sysUserRoleRepository;
        private readonly IRepository<Property> _propertyRepository;
        private readonly IRepository<PropertyGroup> _propertyGroupRepository;
        private readonly IRepository<PropertyGroupItem> _propertyGroupItemRepository;
        private readonly IRepository<ReaderEditProperty> _readerEditPropertyRepository;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly IRepository<Card> _cardRepository;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="basicConfigSetRepository"></param>
        /// <param name="sysRoleRepository"></param>
        /// <param name="sysRoleMenuRepository"></param>
        /// <param name="sysMenuPermissionRepository"></param>
        /// <param name="sysUserRoleRepository"></param>
        /// <param name="propertyRepository"></param>
        /// <param name="propertyGroupRepository"></param>
        /// <param name="propertyGroupItemRepository"></param>
        /// <param name="readerEditPropertyRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="cardRepository"></param>
        public CommonAppService(UserDbContext dbContext
            , IRepository<BasicConfigSet> basicConfigSetRepository
            , IRepository<SysRole> sysRoleRepository
            , IRepository<SysRoleMenu> sysRoleMenuRepository
            , IRepository<SysMenuPermission> sysMenuPermissionRepository
            , IRepository<SysUserRole> sysUserRoleRepository
            , IRepository<Property> propertyRepository
            , IRepository<PropertyGroup> propertyGroupRepository
            , IRepository<PropertyGroupItem> propertyGroupItemRepository
            , IRepository<ReaderEditProperty> readerEditPropertyRepository
            , IRepository<EntityFramework.Core.Entitys.User> userRepository
            , IRepository<Card> cardRepository)
        {
            _dbContext = dbContext;
            _basicConfigSetRepository = basicConfigSetRepository;
            _sysRoleRepository = sysRoleRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
            _propertyRepository = propertyRepository;
            _propertyGroupRepository = propertyGroupRepository;
            _propertyGroupItemRepository = propertyGroupItemRepository;
            _readerEditPropertyRepository = readerEditPropertyRepository;
            _userRepository = userRepository;
            _cardRepository = cardRepository;
        }


        /// <summary>
        /// 租户数据迁移
        /// </summary>
        /// <returns></returns>
        //[UnitOfWork]
        public async Task UpdateDatabaseSchema()
        {
            await _basicConfigSetRepository.Database.MigrateAsync();
            //基础配置种子数据
            await InitBasicConfig();
            await InitPermissionMenu();
            await InitProperty();
            await InitPropertyGroup();
            await InitRole();
            await InitReaderEditProperty();
            await InitAdministrator();
            //await InitArea();
        }

        #region 数据初始化
        /// <summary>
        /// 基础配置
        /// </summary>
        /// <returns></returns>
        private async Task InitBasicConfig()
        {
            var initConfig = new List<BasicConfigSet>
                {
                    new BasicConfigSet
                    {
                        Id=new Guid("8B6A6546-0D0B-48A3-0456-EF8725490DD9"),
                        SensitiveFilter=true,
                        UserInfoConfirm=true,
                        PropertyConfirm=true,
                        CardClaim=true,
                        UserInfoSupply=true,
                        TenantId=_dbContext.TenantInfo?.Name
                    }
                };
            if (!await _basicConfigSetRepository.AnyAsync())
            {
                await _basicConfigSetRepository.InsertNowAsync(initConfig);
            }
        }

        /// <summary>
        /// 初始化系统菜单
        /// </summary>
        /// <returns></returns>
        private async Task InitPermissionMenu()
        {
            var fullMenu = new List<SysMenuPermission>
             {
               //Path同一层级内重新编码
               new SysMenuPermission{Id=new Guid("01F7F728-F598-FE7A-FD9B-7F429D079CD2"), Name="顶级节点",Remark="虚拟节点",Pid="",Type=(int)EnumPermissionType.Dir,Path="0",FullPath=".0." },
               //------------------------管理主页
               new SysMenuPermission{Id=new Guid("58A71C06-B602-A96F-593D-B719DC8AD961"), Name="管理主页",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_userManager",Component="",IsSysMenu=false,Path="1",FullPath=".0.1."},
               //------详情
               new SysMenuPermission{Id=new Guid("8C8C11B4-2DB3-B7AC-8A54-A5B00AAA0C74"), Name="管理主页数据获取",Remark="",Pid=".0.1.",Type=(int)EnumPermissionType.Query,Permission="dashboard:detail",Path="1",FullPath=".0.1.1."},
               new SysMenuPermission{Id=new Guid("D87E2CAA-6130-531C-03CF-F141BEFD482A"), Name="管理主页详情Api",Remark="",Pid=".0.1.1.",Type=(int)EnumPermissionType.Api,Permission="api:dashboard:basic-info",Path="1",FullPath=".0.1.1.1."},
               //用户管理
               //new SysMenuPermission{Id=new Guid("9814B6CE-72EE-5B0A-06F1-47B92261F1B0"), Name="用户管理",Pid=".0.",Type=(int)EnumPermissionType.Dir,Router="/admin_User",Component="", Path="2",FullPath=".0.2."},
               #region ------------------------读者管理
               new SysMenuPermission{Id=new Guid("CAB68997-6ED3-3ED4-8600-CC04147F29DA"), Name="读者管理",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_readerList",Component="",IsSysMenu=false,Path="2",FullPath=".0.2."},
               //------列表
               new SysMenuPermission{Id=new Guid("A8B3DF95-C0AF-BABD-62DA-61E3F68F9611"), Name="读者列表",Pid=".0.2.",Type=(int)EnumPermissionType.Query,Permission="reader:list",Path="1",FullPath=".0.2.1."},
               new SysMenuPermission{Id=new Guid("0E320093-AC52-E331-B88F-FFC5CE20D946"), Name="读者初始数据",Pid=".0.2.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="1",FullPath=".0.2.1.1."},
               new SysMenuPermission{Id=new Guid("EE67BFFD-DF90-EBFF-223C-040BABB8CD4D"), Name="读者列表数据",Pid=".0.2.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:table-data", Path="2",FullPath=".0.2.1.2."},
               //------详情
               new SysMenuPermission{Id=new Guid("35CB16D4-E036-901F-20D3-BCD92EC5559D"), Name="读者详情",Pid=".0.2.",Type=(int)EnumPermissionType.Query,Permission="reader:detail",Path="2",FullPath=".0.2.2."},
               new SysMenuPermission{Id=new Guid("0BE558BF-7EF2-9ED4-DCB4-0020613D6342"), Name="读者初始数据",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="1",FullPath=".0.2.2.1."},
               new SysMenuPermission{Id=new Guid("5E1DAEF5-275E-8D2C-5DF9-39B1CED0564A"), Name="读者详情数据脱敏",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:*",Path="2",FullPath=".0.2.2.2."},
               new SysMenuPermission{Id=new Guid("5FAE7C98-0D13-1E6E-506A-7BD0119BA77C"), Name="读者详情数据编辑",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:by-id-for-edit:*",Path="3",FullPath=".0.2.2.3."},
               new SysMenuPermission{Id=new Guid("842E7666-ED94-E415-9C58-2A4E20BA65AA"), Name="读者积分数据",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:user-points-table-data",Path="4",FullPath=".0.2.2.4."},
               new SysMenuPermission{Id=new Guid("0B6E02B7-F31A-D715-7FE2-8A35BCCE48AB"), Name="读者借阅记录",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:user-borrow-table-data",Path="5",FullPath=".0.2.2.5."},
               new SysMenuPermission{Id=new Guid("F557CA64-59CB-6F34-7254-8A18449B91F3"), Name="读者授权应用",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:user-auth-app-list-data",Path="6",FullPath=".0.2.2.6."},
               new SysMenuPermission{Id=new Guid("6143D284-F9C0-DCF1-8E0D-3D240C81F817"), Name="读者行为日志",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:user-log-table-data",Path="7",FullPath=".0.2.2.7."},
               new SysMenuPermission{Id=new Guid("092C4204-B382-D601-C825-817770E05B64"), Name="读者卡列表",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:user-card-list-data:*",Path="8",FullPath=".0.2.2.8."},
               new SysMenuPermission{Id=new Guid("2018A1F8-34AB-5E6E-BFD5-DF1BFCDB5183"), Name="读者卡申请",Pid=".0.2.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:user-card-apply-list-data:*",Path="9",FullPath=".0.2.2.9."},
               //------新增
               new SysMenuPermission{Id=new Guid("1C6043C4-5124-D3CA-EC51-4A926C08D09A"), Name="读者新增",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="reader:create",Path="3",FullPath=".0.2.3."},
               new SysMenuPermission{Id=new Guid("00BE7806-66C4-3E7A-D9F8-ECE7BB618D33"), Name="读者初始数据",Pid=".0.2.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="1",FullPath=".0.2.3.1."},
               new SysMenuPermission{Id=new Guid("F28BD322-E5FA-867E-9104-2F5583B448B8"), Name="读者新增",Pid=".0.2.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:user",Path="2",FullPath=".0.2.3.2."},
               //------编辑
               new SysMenuPermission{Id=new Guid("A98BA2C6-6AAE-811B-AB2F-1D8027BE9A6F"), Name="读者编辑",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="reader:update",Path="4",FullPath=".0.2.4."},
               new SysMenuPermission{Id=new Guid("2A34A661-3075-9C43-71B3-6A7F4227B2C7"), Name="读者初始数据",Pid=".0.2.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="1",FullPath=".0.2.4.1."},
               new SysMenuPermission{Id=new Guid("0E526EE9-A8E1-8FAC-F46B-99602A92A71F"), Name="读者详情数据",Pid=".0.2.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:*",Path="2",FullPath=".0.2.4.2."},
               new SysMenuPermission{Id=new Guid("B83AFC84-7E44-4F36-08BC-20D1549E831B"), Name="读者编辑数据",Pid=".0.2.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:by-id-for-edit:*",Path="3",FullPath=".0.2.4.3."},
               new SysMenuPermission{Id=new Guid("1516B211-719C-6D9C-E3A4-513BA07B2CE5"), Name="读者编辑",Pid=".0.2.4.",Type=(int)EnumPermissionType.Api,Permission="put-api:user",Path="4",FullPath=".0.2.4.4."},
               new SysMenuPermission{Id=new Guid("20FBEACF-E40F-3202-D109-851398FD061E"), Name="读者卡列表",Pid=".0.2.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:user-card-list-data:*",Path="5",FullPath=".0.2.4.5."},
               new SysMenuPermission{Id=new Guid("532D09B9-DE94-4FD8-7EE0-6882FF7EE298"), Name="读者卡申请",Pid=".0.2.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:user-card-apply-list-data:*",Path="6",FullPath=".0.2.4.6."},
               new SysMenuPermission{Id=new Guid("85AA66F9-DFA1-AAF6-E91B-122E9F42960A"), Name="读者编辑合并",Pid=".0.2.4.",Type=(int)EnumPermissionType.Api,Permission="post-api:user:with-merge",Path="7",FullPath=".0.2.4.7."},
               //------删除
               new SysMenuPermission{Id=new Guid("F4442FBF-5BF5-2DE1-6CD1-54B23CF8E669"), Name="读者删除",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="reader:delete",Path="5",FullPath=".0.2.5."},
               new SysMenuPermission{Id=new Guid("EB6B46AD-CF71-AEB5-AA98-EF81CF1EC9C0"), Name="读者删除",Pid=".0.2.5.",Type=(int)EnumPermissionType.Api,Permission="delete-api:user:*",Path="1",FullPath=".0.2.5.1."},
               //------导入
               new SysMenuPermission{Id=new Guid("5432B644-DD53-D0D7-0F52-B4C2A4BBACF3"), Name="读者导入",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="reader:import",Path="6",FullPath=".0.2.6."},
               new SysMenuPermission{Id=new Guid("ED1BF4D3-707E-4EAE-2A71-3D6AB78BF391"), Name="读者初始数据",Pid=".0.2.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="1",FullPath=".0.2.6.1."},
               new SysMenuPermission{Id=new Guid("8F965BC8-AF83-AAB9-0A09-C4DFBAA0C1FE"), Name="读者导入模板下载",Pid=".0.2.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:download-user-import-template",Path="2",FullPath=".0.2.6.2."},
               new SysMenuPermission{Id=new Guid("85E0D236-A117-E08A-5EDA-4632C0A3F2E0"), Name="读者导入数据",Pid=".0.2.6.",Type=(int)EnumPermissionType.Api,Permission="post-api:user:import-user",Path="3",FullPath=".0.2.6.3."},
               new SysMenuPermission{Id=new Guid("158D533A-14CE-8729-4D86-05984EECF175"), Name="读者待导入数据",Pid=".0.2.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:import-temp-user-data",Path="4",FullPath=".0.2.6.4."},
               new SysMenuPermission{Id=new Guid("1DBE4993-6B4E-B3F2-1AA4-06747F65836B"), Name="读者导入数据确认",Pid=".0.2.6.",Type=(int)EnumPermissionType.Api,Permission="post-api:user:import-user-confirm:*",Path="5",FullPath=".0.2.6.5."},
               //------导出
               new SysMenuPermission{Id=new Guid("607D6E62-DB5E-F73F-A9A6-CF02262C6E3F"), Name="读者导出",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="reader:export",Path="7",FullPath=".0.2.7."},
               new SysMenuPermission{Id=new Guid("66F611CB-1C16-30F8-7055-7816C588B301"), Name="读者初始数据",Pid=".0.2.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="1",FullPath=".0.2.7.1."},
               new SysMenuPermission{Id=new Guid("8804CF14-2E61-5EAA-E586-3238D2437EA6"), Name="读者导出数据简要信息",Pid=".0.2.7.",Type=(int)EnumPermissionType.Api,Permission="post-api:user:export-user-data-brief-info",Path="2",FullPath=".0.2.7.2."},
               new SysMenuPermission{Id=new Guid("F5E29B4C-EA46-E47C-3EC7-5A06B33887CA"), Name="读者导出数据",Pid=".0.2.7.",Type=(int)EnumPermissionType.Api,Permission="post-api:user:export-user-data",Path="3",FullPath=".0.2.7.3."},
               //------批量修改
               new SysMenuPermission{Id=new Guid("48D87C77-9EF6-DC24-44FF-1769AC6E1C20"), Name="读者批量修改",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="reader:batchUpdate",Path="8",FullPath=".0.2.8."},
               new SysMenuPermission{Id=new Guid("12F96FB7-A6B8-EB0A-54EB-BF2083E2417C"), Name="读者批量修改数据",Pid=".0.2.8.",Type=(int)EnumPermissionType.Api,Permission="put-api:user:batch-update",Path="1",FullPath=".0.2.8.1."},
               //------批量设置馆员
               new SysMenuPermission{Id=new Guid("5CB65E33-39A8-C6F6-530A-1874DD908554"), Name="读者转为馆员",Pid=".0.2.",Type=(int)EnumPermissionType.Operate,Permission="reader:batchSetAsStaff",Path="9",FullPath=".0.2.9."},
               new SysMenuPermission{Id=new Guid("7243815B-62D6-00C2-A842-39C9738BACA8"), Name="读者批量设置馆员",Pid=".0.2.9.",Type=(int)EnumPermissionType.Api,Permission="post-api:user:batch-set-user-as-staff",Path="1",FullPath=".0.2.9.1."},
               #endregion
               #region ------------------------读者卡管理
               new SysMenuPermission{Id=new Guid("5049BB91-331C-6318-586B-8177E52DBCBA"), Name="读者卡管理",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_readerCardList",Component="",IsSysMenu=false,Path="3",FullPath=".0.3."},
               //------列表
               new SysMenuPermission{Id=new Guid("565AF4CF-1FEA-32DC-84C7-395AB79C6A8E"), Name="读者卡列表",Pid=".0.3.",Type=(int)EnumPermissionType.Query,Permission="card:list",Path="1",FullPath=".0.3.1."},
               new SysMenuPermission{Id=new Guid("0C7613B8-80AD-BBEB-3779-29AD852232EF"), Name="读者卡初始数据",Pid=".0.3.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:init-data", Path="1",FullPath=".0.3.1.1."},
               new SysMenuPermission{Id=new Guid("0C763422-5BB8-805C-C505-F05780DC71AD"), Name="读者卡列表数据",Pid=".0.3.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:table-data", Path="2",FullPath=".0.3.1.2."},
               //------详情
               new SysMenuPermission{Id=new Guid("2A3B096D-1ADE-F107-FC7D-8F2A970CADAB"), Name="读者卡详情",Pid=".0.3.",Type=(int)EnumPermissionType.Query,Permission="card:detail",Path="2",FullPath=".0.3.2."},
               new SysMenuPermission{Id=new Guid("2DF4B563-00D0-429B-62A7-8BA81724F54F"), Name="读者卡初始数据",Pid=".0.3.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:init-data", Path="1",FullPath=".0.3.2.1."},
               new SysMenuPermission{Id=new Guid("8D2ECB29-9B63-F23C-D2B2-C6C66783EF68"), Name="读者卡详情数据",Pid=".0.3.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:*", Path="2",FullPath=".0.3.2.2."},
               //------新增
               new SysMenuPermission{Id=new Guid("505D4BCC-5C73-8831-D7B6-1C83D233C566"), Name="读者卡新增",Pid=".0.3.",Type=(int)EnumPermissionType.Operate,Permission="card:create",Path="3",FullPath=".0.3.3."},
               new SysMenuPermission{Id=new Guid("CFC54AC3-2F3D-130C-F4DD-8FF500B33447"), Name="读者卡初始数据",Pid=".0.3.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:init-data", Path="1",FullPath=".0.3.3.1."},
               new SysMenuPermission{Id=new Guid("2D80B518-12D5-9A30-F61F-09654DE64AD6"), Name="读者卡新增",Pid=".0.3.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:card", Path="2",FullPath=".0.3.3.2."},
               //------编辑
               new SysMenuPermission{Id=new Guid("776E6480-0E59-35CC-F48D-D4D752D0C969"), Name="读者卡编辑",Pid=".0.3.",Type=(int)EnumPermissionType.Operate,Permission="card:update",Path="4",FullPath=".0.3.4."},
               new SysMenuPermission{Id=new Guid("F9ED6EE6-702E-A724-F4BC-3D4CCA5A3E55"), Name="读者卡初始数据",Pid=".0.3.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:init-data", Path="1",FullPath=".0.3.4.1."},
               new SysMenuPermission{Id=new Guid("779CEE76-6F3F-C739-2DAB-A913FA50BB67"), Name="读者卡详情数据",Pid=".0.3.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:*", Path="2",FullPath=".0.3.4.2."},
               new SysMenuPermission{Id=new Guid("5A1E4F20-E700-5987-A688-4D2CAD05D982"), Name="读者卡编辑",Pid=".0.3.4.",Type=(int)EnumPermissionType.Api,Permission="put-api:card", Path="3",FullPath=".0.3.4.3."},
               //------删除
               new SysMenuPermission{Id=new Guid("71CD6837-6D34-42E1-95F8-9291A8A044D2"), Name="读者卡删除",Pid=".0.3.",Type=(int)EnumPermissionType.Operate,Permission="card:delete",Path="5",FullPath=".0.3.5."},
               new SysMenuPermission{Id=new Guid("73787154-A68E-E6CB-A21B-F1980B87FD2F"), Name="读者卡删除",Pid=".0.3.5.",Type=(int)EnumPermissionType.Api,Permission="delete-api:card:*", Path="1",FullPath=".0.3.5.1."},
               //------导出
               new SysMenuPermission{Id=new Guid("3286B047-43C0-C735-3EA7-578B20A47C28"), Name="读者卡导出",Pid=".0.3.",Type=(int)EnumPermissionType.Operate,Permission="card:export",Path="6",FullPath=".0.3.6."},
               new SysMenuPermission{Id=new Guid("38303C95-EF29-C83A-E85E-4E88478464F0"), Name="读者卡初始数据",Pid=".0.3.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:init-data", Path="1",FullPath=".0.3.6.1."},
               new SysMenuPermission{Id=new Guid("B2BAB31A-B240-D204-506B-4DE0E112DB20"), Name="读者卡导出简要信息",Pid=".0.3.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:export-card-data-brief-info", Path="2",FullPath=".0.3.6.2."},
               new SysMenuPermission{Id=new Guid("4D45B155-FB04-F3EB-3380-75CC50386BD3"), Name="读者卡导出信息",Pid=".0.3.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:export-card-data", Path="3",FullPath=".0.3.6.3."},
               
               //---批量修改
               new SysMenuPermission{Id=new Guid("252550E3-5CAE-12CD-7CDD-5A9D66AA5702"), Name="读者卡批量修改",Pid=".0.3.",Type=(int)EnumPermissionType.Operate,Permission="card:batchUpdate",Path="7",FullPath=".0.3.7."},
               new SysMenuPermission{Id=new Guid("A5690BF1-5D1E-2056-269A-29566DDAD379"), Name="读者卡批量修改",Pid=".0.3.7.",Type=(int)EnumPermissionType.Api,Permission="put-api:card:batch-update", Path="1",FullPath=".0.3.7.1."},
               
               //---密码设置
               new SysMenuPermission{Id=new Guid("C33AC3BE-765F-38AC-4DFA-DE832B68FFE4"), Name="读者卡密码修改",Pid=".0.3.",Type=(int)EnumPermissionType.Operate,Permission="card:setSecret",Path="8",FullPath=".0.3.8."},
               new SysMenuPermission{Id=new Guid("EA3F7EEC-C9E1-CAFA-EAAF-569B344B69D1"), Name="读者卡密码修改",Pid=".0.3.8.",Type=(int)EnumPermissionType.Api,Permission="post-api:card:set-card-secret", Path="1",FullPath=".0.3.8.1."},
               new SysMenuPermission{Id=new Guid("1E4D7704-8BCA-2480-61F1-E88CB2907160"), Name="读者卡密码重置",Pid=".0.3.8.",Type=(int)EnumPermissionType.Api,Permission="post-api:card:reset-card-secret", Path="2",FullPath=".0.3.8.2."},

               //------读者卡同步
               new SysMenuPermission{Id=new Guid("8B944B3D-7DA3-A972-042B-3AF8B7196C5A"), Name="读者卡同步",Pid=".0.3.",Type=(int)EnumPermissionType.Operate,Permission="card:increatSync",Path="9",FullPath=".0.3.9."},
               new SysMenuPermission{Id=new Guid("5857D4D8-8943-3E6B-0433-6899B903EAD6"), Name="同步配置读取",Pid=".0.3.9.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:sync-card-config", Path="1",FullPath=".0.3.9.1."},
               new SysMenuPermission{Id=new Guid("5C5A7595-4074-53C9-37D8-CE635739E793"), Name="同步配置设置",Pid=".0.3.9.",Type=(int)EnumPermissionType.Api,Permission="post-api:card:set-sync-card-config", Path="2",FullPath=".0.3.9.2."},
               new SysMenuPermission{Id=new Guid("A0FD7402-9B22-507A-7B9D-0587C911BB4A"), Name="同步日志获取",Pid=".0.3.9.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:sync-card-log-table-data", Path="3",FullPath=".0.3.9.3."},
               
               #endregion
               #region ------------------------属性管理
               new SysMenuPermission{Id=new Guid("D482B166-C6C8-3C2A-2FBB-6CD597D512FB"), Name="属性管理",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_attributeList",Component="",IsSysMenu=false,Path="4",FullPath=".0.4."},
               //-------列表
               new SysMenuPermission{Id=new Guid("D16A830B-76BD-65C6-9FDB-840443E70DB3"), Name="属性列表",Pid=".0.4.",Type=(int)EnumPermissionType.Query,Permission="property:list",IsSysMenu=false,Path="1",FullPath=".0.4.1."},
               new SysMenuPermission{Id=new Guid("D69106E2-3800-0DB0-F3FE-FA727C970A5E"), Name="属性初始数据",Pid=".0.4.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:property:init-data", Path="1",FullPath=".0.4.1.1."},
               new SysMenuPermission{Id=new Guid("29E86DB5-3BE4-FE36-1756-7E089CEDE742"), Name="属性列表数据",Pid=".0.4.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:property:list-data", Path="2",FullPath=".0.4.1.2."},

               new SysMenuPermission{Id=new Guid("98183DDD-158F-ECEE-6D69-5D320DE53446"), Name="属性详情",Pid=".0.4.",Type=(int)EnumPermissionType.Query,Permission="property:detail",IsSysMenu=false,Path="2",FullPath=".0.4.2."},
               new SysMenuPermission{Id=new Guid("D38149B5-673B-AE64-5B62-5B9AF53D9C57"), Name="属性初始数据",Pid=".0.4.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:property:init-data", Path="1",FullPath=".0.4.2.1."},
               new SysMenuPermission{Id=new Guid("CD314E11-070A-66B9-8B5E-B22AA45AA7A4"), Name="属性详情数据",Pid=".0.4.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:property:*", Path="2",FullPath=".0.4.2.2."},

               new SysMenuPermission{Id=new Guid("4FC2FD27-CC1D-BD57-5734-0654BA3C0926"), Name="属性新增",Pid=".0.4.",Type=(int)EnumPermissionType.Operate,Permission="property:create",IsSysMenu=false,Path="3",FullPath=".0.4.3."},
               new SysMenuPermission{Id=new Guid("0845DCC8-570D-8801-F0CF-4B9635A2AFB1"), Name="属性初始数据",Pid=".0.4.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:property:init-data", Path="1",FullPath=".0.4.3.1."},
               new SysMenuPermission{Id=new Guid("FFEFA1E7-7F9E-B922-E0A3-B3B0D8D53E9E"), Name="属性新增",Pid=".0.4.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:property", Path="2",FullPath=".0.4.3.2."},
               new SysMenuPermission{Id=new Guid("65713601-EF08-A031-A77C-D23102181DA1"), Name="属性设置是否可搜索",Pid=".0.4.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:property:set-can-search", Path="3",FullPath=".0.4.3.3."},
               new SysMenuPermission{Id=new Guid("540F5A94-53E6-CA12-B0E6-E5F9BDCF8819"), Name="属性设置是否列表显示",Pid=".0.4.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:property:set-show-on-table", Path="4",FullPath=".0.4.3.4."},

               new SysMenuPermission{Id=new Guid("B82AC6AC-6CFD-5AFD-9C2A-746F8AB546B7"), Name="属性编辑",Pid=".0.4.",Type=(int)EnumPermissionType.Operate,Permission="property:update",IsSysMenu=false,Path="4",FullPath=".0.4.4."},
               new SysMenuPermission{Id=new Guid("88E4F1E0-A2D2-26C7-BE20-147289B06DE8"), Name="属性初始数据",Pid=".0.4.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:property:init-data", Path="1",FullPath=".0.4.4.1."},
               new SysMenuPermission{Id=new Guid("5ECFAB53-D280-74BC-51B7-637C80199A0E"), Name="属性详情数据",Pid=".0.4.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:property:*", Path="2",FullPath=".0.4.4.2."},
               new SysMenuPermission{Id=new Guid("781560E7-4FA5-0ABE-9717-9475CAD25A35"), Name="属性编辑",Pid=".0.4.4.",Type=(int)EnumPermissionType.Api,Permission="put-api:property", Path="3",FullPath=".0.4.4.3."},
               new SysMenuPermission{Id=new Guid("A9FA6272-6A00-8DBC-396C-609DFF47F322"), Name="属性设置是否可搜索",Pid=".0.4.4.",Type=(int)EnumPermissionType.Api,Permission="post-api:property:set-can-search", Path="4",FullPath=".0.4.4.4."},
               new SysMenuPermission{Id=new Guid("368DF60F-46FE-A839-45A7-2EAF9B36C9A9"), Name="属性设置是否列表显示",Pid=".0.4.4.",Type=(int)EnumPermissionType.Api,Permission="post-api:property:set-show-on-table", Path="5",FullPath=".0.4.4.5."},

               new SysMenuPermission{Id=new Guid("74624165-120E-94FD-896A-1E922235BFAA"), Name="属性删除",Pid=".0.4.",Type=(int)EnumPermissionType.Operate,Permission="property:delete",IsSysMenu=false,Path="5",FullPath=".0.4.5."},
               new SysMenuPermission{Id=new Guid("F2A0C9CE-0F4E-BF72-D768-150258259B3B"), Name="属性删除",Pid=".0.4.5.",Type=(int)EnumPermissionType.Api,Permission="delete-api:property:*", Path="1",FullPath=".0.4.5.1."},

               new SysMenuPermission{Id=new Guid("20458B40-AD2C-F77A-B8D6-AA748F1A8152"), Name="属性组列表",Pid=".0.4.",Type=(int)EnumPermissionType.Query,Permission="propertyGroup:list",IsSysMenu=false,Path="6",FullPath=".0.4.6."},
               new SysMenuPermission{Id=new Guid("E75639CB-B1E6-C0B7-0CFE-37DCDBBAA9E3"), Name="属性组初始数据",Pid=".0.4.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:init-data", Path="1",FullPath=".0.4.6.1."},
               new SysMenuPermission{Id=new Guid("5DDFC7A6-BF77-2838-CB87-40740F631D16"), Name="属性组列表数据",Pid=".0.4.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:list-data", Path="2",FullPath=".0.4.6.2."},
               new SysMenuPermission{Id=new Guid("1A9A4417-3281-3F94-A2C2-A399E5966D57"), Name="属性组变更日志",Pid=".0.4.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:property-change-log", Path="3",FullPath=".0.4.6.3."},
               new SysMenuPermission{Id=new Guid("5E480A8B-E4AF-D249-ECE8-2AD6F95DA539"), Name="属性组变更详情",Pid=".0.4.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:property-change-log-items:*", Path="4",FullPath=".0.4.6.4."},

               new SysMenuPermission{Id=new Guid("3B564366-CA52-D0A9-EDDF-C52E65148180"), Name="属性组详情",Pid=".0.4.",Type=(int)EnumPermissionType.Query,Permission="propertyGroup:detail",IsSysMenu=false,Path="7",FullPath=".0.4.7."},
               new SysMenuPermission{Id=new Guid("91433F17-0F70-4A49-CB69-C46A1603ABD0"), Name="属性组初始数据",Pid=".0.4.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:init-data", Path="1",FullPath=".0.4.7.1."},
               new SysMenuPermission{Id=new Guid("4A9645A4-C6A6-292E-BA87-D96C8200C9E1"), Name="属性组详情数据",Pid=".0.4.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:list-item-data:*", Path="2",FullPath=".0.4.7.2."},
               new SysMenuPermission{Id=new Guid("87D70551-AB44-D352-2954-608CBCE296E7"), Name="属性组组织机构数据",Pid=".0.4.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:org-list", Path="3",FullPath=".0.4.7.3."},
               new SysMenuPermission{Id=new Guid("8C44774F-20B3-9507-C50A-6856CCB81A32"), Name="属性组地区数据",Pid=".0.4.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:region-list", Path="4",FullPath=".0.4.7.4."},

               new SysMenuPermission{Id=new Guid("95A45A6A-17DB-F0CA-81CB-EB2A7A1E63B9"), Name="属性组编辑",Pid=".0.4.",Type=(int)EnumPermissionType.Operate,Permission="propertyGroup:edit",IsSysMenu=false,Path="8",FullPath=".0.4.8."},
               new SysMenuPermission{Id=new Guid("08CF6311-F407-FF7C-EB47-BB6864EBA0D2"), Name="属性组初始数据",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:init-data", Path="1",FullPath=".0.4.7.1."},
               new SysMenuPermission{Id=new Guid("2D60375F-6145-17EC-A013-9E8326C0FA78"), Name="属性组详情数据",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:list-item-data:*", Path="2",FullPath=".0.4.7.2."},
               new SysMenuPermission{Id=new Guid("65674CD5-E494-0E61-7A85-B869B0F01CA4"), Name="属性组组织机构数据",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:org-list", Path="3",FullPath=".0.4.7.3."},
               new SysMenuPermission{Id=new Guid("83650790-B239-DB42-735E-2E4D5720B225"), Name="属性组地区数据",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:region-list", Path="4",FullPath=".0.4.7.4."},
               new SysMenuPermission{Id=new Guid("1470914E-3088-5F1D-8A20-77AE57464935"), Name="创建属性明细",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="post-api:property-group:group-item", Path="5",FullPath=".0.4.7.5."},
               new SysMenuPermission{Id=new Guid("A1CD62DA-5B96-618E-DC7C-EC1939DC8234"), Name="编辑属性明细",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="put-api:property-group:group-item", Path="6",FullPath=".0.4.7.6."},
               new SysMenuPermission{Id=new Guid("ABF63728-BDCE-AF4E-3FE9-C8BC452477EE"), Name="删除属性明细",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="delete-api:property-group:group-item:*", Path="7",FullPath=".0.4.7.7."},
               new SysMenuPermission{Id=new Guid("1DB9BFF0-1971-DF30-31B8-8762889848AA"), Name="创建组织机构",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="post-api:property-group:org", Path="8",FullPath=".0.4.7.8."},
               new SysMenuPermission{Id=new Guid("9600630D-AE03-2452-94EC-260972ED5B80"), Name="编辑组织机构",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="put-api:property-group:org", Path="9",FullPath=".0.4.7.9."},
               new SysMenuPermission{Id=new Guid("DB0FFDC9-FE63-EB39-C53A-63B564334D93"), Name="删除组织机构",Pid=".0.4.8.",Type=(int)EnumPermissionType.Api,Permission="delete-api:property-group:org:*", Path="10",FullPath=".0.4.7.10."},
               
               #endregion
               #region ------------------------馆员管理

               new SysMenuPermission{Id=new Guid("EF20A65E-1E25-7C76-43C1-B9E90D57B3EB"), Name="馆员管理",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_librarianManagement",Component="",IsSysMenu=false,Path="5",FullPath=".0.5."},
               new SysMenuPermission{Id=new Guid("D6D602CD-67DB-B832-F97B-63388F7423E1"), Name="馆员列表",Pid=".0.5.",Type=(int)EnumPermissionType.Query,Permission="staff:list",IsSysMenu=false,Path="1",FullPath=".0.5.1."},
               new SysMenuPermission{Id=new Guid("7C48B054-8D34-79FF-7578-206EBBCBBCDC"), Name="馆员初始数据",Pid=".0.5.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:staff:init-data", Path="1",FullPath=".0.5.1.1."},
               new SysMenuPermission{Id=new Guid("53DF8263-B3E7-4BCF-D23E-4579CC75CA2C"), Name="馆员列表数据",Pid=".0.5.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:staff:table-data", Path="2",FullPath=".0.5.1.2."},

               new SysMenuPermission{Id=new Guid("BFD01AE7-D96E-8624-6477-D124E32DECA6"), Name="馆员详情",Pid=".0.5.",Type=(int)EnumPermissionType.Query,Permission="staff:detail",IsSysMenu=false,Path="2",FullPath=".0.5.2."},
               new SysMenuPermission{Id=new Guid("974E7E41-C1A0-EBCE-48B8-FBD15AF6CF32"), Name="读者初始数据",Pid=".0.5.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="1",FullPath=".0.5.2.1."},
               new SysMenuPermission{Id=new Guid("6D3277F2-9C4B-18F9-747E-CA7F3FD5E5C7"), Name="读者详情数据脱敏",Pid=".0.5.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:*",Path="2",FullPath=".0.5.2.2."},
               new SysMenuPermission{Id=new Guid("3242245C-748A-FAEB-2BE5-19953673133E"), Name="读者详情数据编辑",Pid=".0.5.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:by-id-for-edit:*",Path="3",FullPath=".0.5.2.3."},
               new SysMenuPermission{Id=new Guid("80D06311-4A3D-15C6-E98C-9636316DE9EB"), Name="读者积分数据",Pid=".0.5.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:user-points-table-data",Path="4",FullPath=".0.5.2.4."},
               new SysMenuPermission{Id=new Guid("4BC7F164-3CCD-8594-3210-9F007298D5EE"), Name="读者借阅记录",Pid=".0.5.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:user-borrow-table-data",Path="5",FullPath=".0.5.2.5."},
               new SysMenuPermission{Id=new Guid("39B672B2-BC2F-527E-A68E-0B48897D1BDE"), Name="读者授权应用",Pid=".0.5.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:user-auth-app-list-data",Path="6",FullPath=".0.5.2.6."},
               new SysMenuPermission{Id=new Guid("F0A5575C-02B5-5C4E-4209-251A80E8940F"), Name="读者行为日志",Pid=".0.5.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:user-log-table-data",Path="7",FullPath=".0.5.2.7."},
               new SysMenuPermission{Id=new Guid("D4EB0149-9558-1237-EAD0-B50D0BA5CECA"), Name="读者卡列表",Pid=".0.5.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:user-card-list-data:*",Path="8",FullPath=".0.5.2.8."},
               new SysMenuPermission{Id=new Guid("5D8B5722-E778-148D-34E4-18D5B4231263"), Name="读者卡申请",Pid=".0.5.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:card:user-card-apply-list-data:*",Path="9",FullPath=".0.5.2.9."},

               new SysMenuPermission{Id=new Guid("D8F84DD0-CCCF-EAA3-67ED-E635CECD1337"), Name="新增馆员",Pid=".0.5.",Type=(int)EnumPermissionType.Operate,Permission="staff:create",IsSysMenu=false,Path="3",FullPath=".0.5.3."},
               new SysMenuPermission{Id=new Guid("A65F4509-D86A-E7F0-B573-8D271081F337"), Name="读者初始数据",Pid=".0.5.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="1",FullPath=".0.5.3.1."},
               new SysMenuPermission{Id=new Guid("F20F6CD7-A4C0-733B-1014-C396752CA18D"), Name="读者新增",Pid=".0.5.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:user",Path="2",FullPath=".0.5.3.2."},

               new SysMenuPermission{Id=new Guid("8E8843D9-7E3D-61E5-116B-75B2E65FE073"), Name="新增临时馆员",Pid=".0.5.",Type=(int)EnumPermissionType.Operate,Permission="staff:createTemp",IsSysMenu=false,Path="4",FullPath=".0.5.4."},
               new SysMenuPermission{Id=new Guid("2FA36B80-BEA5-0467-74FF-E543E3ABB1C5"), Name="馆员初始数据",Pid=".0.5.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:staff:init-data", Path="1",FullPath=".0.5.4.1."},
               new SysMenuPermission{Id=new Guid("ECECE95D-7964-8F6D-FDF2-6F35A927C0DE"), Name="馆员临时新增",Pid=".0.5.4.",Type=(int)EnumPermissionType.Api,Permission="post-api:staff:temp-staff", Path="2",FullPath=".0.5.4.2."},

               new SysMenuPermission{Id=new Guid("6848CC6C-80B5-5FFA-8B5D-92F9AE79A74E"), Name="变动组织",Pid=".0.5.",Type=(int)EnumPermissionType.Operate,Permission="staff:setDepart",IsSysMenu=false,Path="5",FullPath=".0.5.5."},
               new SysMenuPermission{Id=new Guid("2CD0929B-E75F-B507-81F3-2A9B7B3D915A"), Name="馆员初始数据",Pid=".0.5.5.",Type=(int)EnumPermissionType.Api,Permission="get-api:staff:init-data", Path="1",FullPath=".0.5.5.1."},
               new SysMenuPermission{Id=new Guid("9323F751-52F0-3DF4-4DE3-4D6DDD83BB96"), Name="织机构数据",Pid=".0.5.5.",Type=(int)EnumPermissionType.Api,Permission="get-api:property-group:org-list", Path="2",FullPath=".0.5.5.2."},
               new SysMenuPermission{Id=new Guid("F90DE2DC-345C-9EE2-2A36-C34EB7A6930D"), Name="组织变动",Pid=".0.5.5.",Type=(int)EnumPermissionType.Api,Permission="post-api:staff:batch-set-department", Path="3",FullPath=".0.5.5.3."},

               new SysMenuPermission{Id=new Guid("544339BA-DD92-DCF3-9378-3160EDB72163"), Name="删除馆员",Pid=".0.5.",Type=(int)EnumPermissionType.Operate,Permission="staff:delete",IsSysMenu=false,Path="6",FullPath=".0.5.6."},
               new SysMenuPermission{Id=new Guid("16809AF2-1384-4345-3FB5-1E6A5CC3D8C1"), Name="删除馆员",Pid=".0.5.6.",Type=(int)EnumPermissionType.Api,Permission="delete-api:staff", Path="1",FullPath=".0.5.6.1."},
               #endregion
               #region ------------------------变动审核

               new SysMenuPermission{Id=new Guid("F815F5E9-78D6-4A96-EC7D-3972CB125F7B"), Name="变动审核",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_changeAudit",Component="",IsSysMenu=false,Path="6",FullPath=".0.6."},
               new SysMenuPermission{Id=new Guid("3296F75A-E8CE-A6C4-DCBE-DEF7BEF27680"), Name="读者修改审核列表",Pid=".0.6.",Type=(int)EnumPermissionType.Query,Permission="approve:readerList",IsSysMenu=false,Path="1",FullPath=".0.6.1."},
               new SysMenuPermission{Id=new Guid("DAA5F2C1-06F7-16B6-8F89-87C1A24F6054"), Name="数据审批初始化数据",Pid=".0.6.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:init-data", Path="1",FullPath=".0.6.1.1."},
               new SysMenuPermission{Id=new Guid("C7A28343-892A-B67D-3038-E8352A536962"), Name="读者变更日志数据",Pid=".0.6.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:user-change-log-table-data", Path="2",FullPath=".0.6.1.2."},

               new SysMenuPermission{Id=new Guid("76979850-B073-B363-74F9-832C74EFB312"), Name="读者修改详情",Pid=".0.6.",Type=(int)EnumPermissionType.Query,Permission="approve:readerDetail",IsSysMenu=false,Path="2",FullPath=".0.6.2."},
               new SysMenuPermission{Id=new Guid("842AA450-574F-593D-159B-20395B752F2E"), Name="数据审批初始化数据",Pid=".0.6.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:init-data", Path="1",FullPath=".0.6.2.1."},
               new SysMenuPermission{Id=new Guid("8B7D5D83-0C5F-FCF2-6609-A1EF8F2D4029"), Name="读者变更详情数据",Pid=".0.6.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:user-change-log-detail-info:*", Path="2",FullPath=".0.6.2.2."},
               new SysMenuPermission{Id=new Guid("3E859574-6B15-31D5-0537-91E7ECB6C86D"), Name="读者变更详情数据",Pid=".0.6.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:user-change-log-detail-info:*:*", Path="3",FullPath=".0.6.2.3."},

               new SysMenuPermission{Id=new Guid("DE24307D-AB30-025C-8BDE-F004FCFD59F8"), Name="读者修改审核",Pid=".0.6.",Type=(int)EnumPermissionType.Operate,Permission="approve:readerApprove",IsSysMenu=false,Path="3",FullPath=".0.6.3."},
               new SysMenuPermission{Id=new Guid("1B5F13A2-0A71-82A3-8704-B5FBEEDF8F6F"), Name="读者变更审批",Pid=".0.6.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:data-approve:approve-user-change", Path="1",FullPath=".0.6.3.1."},

               new SysMenuPermission{Id=new Guid("FCD062F3-A3FA-786C-900B-CBE15E5933E9"), Name="属性修改审核列表",Pid=".0.6.",Type=(int)EnumPermissionType.Query,Permission="approve:propertyList",IsSysMenu=false,Path="4",FullPath=".0.6.4."},
               new SysMenuPermission{Id=new Guid("24181E9A-54B0-A8EB-A270-10177AADC0F8"), Name="数据审批初始化数据",Pid=".0.6.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:init-data", Path="1",FullPath=".0.6.4.1."},
               new SysMenuPermission{Id=new Guid("FB18E245-46AA-F554-2857-6361DE63859A"), Name="属性变更日志数据",Pid=".0.6.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:property-change-log-table-data", Path="2",FullPath=".0.6.4.2."},

               new SysMenuPermission{Id=new Guid("92C462EA-9642-4401-5D22-113282F12DE2"), Name="属性修改详情",Pid=".0.6.",Type=(int)EnumPermissionType.Query,Permission="approve:propertyDetail",IsSysMenu=false,Path="5",FullPath=".0.6.5."},
               new SysMenuPermission{Id=new Guid("6739BF5E-885B-14C2-E344-48C0C0152490"), Name="数据审批初始化数据",Pid=".0.6.5.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:init-data", Path="1",FullPath=".0.6.5.1."},
               new SysMenuPermission{Id=new Guid("46C9F6ED-AE00-C8E5-ECAB-6E6EF2C3E74D"), Name="属性变更详情数据",Pid=".0.6.5.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:property-change-log-detail-items:*", Path="2",FullPath=".0.6.5.2."},

               new SysMenuPermission{Id=new Guid("F185541F-3DF7-52D1-237C-3C93E6065704"), Name="属性修改审核",Pid=".0.6.",Type=(int)EnumPermissionType.Operate,Permission="approve:propertyApprove",IsSysMenu=false,Path="6",FullPath=".0.6.6."},
               new SysMenuPermission{Id=new Guid("5E9DB5E8-33CD-4418-5911-D3F256570E04"), Name="属性变更审核",Pid=".0.6.6.",Type=(int)EnumPermissionType.Api,Permission="post-api:data-approve:approve-property-change", Path="1",FullPath=".0.6.6.1."},

               new SysMenuPermission{Id=new Guid("539DEA32-61D6-AC47-A106-36DD46FCC38F"), Name="用户注册审核列表",Pid=".0.6.",Type=(int)EnumPermissionType.Query,Permission="approve:registerList",IsSysMenu=false,Path="7",FullPath=".0.6.7."},
               new SysMenuPermission{Id=new Guid("43047ECA-2436-685C-A32F-409C0A628816"), Name="数据审批初始化数据",Pid=".0.6.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:init-data", Path="1",FullPath=".0.6.7.1."},
               new SysMenuPermission{Id=new Guid("2230BD68-36D6-6B16-0636-6CC6DF0AEFED"), Name="用户注册审批日志",Pid=".0.6.7.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:user-register-table-data", Path="2",FullPath=".0.6.7.2." },

               new SysMenuPermission{Id=new Guid("D1CDD5B5-C8FB-8807-7D9F-2CB8925A00DA"), Name="用户注册详情",Pid=".0.6.",Type=(int)EnumPermissionType.Query,Permission="approve:registerDetail",IsSysMenu=false,Path="8",FullPath=".0.6.8."},
               new SysMenuPermission{Id=new Guid("105EDF6F-908C-2681-AC87-29AEFF400573"), Name="数据审批初始化数据",Pid=".0.6.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:init-data", Path="1",FullPath=".0.6.8.1."},
               new SysMenuPermission{Id=new Guid("47510A68-C4A9-6EAB-80AF-80A67B8CADE4"), Name="用户注册详情",Pid=".0.6.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:user-register-detail-info:*", Path="2",FullPath=".0.6.8.2." },

               new SysMenuPermission{Id=new Guid("0FE76EA5-61C8-27DE-4DD7-11C4C27F68C1"), Name="用户注册审核",Pid=".0.6.",Type=(int)EnumPermissionType.Operate,Permission="approve:registerApprove",IsSysMenu=false,Path="9",FullPath=".0.6.9."},
               new SysMenuPermission{Id=new Guid("8BCF92A4-D7E2-0BEF-03FC-F6A74488569A"), Name="用户注册审核",Pid=".0.6.9.",Type=(int)EnumPermissionType.Api,Permission="post-api:data-approve:approve-user-register", Path="1",FullPath=".0.6.9.1."},

               new SysMenuPermission{Id=new Guid("F77FC1A1-B1AD-5CB0-E729-87B8C6FEE9DB"), Name="领卡审核列表",Pid=".0.6.",Type=(int)EnumPermissionType.Query,Permission="approve:cardClaimList",IsSysMenu=false,Path="10",FullPath=".0.6.10."},
               new SysMenuPermission{Id=new Guid("EDFCCF2C-30C8-4075-23F9-B736FC231687"), Name="数据审批初始化数据",Pid=".0.6.10.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:init-data", Path="1",FullPath=".0.6.10.1."},
               new SysMenuPermission{Id=new Guid("C51D19F0-8E1F-E136-6723-F9D5A02DB0E4"), Name="用户领卡审批日志",Pid=".0.6.10.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:card-claim-table-data", Path="2",FullPath=".0.6.10.2." },

               new SysMenuPermission{Id=new Guid("B11CBAF0-884D-CD65-3816-FBE8AF6F4E59"), Name="领卡详情",Pid=".0.6.",Type=(int)EnumPermissionType.Query,Permission="approve:cardClaimDetail",IsSysMenu=false,Path="11",FullPath=".0.6.11."},
               new SysMenuPermission{Id=new Guid("9A624AF1-C46D-A977-B471-D2062BCF3E1B"), Name="数据审批初始化数据",Pid=".0.6.11.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:init-data", Path="1",FullPath=".0.6.11.1."},
               new SysMenuPermission{Id=new Guid("50E27B34-340B-036C-EB78-4F04F36196E6"), Name="用户领卡审批日志",Pid=".0.6.11.",Type=(int)EnumPermissionType.Api,Permission="get-api:data-approve:card-claim-detail-info:*", Path="2",FullPath=".0.6.11.2." },

               new SysMenuPermission{Id=new Guid("0F4B05F5-07FF-6E30-26B8-DEFD3779C55B"), Name="领卡审核",Pid=".0.6.",Type=(int)EnumPermissionType.Operate,Permission="approve:cardClaimApprove",IsSysMenu=false,Path="12",FullPath=".0.6.12."},
               new SysMenuPermission{Id=new Guid("AC1ADA35-02BE-D858-8BCB-77AB4AF7C096"), Name="领卡审核",Pid=".0.6.12.",Type=(int)EnumPermissionType.Api,Permission="post-api:data-approve:approve-card-claim", Path="1",FullPath=".0.6.12.1."},
               #endregion
               #region ------------------------管理设置
               new SysMenuPermission{Id=new Guid("9036C946-42CD-683C-CA5F-EFAA57AA897D"), Name="管理设置",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_userSet",Component="",IsSysMenu=true,Path="7",FullPath=".0.7."},
               new SysMenuPermission{Id=new Guid("57288EBE-7D8E-9FEB-6028-18AE7301C39C"), Name="基础设置查看",Pid=".0.7.",Type=(int)EnumPermissionType.Query,Permission="setting:basicDetail",IsSysMenu=false,Path="1",FullPath=".0.7.1."},
               new SysMenuPermission{Id=new Guid("53E2E811-773D-3DE8-E9C9-CC1BA5633C2F"), Name="配置初始化数据",Pid=".0.7.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:basic-config:init-data", Path="1",FullPath=".0.7.1.1."},
               new SysMenuPermission{Id=new Guid("3ABE6766-C0B4-3367-4356-C72EDD521548"), Name="基础配置数据",Pid=".0.7.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:basic-config:basic-config", Path="2",FullPath=".0.7.1.2."},
               new SysMenuPermission{Id=new Guid("CE47C61C-512D-052B-8248-347227CBB7D8"), Name="可认领读者卡用户数据",Pid=".0.7.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:basic-config:card-claim-reader", Path="3",FullPath=".0.7.1.3."},
               new SysMenuPermission{Id=new Guid("FB2E2EE3-ED0C-9390-B8D5-830F39B9A29D"), Name="可补全读者用户数据",Pid=".0.7.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:basic-config:info-append-reader", Path="4",FullPath=".0.7.1.4."},
               new SysMenuPermission{Id=new Guid("A7C8183C-EA54-A16F-7739-C2321561C62B"), Name="可编辑用户数据",Pid=".0.7.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:basic-config:reader-edit-property", Path="5",FullPath=".0.7.1.5."},

               new SysMenuPermission{Id=new Guid("1193CE87-6F53-EC1B-FA8B-1039863D8903"), Name="基础设置修改",Pid=".0.7.",Type=(int)EnumPermissionType.Operate,Permission="setting:basicSet",IsSysMenu=false,Path="2",FullPath=".0.7.2."},
               new SysMenuPermission{Id=new Guid("966D2353-CA91-3ED6-0151-C196DA10AC06"), Name="编辑配置",Pid=".0.7.2.",Type=(int)EnumPermissionType.Api,Permission="post-api:basic-config:set-basic-config", Path="1",FullPath=".0.7.2.1."},
               new SysMenuPermission{Id=new Guid("BD48761E-0F9D-6C2C-F82D-5BB1ED234843"), Name="设置领卡用户",Pid=".0.7.2.",Type=(int)EnumPermissionType.Api,Permission="post-api:basic-config:set-card-claim-reader", Path="2",FullPath=".0.7.2.2."},
               new SysMenuPermission{Id=new Guid("F7D4220F-6D60-7494-1241-EA600903CADC"), Name="设置补充信息用户",Pid=".0.7.2.",Type=(int)EnumPermissionType.Api,Permission="post-api:basic-config:set-info-append-reader", Path="3",FullPath=".0.7.2.3."},
               new SysMenuPermission{Id=new Guid("12D24D94-D3D4-BF82-4944-40D5BBC3B878"), Name="设置可编辑属性",Pid=".0.7.2.",Type=(int)EnumPermissionType.Api,Permission="post-api:basic-config:set-reader-edit-property", Path="4",FullPath=".0.7.2.4."},

               new SysMenuPermission{Id=new Guid("9C1DA566-1CCA-370D-F3D6-B340A6A98605"), Name="权限管理角色列表查看",Pid=".0.7.",Type=(int)EnumPermissionType.Query,Permission="setting:roleList",IsSysMenu=false,Path="3",FullPath=".0.7.3."},
               new SysMenuPermission{Id=new Guid("09BFB0F0-DB20-B837-3701-E877E3141490"), Name="角色表格数据",Pid=".0.7.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:role:role-table-data", Path="1",FullPath=".0.7.3.1."},

               new SysMenuPermission{Id=new Guid("06808991-5E21-303D-7BB3-B24116AFC961"), Name="角色权限编辑",Pid=".0.7.",Type=(int)EnumPermissionType.Operate,Permission="setting:rolePermissionSet",IsSysMenu=false,Path="4",FullPath=".0.7.4."},
               new SysMenuPermission{Id=new Guid("3F10EDF4-0B7A-CC4C-A835-9DD7172CB267"), Name="角色详情数据",Pid=".0.7.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:role:role-data:*", Path="1",FullPath=".0.7.4.1."},
               new SysMenuPermission{Id=new Guid("54565D60-DCE5-D393-DF2C-050EBC8D1CD8"), Name="角色详情数据",Pid=".0.7.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:role:role-data-by-code:*", Path="2",FullPath=".0.7.4.2."},
               new SysMenuPermission{Id=new Guid("648ED08D-8BC0-B54C-62B9-F4B0C0F6DF5C"), Name="角色编辑",Pid=".0.7.4.",Type=(int)EnumPermissionType.Api,Permission="put-api:role", Path="3",FullPath=".0.7.4.3."},

               new SysMenuPermission{Id=new Guid("69BACC1D-B93D-7034-C332-7F879EEE5F1A"), Name="角色新增",Pid=".0.7.",Type=(int)EnumPermissionType.Operate,Permission="setting:roleCreate",IsSysMenu=false,Path="5",FullPath=".0.7.5."},
               new SysMenuPermission{Id=new Guid("274955A0-A6FA-828A-B622-F82331EFFC88"), Name="角色创建",Pid=".0.7.5.",Type=(int)EnumPermissionType.Api,Permission="post-api:role", Path="1",FullPath=".0.7.5.1."},

               new SysMenuPermission{Id=new Guid("0FB0B725-711E-FC17-3E04-71C43C139589"), Name="角色删除",Pid=".0.7.",Type=(int)EnumPermissionType.Operate,Permission="setting:roleDelete",IsSysMenu=false,Path="6",FullPath=".0.7.6."},
               new SysMenuPermission{Id=new Guid("18805947-3D9B-25B9-DA13-1CEA67EBBE83"), Name="角色删除",Pid=".0.7.6.",Type=(int)EnumPermissionType.Api,Permission="delete-api:role:*", Path="1",FullPath=".0.7.6.1."},

               new SysMenuPermission{Id=new Guid("D5807C6A-F17A-AFDC-13CF-2C3FD9797E76"), Name="角色馆员设置",Pid=".0.7.",Type=(int)EnumPermissionType.Operate,Permission="setting:roleStaffSet",IsSysMenu=false,Path="7",FullPath=".0.7.7."},
               new SysMenuPermission{Id=new Guid("FFDF0B7E-2CBF-0F2D-5F2B-E769A663C2A1"), Name="添加角色馆员",Pid=".0.7.7.",Type=(int)EnumPermissionType.Api,Permission="post-api:role:role-users", Path="1",FullPath=".0.7.7.1."},
               new SysMenuPermission{Id=new Guid("9F484A61-F761-5615-ECBD-8C49AB98DC24"), Name="删除角色馆员",Pid=".0.7.7.",Type=(int)EnumPermissionType.Api,Permission="delete-api:role:user-role", Path="2",FullPath=".0.7.7.2."},

               new SysMenuPermission{Id=new Guid("41A0627D-B349-6E20-82F0-F8B0147CEE8C"), Name="权限管理馆员列表查看",Pid=".0.7.",Type=(int)EnumPermissionType.Query,Permission="setting:staffList",IsSysMenu=false,Path="8",FullPath=".0.7.8."},
               new SysMenuPermission{Id=new Guid("B68B2D1C-00B7-AA25-EB09-F2C3B91FFFD1"), Name="角色馆员数据",Pid=".0.7.8.",Type=(int)EnumPermissionType.Api,Permission="get-api:role:staff-table-data", Path="1",FullPath=".0.7.8.1."},

               new SysMenuPermission{Id=new Guid("9316F967-1AD5-58D8-8D8E-AC8A1189432B"), Name="馆员修改角色",Pid=".0.7.",Type=(int)EnumPermissionType.Operate,Permission="setting:staffRoleSet",IsSysMenu=false,Path="9",FullPath=".0.7.9."},
               new SysMenuPermission{Id=new Guid("5151AAFE-0FB6-6BC9-023E-EEAE6F9EFF27"), Name="设置用户角色",Pid=".0.7.9.",Type=(int)EnumPermissionType.Api,Permission="post-api:role:set-user-roles", Path="1",FullPath=".0.7.9.1."},
               new SysMenuPermission{Id=new Guid("9584809F-EA24-330F-69D3-8788245E278F"), Name="删除角色馆员",Pid=".0.7.9.",Type=(int)EnumPermissionType.Api,Permission="delete-api:role:user-role", Path="2",FullPath=".0.7.9.2."},

               #endregion

               //用户分组
               //new SysMenuPermission{Id=new Guid("A87261CD-C808-0C6E-1462-2D6D1EA45B9B"), Name="用户分组",Pid=".0.",Type=(int)EnumPermissionType.Dir,Router="/admin_GroupAdmin",Component="", Path="3",FullPath=".0.3."},
               #region 用户组管理
               //------------------------用户组管理
               new SysMenuPermission{Id=new Guid("75783076-DAD7-4359-D276-08D53EFD1059"), Name="用户组管理",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/admin_userGroupList",Component="", Path="8",FullPath=".0.8."},
               new SysMenuPermission{Id=new Guid("41479130-C0CE-94BE-EA30-6C275EA049A2"), Name="用户组列表",Pid=".0.8.",Type=(int)EnumPermissionType.Query, Permission="userGroup:list", Path="1",FullPath=".0.8.1."},
               new SysMenuPermission{Id=new Guid("A2E39A39-F657-5EB8-7CD0-1B1AAD858BA8"), Name="用户组初始化数据",Pid=".0.8.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:user-group:init-data", Path="1",FullPath=".0.8.1.1."},
               new SysMenuPermission{Id=new Guid("DDF5C8D1-021D-B5E6-4A8E-69DAD0E12B25"), Name="用户组列表数据",Pid=".0.8.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:user-group:table-data", Path="2",FullPath=".0.8.1.2."},

               new SysMenuPermission{Id=new Guid("BCCC514F-8D94-B35A-29F4-142172280420"), Name="用户组详情",Pid=".0.8.",Type=(int)EnumPermissionType.Query, Permission="userGroup:detail", Path="2",FullPath=".0.8.2."},
               new SysMenuPermission{Id=new Guid("E35E3EED-0739-F7C4-6C11-91E666448F90"), Name="用户组初始化数据",Pid=".0.8.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user-group:init-data", Path="1",FullPath=".0.8.2.1."},
               new SysMenuPermission{Id=new Guid("784134A1-E3CE-AFD3-398D-42FAC89F3FCB"), Name="用户组详情数据",Pid=".0.8.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:user-group:by-id:*", Path="2",FullPath=".0.8.2.2."},

               new SysMenuPermission{Id=new Guid("C9FF6C0E-432B-C836-672F-02E3F7D2D17C"), Name="用户组新增",Pid=".0.8.",Type=(int)EnumPermissionType.Operate,Permission="userGroup:create", Path="3",FullPath=".0.8.3."},
               new SysMenuPermission{Id=new Guid("63523827-8540-DB03-A3E1-DC3574F30A8A"), Name="用户组初始化数据",Pid=".0.8.3.",Type=(int)EnumPermissionType.Api,Permission="get-api:user-group:init-data", Path="1",FullPath=".0.8.3.1."},
               new SysMenuPermission{Id=new Guid("CA67C650-F97F-2845-784D-E0CD2407ACC1"), Name="用户组创建",Pid=".0.8.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:user-group", Path="2",FullPath=".0.8.3.2."},
               new SysMenuPermission{Id=new Guid("38910C9D-51D1-A97F-A970-71E246FFDB1A"), Name="用户组数据导入模板",Pid=".0.8.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:download-user-import-template", Path="3",FullPath=".0.8.3.3."},
               new SysMenuPermission{Id=new Guid("55718F69-B43C-EA61-DE68-8571CC963898"), Name="用户组用户导入",Pid=".0.8.3.",Type=(int)EnumPermissionType.Api,Permission="post-api:user-group:import-group-user", Path="4",FullPath=".0.8.3.4."},

               new SysMenuPermission{Id=new Guid("D060841B-8EB1-7716-D410-F19B29AA0DF1"), Name="用户组编辑",Pid=".0.8.",Type=(int)EnumPermissionType.Operate,Permission="userGroup:update", Path="4",FullPath=".0.8.4."},
               new SysMenuPermission{Id=new Guid("2034B29E-4D64-C1FD-BB69-B85053A6EEB1"), Name="用户组初始化数据",Pid=".0.8.4.",Type=(int)EnumPermissionType.Api,Permission="get-api:user-group:init-data", Path="1",FullPath=".0.8.4.1."},
               new SysMenuPermission{Id=new Guid("7EA16C72-5BBE-7593-7E1C-CAE97BE8A479"), Name="用户组编辑",Pid=".0.8.4.",Type=(int)EnumPermissionType.Api,Permission="put-api:user-group", Path="2",FullPath=".0.8.4.2."},
               new SysMenuPermission{Id=new Guid("201B1C6B-51A1-E96D-8914-AF6C4FD811A8"), Name="用户组数据导入模板",Pid=".0.8.4.",Type=(int)EnumPermissionType.Api,Permission="post-api:download-user-import-template", Path="3",FullPath=".0.8.4.3."},
               new SysMenuPermission{Id=new Guid("EA60BFCF-4EBD-B878-4087-0A8A2D1C19E5"), Name="用户组用户导入",Pid=".0.8.4.",Type=(int)EnumPermissionType.Api,Permission="post-api:user-group:import-group-user", Path="4",FullPath=".0.8.4.4."},

               new SysMenuPermission{Id=new Guid("39F58976-99BD-0CDD-D20E-09EF2EFB589B"), Name="用户组删除",Pid=".0.8.",Type=(int)EnumPermissionType.Operate,Permission="userGroup:delete", Path="5",FullPath=".0.8.5."},
               new SysMenuPermission{Id=new Guid("D0CBCCFF-F1F1-22D3-AFD4-363DCAB4306C"), Name="用户组删除",Pid=".0.8.5.",Type=(int)EnumPermissionType.Api,Permission="delete-api:user-group:*", Path="1",FullPath=".0.8.5.1."},

               new SysMenuPermission{Id=new Guid("0A75C689-413B-AA5A-DE7A-C74960198B2E"), Name="用户列表",Pid=".0.8.",Type=(int)EnumPermissionType.Query,Permission="userGroup:userList", Path="6",FullPath=".0.8.6."},
               new SysMenuPermission{Id=new Guid("65C8B8B2-BB0B-9553-9DC3-0412B5264EB6"), Name="用户组初始化数据",Pid=".0.8.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:user-group:init-data", Path="1",FullPath=".0.8.6.1."},
               new SysMenuPermission{Id=new Guid("9990C9D3-2DA8-789C-02E5-9FA32F6C4D08"), Name="读者初始数据",Pid=".0.8.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="2",FullPath=".0.8.6.2."},
               new SysMenuPermission{Id=new Guid("C3AF7E49-181B-4970-85AF-E50C66AC37DA"), Name="用户组简要信息",Pid=".0.8.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:user-group:group-brief-info:*", Path="3",FullPath=".0.8.6.3."},
               new SysMenuPermission{Id=new Guid("98BBC997-D425-C6A5-6C73-30D8F8277027"), Name="用户组列表",Pid=".0.8.6.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:basic-user-table-data", Path="4",FullPath=".0.8.6.4."},

               new SysMenuPermission{Id=new Guid("BC387766-EC1E-E43D-A4CC-9B07336EC7C3"), Name="用户列表添加用户",Pid=".0.8.",Type=(int)EnumPermissionType.Query,Permission="userGroup:userListAddUser", Path="7",FullPath=".0.8.7."},
               new SysMenuPermission{Id=new Guid("7476B79F-5B21-F91A-7CA7-D3CFBEBB9955"), Name="用户组添加用户",Pid=".0.8.7.",Type=(int)EnumPermissionType.Api,Permission="post-api:user-group:user-to-group", Path="1",FullPath=".0.8.7.1."},

               new SysMenuPermission{Id=new Guid("FC4B699D-BF58-A01A-05F5-0F84DEABAD7A"), Name="用户列表删除用户",Pid=".0.8.",Type=(int)EnumPermissionType.Query,Permission="userGroup:userListDeleteUser", Path="8",FullPath=".0.8.8."},
               new SysMenuPermission{Id=new Guid("C82A6CDD-0840-90C2-9879-1290C821C449"), Name="用户组删除用户",Pid=".0.8.8.",Type=(int)EnumPermissionType.Api,Permission="delete-api:user-group:group-users", Path="1",FullPath=".0.8.8.1."},

               new SysMenuPermission{Id=new Guid("C13E1A51-475B-BD2B-D58E-2DB7DEFFA3ED"), Name="用户列表导出",Pid=".0.8.",Type=(int)EnumPermissionType.Query,Permission="userGroup:userListExportUser", Path="9",FullPath=".0.8.9."},
               new SysMenuPermission{Id=new Guid("C1FD61B8-D25B-6FE6-652F-E4283835CC23"), Name="读者初始数据",Pid=".0.8.9.",Type=(int)EnumPermissionType.Api,Permission="get-api:user:init-data", Path="1",FullPath=".0.8.9.1."},
               new SysMenuPermission{Id=new Guid("53BC9336-AD71-3CC7-B829-86FF6BDF75B5"), Name="读者导出数据简要信息",Pid=".0.8.9.",Type=(int)EnumPermissionType.Api,Permission="post-api:user:export-user-data-brief-info",Path="2",FullPath=".0.8.9.2."},
               new SysMenuPermission{Id=new Guid("8A9DBC45-FDC6-ABDB-6496-5696F6E4CDCB"), Name="读者导出数据",Pid=".0.8.9.",Type=(int)EnumPermissionType.Api,Permission="post-api:user:export-user-data",Path="3",FullPath=".0.8.9.3."},
	           #endregion 
               
             };
            var repeatItems = fullMenu.GroupBy(x => x.Id).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            //需要新增的菜单
            if (!await _sysMenuPermissionRepository.AnyAsync())
            {
                //系统菜单及权限
                await _dbContext.BulkInsertAsync(fullMenu);
            }

        }

        /// <summary>
        /// 初始化属性
        /// </summary>
        /// <returns></returns>
        private async Task InitProperty()
        {
            var initProperties = new List<Property>
            {
                new Property {Id=new Guid("4E770B81-DB99-C501-B9D9-6224ADA0ADCB"),Name="读者姓名",Code="User_Name",Type=(int)EnumPropertyType.文本,Intro="",Required=true,Unique=false,ShowOnTable=true,CanSearch=true,ForReader=true,ForCard=true,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("CB4F474B-0DB2-A76C-E93A-212F55E02611"),Name="用户来源",Code="User_SourceFrom",Type=(int)EnumPropertyType.属性组,Intro="",Required=true,Unique=false,ShowOnTable=true,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("601081EE-8CD1-1CE0-5813-385EB645404B"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("77B29B70-9ABE-6E7A-DFBF-E385BD5F2257"),Name="昵称",Code="User_NickName",Type=(int)EnumPropertyType.文本,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=false,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("A7489FA6-EBD2-11D4-38F9-70C55D0860A7"),Name="学工号",Code="User_StudentNo",Type=(int)EnumPropertyType.文本,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("3A2912EA-40C2-530E-A15E-6CCA9A62F5D4"),Name="单位",Code="User_Unit",Type=(int)EnumPropertyType.文本,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("408622B6-2C28-79E4-9A9B-21380A22CDC0"),Name="学历",Code="User_Edu",Type=(int)EnumPropertyType.属性组,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("5E050C0F-89FE-05F1-91B1-469474915B02"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("320F3DD1-338F-CB71-7709-D9997B23B31E"),Name="职称",Code="User_Title",Type=(int)EnumPropertyType.属性组,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("DD8837CC-6AA9-EACA-F8D6-59ED86F37D11"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("4BC3E237-E6C2-56BA-CA3B-A7783FFF5E55"),Name="部门",Code="User_Depart",Type=(int)EnumPropertyType.属性组,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("63478D66-2B11-5B0F-5EDD-B90382A3A3B0"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("00741E36-F03B-CA08-27F9-094040D9ACFD"),Name="学院",Code="User_College",Type=(int)EnumPropertyType.属性组,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("EA58F647-B1FE-7285-0A68-B8AC1A56085E"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("C5B38D8A-338C-E86B-C66A-5B4FE325453F"),Name="系",Code="User_CollegeDepart",Type=(int)EnumPropertyType.属性组,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("6CC719BF-8E48-AA45-5501-B8123A0B9235"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("C414DF2D-FA0D-64CB-59EB-6489FF4C6E9A"),Name="专业",Code="User_Major",Type=(int)EnumPropertyType.属性组,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("5D2A3E39-4DDA-06A7-E034-876D6CF84602"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("DD820CA2-93C1-4643-CC7A-DECADB6128ED"),Name="年级",Code="User_Grade",Type=(int)EnumPropertyType.属性组,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("72A13F7B-4AF6-F673-CBEE-30FC411906E1"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("663F4B88-B34A-7225-C1C1-1A9A4B4D3EA7"),Name="班级",Code="User_Class",Type=(int)EnumPropertyType.属性组,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("E7ED55EA-9139-3ABE-B415-97FC759891E9"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("DED15570-2181-FB43-294A-BDED34254092"),Name="读者类型",Code="User_Type",Type=(int)EnumPropertyType.属性组,Intro="",Required=true,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=true,SysBuildIn=true,PropertyGroupID=new Guid("C2F0BCDB-5FA6-25A2-C41D-97891E02D5AB"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("3FEF11D8-3ABA-3C59-4248-2EDEEECD22AA"),Name="读者状态",Code="User_Status",Type=(int)EnumPropertyType.属性组,Intro="",Required=true,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("0E4F4A9D-A02E-FACA-3F00-FB609B6ED80D"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("88B58F55-D7B1-8CC9-41E3-98597C643000"),Name="身份证号/护照号",Code="User_IdCard",Type=(int)EnumPropertyType.文本,Intro="",Required=true,Unique=true,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("1D022C42-66ED-3E4C-1307-F79C1B39EAE6"),Name="联系电话",Code="User_Phone",Type=(int)EnumPropertyType.文本,Intro="",Required=true,Unique=true,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("F678F7E4-BA65-A50D-3ED1-5EC72D3A22BC"),Name="电子邮箱",Code="User_Email",Type=(int)EnumPropertyType.文本,Intro="",Required=false,Unique=true,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("7B77D90E-237D-1AE3-2960-A20EA039C761"),Name="出生年月",Code="User_Birthday",Type=(int)EnumPropertyType.时间,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("E8102E5D-6E26-CB0F-048C-5E9B141D7E62"),Name="性别",Code="User_Gender",Type=(int)EnumPropertyType.属性组,Intro="",Required=true,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=new Guid("BEB66F80-CE2A-DD3D-919C-9DFE796A21E6"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("87779E3F-A1AD-4BB6-95D3-D538502296CB"),Name="所在地",Code="User_Addr",Type=(int)EnumPropertyType.地址,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("6738467C-28D1-5FD4-1733-65CF14354F48"),Name="详细地址",Code="User_AddrDetail",Type=(int)EnumPropertyType.文本,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("5C9F928B-F99A-22CD-EFDD-D0C3D2D985B0"),Name="照片头像",Code="User_Photo",Type=(int)EnumPropertyType.附件,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("12DB671E-9390-7AFF-61F8-72395BFD3A64"),Name="离校时间",Code="User_LeaveTime",Type=(int)EnumPropertyType.时间,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=true,ForCard=false,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("6212F82E-8168-62E4-C70C-D0C2621CCEF7"),Name="卡号",Code="Card_No",Type=(int)EnumPropertyType.文本,Intro="",Required=true,Unique=true,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("1EB12208-FB7F-125D-A167-CD9B80EE0E9B"),Name="条码号",Code="Card_BarCode",Type=(int)EnumPropertyType.文本,Intro="",Required=false,Unique=true,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("CBEADBFC-AE37-40A5-B65E-9A8CFCBBA91C"),Name="物理码",Code="Card_PhysicNo",Type=(int)EnumPropertyType.文本,Intro="",Required=false,Unique=true,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("EE432260-2B2D-09EB-383F-5BDCB1AB98F8"),Name="统一认证号",Code="Card_IdentityNo",Type=(int)EnumPropertyType.文本,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("70FA7949-FAEA-E2AE-4C52-62CDC8199D8A"),Name="卡类型",Code="Card_Type",Type=(int)EnumPropertyType.属性组,Intro="",Required=true,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=new Guid("A8E10C5E-684F-6EFC-0C31-635AE77F8406"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("4BCEBFAE-002A-A163-07DE-1609A72A43E4"),Name="卡状态",Code="Card_Status",Type=(int)EnumPropertyType.属性组,Intro="",Required=true,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=new Guid("27346A65-0351-839F-69F2-A5BCA0FBF5EC"),Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("F88E34A9-D1B2-B04A-A1C9-9884BE2B1631"),Name="是否主卡",Code="Card_IsPrincipal",Type=(int)EnumPropertyType.是非,Intro="",Required=true,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("D340FDF1-6239-AB1F-A996-5E93B743F157"),Name="发证日期",Code="Card_IssueDate",Type=(int)EnumPropertyType.时间,Intro="",Required=true,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("0C0BD402-2E95-4670-5AC5-88B0A7F5EF4E"),Name="截止日期",Code="Card_ExpireDate",Type=(int)EnumPropertyType.时间,Intro="",Required=true,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
                new Property {Id=new Guid("83F0EC3D-CC16-3231-3604-5FD1CDB3553C"),Name="押金",Code="Card_Deposit",Type=(int)EnumPropertyType.数值,Intro="",Required=false,Unique=false,ShowOnTable=false,CanSearch=true,ForReader=false,ForCard=true,SysBuildIn=true,PropertyGroupID=Guid.Empty,Status=(int)EnumPropertyDataStatus.正常,ApproveStatus=(int)EnumPropertyApproveStatus.正常 },
            };

            if (!await _propertyRepository.AnyAsync())
            {
                //系统菜单及权限
                await _dbContext.BulkInsertAsync(initProperties);
            }
        }

        /// <summary>
        /// 初始化属性组
        /// </summary>
        /// <returns></returns>
        private async Task InitPropertyGroup()
        {
            var initGroups = new List<PropertyGroup>();
            var initGroupItems = new List<PropertyGroupItem>();
            initGroups.AddRange(new[] {
                new PropertyGroup {Id=new Guid("601081EE-8CD1-1CE0-5813-385EB645404B"),Name="用户来源",Code="User_SourceFrom",Type=(int)EnumGroupType.内置,SysBuildIn=true },
                new PropertyGroup {Id=new Guid("5E050C0F-89FE-05F1-91B1-469474915B02"),Name="学历",Code="User_Edu",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("DD8837CC-6AA9-EACA-F8D6-59ED86F37D11"),Name="职称",Code="User_Title",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("63478D66-2B11-5B0F-5EDD-B90382A3A3B0"),Name="部门",Code="User_Depart",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("EA58F647-B1FE-7285-0A68-B8AC1A56085E"),Name="学院",RequiredCode=true,Code="User_College",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("6CC719BF-8E48-AA45-5501-B8123A0B9235"),Name="系",RequiredCode=true,Code="User_CollegeDepart",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("5D2A3E39-4DDA-06A7-E034-876D6CF84602"),Name="专业",Code="User_Major",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("72A13F7B-4AF6-F673-CBEE-30FC411906E1"),Name="年级",Code="User_Grade",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("E7ED55EA-9139-3ABE-B415-97FC759891E9"),Name="班级",Code="User_Class",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("C2F0BCDB-5FA6-25A2-C41D-97891E02D5AB"),Name="读者类型",RequiredCode=true,Code="User_Type",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("0E4F4A9D-A02E-FACA-3F00-FB609B6ED80D"),Name="读者状态",Code="User_Status",Type=(int)EnumGroupType.内置,SysBuildIn=true },
                new PropertyGroup {Id=new Guid("BEB66F80-CE2A-DD3D-919C-9DFE796A21E6"),Name="性别",Code="User_Gender",Type=(int)EnumGroupType.内置,SysBuildIn=true },
                new PropertyGroup {Id=new Guid("A8E10C5E-684F-6EFC-0C31-635AE77F8406"),Name="卡类型",RequiredCode=true,Code="Card_Type",Type=(int)EnumGroupType.内置,SysBuildIn=false },
                new PropertyGroup {Id=new Guid("27346A65-0351-839F-69F2-A5BCA0FBF5EC"),Name="卡状态",Code="Card_Status",Type=(int)EnumGroupType.内置,SysBuildIn=true },
            });
            initGroupItems.AddRange(new[]
            {
                new PropertyGroupItem{Id=new Guid("9DA21185-4148-E9DA-C81D-E265BCD6F2E6"),GroupID=new Guid("601081EE-8CD1-1CE0-5813-385EB645404B"),Name="后台新增",SysBuildIn=true, Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},
                new PropertyGroupItem{Id=new Guid("423C226D-B086-7110-34FB-A71C3D75A183"),GroupID=new Guid("601081EE-8CD1-1CE0-5813-385EB645404B"),Name="用户注册",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},
                new PropertyGroupItem{Id=new Guid("A2B47455-DCDF-6D41-40EB-97D06DB8E113"),GroupID=new Guid("601081EE-8CD1-1CE0-5813-385EB645404B"),Name="后台导入",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},
                new PropertyGroupItem{Id=new Guid("17476075-F304-70E4-14FA-515853D80A64"),GroupID=new Guid("601081EE-8CD1-1CE0-5813-385EB645404B"),Name="数据同步",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},

                new PropertyGroupItem{Id=new Guid("6ABA1843-0AC2-71F3-A6D2-F51CFABC12A7"),GroupID=new Guid("0E4F4A9D-A02E-FACA-3F00-FB609B6ED80D"),Name="未激活",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},
                new PropertyGroupItem{Id=new Guid("18F2BD12-BA6F-5BFD-604F-087CA6D9B144"),GroupID=new Guid("0E4F4A9D-A02E-FACA-3F00-FB609B6ED80D"),Name="正常",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},
                new PropertyGroupItem{Id=new Guid("D1D8DC67-4730-E994-2D1C-D02495EDBEE0"),GroupID=new Guid("0E4F4A9D-A02E-FACA-3F00-FB609B6ED80D"),Name="禁用",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},
                new PropertyGroupItem{Id=new Guid("0A67518A-403A-A255-B634-BF30D4DDA66F"),GroupID=new Guid("0E4F4A9D-A02E-FACA-3F00-FB609B6ED80D"),Name="注销",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},

                new PropertyGroupItem{Id=new Guid("9348D690-F0B9-1D86-3022-939DCBD2C2F2"),GroupID=new Guid("BEB66F80-CE2A-DD3D-919C-9DFE796A21E6"),Name="男",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},
                new PropertyGroupItem{Id=new Guid("6146C69E-DA8A-D291-38A4-446D8AB51BAA"),GroupID=new Guid("BEB66F80-CE2A-DD3D-919C-9DFE796A21E6"),Name="女",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},

                new PropertyGroupItem{Id=new Guid("37209DE6-F60D-F575-BAE6-46AACC2340F3"),GroupID=new Guid("27346A65-0351-839F-69F2-A5BCA0FBF5EC"),Name="正常",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},
                new PropertyGroupItem{Id=new Guid("7EC3D44E-A76E-DE5E-1969-62503DA785D3"),GroupID=new Guid("27346A65-0351-839F-69F2-A5BCA0FBF5EC"),Name="挂失",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},
                new PropertyGroupItem{Id=new Guid("BF0CC916-169C-4A3E-A85D-1909C4B975C1"),GroupID=new Guid("27346A65-0351-839F-69F2-A5BCA0FBF5EC"),Name="停用",SysBuildIn=true,Code="",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},

                new PropertyGroupItem{Id=new Guid("0C85E18A-C022-A21B-E6B3-CFF9790A059B"),GroupID=new Guid("A8E10C5E-684F-6EFC-0C31-635AE77F8406"),Name="临时馆员卡",SysBuildIn=true,Code="tempmanager",Status=(int)EnumGroupItemDataStatus.正常,ApproveStatus=(int)EnumGroupItemApproveStatus.正常},


            });

            if (!await _propertyGroupRepository.AnyAsync())
            {
                //内置属性添加
                await _propertyGroupRepository.InsertAsync(initGroups);
                //内置选项
                await _propertyGroupItemRepository.InsertAsync(initGroupItems);
            }
        }

        /// <summary>
        /// 初始化角色
        /// </summary>
        /// <returns></returns>
        private async Task InitRole()
        {
            var tenantId = _dbContext.TenantInfo?.Name;
            var initRoles = new List<SysRole>();
            var initRolePermissions = new List<SysRoleMenu>();
            initRoles.AddRange(new[] {
                new SysRole{Id=new Guid("34BDF9E3-5EC6-AAE6-61BC-C16F5BACDCEA"), Name="敏感信息查看人员",Code="SensitiveInfoVisitor",Remark="",SysBuildIn=true,TenantId=tenantId},
                new SysRole{Id=new Guid("A638587F-4A3A-DA5E-CD8C-A53AA2040A3A"), Name="用户修改审核人员",Code="UserChangeLogApprover",Remark="",SysBuildIn=true,TenantId=tenantId},
                new SysRole{Id=new Guid("A13CE5A5-093D-9EBB-D785-4A2B4522806B"), Name="属性修改审核人员",Code="PropertyChangeLogApprover",Remark="",SysBuildIn=true,TenantId=tenantId},
                new SysRole{Id=new Guid("D51D302A-825B-9D52-921E-A9EAF702286F"), Name="读者领卡审核人员",Code="CardClaimApprover",Remark="",SysBuildIn=true,TenantId=tenantId},
                new SysRole{Id=new Guid("5C601A50-D74E-F779-29CE-DFF1449F2B0F"), Name="用户注册审核人员",Code="UserRegisterApprover",Remark="",SysBuildIn=true,TenantId=tenantId}
            });

            initRolePermissions.AddRange(new[]
            {
                //用户修改审核人员权限
                new SysRoleMenu{Id=new Guid("31665920-376D-8FF8-E46A-F233991AA6DA"),RoleID=new Guid("A638587F-4A3A-DA5E-CD8C-A53AA2040A3A"),MenuPermissionID=new Guid("01F7F728-F598-FE7A-FD9B-7F429D079CD2")},//顶级节点
                new SysRoleMenu{Id=new Guid("65F1456E-3C35-D170-F3D8-52BC3AA59B7D"),RoleID=new Guid("A638587F-4A3A-DA5E-CD8C-A53AA2040A3A"),MenuPermissionID=new Guid("9814B6CE-72EE-5B0A-06F1-47B92261F1B0")},//用户管理
                new SysRoleMenu{Id=new Guid("5CA21406-72D6-3CB0-18D0-F82C99EA688C"),RoleID=new Guid("A638587F-4A3A-DA5E-CD8C-A53AA2040A3A"),MenuPermissionID=new Guid("F815F5E9-78D6-4A96-EC7D-3972CB125F7B")},//变动审核
                new SysRoleMenu{Id=new Guid("9178076A-57DF-8011-5D36-AB5199AAA1AD"),RoleID=new Guid("A638587F-4A3A-DA5E-CD8C-A53AA2040A3A"),MenuPermissionID=new Guid("3296F75A-E8CE-A6C4-DCBE-DEF7BEF27680")},//读者修改审核列表
                new SysRoleMenu{Id=new Guid("CB6BAE75-7D54-8210-D0FB-7C315DE11A02"),RoleID=new Guid("A638587F-4A3A-DA5E-CD8C-A53AA2040A3A"),MenuPermissionID=new Guid("76979850-B073-B363-74F9-832C74EFB312")},//读者修改详情
                new SysRoleMenu{Id=new Guid("0E7AA3C8-C86E-CA1A-77E8-0B80367E9752"),RoleID=new Guid("A638587F-4A3A-DA5E-CD8C-A53AA2040A3A"),MenuPermissionID=new Guid("DE24307D-AB30-025C-8BDE-F004FCFD59F8")},//读者修改审核
                //属性修改审核人员权限
                new SysRoleMenu{Id=new Guid("9AA9FF5C-B7B6-80F6-B45A-146701DE4183"),RoleID=new Guid("A13CE5A5-093D-9EBB-D785-4A2B4522806B"),MenuPermissionID=new Guid("01F7F728-F598-FE7A-FD9B-7F429D079CD2")},//顶级节点
                new SysRoleMenu{Id=new Guid("5F6A2077-3380-1B65-B6EB-0A34CDDE65CA"),RoleID=new Guid("A13CE5A5-093D-9EBB-D785-4A2B4522806B"),MenuPermissionID=new Guid("9814B6CE-72EE-5B0A-06F1-47B92261F1B0")},//用户管理
                new SysRoleMenu{Id=new Guid("EE02F753-4351-DAC7-1DC2-A5928C5ACBCA"),RoleID=new Guid("A13CE5A5-093D-9EBB-D785-4A2B4522806B"),MenuPermissionID=new Guid("F815F5E9-78D6-4A96-EC7D-3972CB125F7B")},//变动审核
                new SysRoleMenu{Id=new Guid("ACB33864-56FD-3F60-CEA1-D44226A5AC9C"),RoleID=new Guid("A13CE5A5-093D-9EBB-D785-4A2B4522806B"),MenuPermissionID=new Guid("FCD062F3-A3FA-786C-900B-CBE15E5933E9")},//属性修改审核列表
                new SysRoleMenu{Id=new Guid("8BEB31C6-D908-A030-360B-02ED25A5B2C3"),RoleID=new Guid("A13CE5A5-093D-9EBB-D785-4A2B4522806B"),MenuPermissionID=new Guid("92C462EA-9642-4401-5D22-113282F12DE2")},//属性修改详情
                new SysRoleMenu{Id=new Guid("8F406EB9-7317-891F-EADF-0AF0EAA20093"),RoleID=new Guid("A13CE5A5-093D-9EBB-D785-4A2B4522806B"),MenuPermissionID=new Guid("F185541F-3DF7-52D1-237C-3C93E6065704")},//属性修改审核
                //读者领卡审核人员权限
                new SysRoleMenu{Id=new Guid("162009C7-50AD-6F96-BE37-4772FA66E620"),RoleID=new Guid("D51D302A-825B-9D52-921E-A9EAF702286F"),MenuPermissionID=new Guid("01F7F728-F598-FE7A-FD9B-7F429D079CD2")},//顶级节点
                new SysRoleMenu{Id=new Guid("C6E10DFD-D88C-1DA9-9E31-B66B79688045"),RoleID=new Guid("D51D302A-825B-9D52-921E-A9EAF702286F"),MenuPermissionID=new Guid("9814B6CE-72EE-5B0A-06F1-47B92261F1B0")},//用户管理
                new SysRoleMenu{Id=new Guid("0992663A-D682-4C3C-C5F2-BFFCBBBEDDB8"),RoleID=new Guid("D51D302A-825B-9D52-921E-A9EAF702286F"),MenuPermissionID=new Guid("F815F5E9-78D6-4A96-EC7D-3972CB125F7B")},//变动审核
                new SysRoleMenu{Id=new Guid("5EC0DF26-E273-D9A2-36FF-7FB5B38EB0C4"),RoleID=new Guid("D51D302A-825B-9D52-921E-A9EAF702286F"),MenuPermissionID=new Guid("F77FC1A1-B1AD-5CB0-E729-87B8C6FEE9DB")},//读者领卡审核列表
                new SysRoleMenu{Id=new Guid("B3CF4582-0368-6311-4A03-12F7319E8051"),RoleID=new Guid("D51D302A-825B-9D52-921E-A9EAF702286F"),MenuPermissionID=new Guid("B11CBAF0-884D-CD65-3816-FBE8AF6F4E59")},//读者领卡详情
                new SysRoleMenu{Id=new Guid("ABD07FB5-038B-7797-E7C2-AD7F169970E8"),RoleID=new Guid("D51D302A-825B-9D52-921E-A9EAF702286F"),MenuPermissionID=new Guid("0F4B05F5-07FF-6E30-26B8-DEFD3779C55B")},//读者领卡审核
                //用户注册审核人员权限
                new SysRoleMenu{Id=new Guid("1AE5B14C-BD25-0435-30DD-C24C6510B003"),RoleID=new Guid("5C601A50-D74E-F779-29CE-DFF1449F2B0F"),MenuPermissionID=new Guid("01F7F728-F598-FE7A-FD9B-7F429D079CD2")},//顶级节点
                new SysRoleMenu{Id=new Guid("58C1C87E-56DE-704E-25C4-F163426547C3"),RoleID=new Guid("5C601A50-D74E-F779-29CE-DFF1449F2B0F"),MenuPermissionID=new Guid("9814B6CE-72EE-5B0A-06F1-47B92261F1B0")},//用户管理
                new SysRoleMenu{Id=new Guid("0908CF22-07CA-87C4-BDC0-92FD5E955BB6"),RoleID=new Guid("5C601A50-D74E-F779-29CE-DFF1449F2B0F"),MenuPermissionID=new Guid("F815F5E9-78D6-4A96-EC7D-3972CB125F7B")},//变动审核
                new SysRoleMenu{Id=new Guid("2B45E821-D731-2E9F-FFB6-5F9192249E65"),RoleID=new Guid("5C601A50-D74E-F779-29CE-DFF1449F2B0F"),MenuPermissionID=new Guid("539DEA32-61D6-AC47-A106-36DD46FCC38F")},//用户注册审核列表
                new SysRoleMenu{Id=new Guid("955EC3A6-E8E3-C65B-FEF7-F30FCB94F632"),RoleID=new Guid("5C601A50-D74E-F779-29CE-DFF1449F2B0F"),MenuPermissionID=new Guid("D1CDD5B5-C8FB-8807-7D9F-2CB8925A00DA")},//用户注册详情
                new SysRoleMenu{Id=new Guid("51299EA6-8AD1-E686-27CF-03FBF53D2063"),RoleID=new Guid("5C601A50-D74E-F779-29CE-DFF1449F2B0F"),MenuPermissionID=new Guid("0FE76EA5-61C8-27DE-4DD7-11C4C27F68C1")},//用户注册审核
            });

            if (!await _sysRoleRepository.AnyAsync())
            {
                //添加内置角色
                await _sysRoleRepository.InsertAsync(initRoles);
                //添加内置角色权限
                await _sysRoleMenuRepository.InsertAsync(initRolePermissions);
            }
        }

        private async Task InitReaderEditProperty()
        {
            var tenantId = _dbContext.TenantInfo?.Name;
            var initEditProperties = new List<ReaderEditProperty>();
            initEditProperties.AddRange(new[]
            {
                new ReaderEditProperty{Id=new Guid("20CA8EEC-0E43-2581-1EDE-534BD77842A4"),PropertyCode="User_Name",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("8D597809-7FB7-669F-DE85-D2208578F22B"),PropertyCode="User_NickName",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("D88E30C9-1424-82AB-22A3-4FE8EDEF1B30"),PropertyCode="User_Unit",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("CBB8FB91-C346-A671-C4DB-D774891729FA"),PropertyCode="User_Edu",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("6C275994-3D16-49E9-AD55-5E91F78F6E0C"),PropertyCode="User_College",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("47826D4E-1E11-745A-5659-E1DBB4043163"),PropertyCode="User_Major",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("068D0B99-7C8B-1C44-BEB1-FEAAC074ABBB"),PropertyCode="User_Grade",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("BB30173B-90EA-E679-7777-8AF8EA1CED35"),PropertyCode="User_Class",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("05DF95FF-CAA0-8F0D-F645-C38E795E304A"),PropertyCode="User_IdCard",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("D4E1AE03-F23B-CF62-8389-397858E44AA7"),PropertyCode="User_Phone",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("63B34847-BF16-D9B2-A72D-3DC6DDE59B29"),PropertyCode="User_Email",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("1B3EB0C4-3D60-AC74-32D4-5DF3E9FD353D"),PropertyCode="User_Birthday",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("B9EDA4F3-5D51-5E45-5E5F-D7F9C2A377DA"),PropertyCode="User_Gender",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("A6DDCAD6-D995-12B6-2C9C-010782514AB5"),PropertyCode="User_Addr",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("C65C6410-662F-353E-D6DB-5DF5C8E3FF7B"),PropertyCode="User_AddrDetail",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new ReaderEditProperty{Id=new Guid("67DF92B7-9721-5343-4DAB-AB99D3F14DFD"),PropertyCode="User_Photo",IsEnable=true,IsCheck=false,TenantId=tenantId},

            });
            if (!await _readerEditPropertyRepository.AnyAsync())
            {
                //添加可编辑属性
                await _readerEditPropertyRepository.InsertAsync(initEditProperties);
            }
        }

        private async Task InitAdministrator()
        {
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var pwdEncode = encryptProvider.Encrypt("VipSmart_Dev", SiteGlobalConfig.SM2Key.PublicKey);

            var tenantId = _dbContext.TenantInfo?.Name;
            var initUsers = new List<EntityFramework.Core.Entitys.User>();
            var initCards = new List<Card>();
            var userData = new UserDto { ID = new Guid("75369404-547E-C2D7-5E82-727B59A344CF"), Name = "VipSmart_Dev", NickName = "", StudentNo = "vipsmart00001", Status = 1, IsStaff = true, StaffStatus = 1, SourceFrom = 0 };
            var userEntity = userData.Adapt<EntityFramework.Core.Entitys.User>();
            userEntity.Id = new Guid("75369404-547E-C2D7-5E82-727B59A344CF");
            userEntity.StaffBeginTime = DateTime.Now;
            userEntity.UserKey = $"{tenantId}_vipsmart00001";
            userEntity.TenantId = tenantId;
            userEntity.Type = "admin";
            userEntity.Type = "管理员";
            userEntity.Phone = "";
            userEntity.IsSuperVisor = true;
            initUsers.AddRange(new[]
            {
                userEntity
            });
            var cardEntity = new Card { Id = new Guid("01E13E8D-23F1-517A-FDEF-64CBA075070D"), UserID = new Guid("75369404-547E-C2D7-5E82-727B59A344CF"), No = "vipsmart00001", Status = 1, Type = "admin", TypeName = "管理员", IsPrincipal = true, IssueDate = DateTime.Now, ExpireDate = DateTime.Now.AddYears(4), Secret = pwdEncode, SysBuildIn = true };
            initCards.AddRange(new[] {
                cardEntity
            });
            if (!await _userRepository.AnyAsync(x => x.UserKey == userEntity.UserKey))
            {
                //添加可编辑属性
                await _userRepository.InsertAsync(initUsers);
            }
            if (!await _cardRepository.AnyAsync(x => x.No == cardEntity.No))
            {
                //添加可编辑属性
                await _cardRepository.InsertAsync(initCards);
            }

        }


        #endregion

        /// <summary>
        /// 编码测试
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public async Task<string> Base64Encrypt(string text)
        {
            var codeTable = FileHelper.ReadFileToText("EncodeTable.cert");
            var baseEncrypt = new Base64Crypt(codeTable);
            var encodedCode = baseEncrypt.Encode(text);
            return await Task.FromResult(encodedCode);
        }

        /// <summary>
        /// 解码测试
        /// </summary>
        /// <param name="encodedCode"></param>
        /// <returns></returns>
        public async Task<string> Base64Decrypt(string encodedCode)
        {
            var codeTable = FileHelper.ReadFileToText("EncodeTable.cert");
            var baseEncrypt = new Base64Crypt(codeTable);
            var text = baseEncrypt.Decode(encodedCode);
            return await Task.FromResult(text);
        }

    }
}
