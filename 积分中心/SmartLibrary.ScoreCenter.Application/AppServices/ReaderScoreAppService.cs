/*********************************************************
* 名    称：ReaderScoreAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者积分服务
* 更新历史：
*
* *******************************************************/
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Dtos.Common;
using SmartLibrary.ScoreCenter.Application.Services.Consts;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.AppServices
{
    /// <summary>
    /// 读者积分服务
    /// </summary>
    [Authorize(Policy = PolicyKey.ReaderAuth)]
    public class ReaderScoreAppService : BaseAppService
    {
        private readonly IReaderScoreService _readerScoreService;

        public ReaderScoreAppService(IReaderScoreService readerScoreService)
        {
            _readerScoreService = readerScoreService;
        }
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _readerScoreService.GetInitData();
        }

        /// <summary>
        /// 获取读者积分信息
        /// </summary>
        /// <returns></returns>
        public async Task<ReaderScoreInfoOutput> GetReaderScoreInfo()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            var scoreInfoData = await _readerScoreService.GetReaderScoreInfo(CurrentUser.UserKey);
            var targetInfoData = scoreInfoData.Adapt<ReaderScoreInfoOutput>();
            targetInfoData.Name = CurrentUser.UserName;
            targetInfoData.Photo = CurrentUser.UserPhoto;
            return targetInfoData;
        }

        /// <summary>
        /// 查询读者积分任务
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ReaderScoreObtainTask>> QueryTableList([FromQuery] ReaderScoreTaskTableQuery queryFilter)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            queryFilter.UserKey = CurrentUser.UserKey;
            var pagedList = await _readerScoreService.QueryScoreObtainTask(queryFilter);
            var targetList = pagedList.Adapt<PagedList<ReaderScoreObtainTask>>();
            return targetList;
        }

        /// <summary>
        /// 查询积分排行
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ScoreRankListItemOutput>> QueryScoreRankData([FromQuery] ReaderScoreRankTableQuery queryFilter)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            var pagedList = await _readerScoreService.QueryScoreRankData(queryFilter);
            var targetList = pagedList.Adapt<PagedList<ScoreRankListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取积分规则
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetScoreRule()
        {
            var result = await _readerScoreService.GetScoreRule();
            return result;
        }

        /// <summary>
        /// 获取用户积分明细
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<UserScoreEventListItemOutput>> QueryReaderEventScoreTableData([FromQuery] ReaderScoreEventTableQuery queryFilter)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            queryFilter.UserKey = CurrentUser.UserKey;
            var pagedList = await _readerScoreService.QueryReaderEventScoreTableData(queryFilter);
            var targetList = pagedList.Adapt<PagedList<UserScoreEventListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取读者商品订单信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<OrderListItemOutput>> QueryReaderGoodsOrderTableData([FromQuery] OrderManageTableQuery queryFilter)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            queryFilter.UserKey = CurrentUser.UserKey;
            var pagedList = await _readerScoreService.QueryReaderGoodsOrderTableData(queryFilter);
            var targetList = pagedList.Adapt<PagedList<OrderListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDetailOutput> GetReaderGoodsOrderDetail(Guid id)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").BadRequest();
            }
            var orderData = await _readerScoreService.GetReaderGoodsOrderDetail(id);
            var targetData = orderData.Adapt<OrderDetailOutput>();
            return targetData;
        }

        /// <summary>
        /// 获取读者积分等级信息
        /// </summary>
        /// <returns></returns>
        public async Task<ReaderScoreLevelOutput> GetReaderLevelInfo()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            var result = await _readerScoreService.GetReaderLevelInfo(CurrentUser.UserKey);
            return result;
        }

        /// <summary>
        /// 获取读者勋章信息
        /// </summary>
        /// <returns></returns>
        public async Task<ReaderMedalOutput> GetReaderMedalInfo()
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            var result = await _readerScoreService.GetReaderMedalInfo(CurrentUser.UserKey);
            return result;
        }

        

        ///// <summary>
        ///// 获取积分场景数据
        ///// </summary>
        ///// <param name="queryFilter"></param>
        ///// <returns></returns>
        //public async Task<ScoreCenterSceneOutput> QueryScoreCenterSceneData()
        //{
        //    if (CurrentUser == null)
        //    {
        //        throw Oops.Oh("未找到登录用户信息").BadRequest();
        //    }
        //    var result = await _readerScoreService.QueryScoreCenterSceneData(CurrentUser.UserKey);
        //    return await Task.FromResult(result);
        //}

       
    }
}
