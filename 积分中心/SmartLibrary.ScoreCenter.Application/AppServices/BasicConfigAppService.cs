/*********************************************************
* 名    称：积分配置.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分配置服务
* 更新历史：
*
* *******************************************************/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.ScoreCenter.Application.Dtos.Common;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.AppServices
{
    /// <summary>
    /// 积分配置信息
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class BasicConfigAppService : BaseAppService
    {
        private readonly IBasicConfigService _basicConfigService;

        public BasicConfigAppService(IBasicConfigService basicConfigService)
        {
            _basicConfigService = basicConfigService;
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<BasicConfigEditData> GetConfigSet()
        {
            var configData = await _basicConfigService.GetBasicConfigSet();
            return configData;
        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveConfigSet([FromBody] BasicConfigEditData input)
        {
            var result = await _basicConfigService.SaveConfigSet(input);
            return result;
        }
    }
}
