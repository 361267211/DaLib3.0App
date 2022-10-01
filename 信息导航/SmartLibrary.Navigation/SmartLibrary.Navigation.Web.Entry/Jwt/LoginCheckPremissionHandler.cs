using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.Navigation.Application.Services;
using SmartLibrary.Navigation.Common.Const;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Web.Jwt
{
    public class LoginCheckPremissionHandler : AuthorizationHandler<LoginCheckPremissionRequirement>
    {
        /// <summary>
        /// 应用自定义的权限校验规则处理器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, LoginCheckPremissionRequirement requirement)
        {
            //截取路由
            var permission = App.HttpContext.Request.Path.Value[1..].Replace("/", ":");
            var userIdStr = context.User.FindFirst(e => e.Type == "UserKey")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
            {
                App.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "1";
                context.Fail();
                return;
            }
            //根据id获取登陆用户的api权限列表
            var permissionList = await App.GetService<ISysMenuService>().GetUserPermissionList(userIdStr);
            //校验权限

            context.Succeed(requirement);



        }

    }
}
