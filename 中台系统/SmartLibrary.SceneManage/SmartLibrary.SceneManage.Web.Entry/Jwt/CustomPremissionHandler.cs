using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.SceneManage.Application.Services;
using SmartLibrary.SceneManage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartLibrary.SceneManage.Web.Jwt
{
    public class CustomPremissionHandler : AuthorizationHandler<CustomPremissionRequirement>
    {
        /// <summary>
        /// 应用自定义的权限校验规则处理器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomPremissionRequirement requirement)
        {
            //强制权限通过
            context.Succeed(requirement);
            return;




            var httpContext = context.GetCurrentHttpContext();
            //截取路由
            var permission = httpContext.Request.Path.Value[1..].Replace("/", ":");
            var userIdStr = context.User.FindFirst(e => e.Type == "UserKey").Value;

            //根据id获取登陆用户的api权限列表
            var permissionList = await httpContext.RequestServices.GetRequiredService<ISysMenuService>().GetUserPermissionList(new Guid(userIdStr));
            //校验权限
            if (permissionList.Contains(permission))
            {
                context.Succeed(requirement);
            }


        }

    }
}
