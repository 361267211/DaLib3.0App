/*********************************************************
 * ��    �ƣ�ConnectionResolverOption
 * ��    �ߣ�������
 * ��ϵ��ʽ���绰[13883914813],�ʼ�[361267211@qq.com]
 * ����ʱ�䣺2021/8/05 16:57:45
 * ��    �������⻧���ݿ�������Ϣ��ʾ����
 *
 * ������ʷ��
 *
 * *******************************************************/


using System;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    /// <summary>
    /// ���ݿ�������Ϣ��ģ��
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
    /// ���⻧����
    /// </summary>
    public enum ConnectionResolverType
    {
        Default = 0,
        ByDatabase = 1,
        ByTable = 2,
        BySchema = 3
    }
}
