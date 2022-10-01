/*********************************************************
* 名    称：MedalObtainTaskAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：勋章获取任务配置
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
    /// 勋章获取任务配置
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class MedalObtainTaskAppService : BaseAppService
    {
        private readonly IMedalObtainTaskService _medalObtainTaskService;

        public MedalObtainTaskAppService(IMedalObtainTaskService medalObtainTaskService)
        {
            _medalObtainTaskService = medalObtainTaskService;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var initData = await _medalObtainTaskService.GetInitData();
            return initData;
        }

        /// <summary>
        /// 通过应用编码获取勋章事件
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetMedalEventsByAppCode(string appCode)
        {
            var result = await _medalObtainTaskService.GetMedalEventsByAppCode(appCode);
            return result;
        }

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<MedalObtainListItemOutput>> QueryListData()
        {
            var dataList = await _medalObtainTaskService.QueryListData();
            var targetList = dataList.Adapt<List<MedalObtainListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取勋章获取任务数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<MedalObtainTaskDto> GetByID(Guid Id)
        {
            var taskData = await _medalObtainTaskService.GetByID(Id);
            return taskData;
        }

        /// <summary>
        /// 创建勋章任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Guid> Create([FromBody] MedalObtainTaskInput input)
        {
            var taskData = input.Adapt<MedalObtainTaskDto>();
            var result = await _medalObtainTaskService.Create(taskData);
            return result;
        }

        /// <summary>
        /// 修改勋章任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Guid> Update([FromBody] MedalObtainTaskInput input)
        {
            var taskData = input.Adapt<MedalObtainTaskDto>();
            var result = await _medalObtainTaskService.Update(taskData);
            return result;
        }

        /// <summary>
        /// 设置激活状态
        /// </summary>
        /// <param name="statusSet"></param>
        /// <returns></returns>
        public async Task<bool> SetActiveStatus([FromBody] MedalObtainTaskStatusSet statusSet)
        {
            var result = await _medalObtainTaskService.SetActiveStatus(statusSet);
            return result;
        }

        /// <summary>
        /// 删除勋章任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var result = await _medalObtainTaskService.Delete(id);
            return result;
        }

    }
}
