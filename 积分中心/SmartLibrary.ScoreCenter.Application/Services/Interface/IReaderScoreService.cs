/*********************************************************
* 名    称：IReaderScoreService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者积分服务
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
    /// 读者积分服务
    /// </summary>
    public interface IReaderScoreService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();

        /// <summary>
        /// 获取用户积分概要信息
        /// </summary>
        /// <returns></returns>
        Task<ReaderScoreInfoDto> GetReaderScoreInfo(string userKey);
        /// <summary>
        /// 获取读者推荐积分任务
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<ReaderScoreObtainTaskDto>> QueryScoreObtainTask(ReaderScoreTaskTableQuery queryFilter);
        /// <summary>
        /// 获取积分排行
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<ScoreRankListItemDto>> QueryScoreRankData(ReaderScoreRankTableQuery queryFilter);
        /// <summary>
        /// 获取积分规则
        /// </summary>
        /// <returns></returns>
        Task<string> GetScoreRule();
        /// <summary>
        /// 获取用户积分明细
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<UserScoreEventListItemDto>> QueryReaderEventScoreTableData(ReaderScoreEventTableQuery queryFilter);
        /// <summary>
        /// 获取商品订单信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<OrderListItemDto>> QueryReaderGoodsOrderTableData(OrderManageTableQuery queryFilter);
        /// <summary>
        /// 获取商品订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderDetailDto> GetReaderGoodsOrderDetail(Guid id);
        /// <summary>
        /// 获取读者积分等级信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<ReaderScoreLevelOutput> GetReaderLevelInfo(string userKey);
        /// <summary>
        /// 获取读者勋章信息
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<ReaderMedalOutput> GetReaderMedalInfo(string userKey);
        /// <summary>
        /// 获取积分场景数据
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        Task<ScoreCenterSceneOutput> QueryScoreCenterSceneData(string userKey);
    }
}
