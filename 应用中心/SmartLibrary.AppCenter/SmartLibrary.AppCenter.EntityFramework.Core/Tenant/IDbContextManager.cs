/*********************************************************
 * 名    称：IDbContextManager
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：抽象接口，数据库上下文管理器。
 *
 * 更新历史：
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
                if (this.option.Type == ConnectionResolverType.ByTable)
                {
                    optionBuilder.MigrationsHistoryTable($"{this.tenant.Name}__EFMigrationHistory");
                }
            });
        }
    }

    public class SqlServerDbContextManager : IDbContextManager
    {
        private readonly ConnectionResolverOption _Option;
        private readonly TenantInfo _Tenant;
        public SqlServerDbContextManager(ConnectionResolverOption option, TenantInfo tenant)
        {
            this._Option = option;
            this._Tenant = tenant;
        }

        public DatabaseIntegration DatabaseType => DatabaseIntegration.SqlServer;
        public DbContextOptionsBuilder GenerateOptionBuilder(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseSqlServer(connectionString, optionBuilder =>
            {
                if (this._Option.Type == ConnectionResolverType.ByTable)
                {
                    optionBuilder.MigrationsHistoryTable($"{_Tenant.Name}__EFMigrationHistory");
                }
                if (this._Option.Type == ConnectionResolverType.BySchema)
                {
                    optionBuilder.MigrationsHistoryTable("__EFMigrationHistory", _Tenant.Name ?? "public");
                }
            });
        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseIntegration
    {
        None = 0,
        Mysql = 1,
        SqlServer = 2,
        PostgreSql = 3,
    }
}
