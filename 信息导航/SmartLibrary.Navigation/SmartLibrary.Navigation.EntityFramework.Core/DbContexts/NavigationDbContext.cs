using Furion.DatabaseAccessor;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Core.Entitys;
using SmartLibrary.Navigation.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 名    称：NavigationDbContext
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 16:44:18
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationDbContext: AppDbContext<NavigationDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => tenantInfo;
        private readonly TenantInfo tenantInfo;
        public NavigationDbContext(DbContextOptions<NavigationDbContext> options, TenantInfo tenantInfo) : base(options)
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
                modelBuilder.HasDefaultSchema("dbo");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseBatchEF_Npgsql();
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// 创建model时获取其schema
        /// </summary>
        /// <returns></returns>
        private string GetFullSchemaName()
        {
            return this.tenantInfo != null && !string.IsNullOrWhiteSpace(this.tenantInfo.Name) ? $"dbo.{this.tenantInfo.Name}" : "dbo";
        }
        public string GetSchemaName()
        {
            return this.tenantInfo.Name;
        }
    }
}
