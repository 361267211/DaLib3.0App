using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SmartLibrary.DatabaseTerrace.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartLibrary.DatabaseTerrace.Application.Services;
using System.Security.Claims;
using Newtonsoft.Json;
using SmartLibraryUser;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.DatabaseTerrace.Application.Interceptors;
using SmartLibrary.User.RpcService;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
using Mapster;
using Google.Protobuf.WellKnownTypes;
using SmartLibrary.DatabaseTerrace.Common.Const;

namespace SmartLibrary.DatabaseTerrace.Web.Jwt
{
    public class LoginPremissionHandler : AuthorizationHandler<LoginPremissionRequirement>
    {
        /// <summary>
        /// 应用自定义的权限校验规则处理器-只校验是否登陆
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, LoginPremissionRequirement requirement)
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


            context.Succeed(requirement);



        }

    }
}
