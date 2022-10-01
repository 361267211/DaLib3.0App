/*********************************************************
 * 名    称：SceneManageDbContext
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/9/28 15:47:05
 * 描    述：场景
 *
 * 更新历史：
 *
 * *******************************************************/


using Furion.DatabaseAccessor;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.SceneManage.EntityFramework.Core.Entitys;

namespace SmartLibrary.SceneManage.EntityFramework.Core.DbContexts
{
    /// <summary>
    /// 示例数据库上下文
    /// </summary>
    public class SceneManageDbContext : AppDbContext<SceneManageDbContext>, ITenantDbContext
    {
        public TenantInfo TenantInfo => tenantInfo;
        private readonly TenantInfo tenantInfo;
        public SceneManageDbContext(DbContextOptions<SceneManageDbContext> options, TenantInfo tenantInfo) : base(options)
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
            modelBuilder.Entity<Layout>().ToTable(nameof(Layout), schemaName);
            modelBuilder.Entity<Scene>().ToTable(nameof(Scene), schemaName);
            modelBuilder.Entity<Template>().ToTable(nameof(Template), schemaName);
            modelBuilder.Entity<SceneApp>().ToTable(nameof(SceneApp), schemaName);
            modelBuilder.Entity<SceneUser>().ToTable(nameof(SceneUser), schemaName);
            modelBuilder.Entity<TerminalInstance >().ToTable(nameof(TerminalInstance), schemaName);
            modelBuilder.Entity<ThemeColor>().ToTable(nameof(ThemeColor), schemaName);
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
