using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Identity.Application.Dtos.Common;
using SmartLibrary.Identity.Application.Dtos.LoginConfig;
using SmartLibrary.Identity.Application.Services.Interface;
using SmartLibrary.Identity.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.AppServices
{
    //登录配置服务
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class LoginConfigAppService : BaseAppService
    {
        private readonly ILoginConfigService _loginConfigService;
        public LoginConfigAppService(ILoginConfigService loginConfigService)
        {
            _loginConfigService = loginConfigService;
        }

        /// <summary>
        /// 获取登录设置列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<LoginConfigSetOutput>> QueryListData()
        {
            var list = await _loginConfigService.QueryListData();
            var targetList = list.Adapt<List<LoginConfigSetOutput>>();
            return targetList;
        }

        /// <summary>
        /// 设置是否开启
        /// </summary>
        /// <param name="openSet"></param>
        /// <returns></returns>
        public async Task<bool> SetIsOpen([FromBody] LoginSetOpenSetInput openSet)
        {
            var openSetDto = openSet.Adapt<LoginSetOpenDto>();
            var result = await _loginConfigService.SetIsOpen(openSetDto);
            return result;
        }
    }
}
