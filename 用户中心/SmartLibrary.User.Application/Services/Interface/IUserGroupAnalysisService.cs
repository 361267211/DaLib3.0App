/*********************************************************
* 名    称：IUserGroupAnalysisService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组数据画像分析服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.UserGroup;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 用户组数据画像分析服务
    /// </summary>
    public interface IUserGroupAnalysisService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();
        /// <summary>
        /// 获取访问量简要信息
        /// </summary>
        /// <returns></returns>
        Task<UserGroupVisitBriefDto> GetVisitBreifInfo();
        /// <summary>
        /// 获取访问量图标数据
        /// </summary>
        /// <returns></returns>
        Task<List<ChartDataItemDto>> QueryVisitChartData(int intervalType);
        /// <summary>
        /// 查询热点事件排行
        /// </summary>
        /// <returns></returns>
        Task<PagedList<HotEventListItemDto>> QueryHotEventData(TableQueryBase queryFilter);
        /// <summary>
        /// 数据资源排行
        /// </summary>
        /// <returns></returns>
        Task<List<ResourceRankListItemDto>> QueryResourceRankData();
        /// <summary>
        /// 读者排行
        /// </summary>
        /// <returns></returns>
        Task<List<ReaderRankListItemDto>> QueryReaderRankData();

    }
}
