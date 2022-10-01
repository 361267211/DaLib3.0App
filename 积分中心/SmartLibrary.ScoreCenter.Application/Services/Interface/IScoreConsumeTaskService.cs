/*********************************************************
* 名    称：IScoreConsumeTaskService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分扣减任务
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
    /// 积分扣减任务
    /// </summary>
    public interface IScoreConsumeTaskService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();
        /// <summary>
        /// 通过应用编码获取积分消费事件
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetConsumeEventsByAppCode(string appCode);
        /// <summary>
        /// 获取积分消费事件列表
        /// </summary>
        /// <returns></returns>
        Task<List<ScoreConsumeListItemDto>> QueryListData();
        /// <summary>
        /// 获取积分消费事件详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ScoreConsumeTaskDto> GetByID(Guid id);
        /// <summary>
        /// 创建消费任务
        /// </summary>
        /// <param name="consumeTask"></param>
        /// <returns></returns>
        Task<Guid> Create(ScoreConsumeTaskDto consumeTask);
        /// <summary>
        /// 编辑消费任务
        /// </summary>
        /// <param name="consumeTask"></param>
        /// <returns></returns>
        Task<Guid> Update(ScoreConsumeTaskDto consumeTask);
        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="statusSet"></param>
        /// <returns></returns>
        Task<bool> SetActiveStatus(ScoreConsumeTaskStatus statusSet);
        /// <summary>
        /// 删除积分任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
