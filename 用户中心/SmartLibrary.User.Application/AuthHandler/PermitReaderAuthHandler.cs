using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Common.Const;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AuthHandler
{

    public class PermitReaderAuthHandler : AuthorizationHandler<PermitReaderAuthRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermitReaderAuthRequirement requirement)
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
                var frontPermission = await _userPermissionService.GetReaderPermission(userKey.Value);
                if (!frontPermission.HasPermission)
                {
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
