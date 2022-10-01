/*********************************************************
* 名    称：OrderManageService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：订单管理
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Services.Enum;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.Common.Services;
using SmartLibrary.ScoreCenter.Common.Utils;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderManageService : IOrderManageService, IScoped
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<OrderRecord> _orderRepository;
        private readonly IRepository<GoodsRecord> _goodsRepository;
        private readonly IRedisService _redisService;
        private readonly TenantInfo _tenantInfo;
        private readonly IGrpcClientResolver _grpcClientResover;

        public OrderManageService(IDistributedIDGenerator idGenerator
            , IRepository<OrderRecord> orderRepository
            , IRepository<GoodsRecord> goodsRepository
            , IGrpcClientResolver grpcClientResolver
            , IRedisService redisService
            , TenantInfo tenantInfo)
        {
            _idGenerator = idGenerator;
            _orderRepository = orderRepository;
            _goodsRepository = goodsRepository;
            _tenantInfo = tenantInfo;
            _redisService = redisService;
            _grpcClientResover = grpcClientResolver;
        }

        /// <summary>
        /// 获取初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var initData = new
            {
                RecordData = new OrderRecordDto { },
                Status = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumOrderStatus)),
                ObtainWay = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumObtainWay)),
                GoodsType = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumGoodsType)),
            };
            return await Task.FromResult(initData);
        }

        /// <summary>
        /// 查询订单管理列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<OrderListItemDto>> QueryTableData(OrderManageTableQuery queryFilter)
        {
            var listQuery = from order in _orderRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                            .Where(queryFilter.Status.HasValue, x => x.Status == queryFilter.Status)
                            .Where(queryFilter.ObtainWay.HasValue, x => x.ObtainWay == queryFilter.ObtainWay)
                            .Where(!string.IsNullOrWhiteSpace(queryFilter.UserName), x => x.ExchangeName != null && x.ExchangeName != "" && x.ExchangeName.Contains(queryFilter.UserName))
                            join goods in _goodsRepository.DetachedEntities
                            .Where(queryFilter.GoodsType.HasValue, x => x.Type == queryFilter.GoodsType)
                            .Where(!string.IsNullOrWhiteSpace(queryFilter.GoodsName), x => x.Name.Contains(queryFilter.GoodsName))
                            on order.GoodsID equals goods.Id into goodsStore
                            from goods in goodsStore
                            select new OrderListItemDto
                            {
                                ID = order.Id,
                                GoodsID = order.GoodsID,
                                GoodsName = goods.Name,
                                GoodsType = goods.Type,
                                ExchangeCode = order.ExchangeCode,
                                ExchangeCount = order.ExchangeCount,
                                ExchangeUserKey = order.ExchangeUserKey,
                                ExchangeName = order.ExchangeName,
                                Status = order.Status,
                                ObtainWay = order.ObtainWay,
                                ExchangeTime = order.ExchangeTime,
                                ObtainTime = goods.ObtainTime,
                                CreateTime = order.CreateTime
                            };
            var dataList = await listQuery.OrderByDescending(x => x.Status).ThenByDescending(x => x.CreateTime).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return dataList;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderDetailDto> GetByID(Guid id)
        {
            var orderEntity = await _orderRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (orderEntity == null)
            {
                throw Oops.Oh("未找到订单详情数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var goodsEntity = await _goodsRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == orderEntity.GoodsID);
            var orderData = orderEntity.Adapt<OrderDetailDto>();
            orderData.GoodsName = goodsEntity?.Name;
            orderData.GoodsType = goodsEntity?.Type ?? 1;
            orderData.ExchangeScore = Math.Abs(orderData.ExchangeScore);
            var userClient = _grpcClientResover.EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            var userData = await userClient.GetUserByKeyAsync(new Google.Protobuf.WellKnownTypes.StringValue { Value = orderData.ExchangeUserKey });
            orderData.ExchangeStudentNo = userData?.StudentNo;
            orderData.ExchangeCollegeName = userData?.CollegeName;
            orderData.ExchangeCollegeDepartName = userData?.CollegeDepartName;
            return orderData;
        }

        /// <summary>
        /// 兑换出库
        /// </summary>
        /// <param name="exchangeInput"></param>
        /// <returns></returns>
        public async Task<bool> ExchangeGoods(ExchangeInput exchangeInput)
        {
            var orderEntity = await _orderRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == exchangeInput.ID);
            if (orderEntity == null)
            {
                throw Oops.Oh("未找到订单数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (orderEntity.Status == (int)EnumOrderStatus.已出库)
            {
                throw Oops.Oh("订单已出库，不能重复兑换").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var db = SqlSugarHelper.GetTenantDb(_tenantInfo.Name);
            var operateToken = string.IsNullOrWhiteSpace(exchangeInput.Token) ? _idGenerator.CreateGuid().ToString("N") : exchangeInput.Token;
            var operateKey = $"order_exchange_{operateToken}";
            var repeateCount = await _redisService.StringIncrementAsync(operateKey);
            if (repeateCount > 1)
            {
                throw Oops.Oh("请勿重复提交");
            }
            else
            {
                _redisService.KeyExpire(operateKey, TimeSpan.FromSeconds(10));
                //OperateKey需要设置为唯一约束
                var operateRecord = new OperationRecord { Id = _idGenerator.CreateGuid(), OperateKey = operateKey, TenantId = _tenantInfo.Name };
                //处理业务兑换逻辑
                using (var tran = db.UseTran())
                {
                    try
                    {
                        var orderCount = orderEntity.ExchangeCount;
                        var goodsId = orderEntity.GoodsID;
                        var returnId = await db.Insertable(operateRecord).ExecuteCommandAsync();
                        if (returnId <= 0)
                        {
                            throw Oops.Oh("请勿重复提交");
                        }
                        var orderUpdateCount = await db.Updateable<OrderRecord>()
                            .SetColumns(x => x.Status == (int)EnumOrderStatus.已出库)
                            .SetColumns(x => x.UpdateTime == DateTime.Now)
                            .Where(x => x.Id == exchangeInput.ID && x.Status == (int)EnumOrderStatus.待出库)
                            .ExecuteCommandAsync();
                        if (orderUpdateCount <= 0)
                        {
                            throw Oops.Oh("订单已出库，不能重复兑换");
                        }
                        var goodsUpdateCount = await db.Updateable<GoodsRecord>()
                            .SetColumns(x => x.FreezeCount == x.FreezeCount - orderCount)
                            .SetColumns(x => x.UpdateTime == DateTime.Now)
                            .Where(x => x.Id == goodsId && (x.FreezeCount - orderCount) >= 0)
                            .ExecuteCommandAsync();
                        if (goodsUpdateCount <= 0)
                        {
                            throw Oops.Oh("商品冻结库存量不足");
                        }
                        tran.CommitTran();
                    }
                    catch (AppFriendlyException ex)
                    {
                        throw Oops.Oh(ex.Message).BadRequest();
                    }
                    catch (Exception ex)
                    {
                        tran.RollbackTran();
                        throw new Exception(ex.Message);
                    }
                }
            }
            return true;
        }

    }
}
