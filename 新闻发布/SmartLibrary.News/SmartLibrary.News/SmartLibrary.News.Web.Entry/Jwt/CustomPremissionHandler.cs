using Furion;
using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SmartLibrary.News.Application.Services;
using SmartLibrary.News.Common.Const;
using SmartLibrary.News.Application;
using Microsoft.Extensions.DependencyInjection;
using Furion.FriendlyException;

namespace SmartLibrary.News.Web.Jwt
{
    public class CustomPremissionHandler : AuthorizationHandler<CustomPremissionRequirement>
    {

        private readonly IHttpContextAccessor _httpContext;

        public CustomPremissionHandler(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        /// <summary>
        /// 应用自定义的权限校验规则处理器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomPremissionRequirement requirement)
        {

            //截取路由
            var permission = _httpContext.HttpContext.Request.Path.Value[1..].Replace("/", ":");
            var userIdStr = context.User.FindFirst(e => e.Type == "UserKey")?.Value;
            var columnId = permission.Split(':', StringSplitOptions.RemoveEmptyEntries).Last();

            var column = await _httpContext.HttpContext.RequestServices.GetRequiredService<INewsColumnService>().GetNewsColumn(columnId);

            if (column==null)
            {
                throw Oops.Oh("栏目id错误或已删除").StatusCode(HttpStatusKeys.ExceptionCode);
            }

            //是否需要登陆认证
            if (column.IsLoginAcess == 1)
            {
                var userkey = _httpContext.HttpContext.User.FindFirstValue("UserKey");
                if (string.IsNullOrEmpty(userkey))
                {
                    _httpContext.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "1";
                    context.Fail();
                    return;
                }
            }

            //校验权限

            context.Succeed(requirement);



        }

    }
}
