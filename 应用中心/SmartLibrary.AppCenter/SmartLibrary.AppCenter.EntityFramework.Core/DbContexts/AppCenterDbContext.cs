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
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys.Permission;

namespace SmartLibrary.AppCenter.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 示例数据库上下文
    /// </summary>
    public class AppCenterDbContext : AppDbContext<AppCenterDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => _TenantInfo;

        private readonly TenantInfo _TenantInfo;

        public static readonly LoggerFactory _LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

        public AppCenterDbContext(DbContextOptions<AppCenterDbContext> options, TenantInfo tenantInfo) : base(options)
        {
            this._TenantInfo = tenantInfo;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(_LoggerFactory);
        }

        /// <summary>
        /// 创建模型前
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schemaName = GetFullSchemaName();

            modelBuilder.Entity<AppCenterSettings>().ToTable(nameof(AppCenterSettings), schemaName);
            modelBuilder.Entity<AppCollection>().ToTable(nameof(AppCollection), schemaName);
            modelBuilder.Entity<AppManager>().ToTable(nameof(AppManager), schemaName);
            modelBuilder.Entity<AppNavigation>().ToTable(nameof(AppNavigation), schemaName);
            modelBuilder.Entity<AppUser>().ToTable(nameof(AppUser), schemaName);
            modelBuilder.Entity<ManagerRole>().ToTable(nameof(ManagerRole), schemaName);
            modelBuilder.Entity<Navigation>().ToTable(nameof(Navigation), schemaName);
            modelBuilder.Entity<ThirdApplication>().ToTable(nameof(ThirdApplication), schemaName);
            modelBuilder.Entity<UserRole>().ToTable(nameof(UserRole), schemaName);
            modelBuilder.Entity<AppColumnInfo>().ToTable(nameof(AppColumnInfo), schemaName);
            modelBuilder.Entity<AppReName>().ToTable(nameof(AppReName), schemaName);

            modelBuilder.Entity<SysMenuCategory>().ToTable(nameof(SysMenuCategory), schemaName);
            modelBuilder.Entity<SysMenuPermission>().ToTable(nameof(SysMenuPermission), schemaName);
            modelBuilder.Entity<SysRole>().ToTable(nameof(SysRole), schemaName);
            modelBuilder.Entity<SysRoleMenu>().ToTable(nameof(SysRoleMenu), schemaName);
            modelBuilder.Entity<SysUserRole>().ToTable(nameof(SysUserRole), schemaName);

            if (string.IsNullOrWhiteSpace(schemaName))
            {
                modelBuilder.HasDefaultSchema("public");
            }
        }

        /// <summary>
        /// 创建model时获取其schema
        /// </summary>
        /// <returns></returns>
        private string GetFullSchemaName()
        {
            return this._TenantInfo != null && !string.IsNullOrWhiteSpace(_TenantInfo.Name) ? $"{_TenantInfo.Name}" : "public";
        }

    }
}
