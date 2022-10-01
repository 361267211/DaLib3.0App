/*********************************************************
* 名    称：ReaderAuthHandler.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：前台读者权限校验
* 更新历史：
*
* *******************************************************/
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Common.Const;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AuthHandler
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
