/*********************************************************
* 名    称：TokenAuthHandler.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：机构Token权限校验
* 更新历史：
*
* *******************************************************/
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.User.Common.Const;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AuthHandler
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
