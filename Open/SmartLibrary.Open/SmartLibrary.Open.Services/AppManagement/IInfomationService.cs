using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Services.Dtos.Infomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public interface IInfomationService
    {
        /// <summary>
        /// 查看应用动态
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<AppDynamicInfoViewModel>> QueryAppDynamicTableData(AppInfoTableQuery queryFilter);
        /// <summary>
        /// 查询活动日志消息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<ActivityInfoViewModel>> QueryActivityInfoTableData(AppInfoTableQuery queryFilter);
        /// <summary>
        /// 获取应用动态详情
        /// </summary>
        /// <param name="appDynamicId"></param>
        /// <returns></returns>
        Task<AppDynamicInfoViewModel> GetAppDynamicById(Guid appDynamicId);
        /// <summary>
        /// 活动消息详情
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        Task<ActivityInfoViewModel> GetActivityInfoById(Guid activityId);
        /// <summary>
        /// 新增应用动态
        /// </summary>
        /// <param name="dynamicInfoDto"></param>
        /// <returns></returns>
        Task<Guid> CreateAppDynamicInfo(AppDynamicDto dynamicInfoDto);
        /// <summary>
        /// 新增活动消息
        /// </summary>
        /// <param name="activityInfoDto"></param>
        /// <returns></returns>
        Task<Guid> CreateActivityInfo(ActivityInfoDto activityInfoDto);
        /// <summary>
        /// 更新应用动态
        /// </summary>
        /// <param name="dynamicInfoDto"></param>
        /// <returns></returns>
        Task<Guid> UpdateAppDynamicInfo(AppDynamicDto dynamicInfoDto);
        /// <summary>
        /// 更新活动消息
        /// </summary>
        /// <param name="activityInfoDto"></param>
        /// <returns></returns>
        Task<Guid> UpdateActivityInfo(ActivityInfoDto activityInfoDto);
        /// <summary>
        /// 置顶应用动态
        /// </summary>
        /// <param name="topSetIds"></param>
        /// <returns></returns>
        Task<bool> TopSetAppDynamicInfo(List<Guid> topSetIds);
        /// <summary>
        /// 置顶活动消息
        /// </summary>
        /// <param name="topSetIds"></param>
        /// <returns></returns>
        Task<bool> TopSetActivityInfo(List<Guid> topSetIds);
        /// <summary>
        /// 取消置顶应用动态
        /// </summary>
        /// <param name="dynamicIds"></param>
        /// <returns></returns>
        Task<bool> UntopSetAppDynamicInfo(List<Guid> dynamicIds);
        /// <summary>
        /// 取消置顶活动消息
        /// </summary>
        /// <param name="activityIds"></param>
        /// <returns></returns>
        Task<bool> UntopSetActivityInfo(List<Guid> activityIds);
        /// <summary>
        /// 删除应用动态
        /// </summary>
        /// <param name="delIds"></param>
        /// <returns></returns>
        Task<bool> DeleteAppDynamicInfo(List<Guid> delIds);
        /// <summary>
        /// 删除活动消息
        /// </summary>
        /// <param name="delIds"></param>
        /// <returns></returns>
        Task<bool> DeleteActivityInfo(List<Guid> delIds);
    }
}
