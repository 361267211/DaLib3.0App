/*********************************************************
 * 名    称：ConnectionResolverOption
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：多租户数据库连接信息类示例。
 *
 * 更新历史：
 *
 * *******************************************************/


using System;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    /// <summary>
    /// 数据库连接信息的模型
    /// </summary>
    public class ConnectionResolverOption
    {
        public string Key { get; set; } = "default";

        public ConnectionResolverType Type { get; set; }

        public string ConnectinString { get; set; }

        public DatabaseIntegration DBType { get; set; }

        public string MigrationAssembly { get; set; }
    }

    /// <summary>
    /// 多租户类型
    /// </summary>
    public enum ConnectionResolverType
    {
        Default = 0,
        ByDatabase = 1,
        ByTable = 2,
        BySchema = 3
    }
}
