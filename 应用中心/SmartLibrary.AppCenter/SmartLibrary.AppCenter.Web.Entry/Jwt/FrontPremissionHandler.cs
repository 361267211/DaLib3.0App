using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.AppCenter.Application.Services.ApplicationSetting;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Web.Jwt
{
    public class FrontPremissionHandler : AuthorizationHandler<FrontPremissionRequirement>
    {
        /// <summary>
        /// 前
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, FrontPremissionRequirement requirement)
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
            var service = context.GetCurrentHttpContext().RequestServices.GetRequiredService<IApplicationSettingService>();
            var setInfo = await service.GetApplicationSettingAsync();
            if (string.IsNullOrWhiteSpace(userKey) && setInfo != null && setInfo.IsNeedLogin)
            {
                context.GetCurrentHttpContext().Response.Headers.Add("UnAuth", "1");
                context.GetCurrentHttpContext().Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }

    }
}
