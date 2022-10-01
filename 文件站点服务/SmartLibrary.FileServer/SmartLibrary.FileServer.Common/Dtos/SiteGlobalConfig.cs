/*********************************************************
 * 名    称：SiteGlobalConfig
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：全局静态配置类。
 *
 * 更新历史：
 *
 * *******************************************************/
using SmartLibrary.Core.Apollo;
using System.Collections.Generic;

namespace SmartLibrary.FileServer.Common.Dtos
{
    public static class SiteGlobalConfig
    {
        public static List<SqlServerConfig> ListCap { get; set; }

        [IgnoreMapping]
        public static string ApiRedisConnection => RedisServer.RedisConnection;
        [IgnoreMapping]
        public static List<string> FileExtNames => new List<string>(FileConfigInfo.FileExtNames.Split(','));

        public static CapConfig Cap { get; set; }
        public static SqlServerConfig SqlServer { get; set; }
        public static ServiceRegistConfig ServiceRegist { get; set; }
        public static GrpcRegistConfig GrpcRegist { get; set; }
        public static JwtAuthConfig JwtAuth { get; set; }
        public static EsProxyConfig EsProxy { get; set; }
        public static RedisServerConfig RedisServer { get; set; }
        public static ExceptionlessConfig Exceptionless { get; set; }
        public static ListenPortConfig ListenPort { get; set; }
        public static LogConfig Serilog { get; set; }

        public static FileConfigInfo FileConfigInfo { get; set; }



    }

    /// <summary>
    /// 文件站点配置信息
    /// </summary>
    public class FileConfigInfo
    {
        /// <summary>
        /// 允许上传的文件扩展名，多个扩展名用英文逗号分割
        /// </summary>
        public string FileExtNames { get; set; }

    }

    public class LogConfig
    {
        public string MinLevel { get; set; }
    }

    public class ListenPortConfig
    {
        public int GrpcPort { get; set; }
        public int ApiPort { get; set; }
    }

    public class ExceptionlessConfig
    {
        public string Key { get; set; }
        public string Url { get; set; }
    }
    public class EsProxyConfig
    {
        public string BaseAddress { get; set; }
        public string SiteId { get; set; }
        public string SiteUserName { get; set; }
        public string SitePassword { get; set; }


    }

    public class JwtAuthConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; } = "WebApi";
    }
    public class ServiceRegistConfig
    {
        public string Url { get; set; }
        public int Port { get; set; }
        public string ConsulAddress { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTags { get; set; }
    }

    public class GrpcRegistConfig
    {
        public string Url { get; set; }
        public int Port { get; set; }
        public string ConsulAddress { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTags { get; set; }
    }

    public class SqlServerConfig
    {
        public string SqlConnection { get; set; }
        public string MigrationAssembly { get; set; }
    }
    public class RedisServerConfig
    {
        public string RedisConnection { get; set; }
    }
    public class CapConfig
    {
        public string TenantName { get; set; }
        public string SqlConnection { get; set; }
        public CapRabbitMQ RabbitMQ { get; set; }
    }
    public class CapRabbitMQ
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
