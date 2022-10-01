using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Open.Common;
using SmartLibrary.Open.Common.Const;
using SmartLibrary.Open.Common.Extensions;
using SmartLibrary.Open.EntityFramework.Core.Entitys;
using SmartLibrary.Open.Services.AppManagement.Redis;
using SmartLibrary.Open.Services.Dtos;
using SmartLibrary.Open.Services.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderService : IOrderService, IScoped
    {
        public const string OrderNoGenerateKey = "OrderNoGenerateKey";
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<MicroApplication> _appRepository;
        private readonly IRepository<Developer> _devRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<OrderNoSeed> _orderNoSeedRepository;
        private readonly IRedisService _redisService;
        private readonly TenantInfo _tenantInfo;

        public OrderService(IRepository<Order> orderRepository,
                            IRepository<MicroApplication> appRepository,
                            IRepository<Developer> devRepository,
                            IRepository<Customer> customerRepository,
                            IDistributedIDGenerator idGenerator,
                            IRedisService redisService,
                            IRepository<OrderNoSeed> orderNoSeedRepository,
                            TenantInfo tenantInfo)
        {
            _orderRepository = orderRepository;
            _appRepository = appRepository;
            _devRepository = devRepository;
            _customerRepository = customerRepository;
            _idGenerator = idGenerator;
            _redisService = redisService;
            _orderNoSeedRepository = orderNoSeedRepository;
            _tenantInfo = tenantInfo;
        }

        /// <summary>
        /// 订单列表数据查询
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<OrderTableViewModel>> QueryTableData(OrderTableQuery queryFilter)
        {
            var orderQuery = from order in _orderRepository.DetachedEntities.Where(x => !x.DeleteFlag &&
                                  (queryFilter.Status == -99 || x.Status == queryFilter.Status))
                             join customer in _customerRepository.DetachedEntities.Where(x => !x.DeleteFlag &&
                                  (string.IsNullOrWhiteSpace(queryFilter.CustomerOwner)) || x.Owner == queryFilter.CustomerOwner)
                                  on order.CustomerId equals customer.Id.ToString() into customers
                             from customer in customers
                             join app in _appRepository.DetachedEntities.Where(x => !x.DeleteFlag &&
                                    (string.IsNullOrWhiteSpace(queryFilter.Keyword)) || x.Name.Contains(queryFilter.Keyword) || x.Intro.Contains(queryFilter.Keyword))
                                  on order.AppId equals app.Id.ToString() into apps
                             from app in apps
                             join dev in _devRepository.DetachedEntities.Where(x => !x.DeleteFlag &&
                                    (string.IsNullOrWhiteSpace(queryFilter.Keyword) || x.Name.Contains(queryFilter.Keyword)))
                                  on app.DevId equals dev.Id.ToString() into devs
                             from dev in devs
                             select new OrderTableViewModel
                             {
                                 ID = order.Id,
                                 No = order.No,
                                 AppID = app.Id,
                                 AppName = app.Name,
                                 DevID = dev.Id,
                                 DevName = dev.Name,
                                 CustomerID = customer.Id,
                                 CustomerName = customer.Name,
                                 AuthType = order.AuthType,
                                 OpenType = order.OpenType,
                                 Status = order.Status,
                                 Way = order.Way,
                                 BeginDate = order.BeginDate,
                                 ExpireDate = order.ExpireDate,
                                 CommitDate = order.CreatedTime,
                                 ContactMan = order.ContactMan,
                                 Phone = order.ContactPhone,
                                 Remark = order.Remark
                             };
            var tableResult = await orderQuery.OrderByDescending(x => x.BeginDate)
                                              .ThenByDescending(x => x.ID)
                                              .ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            var orderSort = 1 + (queryFilter.PageIndex - 1) * queryFilter.PageSize;
            foreach (var item in tableResult.Items)
            {
                item.SortNo = orderSort;
                orderSort++;
            }
            return tableResult;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderViewModel> GetById(Guid orderId)
        {
            var orderEntity = await _orderRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == orderId);
            if (orderEntity == null)
            {
                throw Oops.Oh("未找到订单数据");
            }
            var orderData = orderEntity.Adapt<OrderViewModel>();
            var customerId = new Guid(orderEntity.CustomerId);
            var customer = await _customerRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == customerId);
            var appId = new Guid(orderEntity.AppId);
            var app = await _appRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == appId);
            orderData.CustomerName = customer != null ? customer.Name : orderData.CustomerName;
            orderData.AppName = app != null ? app.Name : orderData.AppName;
            return orderData;
        }

        /// <summary>
        /// 订单审批通过
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public async Task<bool> Confirm(List<Guid> orderIds)
        {
            var updateBuilder = _orderRepository.Context.BatchUpdate<Order>();
            await updateBuilder.Set(s => s.Status, s => (int)EnumOrderStatus.正常)
                                .Set(s => s.UpdatedTime, s => DateTime.Now)
                                .Where(x => !x.DeleteFlag && x.Status == (int)EnumOrderStatus.待审核 && orderIds.Contains(x.Id))
                                .ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 拒绝授权订单
        /// </summary>
        /// <param name="rejectDto"></param>
        /// <returns></returns>
        public async Task<bool> Reject(OrderRejectDto rejectDto)
        {
            var orderEntity = await _orderRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == rejectDto.OrderID);
            if (orderEntity == null)
            {
                throw Oops.Oh("未找到订单数据");
            }
            orderEntity.Status = (int)EnumOrderStatus.驳回;
            orderEntity.Remark = rejectDto.Reason;
            orderEntity.UpdatedTime = DateTime.Now;
            await _orderRepository.UpdateAsync(orderEntity);
            return true;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        public async Task<Guid> Create(OrderDto orderDto)
        {
            var orderEntity = orderDto.Adapt<Order>();
            orderEntity.Id = _idGenerator.CreateGuid(orderDto.Id);
            orderEntity.Status = (int)EnumOrderStatus.待审核;
            orderEntity.CreatedTime = DateTimeOffset.Now;
            orderEntity.UpdatedTime = DateTimeOffset.Now;
            var orderNo = GenerateOrderNo(1).FirstOrDefault();
            orderEntity.No = orderNo;
            var orderEntry = await _orderRepository.InsertNowAsync(orderEntity);

            return orderEntry.Entity.Id;
        }

        /// <summary>
        /// 判断类型订单是否存在
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        public async Task<bool> IsExist(OrderDto orderDto)
        {
            var isExist = await _orderRepository.DetachedEntities.AnyAsync(c => !c.DeleteFlag && c.Status == (int)EnumOrderStatus.待审核
                              && c.AppId == orderDto.AppID.ToString() && c.CustomerId == orderDto.CustomerID.ToString()
                              && c.OpenType == orderDto.OpenType && c.AuthType == orderDto.AuthType);
            return isExist;
        }

        /// <summary>
        /// 批量创建订单
        /// </summary>
        /// <param name="batchAppOrderDto"></param>
        /// <returns></returns>
        public async Task<bool> CreateBatchAppOrder(BatchAppOrderDto batchAppOrderDto)
        {
            var apps = await _appRepository.DetachedEntities.Where(x => batchAppOrderDto.AppIdList.Contains(x.Id)).ToListAsync();
            var orderNos = GenerateOrderNo(batchAppOrderDto.AppIdList.Count).OrderBy(x => x).ToArray();
            var orderIndex = 0;
            var batchOrders = batchAppOrderDto.AppIdList.Select(x =>
            {
                var mapApp = apps.FirstOrDefault(a => a.Id == x);
                var order = new Order
                {
                    Id = _idGenerator.CreateGuid(),
                    No = orderNos[orderIndex++],
                    AppId = x.ToString(),
                    AppName = mapApp?.Name,
                    CustomerId = batchAppOrderDto.CustomerID.ToString(),
                    AuthType = batchAppOrderDto.AuthType,
                    OpenType = batchAppOrderDto.OpenType,
                    Status = (int)EnumOrderStatus.正常,
                    Way = batchAppOrderDto.Way,
                    BeginDate = batchAppOrderDto.BeginDate,
                    ExpireDate = batchAppOrderDto.ExpireDate,
                    ApplyMan = batchAppOrderDto.ApplyMan,
                    Remark = batchAppOrderDto.Remark,

                };
                return order;
            }).ToList();
            await _orderRepository.UpdateAsync(batchOrders);
            return true;
        }

        /// <summary>
        /// 编辑订单数据
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns></returns>
        public async Task<Guid> Update(OrderDto orderDto)
        {
            var orderEntity = await _orderRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == orderDto.Id);
            if (orderEntity == null)
            {
                throw Oops.Oh("未找到订单数据");
            }
            if (orderEntity.ExpireDate < DateTime.Now)
            {
                throw Oops.Oh("订单已过期，不能修改");
            }
            orderEntity = orderDto.Adapt(orderEntity);
            await _orderRepository.UpdateAsync(orderEntity);
            return orderEntity.Id;
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public async Task<bool> Delete(List<Guid> orderIds)
        {
            var updateBuilder = _orderRepository.Context.BatchUpdate<Order>();
            await updateBuilder.Set(s => s.DeleteFlag, s => true)
                                .Set(s => s.UpdatedTime, s => DateTimeOffset.Now)
                                .Where(x => !x.DeleteFlag && orderIds.Contains(x.Id))
                                .ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public async Task<bool> Cancel(List<Guid> orderIds)
        {
            var updateBuilder = _orderRepository.Context.BatchUpdate<Order>();
            await updateBuilder.Set(s => s.Status, s => (int)EnumOrderStatus.取消)
                               .Set(s => s.UpdatedTime, s => DateTimeOffset.Now)
                               .Where(x => !x.DeleteFlag && x.Status == (int)EnumOrderStatus.待审核 && orderIds.Contains(x.Id))
                               .ExecuteAsync();
            return true;
        }


        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private List<string> GenerateOrderNo(int num)
        {
            var token = Guid.NewGuid().ToString();
            var orderNos = new List<string>();
            try
            {
                if (_redisService.LockTabke(OrderNoGenerateKey, token, TimeSpan.FromSeconds(60)))
                {
                    var seedValue = 0;
                    var todaySeed = _orderNoSeedRepository.DetachedEntities.FirstOrDefault(x => x.SeedDate == DateTime.Now.Date);
                    if (todaySeed == null)
                    {
                        seedValue = 0;
                    }
                    else
                    {
                        seedValue = todaySeed.SeedValue;
                    }
                    for (var i = 0; i < num; i++)
                    {
                        var orderNo = $"{DateTime.Now.Date.ToString("yyyyMMdd")}{(++seedValue).ToString().PadLeft(4, '0')}";
                        orderNos.Add(orderNo);
                    }
                    if (todaySeed == null)
                    {
                        todaySeed = new OrderNoSeed
                        {
                            Id = _idGenerator.CreateGuid(),
                            SeedDate = DateTime.Now.Date,
                            SeedValue = seedValue,
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now,
                        };
                        _orderNoSeedRepository.InsertNow(todaySeed);
                    }
                    else
                    {
                        todaySeed.SeedValue = seedValue;
                        _orderNoSeedRepository.UpdateNow(todaySeed);
                    }
                }

            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message).StatusCode(Const.ExceptionCode);
            }
            finally
            {
                _redisService.LockRelease(OrderNoGenerateKey, token);
            }
            return orderNos;
        }
    }
}
