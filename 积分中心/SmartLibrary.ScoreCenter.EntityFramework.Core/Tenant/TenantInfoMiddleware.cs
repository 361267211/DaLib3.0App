/*********************************************************
 * 名    称：TenantInfoMiddleware
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：中间件-aop实现 生成scope周期 TenantInfo，取token中的信息对其赋值。
 *
 * 更新历史：
 *
 * *******************************************************/

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure
{
    /// <summary>
    /// 中间件，拦截，根据httpcontext请求获取租户信息，后续可拓展定义
    /// </summary>
    public class TenantInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// 获取Scope周期的TenantInfo，为其赋值context中的租户信息
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


