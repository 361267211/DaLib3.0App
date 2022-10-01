using Furion;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartLibrary.Assembly.Application.Attributes;
using SmartLibrary.DatabaseTerrace.Application;
using SmartLibrary.DatabaseTerrace.Application.Interceptors;
using SmartLibrary.DatabaseTerrace.Common.Const;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace SmartLibrary.DatabaseTerrace.Web.Jwt
{
    public class PortalColumnPremissionHandler : AuthorizationHandler<PortalColumnPremissionRequirement>
    {
        /// <summary>
        /// 重写 Handler 添加自动刷新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PortalColumnPremissionRequirement requirement)
        {
            //截取路由
            var permission = App.HttpContext.Request.Path.Value[1..].Replace("/", ":");
            var userIdStr = context.User.FindFirst(e => e.Type == "UserKey")?.Value;

            //取当前接口的特性
            var actionDescriptor = App.HttpContext.GetControllerActionDescriptor();
            var attr = actionDescriptor.MethodInfo.GetCustomAttributes();

            var permissionObjAttr = attr.First(e => e.GetType().Name == nameof(PermissionObjAttribute));
            var objType = ((PermissionObjAttribute)permissionObjAttr).ObjType;

            var kv = HttpUtility.ParseQueryString(App.HttpContext.Request.QueryString.ToString());
            string databaseId = "";
            switch (objType)
            {
                case "Database":
                    databaseId = kv["databaseId"];
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(databaseId))
            {
                context.Fail();
                return;
            }

            //获取入参中的键值对
            var settings = await App.GetService<IDatabaseTerraceService>().GetDatabaseSettings();
            var database = await App.GetService<IDatabaseTerraceService>().GetDatabaseTerrace(new Guid(databaseId));

            if (settings.IsLoginAcess)
            {
                //验证是否登录
                if (string.IsNullOrEmpty(userIdStr))
                {
                    App.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "1";
                    context.Fail();
                    return;
                }
            }

            //是否指定了可访问分组或用户类型
            if (database.WhiteList == "1")
            {
                
                //调用grpc，获取角色信息，
                var userData = await App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>().GetUserByKeyAsync(new Google.Protobuf.WellKnownTypes.StringValue { Value = userIdStr });

                //根据userkey取基本信息
                var isOkGroups = database.UserGroups.Any(e => userData.GroupIds.Contains(e.ToString()));
                var isOkTypes = database.UserTypes.Contains(userData.Type);
                if (!isOkGroups && !isOkTypes)
                {
                    App.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "2";
                    context.Fail();
                    return;
                }
            }

            context.Succeed(requirement);
        }


    }
}
