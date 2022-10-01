/*********************************************************
* 名    称：SceneAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分场景数据接口
* 更新历史：
*
* *******************************************************/
using Microsoft.AspNetCore.Authorization;
using SmartLibrary.ScoreCenter.Application.Dtos.Common;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.AppServices
{
    /// <summary>
    /// 积分场景
    /// </summary>
    [Authorize(Policy = PolicyKey.TokenAuth)]
    public class SceneAppService : BaseAppService
    {
        private readonly IReaderScoreService _readerScoreService;

        public SceneAppService(IReaderScoreService readerScoreService)
        {
            _readerScoreService = readerScoreService;
        }

        /// <summary>
        /// 获取积分场景数据
        /// </summary>
        /// <returns></returns>
        public async Task<ScoreCenterSceneOutput> QueryScoreCenterSceneData()
        {
            if (CurrentUser == null)
            {
                return new ScoreCenterSceneOutput();
            }
            var result = await _readerScoreService.QueryScoreCenterSceneData(CurrentUser.UserKey);
            return await Task.FromResult(result);
        }
    }
}
