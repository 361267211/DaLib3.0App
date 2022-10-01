/*********************************************************
* 名    称：ReaderShopMallAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分商城
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
    /// 积分商城
    /// </summary>
    [Authorize(Policy = PolicyKey.ReaderAuth)]
    public class ReaderShopMallAppService : BaseAppService
    {
        private readonly IReaderShopMallService _readerShopMallService;

        public ReaderShopMallAppService(IReaderShopMallService readerShopMallService)
        {
            _readerShopMallService = readerShopMallService;
        }
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var result = await _readerShopMallService.GetInitData();
            return result;
        }

        /// <summary>
        /// 查询积分商城商品
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ReaderGoodsListItemOutput>> QueryTableData([FromQuery] ReaderGoodsTableQuery queryFilter)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            queryFilter.UserKey = CurrentUser.UserKey;
            var pagedList = await _readerShopMallService.QueryTableData(queryFilter);
            var targetList = pagedList.Adapt<PagedList<ReaderGoodsListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 标记读者是否喜欢商品
        /// </summary>
        /// <param name="likeStatus"></param>
        /// <returns></returns>
        public async Task<bool> MarkLikeStatus([FromBody] ReaderLikeStatus likeStatus)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            likeStatus.UserKey = CurrentUser.UserKey;
            var result = await _readerShopMallService.MarkLikeStatus(likeStatus);
            return result;
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FrontGoodsDetailInfo> GetGoodsInfo(Guid id)
        {
            var detailInfo = await _readerShopMallService.GetGoodsInfo(id);
            return detailInfo;
        }

        /// <summary>
        /// 用户下单
        /// </summary>
        /// <param name="orderInput"></param>
        /// <returns></returns>
        public async Task<Guid> CreateOrder([FromBody] GoodsOrderInput orderInput)
        {
            var orderDto = orderInput.Adapt<GoodsOrderDto>();
            if (CurrentUser == null)
            {
                throw Oops.Oh("未获取到登录用户信息").StatusCode(Consts.ExceptionStatus);
            }
            orderDto.UserKey = CurrentUser.UserKey;
            orderDto.UserName = CurrentUser.UserName;
            var orderId = await _readerShopMallService.CreateOrder(orderDto);
            return orderId;
        }
    }
}
