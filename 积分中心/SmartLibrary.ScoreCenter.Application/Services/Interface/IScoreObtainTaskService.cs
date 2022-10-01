/*********************************************************
* 名    称：IScoreObtainTaskService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分获取任务
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
    /// 积分获取任务
    /// </summary>
    public interface IScoreObtainTaskService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();

        /// <summary>
        /// 通过应用编码获取积分事件
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetScoreEventsByAppCode(string appCode);
        /// <summary>
        /// 获取积分任务列表
        /// </summary>
        /// <returns></returns>
        Task<List<ScoreObtainListItemDto>> QueryListData();
        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ScoreObtainTaskDto> GetByID(Guid Id);
        /// <summary>
        /// 新增积分
        /// </summary>
        /// <param name="scoreTask"></param>
        /// <returns></returns>
        Task<Guid> Create(ScoreObtainTaskDto scoreTask);
        /// <summary>
        /// 编辑积分任务
        /// </summary>
        /// <param name="scoreTask"></param>
        /// <returns></returns>
        Task<Guid> Update(ScoreObtainTaskDto scoreTask);
        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="statusSet"></param>
        /// <returns></returns>
        Task<bool> SetActiveStatus(ScoreObtainTaskStatusSet statusSet);
        /// <summary>
        /// 删除积分任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
