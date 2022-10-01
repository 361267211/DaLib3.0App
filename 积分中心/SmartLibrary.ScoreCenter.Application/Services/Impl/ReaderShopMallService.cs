/*********************************************************
* 名    称：ReaderShopMallService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分商城服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartLibrary.ScoreCenter.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Furion.FriendlyException;
using SmartLibrary.ScoreCenter.Common.Utils;
using Furion.DistributedIDGenerator;
using SmartLibrary.ScoreCenter.Common.Services;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using SmartLibrary.ScoreCenter.Application.Services.Enum;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 积分商城服务
    /// </summary>
    public class ReaderShopMallService : IReaderShopMallService, IScoped
    {
        private readonly IRepository<GoodsRecord> _goodsRepository;
        private readonly IRepository<OrderRecord> _orderRepository;
        private readonly IRepository<UserScore> _userScoreRepository;
        private readonly IRepository<UserGoodsPrefer> _userPreferRepository;
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRedisService _redisService;
        private readonly TenantInfo _tenantInfo;

        public ReaderShopMallService(IRepository<GoodsRecord> goodsRepository
            , IRepository<UserScore> userScoreRepository
            , IRepository<OrderRecord> orderRepository
            , IRepository<UserGoodsPrefer> userPreferRepository
            , IDistributedIDGenerator idGenerator
            , IRedisService redisService
            , TenantInfo tenantInfo)
        {
            _goodsRepository = goodsRepository;
            _orderRepository = orderRepository;
            _userPreferRepository = userPreferRepository;
            _idGenerator = idGenerator;
            _redisService = redisService;
            _tenantInfo = tenantInfo;
            _userScoreRepository = userScoreRepository;
        }


        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var initData = new
            {
                GoodsType = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumGoodsType)),
                GoodsScoreRange = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumGoodsScoreRange)),
            };
            return await Task.FromResult(initData);
        }

        /// <summary>
        /// 查询商品信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ReaderGoodsListItemDto>> QueryTableData(ReaderGoodsTableQuery queryFilter)
        {
            int? rangeStartScore = null;
            int? rangeEndScore = null;
            switch (queryFilter.ScoreRange)
            {
                case 0:
                    break;
                case 1:
                    rangeStartScore = 0;
                    rangeEndScore = 500;
                    break;
                case 2:
                    rangeStartScore = 501;
                    rangeEndScore = 2000;
                    break;
                case 3:
                    rangeStartScore = 2001;
                    rangeEndScore = 5000;
                    break;
                case 4:
                    rangeStartScore = 5001;
                    rangeEndScore = 10000;
                    break;
                case 5:
                    rangeStartScore = 10001;
                    rangeEndScore = null;
                    break;
                default:
                    break;
            }
            var nowDate = DateTime.Now.Date;
            var goodsQuery = from goods in _goodsRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumGoodsStatus.上架)
                             .Where(queryFilter.GoodsType.HasValue, x => x.Type == queryFilter.GoodsType)
                             .Where(rangeStartScore.HasValue, x => x.Score >= rangeStartScore)
                             .Where(rangeEndScore.HasValue, x => x.Score <= rangeEndScore)
                             .Where(x => x.BeginDate == null || x.BeginDate <= nowDate)
                             .Where(x => x.EndDate == null || x.EndDate >= nowDate)
                             select new ReaderGoodsListItemDto
                             {
                                 ID = goods.Id,
                                 Name = goods.Name,
                                 IntroPicUrl = goods.IntroPicUrl,
                                 Score = goods.Score,
                                 CurrentCount = goods.CurrentCount,
                                 ExchangeCount = goods.SaleOutCount,
                                 Like = _userPreferRepository.DetachedEntities.Any(x => !x.DeleteFlag && x.GoodsID == goods.Id && x.UserKey == queryFilter.UserKey),
                                 CreateTime = goods.CreateTime
                             };
            var orderByConditions = new Dictionary<string, string>();
            if (queryFilter.Like)
            {
                orderByConditions.Add("Like", "descend");
            }
            if (queryFilter.HotExchange)
            {
                orderByConditions.Add("ExchangeCount", "descend");
            }
            if (queryFilter.LatestTime)
            {
                orderByConditions.Add("CreateTime", "descend");
            }
            if (!orderByConditions.Any())
            {
                orderByConditions.Add("CreateTime", "descend");
            }
            goodsQuery = goodsQuery.ApplyMultipleOrder(orderByConditions);
            var targetList = await goodsQuery.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            foreach (var item in targetList.Items)
            {
                item.IntroPicUrl = (!string.IsNullOrWhiteSpace(item.IntroPicUrl) && !item.IntroPicUrl.StartsWith("/")) ? $"/{item.IntroPicUrl}" : item.IntroPicUrl;
            }
            return targetList;
        }

        /// <summary>
        /// 标记是否喜欢
        /// </summary>
        /// <param name="likeStatus"></param>
        /// <returns></returns>
        public async Task<bool> MarkLikeStatus(ReaderLikeStatus likeStatus)
        {
            if (likeStatus.Like)
            {
                var preferEntity = await _userPreferRepository.FirstOrDefaultAsync(x => x.UserKey == likeStatus.UserKey && x.GoodsID == likeStatus.GoodsID);
                if (preferEntity == null)
                {
                    await _userPreferRepository.InsertAsync(new UserGoodsPrefer { UserKey = likeStatus.UserKey, GoodsID = likeStatus.GoodsID });
                }
                else
                {
                    preferEntity.DeleteFlag = false;
                    preferEntity.UpdateTime = DateTime.Now;
                    await _userPreferRepository.UpdateAsync(preferEntity);
                }
            }
            else
            {
                await _goodsRepository.Context.BatchUpdate<UserGoodsPrefer>()
                    .Set(x => x.DeleteFlag, x => true)
                    .Where(x => x.DeleteFlag == false && x.UserKey == likeStatus.UserKey && x.GoodsID == likeStatus.GoodsID)
                    .ExecuteAsync();
            }
            return true;
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FrontGoodsDetailInfo> GetGoodsInfo(Guid id)
        {
            var goodsEntity = await _goodsRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == id);
            if (goodsEntity == null)
            {
                throw Oops.Oh("未找到商品信息").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var detailInfo = new FrontGoodsDetailInfo
            {
                ID = goodsEntity.Id,
                Name = goodsEntity.Name,
                IntroPicUrl = (!string.IsNullOrWhiteSpace(goodsEntity.IntroPicUrl) && !goodsEntity.IntroPicUrl.StartsWith("/")) ? $"/{goodsEntity.IntroPicUrl}" : goodsEntity.IntroPicUrl,
                Score = goodsEntity.Score,
                CurrentCount = goodsEntity.CurrentCount,
                ExchangeStartTime = goodsEntity.BeginDate,
                ExchangeEndTime = goodsEntity.EndDate,
                ObtainTime = goodsEntity.ObtainTime,
                DetailInfo = goodsEntity.DetailInfo
            };
            return detailInfo;
        }

        /// <summary>
        /// 读者兑换下单
        /// </summary>
        /// <param name="orderInput"></param>
        /// <returns></returns>
        public async Task<Guid> CreateOrder(GoodsOrderDto orderInput)
        {
            var orderId = _idGenerator.CreateGuid();
            var goodsEntity = await _goodsRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == orderInput.GoodsID);
            if (goodsEntity == null)
            {
                throw Oops.Oh("未找到商品详情数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (goodsEntity.ObtainWay == (int)EnumObtainWay.邮寄 && string.IsNullOrWhiteSpace(orderInput.RecieveAddress))
            {
                throw Oops.Oh("请填写收货地址").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var exchangeCount = orderInput.Count;
            var exchangeScore = orderInput.Count * goodsEntity.Score;
            var userScoreEntity = await _userScoreRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserKey == orderInput.UserKey);
            var userScore = userScoreEntity?.AvailableScore ?? 0;
            //该判断在并发操作时会失效，只能当做是做一次预检测
            if (exchangeScore > 0 && exchangeScore > userScore)
            {
                throw Oops.Oh("读者积分不足").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (exchangeCount > goodsEntity.CurrentCount)
            {
                throw Oops.Oh("商品库存不足").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var db = SqlSugarHelper.GetTenantDb(_tenantInfo.Name);
            var operateToken = string.IsNullOrWhiteSpace(orderInput.Token) ? _idGenerator.CreateGuid().ToString("N") : orderInput.Token;
            var operateKey = $"goods_order_{operateToken}";
            var repeateCount = await _redisService.StringIncrementAsync(operateKey);
            if (repeateCount > 1)
            {
                throw Oops.Oh("请勿重复提交").StatusCode(Consts.Consts.ExceptionStatus);
            }
            else
            {
                _redisService.KeyExpire(operateKey, TimeSpan.FromSeconds(10));
                //OperateKey需要设置为唯一约束
                var operateRecord = new OperationRecord { Id = _idGenerator.CreateGuid(), OperateKey = operateKey, TenantId = _tenantInfo.Name };
                //处理业务逻辑
                using (var tran = db.UseTran())
                {
                    try
                    {
                        var returnId = await db.Insertable(operateRecord).ExecuteCommandAsync();
                        if (returnId <= 0)
                        {
                            throw Oops.Oh("请勿重复提交").StatusCode(Consts.Consts.ExceptionStatus);
                        }
                        //创建订单
                        var order = new OrderRecord
                        {
                            Id = orderId,
                            GoodsID = orderInput.GoodsID,
                            ExchangeCode = _idGenerator.CreateGuid().ToString("N"),
                            ExchangeCount = exchangeCount,
                            ExchangeScore = exchangeScore,
                            ExchangeUserKey = orderInput.UserKey,
                            ExchangeName = orderInput.UserName,
                            Status = (int)EnumOrderStatus.待出库,
                            Express = "",
                            ExpressNo = "",
                            ObtainWay = goodsEntity.ObtainWay,
                            ExchangeTime = DateTime.Now,
                            ObtainTime = goodsEntity.ObtainTime,
                            RecieveAddrss = orderInput.RecieveAddress
                        };
                        await db.Insertable(order).ExecuteCommandAsync();
                        //用户积分扣减明细
                        var scoreEvent = new UserEventScore
                        {
                            Id = _idGenerator.CreateGuid(),
                            AppCode = "scorecenter",
                            EventCode = "ExchangeOrder",
                            FullEventCode = "scorecenter:ExchangeOrder",
                            EventName = $"{goodsEntity.Name}兑换",
                            Type = -1,
                            EventScore = -1 * exchangeScore,
                            UserKey = orderInput.UserKey,
                            TriggerTime = DateTime.Now,
                            TenantId = _tenantInfo.Name,
                        };
                        await db.Insertable(scoreEvent).ExecuteCommandAsync();
                        //读者积分扣减
                        var userScoreUpdateCount = await db.Updateable<UserScore>()
                        .SetColumns(x => x.AvailableScore == x.AvailableScore - exchangeScore)
                        .Where(x => !x.DeleteFlag && x.UserKey == orderInput.UserKey && x.AvailableScore >= exchangeScore)
                        .ExecuteCommandAsync();
                        if (exchangeScore > 0 && userScoreUpdateCount <= 0)
                        {
                            throw Oops.Oh("读者积分不足").StatusCode(Consts.Consts.ExceptionStatus);
                        }
                        //商品库存扣减
                        var goodsUpdateCount = await db.Updateable<GoodsRecord>()
                        .SetColumns(x => x.CurrentCount == x.CurrentCount - exchangeCount)
                        .SetColumns(x => x.FreezeCount == x.FreezeCount + exchangeCount)
                        .SetColumns(x => x.SaleOutCount == x.SaleOutCount + exchangeCount)
                        .Where(x => !x.DeleteFlag && x.Id == orderInput.GoodsID && x.CurrentCount >= exchangeCount)
                        .ExecuteCommandAsync();
                        if (goodsUpdateCount <= 0)
                        {
                            throw Oops.Oh("商品库存不足").StatusCode(Consts.Consts.ExceptionStatus);
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
            return orderId;
        }
    }
}
