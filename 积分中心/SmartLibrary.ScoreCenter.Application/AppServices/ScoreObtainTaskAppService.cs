/*********************************************************
* 名    称：ScoreObtainTaskAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分获取任务配置
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
    /// 获取积分任务配置
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class ScoreObtainTaskAppService : BaseAppService
    {
        private readonly IScoreObtainTaskService _scoreObtainTaskService;
        public ScoreObtainTaskAppService(IScoreObtainTaskService scoreObtainTaskService)
        {
            _scoreObtainTaskService = scoreObtainTaskService;
        }
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var initData = await _scoreObtainTaskService.GetInitData();
            return initData;
        }

        /// <summary>
        /// 通过应用编码获取积分事件
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetScoreEventsByAppCode(string appCode)
        {
            var result = await _scoreObtainTaskService.GetScoreEventsByAppCode(appCode);
            return result;
        }

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreObtainListItemOutput>> QueryListData()
        {
            var dataList = await _scoreObtainTaskService.QueryListData();
            var targetList = dataList.Adapt<List<ScoreObtainListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取积分获取任务数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ScoreObtainTaskDto> GetByID(Guid Id)
        {
            var taskData = await _scoreObtainTaskService.GetByID(Id);
            return taskData;
        }

        /// <summary>
        /// 创建积分任务
        /// </summary>
        /// <param name="scoreObtainInput"></param>
        /// <returns></returns>
        public async Task<Guid> Create([FromBody] ScoreObtainTaskInput scoreObtainInput)
        {
            var taskData = scoreObtainInput.Adapt<ScoreObtainTaskDto>();
            var result = await _scoreObtainTaskService.Create(taskData);
            return result;
        }

        /// <summary>
        /// 更新积分任务
        /// </summary>
        /// <param name="scoreObtainInput"></param>
        /// <returns></returns>
        public async Task<Guid> Update([FromBody] ScoreObtainTaskInput scoreObtainInput)
        {
            var taskData = scoreObtainInput.Adapt<ScoreObtainTaskDto>();
            var result = await _scoreObtainTaskService.Update(taskData);
            return result;
        }

        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="statusSet"></param>
        /// <returns></returns>
        public async Task<bool> SetActiveStatus([FromBody] ScoreObtainTaskStatusSet statusSet)
        {
            var result = await _scoreObtainTaskService.SetActiveStatus(statusSet);
            return result;
        }

        /// <summary>
        /// 删除积分任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var result = await _scoreObtainTaskService.Delete(id);
            return result;
        }
    }
}
