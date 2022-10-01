/*********************************************************
* 名    称：ISyncCardService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者同步服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 同步读者卡服务
    /// </summary>
    public interface ISyncCardService
    {
        /// <summary>
        /// 获取同步读者卡配置
        /// </summary>
        /// <returns></returns>
        Task<SyncCardConfigItemDto> GetSyncCardConfig(string tenant);
        /// <summary>
        /// 设置读者卡配置
        /// </summary>
        /// <returns></returns>
        Task<bool> SetSyncCardConfig(string tenant, SyncCardConfigItemDto syncConfig);
        /// <summary>
        /// 添加读者同步临时任务
        /// </summary>
        /// <returns></returns>
        Task<bool> SetSyncCardTask(string tenant);
        /// <summary>
        /// 获取同步日志
        /// </summary>
        /// <param name="tenant"></param>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<SyncCardLogListItemDto>> GetSyncCardLogTableData(string tenant, SyncCardLogTableQuery queryFilter);

    }
}
