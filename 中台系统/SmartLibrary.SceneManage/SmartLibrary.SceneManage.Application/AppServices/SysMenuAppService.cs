/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartLibrary.AppCenter;
using SmartLibrary.DatabaseTerrace.Application.Filter;
using SmartLibrary.SceneManage.Application.Services;
using SmartLibrary.SceneManage.Common;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.EntityFramework.Core.Dtos;
using SmartLibrary.SceneManage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartLibrary.AppCenter.AppCenterGrpcService;

namespace SmartLibrary.SceneManage.Application.AppServices
{
    /// <summary>
    /// 权限管理接口
    /// </summary>
   // [DatabaseActionFilter]
    public class SysMenuAppService : IDynamicApiController
    {

        public IConfiguration _configuration { get; }
        private readonly ISysMenuService _sysMenuService;
        private IHttpContextAccessor _httpContextAccessor;
        private IGrpcClientResolver _grpcClientResolver;

        public SysMenuAppService(IConfiguration configuration,
            ISysMenuService sysMenuService,
            IHttpContextAccessor httpContextAccessor,
            IGrpcClientResolver grpcClientResolver
            )
        {
            _configuration = configuration;
            _sysMenuService = sysMenuService;
            _httpContextAccessor = httpContextAccessor;
            _grpcClientResolver = grpcClientResolver;
        }

        /// <summary>
        /// 获取完整的权限-树型
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [UnitOfWork]
        [AuthorizeMultiplePolicy("DefaultPolicy", false)]
        public async Task<List<PermissionNode>> GetAllPermissionTree()
        {
            return await _sysMenuService.GetAllPermissionTree();
        }

        /// <summary>
        /// 获取角色信息  基础信息+权限列表
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        [HttpGet]

        [UnitOfWork]
        public async Task<SysRoleInfoDto> GetSysRoleInfo(Guid roleId)
        {
            return await _sysMenuService.GetSysRoleInfo(roleId);
        }

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="sysRoleInfoDto"></param>
        /// <returns></returns>
        [HttpPost]

        [UnitOfWork]
        public async Task SaveSysRoleInfo(SysRoleInfoDto sysRoleInfoDto)
        {
            if (sysRoleInfoDto.Id == Guid.Empty)
            {
                await _sysMenuService.InsertSysRoleInfo(sysRoleInfoDto);
            }
            else
            {
                await _sysMenuService.UpdateSysRoleInfo(sysRoleInfoDto);
            }
        }
        /// <summary>
        /// 获取登陆用户所有的权限菜单-列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [UnitOfWork]
        public async Task<List<SysMenuPermissionDto>> GetUserPermissionList()
        {
            return await _sysMenuService.GetMGRPermissionList();
        }

        /// <summary>
        /// 获取登陆用户所有的权限菜单-树型
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [UnitOfWork]
        public async Task<List<PermissionNode>> GetUserPermissionTree()
        {
            var userIdStr = _httpContextAccessor.HttpContext.User.FindFirst(e => e.Type == "UserKey")?.Value;//调用grpc，获取角色信息，
            var userRole = _grpcClientResolver.EnsureClient<AppCenterGrpcServiceClient>(nameof(AppCenterGrpcServiceClient)).GetUserAppPermissionType(new UserAppPermissionTypeRequest { AppId = SiteGlobalConfig.AppBaseConfig.AppRouteCode });

            switch (userRole.PermissionType)
            {
                case 1://管理员
                    return await _sysMenuService.GetAllPermissionTree();
                case 2://操作者
                    return await _sysMenuService.GetOperatorPermissionTree();
                case 3://浏览者
                    return await _sysMenuService.GetVisitorsPermissionTree();
                default:
                    return new List<PermissionNode>(); //没有权限直接返回

            }
        }


    }
}
