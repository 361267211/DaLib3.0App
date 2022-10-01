using Furion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.AppCenter;
using SmartLibrary.SceneManage.Application.Services;
using SmartLibrary.SceneManage.Common;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.EntityFramework.Core.Dtos;
using SmartLibrary.SceneManage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SmartLibrary.AppCenter.AppCenterGrpcService;

namespace SmartLibrary.SceneManage.Web.Jwt
{
    /// <summary>
    /// 默认的权限校验规则处理器
    /// </summary>
    public class DefaultPremissionHandler : AuthorizationHandler<DefaultPremissionRequirement>
    {
        private const string UnAuthKey = "UnAuth";
        /// <summary>
        /// 重写 Handler 添加自动刷新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultPremissionRequirement requirement)
        {
            //强制权限通过
            context.Succeed(requirement);
            return;


            var httpContext = context.GetCurrentHttpContext();
            //截取路由
            var permission = $"{httpContext.Request.Method.ToLower()}|{httpContext.Request.Path.Value[1..].Replace("/", ":")}";
            var userIdStr = context.User.FindFirst(e => e.Type == "UserKey")?.Value;
            if (string.IsNullOrEmpty(userIdStr))
            {
                context.GetCurrentHttpContext().Response.Headers[UnAuthKey] = "1";
                context.Fail();
                return;
            }
            UserAppPermissionTypeReply userRole = new UserAppPermissionTypeReply();
            //调用grpc，获取角色信息，
            userRole = httpContext.RequestServices.GetRequiredService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcServiceClient>(nameof(AppCenterGrpcServiceClient)).GetUserAppPermissionType(new UserAppPermissionTypeRequest { AppId= SiteGlobalConfig.AppBaseConfig.AppRouteCode });


            //根据默认角色获取权限列表
            List<SysMenuPermissionDto> permissionList = new List<SysMenuPermissionDto>();
            var grpcService = httpContext.RequestServices.GetRequiredService<ISysMenuService>();
            switch (userRole.PermissionType)
            {
                case 1://管理员
                    permissionList = await grpcService.GetMGRPermissionList();
                    break;
                case 2://操作者
                    permissionList = await grpcService.GetOperatorPermissionList();
                    break;
                case 3://浏览者
                    permissionList = await grpcService.GetVisitorsPermissionList();
                    break;
                default:
                    return; //没有权限直接返回

            }
            if (permissionList.Any(e => permission.StartsWith(e.Permission)))
            {
                context.Succeed(requirement);
            }


        }

    }
}
