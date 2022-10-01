/*********************************************************
* 名    称：OrderManageAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：订单管理页面
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.AppServices
{
    /// <summary>
    /// 订单管理页面
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class OrderManageAppService : BaseAppService
    {
        private readonly IOrderManageService _orderManageService;

        public OrderManageAppService(IOrderManageService orderManageService)
        {
            _orderManageService = orderManageService;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _orderManageService.GetInitData();
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<OrderListItemOutput>> QueryTableData([FromQuery] OrderManageTableQuery queryFilter)
        {
            var pagedList = await _orderManageService.QueryTableData(queryFilter);
            var targetList = pagedList.Adapt<PagedList<OrderListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDetailOutput> GetById(Guid id)
        {
            var orderData = await _orderManageService.GetByID(id);
            var targetData = orderData.Adapt<OrderDetailOutput>();
            return targetData;
        }

        /// <summary>
        /// 商品兑换
        /// </summary>
        /// <param name="exchangeInput"></param>
        /// <returns></returns>
        public async Task<bool> ExchangeGoods([FromBody] ExchangeInput exchangeInput)
        {
            var result = await _orderManageService.ExchangeGoods(exchangeInput);
            return result;
        }
    }
}
