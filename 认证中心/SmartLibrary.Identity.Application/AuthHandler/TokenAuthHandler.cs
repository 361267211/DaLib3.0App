using Microsoft.AspNetCore.Authorization;
using SmartLibrary.Identity.Common.Const;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.AuthHandler
{
    public class TokenAuthHandler : AuthorizationHandler<TokenAuthRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenAuthRequirement requirement)
        {
            if (context.User == null)
            {
                context.GetCurrentHttpContext().Response.Headers["UnAuth"] = "1";
                context.Fail();
                return;
            }
            var orgKey = context.User.FindFirst(ClaimConst.Claim_OrgCode);
            if (orgKey == null || string.IsNullOrWhiteSpace(orgKey.Value))
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
            await Task.CompletedTask;
        }
    }
}
