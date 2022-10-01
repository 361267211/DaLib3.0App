using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.AppCenter.Application.Services.Permission;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Web.Jwt
{
    /// <summary>
    /// 后
    /// </summary>
    public class BackPremissionHandler : AuthorizationHandler<BackPremissionRequirement>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BackPremissionRequirement requirement)
        {
            //强制权限通过
            context.Succeed(requirement);
            return;



            if (context.User == null)
            {
                context.GetCurrentHttpContext().Response.Headers.Add("UnAuth", "1");
                context.GetCurrentHttpContext().Response.StatusCode = 401;
                context.Fail();
                return;
            }
            var orgCode = context.User.FindFirst("OrgCode");
            if (orgCode == null || string.IsNullOrWhiteSpace(orgCode.Value))
            {
                context.GetCurrentHttpContext().Response.Headers.Add("UnAuth", "1");
                context.GetCurrentHttpContext().Response.StatusCode = 401;
                context.Fail();
                return;
            }
            var userKey = context.User.FindFirstValue("UserKey");
            if (string.IsNullOrWhiteSpace(userKey))
            {
                context.GetCurrentHttpContext().Response.Headers.Add("UnAuth", "1");
                context.GetCurrentHttpContext().Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Fail();
                return;
            }

            var permission = $"{context.GetCurrentHttpContext().Request.Method.ToLower()}-{context.GetCurrentHttpContext().Request.Path.Value[1..].Replace("/", ":")}";
            var iSysMenuService = context.GetCurrentHttpContext().RequestServices.GetRequiredService<ISysMenuService>();
            var permissionList = await iSysMenuService.GetUserPermissionList();

            if (permissionList.Any(c => permission.StartsWith(c)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.GetCurrentHttpContext().Response.Headers["UnAuth"] = "2";
                context.GetCurrentHttpContext().Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Fail();
            }

        }

    }
}
