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

namespace SmartLibrary.AppCenter.Common.Dtos
{
    /// <summary>
    /// 站点配置类
    /// </summary>
    public static class SiteGlobalConfig
    {
        /// <summary>
        /// 站点基本信息
        /// </summary>
        public static AppBaseConfig AppBaseConfig { get; set; }

        /// <summary>
        /// CAP 配置
        /// </summary>
        public static CapConfig Cap { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public static DatabaseConfig Database { get; set; }

        /// <summary>
        /// API注册信息
        /// </summary>
        public static ServiceRegistConfig ServiceRegist { get; set; }

        /// <summary>
        /// grpc注册信息
        /// </summary>
        public static GrpcRegistConfig GrpcRegist { get; set; }

        /// <summary>
        /// JWT配置信息
        /// </summary>
        public static JwtAuthConfig JwtAuth { get; set; }

        /// <summary>
        /// ES检索配置
        /// </summary>
        public static EsProxyConfig EsProxy { get; set; }

        /// <summary>
        /// Redis配置
        /// </summary>
        public static RedisServerConfig RedisServer { get; set; }

        /// <summary>
        /// Exceptionless 配置
        /// </summary>
        public static ExceptionlessConfig Exceptionless { get; set; }

        /// <summary>
        /// 监听端口 配置
        /// </summary>
        public static ListenPortConfig ListenPort { get; set; }

        /// <summary>
        /// 日志配置
        /// </summary>
        public static LogConfig Serilog { get; set; }

        /// <summary>
        /// 2.2站点配置信息
        /// </summary>
        public static OldSiteConfig OldSite { get; set; }
    }

    /// <summary>
    /// 基本信息
    /// </summary>
    public class AppBaseConfig
    {
        /// <summary>
        /// API 网关
        /// </summary>
        public string RestApiGateway { get; set; }

        /// <summary>
        /// grpc 网关
        /// </summary>
        public string GrpcGateway { get; set; }

        /// <summary>
        /// JWT 验证中心
        /// </summary>
        public string JwtIdentifyCenter { get; set; }

        /// <summary>
        /// APP code  每个应用固定
        /// </summary>
        public string AppRouteCode { get; set; }
    }

    /// <summary>
    /// 日志配置
    /// </summary>
    public class LogConfig
    {
        public string MinLevel { get; set; }
    }

    /// <summary>
    /// 监听端口
    /// </summary>
    public class ListenPortConfig
    {
        /// <summary>
        /// GRPC端口
        /// </summary>
        public int GrpcPort { get; set; }

        /// <summary>
        /// API端口
        /// </summary>
        public int ApiPort { get; set; }
    }

    /// <summary>
    /// Exceptionless Config
    /// </summary>
    public class ExceptionlessConfig
    {
        public string Key { get; set; }
        public string Url { get; set; }
    }

    /// <summary>
    /// ES检索配置
    /// </summary>
    public class EsProxyConfig
    {
        public string BaseAddress { get; set; }
        public string SiteId { get; set; }
        public string SiteUserName { get; set; }
        public string SitePassword { get; set; }
    }

    /// <summary>
    /// jwt 配置
    /// </summary>
    public class JwtAuthConfig
    {
        public string Issuer { get; set; }
        public string Audience { get; set; } = "WebApi";
    }

    /// <summary>
    /// 应用注册
    /// </summary>
    public class ServiceRegistConfig
    {
        public string Url { get; set; }
        public int Port { get; set; }
        public string ConsulAddress { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTags { get; set; }
    }

    /// <summary>
    /// GRPC 注册
    /// </summary>
    public class GrpcRegistConfig
    {
        public string Url { get; set; }
        public int Port { get; set; }
        public string ConsulAddress { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTags { get; set; }
    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DatabaseConfig
    {
        public string SqlConnection { get; set; }
        public string MigrationAssembly { get; set; }
    }

    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisServerConfig
    {
        public string RedisConnection { get; set; }
    }

    /// <summary>
    /// CAP 配置
    /// </summary>
    public class CapConfig
    {
        public string TenantName { get; set; }
        public string SqlConnection { get; set; }
        public CapRabbitMQ RabbitMQ { get; set; }
    }

    /// <summary>
    /// MQ 配置
    /// </summary>
    public class CapRabbitMQ
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// 2.2站点配置
    /// </summary>
    public class OldSiteConfig
    {
        /// <summary>
        /// 前台站点地址
        /// </summary>
        public string WebUrl { get; set; }
        /// <summary>
        /// aid
        /// </summary>
        public string Aid { get; set; }
        /// <summary>
        /// secret
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// owner
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// 后台站点地址
        /// </summary>
        public string MgrUrl { get; set; }
    }
}
