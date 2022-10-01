using Furion.DatabaseAccessor;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Core.Entitys;
using SmartLibrary.News.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 名    称：NewsPlatesDbContext
    /// 作    者：张泽军
    /// 创建时间：2021/9/6 10:23:51
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsDbContext : AppDbContext<NewsDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => tenantInfo;
        private readonly TenantInfo tenantInfo;
        public NewsDbContext(DbContextOptions<NewsDbContext> options, TenantInfo tenantInfo) : base(options)
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
            modelBuilder.Entity<NewsColumn>().ToTable(nameof(NewsColumn), schemaName);
            modelBuilder.Entity<NewsColumnPermissions>().ToTable(nameof(NewsColumnPermissions), schemaName);
            modelBuilder.Entity<NewsContent>().ToTable(nameof(NewsContent), schemaName);
            modelBuilder.Entity<NewsSettings>().ToTable(nameof(NewsSettings), schemaName);
            modelBuilder.Entity<LableInfo>().ToTable(nameof(LableInfo), schemaName);
            modelBuilder.Entity<NewsContentExpend>().ToTable(nameof(NewsContentExpend), schemaName);
            modelBuilder.Entity<NewsTemplate>().ToTable(nameof(NewsTemplate), schemaName);
            modelBuilder.Entity<NewsBodyTemplate>().ToTable(nameof(NewsBodyTemplate), schemaName);
            modelBuilder.Entity<NewsContentBack>().ToTable(nameof(NewsContentBack), schemaName);
            modelBuilder.Entity<NewsColumnPermissionsBack>().ToTable(nameof(NewsColumnPermissionsBack), schemaName);
            modelBuilder.Entity<NewsColumnBack>().ToTable(nameof(NewsColumnBack), schemaName);

            //应用权限
            modelBuilder.Entity<SysMenuCategory>().ToTable(nameof(SysMenuCategory), schemaName);
            modelBuilder.Entity<SysMenuPermission>().ToTable(nameof(SysMenuPermission), schemaName);
            modelBuilder.Entity<SysRole>().ToTable(nameof(SysRole), schemaName);
            modelBuilder.Entity<SysRoleMenu>().ToTable(nameof(SysRoleMenu), schemaName);
            modelBuilder.Entity<SysUserRole>().ToTable(nameof(SysUserRole), schemaName);

            ////修改DatabaseColumn表的约束索引
            //var databaseColumnBulilder = modelBuilder.Entity<DatabaseColumn>().ToTable(nameof(DatabaseColumn), schemaName);
            //databaseColumnBulilder.HasKey(p => p.Id).IsClustered(false);
            //databaseColumnBulilder.HasIndex(p => p.CreateTime).IsClustered(true);

            if (string.IsNullOrWhiteSpace(schemaName))
            {
                modelBuilder.HasDefaultSchema("dbo");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseBatchEF_Npgsql();
            //optionsBuilder.UseBatchEF_MSSQL();
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// 创建model时获取其schema
        /// </summary>
        /// <returns></returns>
        private string GetFullSchemaName()
        {
            return this.tenantInfo != null && !string.IsNullOrWhiteSpace(this.tenantInfo.Name) ? this.tenantInfo.Name :"dbo";
        }
        public string GetSchemaName()
        {
            return this.tenantInfo.Name;
        }
    }
}
