/*********************************************************
* 名    称：IMedalObtainTaskService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：勋章获取任务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Interface
{
    /// <summary>
    /// 勋章获取任务
    /// </summary>
    public interface IMedalObtainTaskService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();
        /// <summary>
        /// 通过应用编码获取勋章事件
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetMedalEventsByAppCode(string appCode);
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        Task<List<MedalObtainListItemDto>> QueryListData();
        /// <summary>
        /// 获取勋章任务详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MedalObtainTaskDto> GetByID(Guid id);

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="medalTask"></param>
        /// <returns></returns>
        Task<Guid> Create(MedalObtainTaskDto medalTask);
        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="medalTask"></param>
        /// <returns></returns>
        Task<Guid> Update(MedalObtainTaskDto medalTask);
        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="statusSet"></param>
        /// <returns></returns>
        Task<bool> SetActiveStatus(MedalObtainTaskStatusSet statusSet);
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
