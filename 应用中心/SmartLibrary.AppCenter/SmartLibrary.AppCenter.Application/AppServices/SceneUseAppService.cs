using Microsoft.AspNetCore.Mvc;
using SmartLibrary.AppCenter.Application.Dtos.SceneUse;
using SmartLibrary.AppCenter.Application.Services.SceneUse;
using SmartLibrary.AppCenter.Common.BaseService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.AppServices
{
    /// <summary>
    /// 应用中心-前台使用接口
    /// </summary>
    public class SceneUseAppService : BaseAppService
    {
        private readonly ISceneUseService _SceneUseService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sceneUseService"></param>
        public SceneUseAppService(ISceneUseService sceneUseService)
        {
            _SceneUseService = sceneUseService;
        }

        /// <summary>
        /// 根据栏目ID获取应用列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<SceneUseItemDto> GetSceneUseById([FromQuery] SceneUseRequsetParameter parameter)
        {
            return await _SceneUseService.GetSceneUseItemById(parameter);
        }

        /// <summary>
        /// 批量获取栏目信息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SceneUseDto> GetSceneUseByIdBatch([FromBody] List<SceneUseRequsetParameter> parameters)
        {
            return await _SceneUseService.GetSceneUseByIdBatch(parameters);
        }

        /// <summary>
        /// 获取首页应用信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<SceneUseDto> GetAppForIndex()
        {
            return await _SceneUseService.GetAppForIndex();
        }
    }
}
