using Furion;
using Microsoft.AspNetCore.Authorization;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartLibrary.News.Application.Interceptors;
using SmartLibrary.News.Application.Services;
using SmartLibrary.News.EntityFramework.Core.Dtos;
using Microsoft.AspNetCore.Http;
using SmartLibrary.News.Application;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.News.Common.Const;

namespace SmartLibrary.News.Web.Jwt
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
            var userIdStr = "";


 

            //取用户key
            if (context.User != null && context.User.HasClaim(x => x.Type == "UserKey"))
            {
                userIdStr = context.User.Claims.FirstOrDefault(x => x.Type == "UserKey").Value;
            }
            UserRoleReply userRole = new UserRoleReply();
            //调用grpc，获取角色信息，
            // userRole = App.GetService<IGrpcClientResolver>().EnsureClient<UserRoleGrpcService.UserRoleGrpcServiceClient>().GetUserRole(new UserRoleRequest {  UserKey = "zzq" });

            //模拟假数据TODO：GRPC调用正常后删除
            userRole = new UserRoleReply();
            userRole.UserRole = 1;

            //根据默认角色获取权限列表
            List<SysMenuPermissionDto> permissionList = new List<SysMenuPermissionDto>();
            var grpcService = App.GetService<ISysMenuService>();
            switch (userRole.UserRole)
            {
                case 1://管理员
                    permissionList = await grpcService.GetMGRPermissionList();
                    break;
                case 2://操作者
                    //permissionList = await grpcService.GetOperatorPermissionList();
                    permissionList = await grpcService.GetOperatorPermissionListByColumnPerimission();
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
