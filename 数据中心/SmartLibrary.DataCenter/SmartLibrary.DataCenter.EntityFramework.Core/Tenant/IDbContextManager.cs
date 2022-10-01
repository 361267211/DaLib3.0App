/*********************************************************
 * ��    �ƣ�IDbContextManager
 * ��    �ߣ�������
 * ��ϵ��ʽ���绰[13883914813],�ʼ�[361267211@qq.com]
 * ����ʱ�䣺2021/8/05 16:57:45
 * ��    ��������ӿڣ����ݿ������Ĺ�������
 *
 * ������ʷ��
 *
 * *******************************************************/

using Microsoft.EntityFrameworkCore;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public interface IDbContextManager
    {
        DatabaseIntegration DatabaseType { get; }
        DbContextOptionsBuilder GenerateOptionBuilder(DbContextOptionsBuilder builder, string connectionString);
    }

    public class MySqlDbContextManager : IDbContextManager
    {
        private readonly ConnectionResolverOption option;
        private readonly TenantInfo tenant;
        public MySqlDbContextManager(ConnectionResolverOption option, TenantInfo tenant)
        {
            this.tenant = tenant;
            this.option = option;
        }

        public DatabaseIntegration DatabaseType => DatabaseIntegration.Mysql;

        public DbContextOptionsBuilder GenerateOptionBuilder(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseMySql(connectionString, optionBuilder =>
            {
                if (this.option.Type == ConnectionResolverType.ByTabel)
                {
                    optionBuilder.MigrationsHistoryTable($"{this.tenant.Name}__EFMigrationHistory", $"dbo.{this.tenant.Name}");
                }
            });
        }
    }

    public class SqlServerDbContextManager : IDbContextManager
    {
        private readonly ConnectionResolverOption option;
        private readonly TenantInfo tenant;
        public SqlServerDbContextManager(ConnectionResolverOption option, TenantInfo tenant)
        {
            this.option = option;
            this.tenant = tenant;
        }

        public DatabaseIntegration DatabaseType => DatabaseIntegration.SqlServer;
        public DbContextOptionsBuilder GenerateOptionBuilder(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseSqlServer(connectionString, optionBuilder =>
            {
                if (this.option.Type == ConnectionResolverType.ByTabel)
                {
                    optionBuilder.MigrationsHistoryTable($"{this.tenant.Name}__EFMigrationHistory", $"dbo.{this.tenant.Name}");
                }
                if (this.option.Type == ConnectionResolverType.BySchema)
                {
                    optionBuilder.MigrationsHistoryTable("__EFMigrationHistory", $"dbo.{this.tenant.Name}");
                }
            });
        }
    }

    public enum DatabaseIntegration
    {
        None = 0,
        Mysql = 1,
        SqlServer = 2,
        PGSql = 3,
    }
}
