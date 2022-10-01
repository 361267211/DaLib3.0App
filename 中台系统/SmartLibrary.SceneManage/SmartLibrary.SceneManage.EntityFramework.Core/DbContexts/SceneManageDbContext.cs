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
            modelBuilder.Entity<SceneScreen>().ToTable(nameof(SceneScreen), schemaName);
            modelBuilder.Entity<TerminalInstance >().ToTable(nameof(TerminalInstance), schemaName);
            modelBuilder.Entity<SceneAppPlate>().ToTable(nameof(SceneAppPlate), schemaName);
            modelBuilder.Entity<ThemeColor>().ToTable(nameof(ThemeColor), schemaName);
            modelBuilder.Entity<SysMenuCategory>().ToTable(nameof(SysMenuCategory), schemaName);
            modelBuilder.Entity<SysMenuPermission>().ToTable(nameof(SysMenuPermission), schemaName);
            modelBuilder.Entity<SysRole>().ToTable(nameof(SysRole), schemaName);
            modelBuilder.Entity<SysRoleMenu>().ToTable(nameof(SysRoleMenu), schemaName);
            modelBuilder.Entity<SysUserRole>().ToTable(nameof(SysUserRole), schemaName);
            modelBuilder.Entity<HeadTemplateSetting>().ToTable(nameof(HeadTemplateSetting), schemaName);
            modelBuilder.Entity<FootTemplateSetting>().ToTable(nameof(FootTemplateSetting), schemaName);

             
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
            return this.tenantInfo != null && !string.IsNullOrWhiteSpace(this.tenantInfo.Name) ? $"{this.tenantInfo.Name}" : "public";
        }
        public string GetSchemaName()
        {
            return this.tenantInfo.Name;
        }
    }
}
