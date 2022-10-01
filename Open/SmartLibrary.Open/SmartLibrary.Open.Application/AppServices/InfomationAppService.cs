/*********************************************************
* 名    称：InfomationPublishAppService.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210910
* 描    述：应用消息管理服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.Services.Dtos.Infomation;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 应用消息发布服务
    /// </summary>
    public class InfomationAppService : BaseAppService
    {
        /// <summary>
        /// 查询应用动态
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<PagedList<AppDynamicInfoViewModel>> QueryAppDynamicTableData([FromQuery] AppInfoTableQuery queryFilter)
        {
            return Task.FromResult(new PagedList<AppDynamicInfoViewModel>());
        }

        /// <summary>
        /// 查询活动消息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public Task<TableQueryResult<ActivityInfoViewModel>> QueryActivityInfoTableData([FromQuery] AppInfoTableQuery queryFilter)
        {
            return Task.FromResult(new TableQueryResult<ActivityInfoViewModel>());
        }

        /// <summary>
        /// 获取应用动态详情
        /// </summary>
        /// <param name="appDynamicId"></param>
        /// <returns></returns>
        [Route("[action]/{appDynamicId}")]
        public Task<AppDynamicInfoViewModel> GetAppDynamicById(Guid appDynamicId)
        {
            return Task.FromResult(new AppDynamicInfoViewModel());
        }

        /// <summary>
        /// 获取活动消息详情
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [Route("[action]/{activityId}")]
        public Task<ActivityInfoViewModel> GetActivityInfoById(Guid activityId)
        {
            return Task.FromResult(new ActivityInfoViewModel());
        }

        /// <summary>
        /// 添加应用动态
        /// </summary>
        /// <param name="dynamicInfoDto"></param>
        /// <returns></returns>
        public Task<Guid> CreateAppDynamicInfo([FromBody] AppDynamicDto dynamicInfoDto)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        /// <summary>
        /// 添加活动信息
        /// </summary>
        /// <param name="activityInfoDto"></param>
        /// <returns></returns>
        public Task<Guid> CreateActivityInfo([FromBody] ActivityInfoDto activityInfoDto)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        /// <summary>
        /// 更新应用动态
        /// </summary>
        /// <param name="dynamicInfoDto"></param>
        /// <returns></returns>
        public Task<Guid> UpdateAppDynamicInfo([FromBody] AppDynamicDto dynamicInfoDto)
        {
            return Task.FromResult(Guid.NewGuid());
        }
        /// <summary>
        /// 更新活动消息
        /// </summary>
        /// <param name="activityInfoDto"></param>
        /// <returns></returns>
        public Task<Guid> UpdateActivityInfo([FromBody] ActivityInfoDto activityInfoDto)
        {
            return Task.FromResult(Guid.NewGuid());
        }

        /// <summary>
        /// 置顶应用动态
        /// </summary>
        /// <param name="topSetIds"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<bool> TopSetAppDynamicInfo([FromBody] List<Guid> topSetIds)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 置顶活动消息
        /// </summary>
        /// <param name="topSetIds"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<bool> TopSetActivityInfo([FromBody] List<Guid> topSetIds)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 取消置顶应用动态
        /// </summary>
        /// <param name="dynamicIds"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<bool> UntopSetAppDynamicInfo([FromBody] List<Guid> dynamicIds)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 取消置顶活动消息
        /// </summary>
        /// <param name="activityIds"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<bool> UntopSetActivityInfo([FromBody] List<Guid> activityIds)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 删除应用动态
        /// </summary>
        /// <param name="delIds"></param>
        /// <returns></returns>
        public Task<bool> DeleteAppDynamicInfo([FromBody] List<Guid> delIds)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 删除活动消息
        /// </summary>
        /// <param name="delIds"></param>
        /// <returns></returns>
        public Task<bool> DeleteActivityInfo([FromBody] List<Guid> delIds)
        {
            return Task.FromResult(true);
        }
    }
}
