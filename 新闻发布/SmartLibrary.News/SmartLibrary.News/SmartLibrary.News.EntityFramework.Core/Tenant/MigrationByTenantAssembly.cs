/*********************************************************
 * 名    称：AssetDbContext
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义的MigrationsAssembly，用于将 Migrations 模板 构造函数改写成带参数的版本，实现接口控制。
 *
 * 更新历史：
 *
 * *******************************************************/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System;
using System.Reflection;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    /// <summary>
    /// 自定义的MigrationsAssembly：用于更改模板的格式（引入 带"schema" 的构造函数）
    /// </summary>
    public class MigrationByTenantAssembly : MigrationsAssembly
    {
        private readonly DbContext context;

        public MigrationByTenantAssembly(ICurrentDbContext currentContext,
              IDbContextOptions options, IMigrationsIdGenerator idGenerator,
              IDiagnosticsLogger<DbLoggerCategory.Migrations> logger)
          : base(currentContext, options, idGenerator, logger)
        {
            context = currentContext.Context;
        }

        /// <summary>
        /// 调用模板方法构造函数，获取 改造后的 Migration 的子类
        /// </summary>
        /// <param name="migrationClass"></param>
        /// <param name="activeProvider"></param>
        /// <returns></returns>
        public override Migration CreateMigration(TypeInfo migrationClass,
              string activeProvider)
        {
            if (activeProvider == null)
                throw new ArgumentNullException($"{nameof(activeProvider)} argument is null");

            var hasCtorWithSchema = migrationClass
                    .GetConstructor(new[] { typeof(string) }) != null;

            if (hasCtorWithSchema && context is ITenantDbContext tenantDbContext)
            {
                var instance = (Migration)Activator.CreateInstance(migrationClass.AsType(), tenantDbContext?.TenantInfo?.Name);
                instance.ActiveProvider = activeProvider;
                return instance;
            }

            return base.CreateMigration(migrationClass, activeProvider);
        }
    }
}
