using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.AppCenter.Application.Dtos.Application;
using SmartLibrary.AppCenter.Application.Services.Common;
using SmartLibrary.AppCenter.Common.BaseService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.AppServices
{
    /// <summary>
    /// 通用接口，获取终端类型，应用类型等
    /// </summary>
    [Authorize(Policy = "back")]
    public class GeneralAppService : BaseAppService
    {
        private readonly IGeneralService _generalService;

        public GeneralAppService(IGeneralService generalService)
        {
            _generalService = generalService;
        }

        /// <summary>
        /// 获取推荐图标
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<List<RecommendIconDto>> GetRecommendIcons()
        {
            var result =
                //Enumerable.Range(1, 18).
                Enumerable.Empty<int>()//注释掉最开始那些
                    .Select(x => new RecommendIconDto
                    {
                        IconUrl = $"/uploads/cqu/app_icons/icon({x}).png",
                        RelativeUrl = $"/uploads/cqu/app_icons/icon({x}).png"
                    })
                .Concat(
                    Enumerable.Range(1, 20).Select(x => new RecommendIconDto
                    {
                        IconUrl = $"/uploads/cqu/app_icons/application default - {x}.svg",
                        RelativeUrl = $"/uploads/cqu/app_icons/application default - {x}.svg"
                    })
                ).ToList();

            return Task.FromResult(result);
        }

        /// <summary>
        /// 获取终端类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DictionaryDto>> GetTerminalType()
        {
            return await _generalService.GetTerminalType();
        }

        /// <summary>
        /// 获取应用类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DictionaryDto>> GetAppType()
        {
            return await _generalService.GetAppType();
        }

        /// <summary>
        /// 获取应用类型 用于前台应用中心
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DictionaryDto>> GetAppTypeForFront()
        {
            var result = new List<DictionaryDto>();
            var typeList = await _generalService.GetAppType();

            foreach (var item in typeList)
            {
                if (item.Name == "资源服务" || item.Name == "学术情报" || item.Name == "读者服务")
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取采购类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DictionaryDto>> GetPurchaseType()
        {
            return await _generalService.GetPurchaseType();
        }

        /// <summary>
        /// 获取用户类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DictionaryDto>> GetUserTypeList()
        {
            return await _generalService.GetUserTypeList();
        }

        /// <summary>
        /// 获取用户分组列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DictionaryDto>> GetUserGroupList()
        {
            return await _generalService.GetUserGroupList();
        }
    }
}
