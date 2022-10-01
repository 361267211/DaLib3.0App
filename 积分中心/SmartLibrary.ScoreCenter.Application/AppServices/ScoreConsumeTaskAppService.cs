/*********************************************************
* 名    称：ScoreConsumeTaskAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分消费任务配置
* 更新历史：
*
* *******************************************************/
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Dtos.Common;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.AppServices
{
    /// <summary>
    /// 获取积分消费任务配置
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class ScoreConsumeTaskAppService : BaseAppService
    {
        private readonly IScoreConsumeTaskService _scoreConsumeTaskService;
        public ScoreConsumeTaskAppService(IScoreConsumeTaskService scoreConsumeTaskService)
        {
            _scoreConsumeTaskService = scoreConsumeTaskService;
        }

        /// <summary>
        /// 获取初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var initData = await _scoreConsumeTaskService.GetInitData();
            return initData;
        }

        /// <summary>
        /// 通过应用编码获取消费积分事件
        /// </summary>
        public async Task<Dictionary<string, string>> GetConsumeEventsByAppCode(string appCode)
        {
            var result = await _scoreConsumeTaskService.GetConsumeEventsByAppCode(appCode);
            return result;
        }

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreConsumeListItemOutput>> QueryListData()
        {
            var dataList = await _scoreConsumeTaskService.QueryListData();
            var targetList = dataList.Adapt<List<ScoreConsumeListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取积分消费任务数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ScoreConsumeTaskDto> GetByID(Guid Id)
        {
            var taskData = await _scoreConsumeTaskService.GetByID(Id);
            return taskData;
        }

        /// <summary>
        /// 创建积分消费任务
        /// </summary>
        /// <param name="scoreConsumeInput"></param>
        /// <returns></returns>
        public async Task<Guid> Create([FromBody] ScoreConsumeTaskInput scoreConsumeInput)
        {
            var taskData = scoreConsumeInput.Adapt<ScoreConsumeTaskDto>();
            var result = await _scoreConsumeTaskService.Create(taskData);
            return result;
        }

        /// <summary>
        /// 更新积分消费任务
        /// </summary>
        /// <param name="scoreConsumeInput"></param>
        /// <returns></returns>
        public async Task<Guid> Update([FromBody] ScoreConsumeTaskInput scoreConsumeInput)
        {
            var taskData = scoreConsumeInput.Adapt<ScoreConsumeTaskDto>();
            var result = await _scoreConsumeTaskService.Update(taskData);
            return result;
        }

        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="statusSet"></param>
        /// <returns></returns>
        public async Task<bool> SetActiveStatus([FromBody] ScoreConsumeTaskStatus statusSet)
        {
            var result = await _scoreConsumeTaskService.SetActiveStatus(statusSet);
            return result;
        }

        /// <summary>
        /// 删除积分消费任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var result = await _scoreConsumeTaskService.Delete(id);
            return result;
        }
    }
}
