using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.AppCenter.Application.Services.ApplicationSetting;
using SmartLibrary.AppCenter.Application.Services.Permission;
using SmartLibrary.AppCenter.EntityFramework.Core.Dto.Permission;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.AppServices
{
    /// <summary>
    /// 权限管理接口
    /// </summary>
    public class SysMenuAppService : IDynamicApiController
    {
        private readonly ISysMenuService _SysMenuService;
        private readonly IApplicationSettingService _ApplicationSettingService;

        public SysMenuAppService(ISysMenuService sysMenuService,
                                 IApplicationSettingService applicationSettingService)
        {
            _SysMenuService = sysMenuService;
            _ApplicationSettingService = applicationSettingService;
        }

        /// <summary>
        /// 获取完整的权限-树型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PermissionNode> GetAllPermissionTree()
        {
            return await _SysMenuService.GetAllPermissionTree();
        }


        /// <summary>
        /// 获取登陆用户所有的权限菜单-树型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PermissionNode> GetUserPermissionTree()
        {
            PermissionNode permissionNode = await _SysMenuService.GetUserPermissionTree();

            var settingInfo = await _ApplicationSettingService.GetApplicationSettingAsync();
            if (!settingInfo.IsShowChargeApp)
            {
                permissionNode.PermissionNodes.RemoveAll(c => c.Name == "付费应用推荐");
            }
            return permissionNode;
        }


    }
}
