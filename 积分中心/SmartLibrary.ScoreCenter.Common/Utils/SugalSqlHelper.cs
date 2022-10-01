/*********************************************************
* 名    称：SqlSugarHelper.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：SqlSugar连接服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.ScoreCenter.Common.Dtos;
using SqlSugar;
using System;

namespace SmartLibrary.ScoreCenter.Common.Utils
{
    /// <summary>
    /// SqlSugar连接服务
    /// </summary>
    public class SqlSugarHelper
    {
        public static SqlSugarClient GetTenantDb(string tenantId = "public")
        {
            var conn = SiteGlobalConfig.SqlServer.SqlConnection;
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = conn,//数据库连接字符串
                DbType = (DbType)Enum.Parse(typeof(DbType), SiteGlobalConfig.SugarDbType),//数据库类型
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
                        if (!string.IsNullOrWhiteSpace(tenantId))
                        {
                            entity.DbTableName = $"{tenantId}.{entity.DbTableName}";
                        }

                    }
                }
            });

            db.Aop.OnLogExecuting = (sql, pars) =>//Sql执行前
            {
                var sqlString = sql;
            };
            db.Aop.OnLogExecuted = (sql, pars) =>//Sql执行后
            {
                var sqlString = sql;
            };
            db.Aop.OnError = (exp) =>//执行错误
            {
                var expStr = exp;
            };
            return db;
        }



    }
}
