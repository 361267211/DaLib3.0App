/*********************************************************
* 名    称：PermitReaderAuthHandler.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：用于校验配置前台访问权限的应用
* 更新历史：
*
* *******************************************************/
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Common.Const;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.AuthHandler
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
