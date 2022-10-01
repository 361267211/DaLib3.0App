/*********************************************************
 * 名    称：MultipleTenancyExtension
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：注册数据库上下文。
 *
 * 更新历史：
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
    /// 注册DbContext
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
        /// 基于数据库连接的多租户
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
        /// 注册
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
                //注册主DbContext追踪器
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
                            //TODO:连接字符串待配置化
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
                            //TODO:连接字符串待配置化
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
        /// 基于表的多租户
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
        /// 基于Schema的多租户
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