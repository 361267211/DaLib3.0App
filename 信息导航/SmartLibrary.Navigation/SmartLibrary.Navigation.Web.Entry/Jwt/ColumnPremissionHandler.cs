using Furion;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.Navigation.Application.Interceptors;
using SmartLibrary.Navigation.Application.Services;
using SmartLibrary.Navigation.Common.Const;
using SmartLibrary.Navigation.EntityFramework.Core.Dto.Permission;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Web.Jwt
{
    /// <summary>
    /// 默认的权限校验规则处理器
    /// </summary>
    public class ColumnPremissionHandler : AuthorizationHandler<ColumnPremissionRequirement>
    {
        /// <summary>
        /// 重写 Handler 添加自动刷新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ColumnPremissionRequirement requirement)
        {
            //截取路由
            var permission = App.HttpContext.Request.Path.Value[1..].Replace("/", ":");
            var userIdStr = "";
            if (context.User != null && context.User.HasClaim(x => x.Type == "UserKey"))
            {
                userIdStr = context.User.Claims.FirstOrDefault(x => x.Type == "UserKey")?.Value;
            }

            if (string.IsNullOrEmpty(userIdStr))
            {
                App.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "1";
                context.Fail();
                return;
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
            if (permissionList.Any(e => Regex.Replace(e.Permission, @"[^(\x21-\x7E)]", "") == permission))
            {
                context.Succeed(requirement);
            }


        }

    }
}
