using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.AppCenter.Application.Dtos.ApplicationSetting;
using SmartLibrary.AppCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.ApplicationSetting
{
    /// <summary>
    /// 应用中心设置
    /// </summary>
    public class ApplicationSettingService : IApplicationSettingService, IScoped
    {
        private readonly IRepository<AppCenterSettings> _Repository;

        public ApplicationSettingService(IRepository<AppCenterSettings> repository)
        {
            _Repository = repository;
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApplicationSettingDto> GetApplicationSettingAsync()
        {
            var response = new ApplicationSettingDto();

            var settings = await _Repository.DetachedEntities.Where(c => c.DeleteFlag == false && c.ItemName == "BaseConfig").ToListAsync();

            response.IsShowChargeApp = settings.FirstOrDefault(c => c.ItemKey == "IsShowChargeApp")?.ItemValue == "1";
            response.IsNeedLogin = settings.FirstOrDefault(c => c.ItemKey == "IsNeedLogin")?.ItemValue == "1";

            return response;
        }

        /// <summary>
        /// 修改设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateApplicationSettingAsync(ApplicationSettingDto dto)
        {
            var item1 = await _Repository.DetachedEntities.FirstOrDefaultAsync(c => !c.DeleteFlag && c.ItemKey == "IsShowChargeApp");
            var item2 = await _Repository.DetachedEntities.FirstOrDefaultAsync(c => !c.DeleteFlag && c.ItemKey == "IsNeedLogin");

            if (item1 != null)
            {
                item1.UpdatedTime = DateTimeOffset.UtcNow;
                item1.ItemValue = dto.IsShowChargeApp ? "1" : "0";
                await _Repository.UpdateNowAsync(item1);
            }

            if (item2 != null)
            {
                item2.UpdatedTime = DateTimeOffset.UtcNow;
                item2.ItemValue = dto.IsNeedLogin ? "1" : "0";
                await _Repository.UpdateNowAsync(item2);
            }

            return true;
        }
    }
}
