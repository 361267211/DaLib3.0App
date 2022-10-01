using Furion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.AuthHandler
{
    public class ReaderAuthHandler : AuthorizationHandler<ReaderAuthRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ReaderAuthRequirement requirement)
        {
            if (context.User == null)
            {
                context.GetCurrentHttpContext().Response.Headers["UnAuth"] = "1";
                context.Fail();
                return;
            }
            var userKey = context.User.FindFirst(ClaimConst.Claim_UserKey);
            if (userKey == null || string.IsNullOrWhiteSpace(userKey.Value))
            {
                context.GetCurrentHttpContext().Response.Headers["UnAuth"] = "1";
                context.Fail();
                return;
            }
            try
            {
                var _userPermissionService = context.GetCurrentHttpContext().RequestServices.GetRequiredService<IUserPermissionService>();
                var userInfo = await _userPermissionService.GetUserInfo(userKey.Value);
                if (userInfo == null)
                {
                    context.GetCurrentHttpContext().Response.Headers["UnAuth"] = "1";
                    context.Fail();
                    return;
                }

                context.Succeed(requirement);
            }
            catch
            {
                context.Fail();
                return;
            }
        }
    }
}
