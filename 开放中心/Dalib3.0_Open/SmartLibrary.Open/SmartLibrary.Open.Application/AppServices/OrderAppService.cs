using SmartLibrary.Open.Services.Dtos;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartLibrary.Open.Common.AssemblyBase;
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.Services.Dtos.Order;
using SmartLibrary.Open.Services;

namespace SmartLibrary.Open.Application.AppServices
{
    /// <summary>
    /// 授权订单管理
    /// </summary>
    public class OrderAppService : BaseAppService
    {

        private readonly IOrderService _orderService;
        public OrderAppService(IOrderService orderSerivce)
        {
            _orderService = orderSerivce;
        }

        /// <summary>
        /// 获取授权订单列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<OrderTableViewModel>> QueryTableData([FromQuery] OrderTableQuery queryFilter)
        {
            var pageList = await _orderService.QueryTableData(queryFilter);
            return pageList;
        }

        /// <summary>
        /// 获取授权订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("{orderId}")]
        public async Task<OrderViewModel> GetById(Guid orderId)
        {
            var orderData = await _orderService.GetById(orderId);
            return orderData;
        }

        /// <summary>
        /// 批量删除授权订单
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public async Task<bool> Delete([FromBody] List<Guid> orderIds)
        {
            var result = await _orderService.Delete(orderIds);
            return result;
        }

        /// <summary>
        /// 批量审核授权订单
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Confirm([FromBody] List<Guid> orderIds)
        {
            var result = await _orderService.Confirm(orderIds);
            return result;
        }

        /// <summary>
        /// 批量拒绝授权订单
        /// </summary>
        /// <param name="rejectDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Reject([FromBody] OrderRejectDto rejectDto)
        {
            var result = await _orderService.Reject(rejectDto);
            return result;
        }

        /// <summary>
        /// 应用授权开通
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        public async Task<Guid> Create([FromBody] OrderDto orderDto)
        {
            var orderId = await _orderService.Create(orderDto);
            return orderId;
        }

        /// <summary>
        /// 应用授权订单编辑
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        public async Task<Guid> Update([FromBody] OrderDto orderDto)
        {
            var orderId = await _orderService.Update(orderDto);
            return orderId;
        }
        /// <summary>
        /// 客户批量应用添加
        /// </summary>
        /// <param name="batchAppOrderDto"></param>
        /// <returns></returns>
        public async Task<bool> CreateBatchAppOrder([FromBody] BatchAppOrderDto batchAppOrderDto)
        {
            var result = await _orderService.CreateBatchAppOrder(batchAppOrderDto);
            return result;
        }
    }
}
