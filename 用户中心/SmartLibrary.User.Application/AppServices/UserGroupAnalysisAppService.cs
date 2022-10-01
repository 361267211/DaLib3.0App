/*********************************************************
* 名    称：UserGroupAnalysisAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：用户组报表分析页面Api,目前都是假数据，需要提供详细需求
* 更新历史：
*
* *******************************************************/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.UserGroup;
using SmartLibrary.User.Application.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class UserGroupAnalysisAppService : BaseAppService
    {
        private readonly IUserGroupAnalysisService _userGroupAnalysisService;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="userAnalysisService"></param>
        public UserGroupAnalysisAppService(IUserGroupAnalysisService userAnalysisService)
        {
            _userGroupAnalysisService = userAnalysisService;
        }

        /// <summary>
        /// 获取初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _userGroupAnalysisService.GetInitData();
        }

        /// <summary>
        /// 获取访问量简要信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserGroupVisitBriefDto> GetVisitBriefInfo()
        {
            return await _userGroupAnalysisService.GetVisitBreifInfo();
        }

        /// <summary>
        /// 查询访问图表
        /// </summary>
        /// <param name="intervalType"></param>
        /// <returns></returns>
        public async Task<List<ChartDataItemDto>> QueryVisitChartData(int intervalType)
        {
            return await _userGroupAnalysisService.QueryVisitChartData(intervalType);
        }

        /// <summary>
        /// 查询热点事件
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<HotEventListItemDto>> QueryHotEventData([FromQuery] TableQueryBase queryFilter)
        {
            return await _userGroupAnalysisService.QueryHotEventData(queryFilter);
        }
        /// <summary>
        /// 数据资源排行
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResourceRankListItemDto>> QueryResourceRankData()
        {
            return await _userGroupAnalysisService.QueryResourceRankData();
        }
        /// <summary>
        /// 读者排行
        /// </summary>
        /// <returns></returns>
        public async Task<List<ReaderRankListItemDto>> QueryReaderRankData()
        {
            return await _userGroupAnalysisService.QueryReaderRankData();
        }


    }
}
