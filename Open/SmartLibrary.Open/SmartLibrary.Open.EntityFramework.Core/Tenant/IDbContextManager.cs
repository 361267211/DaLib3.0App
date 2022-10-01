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
                    optionBuilder.MigrationsHistoryTable($"{this.tenant.Name}__EFMigrationHistory");
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
                    optionBuilder.MigrationsHistoryTable($"{this.tenant.Name}__EFMigrationHistory");
                }
                if (this.option.Type == ConnectionResolverType.BySchema)
                {
                    optionBuilder.MigrationsHistoryTable("__EFMigrationHistory", $"dbo.{this.tenant.Name}");
                }
            });
        }
    }


    public class PostgreSqlDbContextManager : IDbContextManager
    {
        private readonly ConnectionResolverOption option;
        private readonly TenantInfo tenant;
        public PostgreSqlDbContextManager(ConnectionResolverOption option, TenantInfo tenant)
        {
            this.option = option;
            this.tenant = tenant;
        }

        public DatabaseIntegration DatabaseType => DatabaseIntegration.PostgreSql;
        public DbContextOptionsBuilder GenerateOptionBuilder(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseNpgsql(connectionString, optionBuilder =>
            {
                switch (option.Type)
                {
                    case ConnectionResolverType.ByTabel:
                        optionBuilder.MigrationsHistoryTable($"{this.tenant.Name}__EFMigrationHistory");
                        break;
                    case ConnectionResolverType.BySchema:
                        optionBuilder.MigrationsHistoryTable("__EFMigrationHistory", $"dbo.{this.tenant.Name}");
                        break;
                    default:
                        optionBuilder.MigrationsHistoryTable("__EFMigrationHistory");
                        break;

                }
            });
        }
    }

    public enum DatabaseIntegration
    {
        None = 0,
        Mysql = 1,
        SqlServer = 2,
        PostgreSql = 3
    }
}