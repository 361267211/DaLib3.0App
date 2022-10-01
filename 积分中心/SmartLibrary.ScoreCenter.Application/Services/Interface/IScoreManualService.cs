/*********************************************************
* 名    称：IScoreManualService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：手动调整用户积分
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
    /// 手动调整用户积分
    /// </summary>
    public interface IScoreManualService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();

        /// <summary>
        /// 获取手动调整任务
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<ScoreManualListItemDto>> QueryTableData(ScoreManualTableQuery queryFilter);

        /// <summary>
        /// 创建手动调整积分任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Guid> Create(ScoreManualProcessDto input);

        /// <summary>
        /// 通过用户组获取用户列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<SimpleUserListItemDto>> GetUserListByGroups(GroupUserTableQuery queryFilter);
        /// <summary>
        /// 通过用户类型获取用户列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<SimpleUserListItemDto>> GetUserListByUserType(TypeUserTableQuery queryFilter);
        /// <summary>
        /// 通过条件查询用户列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<SimpleUserListItemDto>> GetUserListByConditions(ViewModels.UserTableQuery queryFilter);
    }
}
