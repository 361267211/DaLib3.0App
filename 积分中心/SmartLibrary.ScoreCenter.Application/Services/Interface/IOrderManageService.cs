/*********************************************************
* 名    称：IOrderManageService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：订单管理服务
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
    /// 订单管理服务
    /// </summary>
    public interface IOrderManageService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();
        /// <summary>
        /// 查询商品订单详情列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<OrderListItemDto>> QueryTableData(OrderManageTableQuery queryFilter);
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderDetailDto> GetByID(Guid id);
        
        /// <summary>
        /// 商品实际兑换（出库）逻辑
        /// </summary>
        /// <param name="exchangeInput"></param>
        /// <returns></returns>
        Task<bool> ExchangeGoods(ExchangeInput exchangeInput);
    }
}
