/*********************************************************
* 名    称：IReaderShopMallService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分商城服务
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
    /// 积分商城服务
    /// </summary>
    public interface IReaderShopMallService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();
        /// <summary>
        /// 获取积分商城商品
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<ReaderGoodsListItemDto>> QueryTableData(ReaderGoodsTableQuery queryFilter);
        /// <summary>
        /// 标记用户是否喜欢商品
        /// </summary>
        /// <param name="likeStatus"></param>
        /// <returns></returns>
        Task<bool> MarkLikeStatus(ReaderLikeStatus likeStatus);
        /// <summary>
        /// 获取用户商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FrontGoodsDetailInfo> GetGoodsInfo(Guid id);
        /// <summary>
        /// 用户下单
        /// </summary>
        /// <param name="orderInput"></param>
        /// <returns></returns>
        Task<Guid> CreateOrder(GoodsOrderDto orderInput);
    }
}
