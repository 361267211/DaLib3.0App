using SmartLibrary.AppCenter.Application.Dtos.SceneUse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Services.SceneUse
{
    /// <summary>
    /// 应用中心-前台接口
    /// </summary>
    public interface ISceneUseService
    {
        /// <summary>
        /// 根据栏目ID获取应用列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<SceneUseItemDto> GetSceneUseItemById(SceneUseRequsetParameter parameter);

        /// <summary>
        /// 批量获取栏目内容
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<SceneUseDto> GetSceneUseByIdBatch(List<SceneUseRequsetParameter> parameters);
        
        /// <summary>
        /// 获取首页应用
        /// </summary>
        /// <returns></returns>
        Task<SceneUseDto> GetAppForIndex();
    }
}
