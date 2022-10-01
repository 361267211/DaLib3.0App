using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;

namespace Scheduler.Service
{
    public class DataHelper
    {
        public static SqlSugarClient GetInstance(IConfigurationRoot config)
        {
            var conn = config.GetSection("Appsettings")["conn"].ToString();
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = config.GetSection("Appsettings")["conn"].ToString(),//数据库连接字符串
                DbType = (DbType)Enum.Parse(typeof(DbType), config.GetSection("Appsettings")["dbType"].ToString()),//数据库类型
                IsAutoCloseConnection = true,//自动关闭
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings
                {
                    PgSqlIsAutoToLower = false,
                }
            });

            db.Aop.OnLogExecuting = (sql, pars) =>//Sql执行前
            {
            };
            db.Aop.OnLogExecuted = (sql, pars) =>//Sql执行后
            {
            };
            db.Aop.OnError = (exp) =>//执行错误
            {
            };
            return db;
        }

        public static SqlSugarClient GetTenantDb(IConfigurationRoot config, string tenantId)
        {
            var conn = config.GetSection("Appsettings")["conn"].ToString();
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = config.GetSection("Appsettings")["conn"].ToString(),//数据库连接字符串
                DbType = (DbType)Enum.Parse(typeof(DbType), config.GetSection("Appsettings")["dbType"].ToString()),//数据库类型
                IsAutoCloseConnection = true,//自动关闭
                InitKeyType = InitKeyType.Attribute,
                MoreSettings = new ConnMoreSettings
                {
                    PgSqlIsAutoToLower = false,
                },
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityNameService = (type, entity) =>
                     {
                         if (!string.IsNullOrWhiteSpace(tenantId) && tenantId != SchedulerKey.PUBLICTENANT_KEY)
                         {
                             entity.DbTableName = $"{tenantId}.{entity.DbTableName}";
                         }

                     }
                }
            });

            db.Aop.OnLogExecuting = (sql, pars) =>//Sql执行前
            {
            };
            db.Aop.OnLogExecuted = (sql, pars) =>//Sql执行后
            {
            };
            db.Aop.OnError = (exp) =>//执行错误
            {
            };
            return db;
        }


        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient GetInstance(string connStr, int dbType, bool isAutoClose = true)
        {

            var sqlClient = new SqlSugarClient(
             new ConnectionConfig()
             {
                 ConnectionString = connStr,//必填, 数据库连接字符串
                 DbType = (DbType)dbType,         //必填, 数据库类型
                 IsAutoCloseConnection = isAutoClose,      //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                 InitKeyType = InitKeyType.Attribute
             });
            sqlClient.Aop.OnLogExecuted = (sql, pars) => //SQL执行完事件
            {

            };
            sqlClient.Aop.OnLogExecuting = (sql, pars) => //SQL执行前事件
            {

            };
            sqlClient.Aop.OnError = (exp) =>//执行SQL 错误事件
            {

            };
            return sqlClient;
        }
    }
}
