/*********************************************************
 * ��    �ƣ�AssetDbContext
 * ��    �ߣ�������
 * ��ϵ��ʽ���绰[13883914813],�ʼ�[361267211@qq.com]
 * ����ʱ�䣺2021/8/05 16:57:45
 * ��    �����⻧��Ϣ�����ġ�
 *
 * ������ʷ��
 *
 * *******************************************************/

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    public interface ITenantDbContext
    {
        TenantInfo TenantInfo { get; }
    }
}
