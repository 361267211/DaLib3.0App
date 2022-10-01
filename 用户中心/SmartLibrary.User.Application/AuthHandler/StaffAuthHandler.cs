/*********************************************************
* 名    称：StaffAuthHandler.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：馆员后台操作权限校验
* 更新历史：
*
* *******************************************************/
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Common.Const;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AuthHandler
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
                var checkResult = this.CheckUserPermission(permissionList, routeMethod, $"{routeMethod}-{routeName}");
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
            for (var i = 0; i < userPermissions.Count(); i++)
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
                for (var j = 0; j < xroutes.Length; j++)
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
