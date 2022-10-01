/*********************************************************
 * 名    称：AssetDbContext
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：数据库上下文。
 *
 * 更新历史：
 *
 * *******************************************************/


using Furion.DatabaseAccessor;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging;
using SmartLibrary.User.EntityFramework.Core.Entitys;

namespace SmartLibrary.User.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 示例数据库上下文
    /// </summary>
    public class UserDbContext : AppDbContext<UserDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => _TenantInfo;
        private readonly TenantInfo _TenantInfo;
        public static readonly LoggerFactory _LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });


        public UserDbContext(DbContextOptions<UserDbContext> options, TenantInfo tenantInfo) : base(options)
        {
            this._TenantInfo = tenantInfo;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseBatchEF_Npgsql();
            optionsBuilder.UseLoggerFactory(_LoggerFactory);
        }

        /// <summary>
        /// 创建模型前
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schemaName = GetFullSchemaName();
            //属性组选项配置
            var propertyGroupItemBuilder = modelBuilder.Entity<PropertyGroupItem>().ToTable(nameof(PropertyGroupItem), schemaName);
            propertyGroupItemBuilder.HasKey(x => x.Id).IsClustered(false);
            propertyGroupItemBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //属性组配置
            var propertyGroupBuilder = modelBuilder.Entity<PropertyGroup>().ToTable(nameof(PropertyGroup), schemaName);
            propertyGroupBuilder.HasKey(x => x.Id).IsClustered(false);
            propertyGroupBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //属性配置
            var propertyBuilder = modelBuilder.Entity<Property>().ToTable(nameof(Property), schemaName);
            propertyBuilder.HasKey(x => x.Id).IsClustered(false);
            propertyBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户属性变更日志
            var userProeprtyChangeLogBuilder = modelBuilder.Entity<UserPropertyChangeLog>().ToTable(nameof(UserPropertyChangeLog), schemaName);
            userProeprtyChangeLogBuilder.HasKey(x => x.Id).IsClustered(false);
            userProeprtyChangeLogBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户属性变更明细
            var userPropertyChangeItemBuilder = modelBuilder.Entity<UserPropertyChangeItem>().ToTable(nameof(UserPropertyChangeItem), schemaName);
            userPropertyChangeItemBuilder.HasKey(x => x.Id).IsClustered(false);
            userPropertyChangeItemBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //属性变更日志
            var proeprtyChangeLogBuilder = modelBuilder.Entity<PropertyChangeLog>().ToTable(nameof(PropertyChangeLog), schemaName);
            proeprtyChangeLogBuilder.HasKey(x => x.Id).IsClustered(false);
            proeprtyChangeLogBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //属性变更明细
            var propertyChangeItemBuilder = modelBuilder.Entity<PropertyChangeItem>().ToTable(nameof(PropertyChangeItem), schemaName);
            propertyChangeItemBuilder.HasKey(x => x.Id).IsClustered(false);
            propertyChangeItemBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户
            var userBuilder = modelBuilder.Entity<Entitys.User>().ToTable(nameof(Entitys.User), schemaName);
            userBuilder.HasKey(x => x.Id).IsClustered(false);
            userBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //用户扩展属性
            var userPropertyBuilder = modelBuilder.Entity<UserProperty>().ToTable(nameof(UserProperty), schemaName);
            userPropertyBuilder.HasKey(x => x.Id).IsClustered(false);
            userPropertyBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //卡
            var cardBuilder = modelBuilder.Entity<Entitys.Card>().ToTable(nameof(Entitys.Card), schemaName);
            cardBuilder.HasKey(x => x.Id).IsClustered(false);
            cardBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //卡扩展属性
            var cardPropertyBuilder = modelBuilder.Entity<CardProperty>().ToTable(nameof(CardProperty), schemaName);
            cardPropertyBuilder.HasKey(x => x.Id).IsClustered(false);
            cardPropertyBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //三方授权
            var userThirdAuthBuilder = modelBuilder.Entity<UserThirdAuth>().ToTable(nameof(UserThirdAuth), schemaName);
            userThirdAuthBuilder.HasKey(x => x.Id).IsClustered(false);
            userThirdAuthBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //基础设置
            var basicConfigBuilder = modelBuilder.Entity<BasicConfigSet>().ToTable(nameof(BasicConfigSet), schemaName);
            basicConfigBuilder.HasKey(x => x.Id).IsClustered(false);
            basicConfigBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //分组管理
            var groupBuilder = modelBuilder.Entity<Group>().ToTable(nameof(Group), schemaName);
            groupBuilder.HasKey(x => x.Id).IsClustered(false);
            groupBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //分组属性规则
            var propertyGroupRuleBuilder = modelBuilder.Entity<PropertyGroupRule>().ToTable(nameof(PropertyGroupRule), schemaName);
            propertyGroupRuleBuilder.HasKey(x => x.Id).IsClustered(false);
            propertyGroupRuleBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //用户组
            var userGroupBuilder = modelBuilder.Entity<UserGroup>().ToTable(nameof(UserGroup), schemaName);
            userGroupBuilder.HasKey(x => x.Id).IsClustered(false);
            userGroupBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //系统菜单管理
            var sysMenuPermissionBuilder = modelBuilder.Entity<SysMenuPermission>().ToTable(nameof(SysMenuPermission), schemaName);
            sysMenuPermissionBuilder.HasKey(x => x.Id).IsClustered(false);
            sysMenuPermissionBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //系统角色
            var sysRoleBuilder = modelBuilder.Entity<SysRole>().ToTable(nameof(SysRole), schemaName);
            sysRoleBuilder.HasKey(x => x.Id).IsClustered(false);
            sysRoleBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //角色菜单
            var sysRoleMenuBuilder = modelBuilder.Entity<SysRoleMenu>().ToTable(nameof(SysRoleMenu), schemaName);
            sysRoleMenuBuilder.HasKey(x => x.Id).IsClustered(false);
            sysRoleMenuBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            //用户角色关系
            var sysUserRoleBuilder = modelBuilder.Entity<SysUserRole>().ToTable(nameof(SysUserRole), schemaName);
            sysUserRoleBuilder.HasKey(x => x.Id).IsClustered(false);
            sysUserRoleBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            ///读者可编辑属性配置
            var readerEditPropertyBuilder = modelBuilder.Entity<ReaderEditProperty>().ToTable(nameof(ReaderEditProperty), schemaName);
            readerEditPropertyBuilder.HasKey(x => x.Id).IsClustered(false);
            readerEditPropertyBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            ///读者权限
            var readerPermitBuilder = modelBuilder.Entity<InfoPermitReader>().ToTable(nameof(InfoPermitReader), schemaName);
            readerPermitBuilder.HasKey(x => x.Id).IsClustered(false);
            readerPermitBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            ///读者领卡
            var cardClaimBuilder = modelBuilder.Entity<UserCardClaim>().ToTable(nameof(UserCardClaim), schemaName);
            cardClaimBuilder.HasKey(x => x.Id).IsClustered(false);
            cardClaimBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            ///用户注册
            var userRegisterBuilder = modelBuilder.Entity<UserRegister>().ToTable(nameof(UserRegister), schemaName);
            userRegisterBuilder.HasKey(x => x.Id).IsClustered(false);
            userRegisterBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            ///用户导入
            var userTempBuilder = modelBuilder.Entity<UserImportTempData>().ToTable(nameof(UserImportTempData), schemaName);
            userTempBuilder.HasKey(x => x.Id).IsClustered(false);
            userTempBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            ///组织机构
            var orgBuilder = modelBuilder.Entity<SysOrg>().ToTable(nameof(SysOrg), schemaName);
            orgBuilder.HasKey(x => x.Id).IsClustered(false);
            orgBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

            //执行任务
            var scheduleBuilder = modelBuilder.Entity<SchedulerEntity>().ToTable(nameof(SchedulerEntity), "public");
            scheduleBuilder.HasKey(x => x.Id).IsClustered(true);

            //执行任务日志
            var scheduleLogBuilder = modelBuilder.Entity<SchedulerLogEntity>().ToTable(nameof(SchedulerLogEntity), "public");
            scheduleLogBuilder.HasKey(x => x.Id).IsClustered(true);

            //地区
            var regionBuilder = modelBuilder.Entity<Region>().ToTable(nameof(Region), "public");
            regionBuilder.HasKey(x => x.ID).IsClustered(true);

            if (string.IsNullOrWhiteSpace(schemaName))
            {
                modelBuilder.HasDefaultSchema("dbo");
            }
        }

        /// <summary>
        /// 创建model时获取其schema
        /// </summary>
        /// <returns></returns>
        private string GetFullSchemaName()
        {
            return this._TenantInfo != null && !string.IsNullOrWhiteSpace(this._TenantInfo.Name) ? $"{this._TenantInfo.Name}" : "dbo";
        }
        
    }
}
