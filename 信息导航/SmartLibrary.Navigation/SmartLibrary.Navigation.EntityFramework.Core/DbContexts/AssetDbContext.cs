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
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;

namespace SmartLibrary.Navigation.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 示例数据库上下文
    /// </summary>
    public class AssetDbContext : AppDbContext<AssetDbContext>, ITenantDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public TenantInfo TenantInfo => tenantInfo;

        private readonly TenantInfo tenantInfo;

        /// <summary>
        /// 
        /// </summary>
        public static readonly LoggerFactory _LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="tenantInfo"></param>
        public AssetDbContext(DbContextOptions<AssetDbContext> options, TenantInfo tenantInfo) : base(options)
        {
            this.tenantInfo = tenantInfo;
        }

        /// <summary>
        /// 创建模型前
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schemaName = GetFullSchemaName();

            modelBuilder.Entity<Content>().ToTable(nameof(Content), schemaName);
            modelBuilder.Entity<NavigationBodyTemplate>().ToTable(nameof(NavigationBodyTemplate), schemaName);
            modelBuilder.Entity<NavigationCatalogue>().ToTable(nameof(NavigationCatalogue), schemaName);
            modelBuilder.Entity<NavigationColumn>().ToTable(nameof(NavigationColumn), schemaName);
            modelBuilder.Entity<NavigationColumnPermissions>().ToTable(nameof(NavigationColumnPermissions), schemaName);
            modelBuilder.Entity<NavigationLableInfo>().ToTable(nameof(NavigationLableInfo), schemaName);
            modelBuilder.Entity<NavigationSettings>().ToTable(nameof(NavigationSettings), schemaName);
            modelBuilder.Entity<NavigationTemplate>().ToTable(nameof(NavigationTemplate), schemaName);

            //应用权限
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
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseBatchEF_Npgsql();
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(_LoggerFactory);
        }

        /// <summary>
        /// 创建model时获取其schema
        /// </summary>
        /// <returns></returns>
        private string GetFullSchemaName()
        {
            return this.tenantInfo != null && !string.IsNullOrWhiteSpace(this.tenantInfo.Name) ? $"{this.tenantInfo.Name}" : "public";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSchemaName()
        {
            return this.tenantInfo.Name;
        }
    }
}
