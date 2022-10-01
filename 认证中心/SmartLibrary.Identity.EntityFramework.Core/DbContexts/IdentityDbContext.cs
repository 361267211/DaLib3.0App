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
using SmartLibrary.Identity.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 示例数据库上下文
    /// </summary>
    public class IdentityDbContext : AppDbContext<IdentityDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => tenantInfo;
        private readonly TenantInfo tenantInfo;



        public IdentityDbContext(DbContextOptions<IdentityDbContext> options, TenantInfo tenantInfo) : base(options)
        {
            this.tenantInfo = tenantInfo;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseBatchEF_Npgsql();
        }

        /// <summary>
        /// 创建模型前
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schemaName = GetFullSchemaName();
            modelBuilder.Entity<SysMenuPermission>().ToTable(nameof(SysMenuPermission), schemaName);
            ///登录配置
            var loginConfigBuilder = modelBuilder.Entity<LoginConfigSet>().ToTable(nameof(LoginConfigSet), schemaName);
            loginConfigBuilder.HasKey(x => x.Id).IsClustered(false);
            loginConfigBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            ///注册配置
            var registerConfigBuilder = modelBuilder.Entity<RegisterConfigSet>().ToTable(nameof(RegisterConfigSet), schemaName);
            registerConfigBuilder.HasKey(x => x.Id).IsClustered(false);
            registerConfigBuilder.HasIndex(x => x.CreateTime).IsClustered(true);
            ///用户注册属性
            var userRegisterBuilder = modelBuilder.Entity<UserRegisterProperty>().ToTable(nameof(UserRegisterProperty), schemaName);
            userRegisterBuilder.HasKey(x => x.Id).IsClustered(false);
            userRegisterBuilder.HasIndex(x => x.CreateTime).IsClustered(true);

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
            return this.tenantInfo != null && !string.IsNullOrWhiteSpace(this.tenantInfo.Name) ? $"{this.tenantInfo.Name}" : "dbo";
        }
        public string GetSchemaName()
        {
            return this.tenantInfo.Name;
        }
    }
}
