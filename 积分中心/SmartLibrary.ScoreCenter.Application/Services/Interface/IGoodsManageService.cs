/*********************************************************
* 名    称：IGoodsManageService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：商品管理服务
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
    /// 商品管理服务
    /// </summary>
    public interface IGoodsManageService
    {
        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        Task<object> GetInitData();
        /// <summary>
        /// 获取商品表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        Task<PagedList<GoodsListItemDto>> QueryTableData(GoodsManageTableQuery queryFilter);
        /// <summary>
        /// 获取商品详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GoodsRecordDto> GetByID(Guid id);
        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="goodsRecord"></param>
        /// <returns></returns>
        Task<Guid> Create(GoodsRecordDto goodsRecord);
        /// <summary>
        /// 修改商品，不修改库存数据
        /// </summary>
        /// <param name="goodsRecord"></param>
        /// <returns></returns>
        Task<Guid> Update(GoodsRecordDto goodsRecord);
        /// <summary>
        /// 设置商品上下架状态
        /// </summary>
        /// <param name="goodsStatus"></param>
        /// <returns></returns>
        Task<bool> SetGoodsStatus(SetGoodsStatus goodsStatus);
        /// <summary>
        /// 批量设置上下架状态
        /// </summary>
        /// <param name="goodsStatus"></param>
        /// <returns></returns>
        Task<bool> BatchSetGoodsStatus(BatchSetGoodsStatus goodsStatus);
        /// <summary>
        /// 商品增减库存量
        /// </summary>
        /// <param name="goodsInventory"></param>
        /// <returns></returns>
        Task<bool> SetGoodsStoreCount(ChangeGoodsInventory goodsInventory);

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// 检查商品导入数据
        /// </summary>
        /// <param name="goodsList"></param>
        /// <returns></returns>
        Task CheckGoodsTempData(List<GoodsRecordDto> goodsList);

        /// <summary>
        /// 获取待导入数据
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        Task<List<GoodsRecordDto>> GetStoreTempGoods(Guid batchId);

        /// <summary>
        /// 暂存商品数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goodsList"></param>
        /// <returns></returns>
        Task StoreTempGoods(Guid id, List<GoodsRecordDto> goodsList);

        /// <summary>
        /// 导入确认，只导入没有错误的数据
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        Task<(int, int)> ImportGoodsConfirm(Guid batchId);
    }
}
