/*********************************************************
* 名    称：ServiceCollectionExtensions.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Dtm服务扩展
* 更新历史：
*
* *******************************************************/
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.DtmClient.Dtm;
using SmartLibrary.DtmClient.Dtm.Tcc;
using SqlSugar;
using System;

namespace SmartLibrary.Dtm.Dtm
{
    /// <summary>
    /// Dtm服务注册
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ServiceCollection;
        public static IServiceCollection AddDtmClient(this IServiceCollection services, Action<DtmOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }
            var options = new DtmOptions();
            setupAction(options);
            ServiceCollection = services;
            services.AddHttpClient("dtmClient", client =>
            {
                client.BaseAddress = new Uri(options.DtmUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddTypedClient<IDtmClient, DtmClient.Dtm.DtmClient>();
            services.AddScoped<BranchBarrier>();
            services.AddScoped<IDtmClient, DtmClient.Dtm.DtmClient>();
            services.AddScoped<TccGlobalTransaction>();
            switch (options.DbType)
            {
                case (int)EnumDbType.PostgreSQL:
                    services.AddScoped<IDataOperator, DataOperatorForPgSql>();
                    break;
                default:
                    services.AddScoped<IDataOperator, DataOperatorForPgSql>();
                    break;
            }
            var serviceProvider = services.BuildServiceProvider();
            var dbOperator = serviceProvider.GetRequiredService<IDataOperator>();
            var dbClient = GetSugarDb(options.ConnectionString, options.DbType);
            dbOperator.InitData(dbClient).GetAwaiter().GetResult();
            dbClient.Close();
            return services;
        }

        private static SqlSugarClient GetSugarDb(string connectionStr, int dbType)
        {
            var conn = connectionStr;
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = conn,//数据库连接字符串
                DbType = (DbType)dbType,//数据库类型
                IsAutoCloseConnection = true,//自动关闭
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings
                {
                    PgSqlIsAutoToLower = false,
                },
            });
            return db;
        }
    }
}
