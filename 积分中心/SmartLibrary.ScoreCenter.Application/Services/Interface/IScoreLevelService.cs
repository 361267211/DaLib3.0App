/*********************************************************
* 名    称：IScoreLevelService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分等级设置
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
    /// 积分等级设置
    /// </summary>
    public interface IScoreLevelService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        Task<List<ScoreLevelListItemDto>> QueryListData();
        /// <summary>
        /// 新增积分等级
        /// </summary>
        /// <param name="scoreLevel"></param>
        /// <returns></returns>
        Task<Guid> Create(ScoreLevelDto scoreLevel);
        /// <summary>
        /// 编辑积分等级
        /// </summary>
        /// <param name="scoreLevel"></param>
        /// <returns></returns>
        Task<Guid> Update(ScoreLevelDto scoreLevel);
        /// <summary>
        /// 删除积分等级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
        /// <summary>
        /// 设置积分等级是否显示
        /// </summary>
        /// <param name="showLevel"></param>
        /// <returns></returns>
        Task<bool> SetLevelShowStatus(ScoreLevelShowStatus showLevel);
    }
}
