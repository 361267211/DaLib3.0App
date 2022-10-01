using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.AppCenter.Application.Dtos.ApplicationSetting;
using SmartLibrary.AppCenter.Application.Services.ApplicationSetting;
using SmartLibrary.AppCenter.Common.BaseService;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.AppServices
{
    /// <summary>
    /// 应用中心设置
    /// </summary>
    [Authorize(Policy = "back")]
    public class ApplicationSettingAppService : BaseAppService
    {
        private readonly IApplicationSettingService _ApplicationSettingService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="applicationSettingService"></param>
        public ApplicationSettingAppService(IApplicationSettingService applicationSettingService)
        {
            _ApplicationSettingService = applicationSettingService;
        }

        /// <summary>
        /// 获取应用中心设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApplicationSettingDto> GetApplicationSetting()
        {
            return await _ApplicationSettingService.GetApplicationSettingAsync();
        }

        /// <summary>
        /// 修改应用中心设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UpdateApplicationSetting([FromBody] ApplicationSettingDto dto)
        {
            return await _ApplicationSettingService.UpdateApplicationSettingAsync(dto);
        }
    }
}
