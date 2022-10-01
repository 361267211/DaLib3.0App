using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SmartLibrary.Open.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Assembly.Common.Extensions
{
    /// <summary>
    /// 提供一些扩展方法，用以读取http上下文中的一些信息，如claim等
    /// </summary>
    public static class HttpContextExtension
    {
        /// <summary>
        /// 获取Owner
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string EnsureOwner(this HttpContext httpContext)
        {
            if (httpContext?.User.Identity?.IsAuthenticated != true) throw new UnauthorizedAccessException("请先登录");
            return httpContext.EnsureClaimValue(ClaimConst.Claim_OrgCode);
        }
        /// <summary>
        /// 获取Owner
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static string EnsureOwner(this IHttpContextAccessor httpContextAccessor) =>
            httpContextAccessor?.HttpContext.EnsureOwner();
        /// <summary>
        /// 获取指定claim名的值
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="claimName"></param>
        /// <returns></returns>
        public static string EnsureClaimValue(this HttpContext httpContext, string claimName)
        {
            if (httpContext?.User.Identity?.IsAuthenticated != true) throw new UnauthorizedAccessException("请先登录");
            return httpContext.User.FindFirstValue(claimName);
        }
        /// <summary>
        /// 获取指定claim名的值
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="claimName"></param>
        /// <returns></returns>
        public static string EnsureClaimValue(this IHttpContextAccessor httpContextAccessor, string claimName) =>
            httpContextAccessor?.HttpContext.EnsureClaimValue(claimName);


        public static string EnsureRemoteClientIp(this HttpContext context)
        {
            var result = context.Request.Headers["CDR-SRC-IP"].ToString();
            if (string.IsNullOrWhiteSpace(result))
            {
                result = context.Request.Headers["X-REAL-IP"].ToString();

            }
            if (string.IsNullOrWhiteSpace(result))
            {
                var temp = context.Request.Headers["X-FORWARDED-FOR"];
                if (temp != StringValues.Empty && temp.Count > 0)
                {
                    result = temp[0];
                }

            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = context.Request.Headers["REMOTE_ADDR"].ToString();
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = context.Connection.RemoteIpAddress?.ToString();
            }
            return result;

        }
        /// <summary>
        /// 获取客户端ip
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static string EnsureRemoteClientIp(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.EnsureRemoteClientIp();
        }
    }
}
