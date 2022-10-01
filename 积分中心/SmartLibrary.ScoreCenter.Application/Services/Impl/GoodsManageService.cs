/*********************************************************
* 名    称：GoodsManageService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品管理服务
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
using Microsoft.Extensions.Caching.Distributed;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Services.Enum;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.Common.Services;
using SmartLibrary.ScoreCenter.Common.Utils;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 商品管理
    /// </summary>
    public class GoodsManageService : IGoodsManageService, IScoped
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<GoodsRecord> _goodsRecordRepository;
        private readonly IRedisService _redisService;
        private readonly TenantInfo _tenantInfo;
        private readonly ITenantDistributedCache _distributedCache;

        public GoodsManageService(IDistributedIDGenerator idGenerator
            , IRepository<GoodsRecord> goodsRecordRepository
            , IRedisService redisService
            , TenantInfo tenantInfo
            , ITenantDistributedCache distributedCache)
        {
            _idGenerator = idGenerator;
            _goodsRecordRepository = goodsRecordRepository;
            _redisService = redisService;
            _tenantInfo = tenantInfo;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            //获取所有应用
            var initData = new
            {
                goodsData = new GoodsRecordDto
                {
                    ID = _idGenerator.CreateGuid(),
                },
                GoodsType = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumGoodsType)),
                ObtainWay = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumObtainWay)),
                Status = EnumHelper.GetEnumDescDictionaryItems(typeof(EnumGoodsStatus))
            };
            return await Task.FromResult(initData);
        }

        /// <summary>
        /// 获取商品列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<GoodsListItemDto>> QueryTableData(GoodsManageTableQuery queryFilter)
        {
            var listQuery = _goodsRecordRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                .Where(!string.IsNullOrWhiteSpace(queryFilter.Name), x => x.Name != null && x.Name != "" && x.Name.Contains(queryFilter.Name))
                .Where(queryFilter.Type.HasValue, x => x.Type == queryFilter.Type)
                .Where(queryFilter.Status.HasValue, x => x.Status == queryFilter.Status)
                .ProjectToType<GoodsListItemDto>();
            var dataList = await listQuery.OrderByDescending(x => x.Status).ThenByDescending(x => x.CreateTime).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            foreach (var x in dataList.Items)
            {
                x.IntroPicUrl = (!string.IsNullOrWhiteSpace(x.IntroPicUrl) && !x.IntroPicUrl.StartsWith("/")) ? $"/{x.IntroPicUrl}" : x.IntroPicUrl;
            }
            return dataList;
        }


        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoodsRecordDto> GetByID(Guid id)
        {
            var goodsEntity = await _goodsRecordRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (goodsEntity == null)
            {
                throw Oops.Oh("未找到商品详情数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var goodsData = goodsEntity.Adapt<GoodsRecordDto>();
            goodsData.IntroPicUrl = (!string.IsNullOrWhiteSpace(goodsData.IntroPicUrl) && !goodsData.IntroPicUrl.StartsWith("/")) ? $"/{goodsData.IntroPicUrl}" : goodsData.IntroPicUrl;
            return goodsData;
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="goodsRecord"></param>
        /// <returns></returns>
        public async Task<Guid> Create(GoodsRecordDto goodsRecord)
        {
            goodsRecord.ID = _idGenerator.CreateGuid();
            var goodsEntity = goodsRecord.Adapt<GoodsRecord>();
            var entityEntry = await _goodsRecordRepository.InsertAsync(goodsEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="goodsRecord"></param>
        /// <returns></returns>
        public async Task<Guid> Update(GoodsRecordDto goodsRecord)
        {
            var updateBuilder = _goodsRecordRepository.Context.BatchUpdate<GoodsRecord>();
            updateBuilder
            .Set(x => x.Name, x => goodsRecord.Name)
            .Set(x => x.Type, x => goodsRecord.Type)
            .Set(x => x.Score, x => goodsRecord.Score)
            .Set(x => x.IntroPicUrl, x => goodsRecord.IntroPicUrl)
            .Set(x => x.ObtainWay, x => goodsRecord.ObtainWay)
            .Set(x => x.ObtainAddress, x => goodsRecord.ObtainAddress)
            .Set(x => x.ObtainContact, x => goodsRecord.ObtainContact)
            .Set(x => x.BeginDate, x => goodsRecord.BeginDate)
            .Set(x => x.EndDate, x => goodsRecord.EndDate)
            .Set(x => x.DetailInfo, x => goodsRecord.DetailInfo)
            .Set(x => x.ObtainTime, x => goodsRecord.ObtainTime)
            .Where(x => x.Id == goodsRecord.ID);
            await updateBuilder.ExecuteAsync();
            return goodsRecord.ID;
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var goodsEntity = await _goodsRecordRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (goodsEntity == null)
            {
                throw Oops.Oh("未找到商品详情数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            goodsEntity.DeleteFlag = true;
            goodsEntity.UpdateTime = DateTime.Now;
            await _goodsRecordRepository.UpdateAsync(goodsEntity);
            return true;
        }

        /// <summary>
        /// 设置商品状态
        /// </summary>
        /// <param name="goodsStatus"></param>
        /// <returns></returns>
        public async Task<bool> SetGoodsStatus(SetGoodsStatus goodsStatus)
        {
            var goodsEntity = await _goodsRecordRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == goodsStatus.ID);
            if (goodsEntity == null)
            {
                throw Oops.Oh("未找到商品详情数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            goodsEntity.Status = goodsStatus.Status;
            goodsEntity.UpdateTime = DateTime.Now;
            await _goodsRecordRepository.UpdateAsync(goodsEntity);
            return true;
        }

        /// <summary>
        /// 批量设置上下架状态
        /// </summary>
        /// <param name="goodsStatus"></param>
        /// <returns></returns>
        public async Task<bool> BatchSetGoodsStatus(BatchSetGoodsStatus goodsStatus)
        {
            var updateResult = await _goodsRecordRepository.Context.BatchUpdate<GoodsRecord>()
                .Set(x => x.Status, x => goodsStatus.Status)
                .Set(x => x.UpdateTime, x => DateTime.Now)
                .Where(x => !x.DeleteFlag && goodsStatus.IdList.Contains(x.Id))
                .ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 设置商品库存增减
        /// </summary>
        /// <param name="goodsInventory"></param>
        /// <returns></returns>
        public async Task<bool> SetGoodsStoreCount(ChangeGoodsInventory goodsInventory)
        {
            var goodsEntity = await _goodsRecordRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == goodsInventory.ID);
            if (goodsEntity == null)
            {
                throw Oops.Oh("未找到商品详情数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var db = SqlSugarHelper.GetTenantDb(_tenantInfo.Name);
            var operateToken = string.IsNullOrWhiteSpace(goodsInventory.Token) ? _idGenerator.CreateGuid().ToString("N") : goodsInventory.Token;
            var operateKey = $"goods_store_{operateToken}";
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
                        var count = goodsInventory.Count;
                        if (goodsInventory.OperateType > 0)
                        {
                            //加库存
                            var updateCount = await db.Updateable<GoodsRecord>()
                                .SetColumns(x => x.CurrentCount == x.CurrentCount + count)
                                .SetColumns(x => x.TotalCount == x.TotalCount + count)
                                .Where(x => x.Id == goodsInventory.ID)
                                .ExecuteCommandAsync();
                        }
                        else
                        {
                            //减库存
                            var updateCount = await db.Updateable<GoodsRecord>()
                                .SetColumns(x => x.CurrentCount == x.CurrentCount - count)
                                .SetColumns(x => x.TotalCount == x.TotalCount - count)
                                .Where(x => x.Id == goodsInventory.ID && x.CurrentCount >= count)
                                .ExecuteCommandAsync();
                            if (updateCount <= 0)
                            {
                                throw Oops.Oh("库存不足以用于扣减操作").StatusCode(Consts.Consts.ExceptionStatus);
                            }
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

        /// <summary>
        /// 检查数据是否有效
        /// </summary>
        /// <param name="goodsList"></param>
        /// <returns></returns>
        public async Task CheckGoodsTempData(List<GoodsRecordDto> goodsList)
        {
            foreach (var item in goodsList)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    item.ErrMsg += "名称必填;";
                }
                if (item.Type < 0)
                {
                    item.ErrMsg += "类型填写错误";
                }
                if (item.TotalCount < 0)
                {
                    item.ErrMsg += "数量填写错误";
                }
                if (item.Score < 0)
                {
                    item.ErrMsg += "积分填写错误";
                }
                if (item.ObtainWay < 0)
                {
                    item.ErrMsg += "领取方式填写错误";
                }
                if (item.ObtainWay > 0 && string.IsNullOrWhiteSpace(item.ObtainAddress))
                {
                    item.ErrMsg += "领取地址必填";
                }
                if (item.ObtainWay > 0 && string.IsNullOrWhiteSpace(item.ObtainContact))
                {
                    item.ErrMsg += "联系方式必填";
                }
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 暂存商品数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goodsList"></param>
        /// <returns></returns>
        public async Task StoreTempGoods(Guid id, List<GoodsRecordDto> goodsList)
        {
            await _distributedCache.SetAsync<List<GoodsRecordDto>>(id.ToString(), goodsList, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
        }

        /// <summary>
        /// 获取待导入数据
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public async Task<List<GoodsRecordDto>> GetStoreTempGoods(Guid batchId)
        {
            var goods = await _distributedCache.GetAsync<List<GoodsRecordDto>>(batchId.ToString());
            if (goods == null)
            {
                throw Oops.Oh("未找到待导入数据").StatusCode(Consts.Consts.ExceptionStatus);
            }
            return goods;
        }

        /// <summary>
        /// 确认导入
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public async Task<(int, int)> ImportGoodsConfirm(Guid batchId)
        {
            var goods = await _distributedCache.GetAsync<List<GoodsRecordDto>>(batchId.ToString());
            if (goods == null)
            {
                throw Oops.Oh("未找到待导入数据").StatusCode(Consts.Consts.ExceptionStatus);
            }

            var insertGoods = goods.Where(x => string.IsNullOrWhiteSpace(x.ErrMsg)).ToList();
            var ignoreGoods = goods.Where(x => !string.IsNullOrWhiteSpace(x.ErrMsg)).ToList();
            var insertEntities = insertGoods.Select(x => new GoodsRecord
            {
                Id = _idGenerator.CreateGuid(),
                Name = x.Name,
                Type = x.Type,
                TotalCount = x.TotalCount,
                CurrentCount = x.CurrentCount,
                FreezeCount = x.FreezeCount,
                SaleOutCount = 0,
                Score = x.Score,
                ObtainWay = x.ObtainWay,
                ObtainAddress = x.ObtainAddress,
                ObtainContact = x.ObtainContact,
                BeginDate = x.BeginDate,
                EndDate = x.EndDate,
                Status = x.Status,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                DeleteFlag = false
            });
            await _goodsRecordRepository.Context.BulkInsertAsync(insertEntities);
            return new(insertGoods.Count, ignoreGoods.Count);
        }
    }
}
