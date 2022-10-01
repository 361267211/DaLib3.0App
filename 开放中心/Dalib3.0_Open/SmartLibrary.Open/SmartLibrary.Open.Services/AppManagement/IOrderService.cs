/*********************************************************
* 名    称：IOrderService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20211104
* 描    述：订单管理接口
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Services.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// 查询订单列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<OrderTableViewModel>> QueryTableData(OrderTableQuery queryFilter);
        /// <summary>
        /// 批量删除授权订单
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        Task<bool> Delete(List<Guid> orderIds);
        /// <summary>
        /// 批量取消订单
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        Task<bool> Cancel(List<Guid> orderIds);
        /// <summary>
        /// 批量审核授权订单
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        Task<bool> Confirm(List<Guid> orderIds);
        /// <summary>
        /// 拒绝授权订单
        /// </summary>
        /// <param name="rejectDto"></param>
        /// <returns></returns>
        Task<bool> Reject(OrderRejectDto rejectDto);
        /// <summary>
        /// 获取授权订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<OrderViewModel> GetById(Guid orderId);
        /// <summary>
        /// 订单开通
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        Task<Guid> Create(OrderDto orderDto);
        /// <summary>
        /// 判断类似订单是否存在
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        Task<bool> IsExist(OrderDto orderDto);
        /// <summary>
        /// 订单编辑
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        Task<Guid> Update(OrderDto orderDto);
        /// <summary>
        /// 批量添加授权订单
        /// </summary>
        /// <param name="batchAppOrderDto"></param>
        /// <returns></returns>
        Task<bool> CreateBatchAppOrder(BatchAppOrderDto batchAppOrderDto);
    }
}
