using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.AppCenter.Application.Dtos.Application;
using SmartLibrary.AppCenter.Application.Dtos.ApplicationSetting;
using SmartLibrary.AppCenter.Application.Dtos.UserApplication;
using SmartLibrary.AppCenter.Application.Services.ApplicationSetting;
using SmartLibrary.AppCenter.Application.Services.UserApplication;
using SmartLibrary.AppCenter.Common.BaseService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.AppServices
{
    /// <summary>
    /// 个人应用中心-前台
    /// </summary>
    [Authorize(Policy = "front")]
    public class UserApplicationAppService : BaseAppService
    {
        private readonly IUserAppService _UserAppService;
        private readonly IApplicationSettingService _ApplicationSettingService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userAppService"></param>
        /// <param name="applicationSettingService"></param>
        public UserApplicationAppService(IUserAppService userAppService,
                                         IApplicationSettingService applicationSettingService)
        {
            _UserAppService = userAppService;
            _ApplicationSettingService = applicationSettingService;
        }

        /// <summary>
        /// 收藏应用
        /// </summary>
        [HttpPost]
        public async Task<bool> CollectionApp(string id)
        {
            return await _UserAppService.CollectionApp(id);
        }

        /// <summary>
        /// 删除收藏应用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> DeleteMyApp(string id)
        {
            return await _UserAppService.DeleteMyApp(id);
        }

        /// <summary>
        /// 获取推荐应用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<RecommendAppDto>> GetRecommendApps()
        {
            return await _UserAppService.GetRecommendApps();
        }

        /// <summary>
        /// 获取推荐应用，带应用中心跳转地址
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<RecommendAppMoreDto> GetRecommendAppMore()
        {
            return await _UserAppService.GetRecommendAppMore();
        }

        /// <summary>
        /// 获取我的收藏应用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<MyCollectionAppDto>> GetMyCollectionApps()
        {
            return await _UserAppService.GetMyCollectionApps();
        }

        /// <summary>
        /// 获取所有应用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AllAppDto>> GetAllApps()
        {
            return await _UserAppService.GetAllApps();
        }

        /// <summary>
        /// 获取应用中心设置，是否显示付费推荐应用和是否需要登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApplicationSettingDto> GetAppCenterSetting()
        {
            return await _ApplicationSettingService.GetApplicationSettingAsync();
        }

        /// <summary>
        /// 根据应用ID获取应用详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AppDetailDto> GetAppDetail(string id)
        {
            return await _UserAppService.GetAppDetail(id);
        }
    }
}
