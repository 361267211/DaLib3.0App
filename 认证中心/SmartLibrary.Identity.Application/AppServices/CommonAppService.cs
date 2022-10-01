
using Furion.DatabaseAccessor;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartLibrary.Identity.Application.AppServices;
using SmartLibrary.Identity.Application.Dtos.Common;
using SmartLibrary.Identity.Application.Filter;
using SmartLibrary.Identity.Application.Services.Enum;
using SmartLibrary.Identity.Common.Dtos;
using SmartLibrary.Identity.EntityFramework.Core.DbContexts;
using SmartLibrary.Identity.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application
{
    /// <summary>
    /// 公共接口
    /// </summary>
    [Authorize(Policy = PolicyKey.TokenAuth)]
    public class CommonAppService : BaseAppService
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IRepository<LoginConfigSet> _loginConfigSetRepository;
        private readonly IRepository<RegisterConfigSet> _registerConfigSetRepository;
        private readonly IRepository<UserRegisterProperty> _userRegisterPropertyRepository;
        private readonly IRepository<SysMenuPermission> _sysMenuPermissionRepository;

        public CommonAppService(IdentityDbContext dbContext
            , IRepository<LoginConfigSet> loginConfigSetRepository
            , IRepository<RegisterConfigSet> registerConfigSetRepository
            , IRepository<UserRegisterProperty> userRegisterPropertyRepository
            , IRepository<SysMenuPermission> sysMenuPermissionRepository)
        {
            _dbContext = dbContext;
            _loginConfigSetRepository = loginConfigSetRepository;
            _registerConfigSetRepository = registerConfigSetRepository;
            _userRegisterPropertyRepository = userRegisterPropertyRepository;
            _sysMenuPermissionRepository = sysMenuPermissionRepository;
        }

        /// <summary>
        /// 租户数据迁移
        /// </summary>
        /// <returns></returns>
        //[UnitOfWork]
        public async Task UpdateDatabaseSchema()
        {
            await _dbContext.Database.MigrateAsync();
            //初始化权限配置
            await InitPermissionMenu();
            //登录配置种子数据
            await InitLoginConfig();
            //注册配置
            await InitRegisterConfig();
            await InitRegisterProperty();
        }

        #region 数据初始化
        /// <summary>
        /// 登录配置
        /// </summary>
        /// <returns></returns>
        private async Task InitLoginConfig()
        {
            var tenantName = _dbContext.TenantInfo?.Name ?? "";
            var fullLoginConfig = new List<LoginConfigSet>
            {
                new LoginConfigSet{Id=new Guid("1EA690E8-FAC4-F5B5-89C3-98EE58225F94"),LoginType=(int)EnumLoginType.微信登录,Icon="icon-wx",LoginName="微信登录",Desc="",Enable=false,IsOpen=false,LoginConfig="",Sort=1,TenantId=tenantName},
                new LoginConfigSet{Id=new Guid("90AC59DD-C73D-8C60-E4FB-31BD5C6DEE85"),LoginType=(int)EnumLoginType.QQ登录,Icon="icon-qq",LoginName="QQ登录",Desc="",Enable=false,IsOpen=false,LoginConfig="",Sort=2,TenantId=tenantName},
                new LoginConfigSet{Id=new Guid("25AD1595-EE32-E0BC-06AF-0461F9234785"),LoginType=(int)EnumLoginType.短信验证登录,Icon="icon-mobile",LoginName="短信验证码登录",Desc="",Enable=false,IsOpen=false,LoginConfig="",Sort=3,TenantId=tenantName},
                new LoginConfigSet{Id=new Guid("DCCEFBFD-9AC7-6A0A-2EA8-F3504B74DAB2"),LoginType=(int)EnumLoginType.身份证登录,Icon="icon-card",LoginName="身份证登录",Desc="",Enable=false,IsOpen=false,LoginConfig="",Sort=4,TenantId=tenantName},
                new LoginConfigSet{Id=new Guid("E07DF0F6-883F-809F-2F58-78380C8185A5"),LoginType=(int)EnumLoginType.学校统一认证,Icon="icon-cc",LoginName="学校统一认证",Desc="",Enable=false,IsOpen=false,LoginConfig="",Sort=5,TenantId=tenantName},
                new LoginConfigSet{Id=new Guid("D539F88E-5A6D-B1F9-6517-A7916347617A"),LoginType=(int)EnumLoginType.读者证密码登录,Icon="icon-cc",LoginName="读者证密码登录",Desc="",Enable=true,IsOpen=false,LoginConfig="",Sort=6,TenantId=tenantName}
            };
            if (!await _loginConfigSetRepository.AnyAsync())
            {
                await _loginConfigSetRepository.InsertAsync(fullLoginConfig);
            }
        }

        /// <summary>
        /// 注册配置
        /// </summary>
        /// <returns></returns>
        private async Task InitRegisterConfig()
        {
            var tenantName = _dbContext.TenantInfo?.Name ?? "";
            var fullRegisterConfig = new List<RegisterConfigSet>
            {
                new RegisterConfigSet{Id=new Guid("05FAAA99-4353-1D8C-B63D-9F0F374EC25A"),OpenRegistion=true,RegisteType=(int)EnumRegisterType.手机验证码,RegisteFlow=(int)EnumRegisterFlow.馆员审核,ProtoUrl="",TenantId=tenantName}
            };
            if (!await _registerConfigSetRepository.AnyAsync())
            {
                await _registerConfigSetRepository.InsertAsync(fullRegisterConfig);
            }
        }

        /// <summary>
        /// 注册属性配置
        /// </summary>
        /// <returns></returns>
        private async Task InitRegisterProperty()
        {
            var tenantId = _dbContext.TenantInfo?.Name ?? "";
            var initProperties = new List<UserRegisterProperty>();
            initProperties.AddRange(new[] {
                new UserRegisterProperty{Id=new Guid("0B795D98-4D37-BF28-215E-FAA20AED3B59"),PropertyCode="User_Name",IsEnable=false,IsCheck=true,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("EA42BC3F-A829-FD7F-95AA-F500DF62E878"),PropertyCode="User_NickName",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("881A919F-0A76-864D-64F1-83DFE4BD7614"),PropertyCode="User_Unit",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("FF142661-F29A-A256-60EC-B3B1C0D203CC"),PropertyCode="User_Edu",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("05CC107B-FB3A-4A68-0D36-3C8CEF209325"),PropertyCode="User_Title",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("D553D915-8B12-CAF5-C24A-4F3BEEF65891"),PropertyCode="User_Depart",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("1F9DA37A-BE81-E581-3DA2-A1FEF3942CD5"),PropertyCode="User_College",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("E5E1C624-5FB7-83B0-9D62-0FA817B6E3E5"),PropertyCode="User_Major",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("D9B1BCB2-13D9-DDEB-1F5D-E4224587E623"),PropertyCode="User_Grade",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("7E9CFD49-5341-1487-402A-993EA4B57F41"),PropertyCode="User_Class",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("33F5575E-304B-8CF7-A960-A7EC72328AFA"),PropertyCode="User_IdCard",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("A6017329-EFA3-AC7E-3C3D-29B269460292"),PropertyCode="User_Phone",IsEnable=false,IsCheck=true,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("FF3C16F0-3C75-7545-857D-8D87D7074F8A"),PropertyCode="User_Email",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("A5A44273-ED2F-B98D-4A70-47324646F52C"),PropertyCode="User_Birthday",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("27A09405-3B30-9958-01C7-A959E75D7FD3"),PropertyCode="User_Gender",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("20A55888-319F-23A9-B1D0-4181697C6F31"),PropertyCode="User_Addr",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("75865D85-55CD-A759-4388-E8AE205B1E4A"),PropertyCode="User_AddDetail",IsEnable=true,IsCheck=false,TenantId=tenantId},
                new UserRegisterProperty{Id=new Guid("3B78985A-51A7-A7AF-BA32-2799AA4A988C"),PropertyCode="User_Photo",IsEnable=true,IsCheck=false,TenantId=tenantId},
            });
            if (!await _userRegisterPropertyRepository.AnyAsync())
            {
                await _userRegisterPropertyRepository.InsertAsync(initProperties);
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
               new SysMenuPermission{Id=new Guid("E15C76F6-52C4-BCE5-355D-7CA4A0EA906E"), Name="顶级节点",Remark="虚拟节点",Pid="",Type=(int)EnumPermissionType.Dir,Path="0",FullPath=".0." },
               //--------登录配置
               new SysMenuPermission{Id=new Guid("2D2AC6DD-1548-FC5F-73B1-8A8B89787F4D"), Name="登录设置",Pid=".0.",Type=(int)EnumPermissionType.Menu,Router="/loginConfig",Component="",IsSysMenu=false,Path="1",FullPath=".0.1."},
               //------详情
               new SysMenuPermission{Id=new Guid("09B25EB7-662D-A57A-E7DA-4C5306FBC0CF"), Name="配置查看",Remark="",Pid=".0.1.",Type=(int)EnumPermissionType.Query,Permission="loginConfig:detail",Path="1",FullPath=".0.1.1."},
               new SysMenuPermission{Id=new Guid("6C583DD3-61ED-490A-801F-1CDFA71ADDBE"), Name="登录配置详情Api",Remark="",Pid=".0.1.1.",Type=(int)EnumPermissionType.Api,Permission="get-api:login-config:detail-info",Path="1",FullPath=".0.1.1.1."},
               new SysMenuPermission{Id=new Guid("ECC2C208-930F-295F-D1D7-79C6CE4F8312"), Name="配置修改",Remark="",Pid=".0.2.",Type=(int)EnumPermissionType.Query,Permission="loginConfig:edit",Path="2",FullPath=".0.1.2."},
               new SysMenuPermission{Id=new Guid("02DB9D74-8C32-1B79-8033-AA399FB559F4"), Name="登录配置详情Api",Remark="",Pid=".0.1.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:login-config:detail-info",Path="1",FullPath=".0.1.2.1."},
               new SysMenuPermission{Id=new Guid("5A911AC5-3F0F-A82A-6FE8-C12F5D9664E8"), Name="设置是否开启Api",Remark="",Pid=".0.1.2.",Type=(int)EnumPermissionType.Api,Permission="post-api:login-config:switch",Path="2",FullPath=".0.1.2.2."},
               new SysMenuPermission{Id=new Guid("950BB7D8-58B9-66E2-4078-E84AE6EBC449"), Name="获取方式配置Api",Remark="",Pid=".0.1.2.",Type=(int)EnumPermissionType.Api,Permission="get-api:login-config:detail-config",Path="3",FullPath=".0.1.2.3."},
               new SysMenuPermission{Id=new Guid("943426E7-2B5B-CFE2-C383-B9236FA31E9E"), Name="保存方式配置Api",Remark="",Pid=".0.1.2.",Type=(int)EnumPermissionType.Api,Permission="put-api:login-config:edit-config",Path="4",FullPath=".0.1.2.4."},
               //--------注册配置
               new SysMenuPermission{Id=new Guid("74F66150-2EB7-B731-B4BA-2481B68FF56D"), Name="注册设置",Pid=".1.",Type=(int)EnumPermissionType.Menu,Router="/registerConfig",Component="",IsSysMenu=false,Path="2",FullPath=".0.2."},
               //------详情
               new SysMenuPermission{Id=new Guid("5586F81E-6841-1055-83DF-29EA4A780EC2"), Name="配置查看",Remark="",Pid=".0.2.",Type=(int)EnumPermissionType.Query,Permission="registerConfig:detail",Path="1",FullPath=".0.2.1."},
               new SysMenuPermission{Id=new Guid("41F56094-128E-8742-F6F9-405E722085AD"), Name="注册配置详情Api",Remark="",Pid=".0.2.1",Type=(int)EnumPermissionType.Api,Permission="get-api:register-config:detail-info",Path="1",FullPath=".0.2.1.1"},
               new SysMenuPermission{Id=new Guid("6F3FF715-B4D4-B7A0-0B9D-561873CE44B7"), Name="配置修改",Remark="",Pid=".0.2.",Type=(int)EnumPermissionType.Query,Permission="registerConfig:edit",Path="2",FullPath=".0.2.2."},
               new SysMenuPermission{Id=new Guid("6490DB05-6AE4-A9F9-1257-0C1F7CD5165D"), Name="注册配置详情Api",Remark="",Pid=".0.2.2",Type=(int)EnumPermissionType.Api,Permission="get-api:register-config:detail-info",Path="1",FullPath=".0.2.2.1"},
               new SysMenuPermission{Id=new Guid("24AD89AE-5A4F-4C54-562D-7A682AF58EDD"), Name="设置注册配置Api",Remark="",Pid=".0.2.2",Type=(int)EnumPermissionType.Api,Permission="put-api:register-config:edit-config",Path="2",FullPath=".0.2.2.2"},
            };
            //需要新增的菜单
            if (!await _sysMenuPermissionRepository.AnyAsync())
            {
                //系统菜单及权限
                await _dbContext.BulkInsertAsync(fullMenu);
            }

        }

        #endregion


        /// <summary>
        /// 测试登录消息投递
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [CapActionEvent("ReaderLogin", "用户登录")]
        public async Task TestLoginMessage()
        {
            await Task.CompletedTask;
        }


    }
}
