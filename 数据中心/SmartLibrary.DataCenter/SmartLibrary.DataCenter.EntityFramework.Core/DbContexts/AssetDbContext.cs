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


using Furion;
using Furion.DatabaseAccessor;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Core.Entitys;
using SmartLibrary.DataCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 示例数据库上下文
    /// </summary>
    public class AssetDbContext : AppDbContext<AssetDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => tenantInfo;
        private readonly TenantInfo tenantInfo;



        public AssetDbContext(DbContextOptions<AssetDbContext> options, TenantInfo tenantInfo) : base(options)
        {
            this.tenantInfo = tenantInfo;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseBatchEF_Npgsql();
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// 创建模型前
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schemaName = this.tenantInfo.Name ?? "dbo";


            modelBuilder.Entity<Asset>().ToTable(nameof(Asset), schemaName);
            modelBuilder.Entity<Person>().ToTable(nameof(Person), schemaName);
            modelBuilder.Entity<SysMenuCategory>().ToTable(nameof(SysMenuCategory), schemaName);
            modelBuilder.Entity<SysMenuPermission>().ToTable(nameof(SysMenuPermission), schemaName);
            modelBuilder.Entity<SysRole>().ToTable(nameof(SysRole), schemaName);
            modelBuilder.Entity<SysRoleMenu>().ToTable(nameof(SysRoleMenu), schemaName);
            modelBuilder.Entity<SysUserRole>().ToTable(nameof(SysUserRole), schemaName);

            modelBuilder.Entity<DatabaseCollectKind>().ToTable(nameof(DatabaseCollectKind), schemaName);
            modelBuilder.Entity<DatabaseProvider>().ToTable(nameof(DatabaseProvider), schemaName);
            modelBuilder.Entity<ProviderResource>().ToTable(nameof(ProviderResource), schemaName);
            modelBuilder.Entity<DomainInfo>().ToTable(nameof(DomainInfo), schemaName);
            modelBuilder.Entity<SourceType>().ToTable(nameof(SourceType), schemaName);
            modelBuilder.Entity<ResourceAlbum>().ToTable(nameof(ResourceAlbum), schemaName);



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
            return this.tenantInfo != null && !string.IsNullOrWhiteSpace(this.tenantInfo.Name) ? $"dbo.{this.tenantInfo.Name}" : "dbo";
        }
        public string GetSchemaName()
        {
            return this.tenantInfo.Name;
        }
    }
}
