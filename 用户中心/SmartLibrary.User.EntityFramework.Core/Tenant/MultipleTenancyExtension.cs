/*********************************************************
 * ��    �ƣ�MultipleTenancyExtension
 * ��    �ߣ�������
 * ��ϵ��ʽ���绰[13883914813],�ʼ�[361267211@qq.com]
 * ����ʱ�䣺2021/8/05 16:57:45
 * ��    ����ע�����ݿ������ġ�
 *
 * ������ʷ��
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    /// <summary>
    /// ע��DbContext
    /// </summary>
    public static class MultipleTenancyExtension
    {
        public static IServiceCollection AddDatabasePerConnection<TDbContext>(this IServiceCollection services,
                string key = "default", DatabaseIntegration database = DatabaseIntegration.Mysql)
            where TDbContext : DbContext, ITenantDbContext
        {
            var option = new ConnectionResolverOption()
            {
                Key = key,
                Type = ConnectionResolverType.ByDatabase,
                DBType = database
            };
            return services.AddDatabasePerConnection<TDbContext>(option);
        }

        /// <summary>
        /// �������ݿ����ӵĶ��⻧
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabasePerConnection<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            if (option == null)
            {
                option = new ConnectionResolverOption()
                {
                    Key = "default",
                    Type = ConnectionResolverType.ByDatabase,
                    DBType = DatabaseIntegration.Mysql
                };
            }

            return services.AddDatabase<TDbContext>(option);
        }

        /// <summary>
        /// ע��
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        internal static IServiceCollection AddDatabase<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            services.AddSingleton(option);
            services.AddScoped<TenantInfo>();
            services.AddDatabaseAccessor(options =>
            {
                //ע����DbContext׷����
                services.RegisterDbContext<TDbContext, MasterDbContextLocator>();

                services.AddDbContext<TDbContext>((serviceProvider, options) =>
                {

                    //services.AddScoped<TenantInfo>();
                    //services.BuildServiceProvider();
                    var dbContextManager = serviceProvider.GetService<IDbContextManager>();
                    var tenant = serviceProvider.GetService<TenantInfo>();

                    DbContextOptionsBuilder dbOptionBuilder = null;
                    switch (option.DBType)
                    {
                        case DatabaseIntegration.PostgreSql:
                            dbOptionBuilder = options.UseNpgsql(option.ConnectinString,
                                optionBuilder =>
                                {
                                    if (option.Type == ConnectionResolverType.ByTabel)
                                    {
                                        optionBuilder.MigrationsHistoryTable($"{tenant.Name}__EFMigrationsHistory");
                                    }
                                    if (option.Type == ConnectionResolverType.BySchema)
                                    {
                                        optionBuilder.MigrationsHistoryTable("__EFMigrationsHistory", string.IsNullOrWhiteSpace(tenant.Name) ? "public" : tenant.Name);
                                    }
                                    optionBuilder.MigrationsAssembly(option.MigrationAssembly);
                                });
                            break;
                        case DatabaseIntegration.SqlServer:
                            //TODO:�����ַ��������û�
                            dbOptionBuilder = options.UseSqlServer(option.ConnectinString,
                                optionBuilder =>
                                {
                                    if (option.Type == ConnectionResolverType.ByTabel)
                                    {
                                        optionBuilder.MigrationsHistoryTable($"{tenant.Name}__EFMigrationsHistory");
                                    }
                                    if (option.Type == ConnectionResolverType.BySchema)
                                    {
                                        optionBuilder.MigrationsHistoryTable("__EFMigrationsHistory", string.IsNullOrWhiteSpace(tenant.Name) ? "public" : tenant.Name);
                                    }
                                    optionBuilder.MigrationsAssembly(option.MigrationAssembly);
                                });
                            break;
                        case DatabaseIntegration.Mysql:
                            //TODO:�����ַ��������û�
                            dbOptionBuilder = options.UseMySql(option.ConnectinString,
                                optionBuilder =>
                                {
                                    if (option.Type == ConnectionResolverType.ByTabel)
                                    {
                                        optionBuilder.MigrationsHistoryTable($"{tenant.Name}__EFMigrationsHistory");
                                    }
                                });
                            break;
                        default:
                            throw new System.NotSupportedException("db type not supported");
                    }
                    if (option.Type == ConnectionResolverType.ByTabel || option.Type == ConnectionResolverType.BySchema)
                    {
                        dbOptionBuilder.ReplaceService<IModelCacheKeyFactory, TenantModelCacheKeyFactory<TDbContext>>();
                        dbOptionBuilder.ReplaceService<Microsoft.EntityFrameworkCore.Migrations.IMigrationsAssembly, MigrationByTenantAssembly>();
                    }

                });


            });

            return services;
        }

        /// <summary>
        /// ���ڱ�Ķ��⻧
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection AddTenantDatabasePerTable<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            if (option == null)
            {
                option = new ConnectionResolverOption()
                {
                    Key = "default",
                    Type = ConnectionResolverType.ByTabel,
                    ConnectinString = "default",
                    DBType = DatabaseIntegration.PostgreSql
                };
            }
            return services.AddDatabase<TDbContext>(option);
        }

        /// <summary>
        /// ����Schema�Ķ��⻧
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IServiceCollection AddTenantDatabasePerSchema<TDbContext>(this IServiceCollection services,
                ConnectionResolverOption option)
            where TDbContext : DbContext, ITenantDbContext
        {
            if (option == null)
            {
                option = new ConnectionResolverOption()
                {
                    Key = "default",
                    Type = ConnectionResolverType.BySchema,
                    ConnectinString = "default",
                    DBType = DatabaseIntegration.PostgreSql
                };
            }

            return services.AddTenantDatabasePerTable<TDbContext>(option);
        }
    }
}