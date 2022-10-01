using SmartLibrary.AppCenter.Application.Dtos.ApplicationSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.ApplicationSetting
{
    public interface IApplicationSettingService
    {
        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        Task<ApplicationSettingDto> GetApplicationSettingAsync();

        /// <summary>
        /// 修改设置
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateApplicationSettingAsync(ApplicationSettingDto dto);
    }
}
