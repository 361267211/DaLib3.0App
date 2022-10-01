/*********************************************************
 * ��    �ƣ�TenantInfoMiddleware
 * ��    �ߣ�������
 * ��ϵ��ʽ���绰[13883914813],�ʼ�[361267211@qq.com]
 * ����ʱ�䣺2021/8/05 16:57:45
 * ��    �����м��-aopʵ�� ����scope���� TenantInfo��ȡtoken�е���Ϣ���丳ֵ��
 *
 * ������ʷ��
 *
 * *******************************************************/

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    /// <summary>
    /// �м�������أ�����httpcontext�����ȡ�⻧��Ϣ����������չ����
    /// </summary>
    public class TenantInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// ��ȡScope���ڵ�TenantInfo��Ϊ�丳ֵcontext�е��⻧��Ϣ
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var tenantInfo = context.RequestServices.GetRequiredService<TenantInfo>();
            var tenantName = "";
            if (context.User != null && context.User.HasClaim(x => x.Type == "OrgCode"))
            {
                tenantName = context.User.Claims.FirstOrDefault(x => x.Type == "OrgCode").Value;
            }
            tenantInfo.Name = tenantName;
            await _next(context);
        }
    }
}


