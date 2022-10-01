/*********************************************************
* 名    称：ScoreLevelAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分等级管理
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
    /// 获取积分等级
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class ScoreLevelAppService : BaseAppService
    {
        private readonly IScoreLevelService _scoreLevelService;
        public ScoreLevelAppService(IScoreLevelService scoreLevelService)
        {
            _scoreLevelService = scoreLevelService;
        }


        /// <summary>
        /// 初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var initData = await _scoreLevelService.GetInitData();
            return initData;
        }

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScoreLevelListItemOutput>> QueryListData()
        {
            var pagedList = await _scoreLevelService.QueryListData();
            var targetList = pagedList.Adapt<List<ScoreLevelListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 创建积分等级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Guid> Create([FromBody] ScoreLevelInput input)
        {
            var inputData = input.Adapt<ScoreLevelDto>();
            var result = await _scoreLevelService.Create(inputData);
            return result;
        }

        /// <summary>
        /// 编辑积分等级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Guid> Update([FromBody] ScoreLevelInput input)
        {
            var inputData = input.Adapt<ScoreLevelDto>();
            var result = await _scoreLevelService.Update(inputData);
            return result;
        }

        /// <summary>
        /// 设置积分等级是否显示
        /// </summary>
        /// <param name="showLevel"></param>
        /// <returns></returns>
        public async Task<bool> SetLevelShowStatus([FromBody] ScoreLevelShowStatus showLevel)
        {
            var result = await _scoreLevelService.SetLevelShowStatus(showLevel);
            return result;
        }

        /// <summary>
        /// 删除积分等级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var result = await _scoreLevelService.Delete(id);
            return result;

        }
    }
}
