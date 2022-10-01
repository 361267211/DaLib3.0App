/*********************************************************
 * ��    �ƣ�AssetDbContext
 * ��    �ߣ�������
 * ��ϵ��ʽ���绰[13883914813],�ʼ�[361267211@qq.com]
 * ����ʱ�䣺2021/8/05 16:57:45
 * ��    �����Զ����MigrationsAssembly�����ڽ� Migrations ģ�� ���캯����д�ɴ������İ汾��ʵ�ֽӿڿ��ơ�
 *
 * ������ʷ��
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
    /// �Զ����MigrationsAssembly�����ڸ���ģ��ĸ�ʽ������ ��"schema" �Ĺ��캯����
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
        /// ����ģ�巽�����캯������ȡ ������ Migration ������
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
