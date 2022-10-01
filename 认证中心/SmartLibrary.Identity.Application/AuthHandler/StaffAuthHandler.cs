using Furion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.Identity.Application.Dtos.Common;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.AuthHandler
{
    /// <summary>
    /// 馆员权限校验
    /// </summary>
    public class StaffAuthHandler : AuthorizationHandler<StaffAuthRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StaffAuthRequirement requirement)
        {

            if (context.User == null)
            {
                context.GetCurrentHttpContext().Response.Headers[PolicyKey.UnAuthKey] = "1";
                context.Fail();
                return;
            }
            var userKey = context.User.FindFirst(ClaimConst.Claim_UserKey);
            if (userKey == null || string.IsNullOrWhiteSpace(userKey.Value))
            {
                context.GetCurrentHttpContext().Response.Headers[PolicyKey.UnAuthKey] = "1";
                context.Fail();
                return;
            }
            try
            {
                var _userPermissionService = context.GetCurrentHttpContext().RequestServices.GetRequiredService<IUserPermissionService>();
                var userInfo = await _userPermissionService.GetUserInfo(userKey.Value);
                if (userInfo == null)
                {
                    context.GetCurrentHttpContext().Response.Headers[PolicyKey.UnAuthKey] = "1";
                    context.Fail();
                    return;
                }
                if (!userInfo.IsStaff)
                {
                    context.Fail();
                    return;
                }
                var permissionList = (await _userPermissionService.GetUserPermission(userKey.Value)).PermissionList.Select(x => x.ToLower()).ToList();
                // 路由名称
                var routeName = context.GetCurrentHttpContext().Request.Path.Value[1..].Replace("/", ":").ToLower();
                var routeMethod = context.GetCurrentHttpContext().Request.Method.ToLower();
                // 默认路由(获取登录用户信息)
                var defalutRoute = new List<string>()
                {
                    "get-api:sys-menu:user-permission-tree",
                    "get-api:sys-menu:user-permission-list",
                };
                permissionList.AddRange(defalutRoute);
                // 检查授权
                var checkResult = this.CheckUserPermission(permissionList, routeMethod, routeName);
                if (checkResult)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            catch
            {
                context.Fail();
                return;
            }

        }

        /// <summary>
        /// 检查路由信息
        /// </summary>
        /// <param name="userPermissions"></param>
        /// <param name="routeMethod"></param>
        /// <param name="routeName"></param>
        /// <returns></returns>
        private bool CheckUserPermission(List<string> userPermissions, string routeMethod, string routeName)
        {
            var resultFlag = false;
            var routeSplits = routeName.Split(":").ToArray();
            var routeSplitsLength = routeSplits.Length;
            for (var i = 0; i <= userPermissions.Count(); i++)
            {
                var x = userPermissions[i];
                var xroutes = x.Split(":").ToArray();
                if (xroutes.Length == 0 || xroutes.Length != routeSplitsLength)
                {
                    continue;
                }
                var method = xroutes[0].Replace("-api", "");
                if (method != routeMethod)
                {
                    continue;
                }
                var allMatch = true;
                for (var j = 0; i < xroutes.Length; j++)
                {
                    if (xroutes[j] != "*" && xroutes[j] != routeSplits[j])
                    {
                        allMatch = false;
                        break;
                    }
                }
                if (!allMatch)
                {
                    continue;
                }
                resultFlag = true;
                break;
            }
            return resultFlag;
        }
    }
}
