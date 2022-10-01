using Furion;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.News.Application;
using SmartLibrary.News.Application.Attributes;
using SmartLibrary.News.Application.Interceptors;
using SmartLibrary.News.Common.Const;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace SmartLibrary.News.Web.Jwt
{
    public class PortalColumnPremissionHandler : AuthorizationHandler<PortalColumnPremissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContext;


        public PortalColumnPremissionHandler(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        /// <summary>
        /// 重写 Handler 添加自动刷新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PortalColumnPremissionRequirement requirement)
        {
            //截取路由
            var permission = _httpContext.HttpContext.Request.Path.Value[1..].Replace("/", ":");
            var userIdStr = context.User.FindFirst(e => e.Type == "UserKey")?.Value;

            //取当前接口的特性
            var actionDescriptor = _httpContext.HttpContext.GetControllerActionDescriptor();
            var attr = actionDescriptor.MethodInfo.GetCustomAttributes();

            var permissionObjAttr = attr.FirstOrDefault(e => e.GetType().Name == nameof(PermissionObjAttribute));
            var objType = ((PermissionObjAttribute)permissionObjAttr).ObjType;

            var kv = HttpUtility.ParseQueryString(_httpContext.HttpContext.Request.QueryString.ToString());
            string columnId = "";
            switch (objType)
            {
                case "Content":
                     columnId = await _httpContext.HttpContext.RequestServices.GetRequiredService<INewsContentService>().GetNewsColumnIdByContentId(kv["contentID"]);
                    //columnId = assembly.FirstOrDefault(e => e.Id.ToString() == kv["contenID"])?.ColumnID.ToString();
                    break;
                case "Column":
                    columnId = kv["columnID"];
                    break;
                default:
                    break;
            }



            //获取入参中的键值对
            var column = await _httpContext.HttpContext.RequestServices.GetRequiredService<INewsColumnService>().GetNewsColumn(columnId);
            if (column==null)
            {
                throw Oops.Oh("栏目id有问题");
            }

            // var column = await _httpContext.HttpContext.RequestServices.GetRequiredService<IColumnService>().GetAssemblyColumnById(new Guid(columnId));
            if (column.IsLoginAcess==1)
            {
                //验证是否登录
                if (string.IsNullOrEmpty(userIdStr))
                {
                    _httpContext.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "1";
                    context.Fail();
                    return;
                }
            }

            //是否指定了可访问分组或用户类型
            if (!column.AcessAll)
            {
                //调用grpc，获取角色信息，
                var userData = await _httpContext.HttpContext.RequestServices.GetRequiredService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>().GetUserByKeyAsync(new Google.Protobuf.WellKnownTypes.StringValue { Value = userIdStr });

                //根据userkey取基本信息
                // var isOkGroups = (column.UserGroups ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries).Any(e => userData.GroupIds.Contains(e.ToString()));
                var isOkTypes = (column.VisitingList ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).Contains(userData.Type);
                if (/*!isOkGroups &&*/ !isOkTypes)
                {
                    _httpContext.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "2"; 

                    context.Fail();
                    return;
                }
            }

            context.Succeed(requirement);
        }


    }
}
