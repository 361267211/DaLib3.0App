using Furion;
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.AppCenter;
using SmartLibrary.Assembly.EntityFramework.Core.Enum;
using SmartLibrary.DatabaseTerrace.Application.Interceptors;
using SmartLibrary.DatabaseTerrace.Application.Services;
using SmartLibrary.DatabaseTerrace.Common.Const;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.Dtos;
//using SmartLibrary.User.RpcService;
using SmartLibraryUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.Web.Jwt
{
    /// <summary>
    /// 默认的权限校验规则处理器
    /// </summary>
    public class StaffPremissionHandler : AuthorizationHandler<StaffPremissionRequirement>
    {
        /// <summary>
        /// 重写 Handler 添加自动刷新
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StaffPremissionRequirement requirement)
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

            //2.根据用户信息取他的通用角色  1管理员/2操作员/3浏览者  TODO:暂时伪造数据
            var grpcClient1 = App.GetService<IGrpcClientResolver>().EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>();
            UserAppPermissionTypeRequest request1 = new UserAppPermissionTypeRequest { AppId = "assembly" };
            UserAppPermissionTypeReply reply1 = new UserAppPermissionTypeReply();
            try
            {
                reply1 = await grpcClient1.GetUserAppPermissionTypeAsync(request1);
            }
            catch (Exception)
            {
                // throw Oops.Oh("grpc调用异常").StatusCode(HttpStatusKeys.ExceptionCode);
                reply1 = new UserAppPermissionTypeReply { PermissionType = 1 };
            }


            UserRoleEnum userRoleType = (UserRoleEnum)reply1.PermissionType;



            //根据默认角色获取权限列表
            List<SysMenuPermissionDto> permissionList = new List<SysMenuPermissionDto>();
            var grpcService = App.GetService<ISysMenuService>();
            switch (reply1.PermissionType)
            {
                case 1://管理员
                    permissionList =await grpcService.GetMGRApiPermissionList();
                    break;
                case 2://操作者
                    App.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "2";
                    context.Fail();
                    return;

                    //permissionList = await grpcService.GetOperatorApiPermissionList();
                    //break;
                case 3://浏览者
                    App.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "2";
                    context.Fail();
                    return;
                    //permissionList = await grpcService.GetVisitorsApiPermissionList();
                    //break;
                default:

                    return; //没有权限直接返回

            }
            if (permissionList.Any(e=>e.Permission==permission||permission.StartsWith($"{e.Permission}:")))
            {
                context.Succeed(requirement);
            }
            else
            {
                App.HttpContext.Response.Headers[PolicyKey.UnAuthKey] = "2";
                context.Fail();
                return;
            }


        }

    }
}
