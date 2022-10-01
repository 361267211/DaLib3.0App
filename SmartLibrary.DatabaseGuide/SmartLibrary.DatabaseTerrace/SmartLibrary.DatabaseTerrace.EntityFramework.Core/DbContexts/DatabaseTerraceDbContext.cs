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
using SmartLibrary.Core.Entitys;
using SmartLibrary.DatabaseTerrace.Common.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.DbContexts
{

    /// <summary>
    /// 示例数据库上下文
    /// </summary>
    public class DatabaseTerraceDbContext : AppDbContext<DatabaseTerraceDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => tenantInfo;
        private readonly TenantInfo tenantInfo;
        public DatabaseTerraceDbContext(DbContextOptions<DatabaseTerraceDbContext> options, TenantInfo tenantInfo) : base(options)
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
            var schemaName = this.tenantInfo.Name;


            //修改DatabaseColumn表
            var databaseColumnBuilder = modelBuilder.Entity<DatabaseColumn>().ToTable(nameof(DatabaseColumn), schemaName);
            databaseColumnBuilder.HasKey(p => p.Id).IsClustered(false);
            databaseColumnBuilder.HasIndex(p => p.CreatedTime).IsClustered(true);


            modelBuilder.Entity<DatabaseColumnRule>().ToTable(nameof(DatabaseColumnRule), schemaName);
            modelBuilder.Entity<SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DatabaseTerrace>().ToTable(nameof(SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys.DatabaseTerrace), schemaName);
            modelBuilder.Entity<DatabaseTerraceSettings>().ToTable(nameof(DatabaseTerraceSettings), schemaName);
            modelBuilder.Entity<DatabaseSubscriber>().ToTable(nameof(DatabaseSubscriber), schemaName);
            modelBuilder.Entity<DomainEscsAttr>().ToTable(nameof(DomainEscsAttr), schemaName);
            modelBuilder.Entity<DatabaseAcessUrl>().ToTable(nameof(DatabaseAcessUrl), schemaName);
            modelBuilder.Entity<CustomLabel>().ToTable(nameof(CustomLabel), schemaName);
            modelBuilder.Entity<SysRole>().ToTable(nameof(SysRole), schemaName);
            modelBuilder.Entity<SysRoleMenu>().ToTable(nameof(SysRoleMenu), schemaName);
            modelBuilder.Entity<SysMenuCategory>().ToTable(nameof(SysMenuCategory), schemaName);
            modelBuilder.Entity<SysMenuPermission>().ToTable(nameof(SysMenuPermission), schemaName);
            modelBuilder.Entity<SysUserRole>().ToTable(nameof(SysUserRole), schemaName);
            modelBuilder.Entity<DatabaseUrlName>().ToTable(nameof(DatabaseUrlName), schemaName);
            modelBuilder.Entity<ProviderResource>().ToTable(nameof(ProviderResource), schemaName);
            modelBuilder.Entity<DatabaseDefaultTemplate>().ToTable(nameof(DatabaseDefaultTemplate), schemaName);

             

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
            return this.tenantInfo != null && !string.IsNullOrWhiteSpace(this.tenantInfo.Name) ? this.tenantInfo.Name : "dbo";
        }
        public string GetSchemaName()
        {
            return this.tenantInfo.Name;
        }
    }
}
