using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Identity.Application.Dtos.Common;
using SmartLibrary.Identity.Application.Dtos.RegisterConfig;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.AppServices
{
    /// <summary>
    /// 用户注册配置
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class RegisterConfigAppService : BaseAppService
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        private readonly IRegisterConfigService _registerConfigService;

        public RegisterConfigAppService(IRegisterConfigService registerConfigService)
        {
            _registerConfigService = registerConfigService;
        }

        /// <summary>
        /// 获取注册配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<RegisterConfigSetOutput> GetDetailInfo()
        {
            var registerConfig = await _registerConfigService.GetDetailInfo();
            var targetConfig = registerConfig.Adapt<RegisterConfigSetOutput>();
            return targetConfig;
        }

        /// <summary>
        /// 保存注册配置
        /// </summary>
        /// <param name="configSet"></param>
        /// <returns></returns>
        public async Task<bool> Save([FromBody] RegisterConfigSetInput configSet)
        {
            var configSetDto = configSet.Adapt<RegisterConfigSetDto>();
            var result = await _registerConfigService.Save(configSetDto);
            return result;
        }
    }
}
