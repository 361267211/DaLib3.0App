using Furion;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.FileServer.Application.Services;
using SmartLibrary.FileServer.EntityFramework.Core.Dtos;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.FileServer.Web.Jwt
{
    /// <summary>
    /// 默认的权限校验规则处理器
    /// </summary>
    public class DefaultPremissionHandler : AuthorizationHandler<DefaultPremissionRequirement>
    {
        /// <summary>
        /// 重写 Handler 添加自动刷新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultPremissionRequirement requirement)
        {
            //截取路由
            var permission = App.HttpContext.Request.Path.Value[1..].Replace("/", ":");
            var userIdStr = context.User.FindFirst(e => e.Type == "UserKey").Value;

            //调用grpc，获取角色信息，
            //  UserRoleReply userRole = App.GetService<IGrpcClientResolver>().EnsureClient<UserGrpcService.UserGrpcServiceClient>().GetUserRole(new UserRoleRequest {  UserKey = "zzq" });

            //模拟假数据TODO：可以调用后删除
            UserRoleReply userRole = new UserRoleReply();
            userRole.UserRole = 2;

            //根据默认角色获取权限列表
            List<SysMenuPermissionDto> permissionList = new List<SysMenuPermissionDto>();
            var grpcService = App.GetService<ISysMenuService>();
            switch (userRole.UserRole)
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
            if (permissionList.Any(e => e.Permission == permission))
            {
                context.Succeed(requirement);
            }


        }

    }
}
