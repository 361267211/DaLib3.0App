/*********************************************************
 * ��    �ƣ��⻧��Ϣ
 * ��    �ߣ�������
 * ��ϵ��ʽ���绰[13883914813],�ʼ�[361267211@qq.com]
 * ����ʱ�䣺2021/8/05 16:57:45
 * ��    �����⻧��Ϣ��ģ�͡�
 *
 * ������ʷ��
 *
 * *******************************************************/

using System;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    /// <summary>
    /// �⻧��Ϣ��ģ���࣬���ڿɼ�����չ
    /// </summary>
    public class TenantInfo
    {
        /// <summary>
        /// ���⻧����
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// �⻧UserKey
        /// </summary>
        public string UserKey { get; set; }
    }
}
