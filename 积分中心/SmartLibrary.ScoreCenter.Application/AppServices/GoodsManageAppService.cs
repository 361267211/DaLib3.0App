/*********************************************************
* 名    称：GoodsManageAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分商城管理
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SmartLibrary.ScoreCenter.Application.Dtos;
using SmartLibrary.ScoreCenter.Application.Dtos.Common;
using SmartLibrary.ScoreCenter.Application.Services.Enum;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.Common.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.AppServices
{
    /// <summary>
    /// 积分商城管理
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class GoodsManageAppService : BaseAppService
    {
        private readonly IGoodsManageService _goodsManageService;
        private readonly IDistributedIDGenerator _idGenerator;

        public GoodsManageAppService(IGoodsManageService goodsManageService
            , IDistributedIDGenerator idGenerator)
        {
            _goodsManageService = goodsManageService;
            _idGenerator = idGenerator;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _goodsManageService.GetInitData();
        }

        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <returns></returns>
        public async Task<PagedList<GoodsListItemOutput>> QueryTableData([FromQuery] GoodsManageTableQuery queryFilter)
        {
            var pagedList = await _goodsManageService.QueryTableData(queryFilter);
            var targetList = pagedList.Adapt<PagedList<GoodsListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 查询商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GoodsRecordDto> GetByID(Guid id)
        {
            var goodsData = await _goodsManageService.GetByID(id);
            return goodsData;
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Guid> Create([FromBody] GoodsRecordInput input)
        {
            var inputData = input.Adapt<GoodsRecordDto>();
            var result = await _goodsManageService.Create(inputData);
            return result;

        }

        /// <summary>
        /// 修改商品信息，不能修改数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Guid> Update([FromBody] GoodsRecordInput input)
        {
            var inputData = input.Adapt<GoodsRecordDto>();
            var result = await _goodsManageService.Update(inputData);
            return result;
        }

        /// <summary>
        /// 设置商品状态
        /// </summary>
        /// <param name="goodsStatus"></param>
        /// <returns></returns>
        public async Task<bool> SetGoodsStatus([FromBody] SetGoodsStatus goodsStatus)
        {
            var result = await _goodsManageService.SetGoodsStatus(goodsStatus);
            return result;
        }

        /// <summary>
        /// 批量设置商品上下架
        /// </summary>
        /// <param name="goodsStatus"></param>
        /// <returns></returns>
        public async Task<bool> BatchSetGoodsStatus([FromBody] BatchSetGoodsStatus goodsStatus)
        {
            var result = await _goodsManageService.BatchSetGoodsStatus(goodsStatus);
            return result;
        }

        /// <summary>
        /// 修改商品库存量
        /// </summary>
        /// <param name="goodsInventory"></param>
        /// <returns></returns>
        public async Task<bool> SetGoodsStoreCount([FromBody] ChangeGoodsInventory goodsInventory)
        {
            var result = await _goodsManageService.SetGoodsStoreCount(goodsInventory);
            return result;
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            var result = await _goodsManageService.Delete(id);
            return result;
        }

        /// <summary>
        /// 下载商品导入模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DownloadGoodsImportTemplate()
        {
            var fileStream = new MemoryStream();
            try
            {
                var path = FileHelper.GetAbsolutePath("/Template/GoodsImportDataTemp.xls");
                return new FileStreamResult(new FileStream(path, FileMode.Open), "application/octet-stream") { FileDownloadName = "商品导入元数据模板.xls" };

            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
            finally
            {
                await fileStream.FlushAsync();
                fileStream.Dispose();
                fileStream.Close();
            }
        }

        /// <summary>
        /// 用户数据导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<Guid> ImportGoods(IFormFile file)
        {
            var batchId = _idGenerator.CreateGuid();
            if (file == null || string.IsNullOrWhiteSpace(file.FileName))
            {
                throw Oops.Oh("未获取到文件信息");
            }
            var strExtName = Path.GetExtension(file.FileName).ToLower();
            if (strExtName != ".xls" && strExtName != ".xlsx")
            {
                throw Oops.Oh("请上传Excel文件");
            }
            var dt = await LoadToDataTable(file);
            if (dt.Rows.Count <= 0)
            {
                throw Oops.Oh("未从文件中获取到数据信息");
            }
            var dataList = DataToList(dt);
            var batchInsertTempData = dataList.Adapt<List<GoodsRecordDto>>();
            await _goodsManageService.CheckGoodsTempData(batchInsertTempData);
            await _goodsManageService.StoreTempGoods(batchId, batchInsertTempData);
            return batchId;
        }

        private async Task<DataTable> LoadToDataTable(IFormFile file)
        {
            var dt = new DataTable();
            var fileStream = new MemoryStream();
            try
            {
                await file.CopyToAsync(fileStream);
                fileStream.Position = 0;
                var workbook = new XSSFWorkbook(fileStream);
                if (workbook.NumberOfSheets <= 0)
                {
                    throw Oops.Oh("未找到Excel文件内容");
                }
                var sheet = workbook.GetSheetAt(0);
                dt.Columns.AddRange(new[] {
                    new DataColumn("Name",typeof(string)),
                    new DataColumn("Type",typeof(string)),
                    new DataColumn("CurrentCount",typeof(int)),
                    new DataColumn("Score",typeof(int)),
                    new DataColumn("ObtainWay",typeof(string)),
                    new DataColumn("ObtainAddress",typeof(string)),
                    new DataColumn("ObtainContact",typeof(string)),
                });
                //写入内容
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                while (rows.MoveNext())
                {
                    IRow row = (XSSFRow)rows.Current;
                    if (row.RowNum == sheet.FirstRowNum)
                    {
                        continue;
                    }
                    if (row.Cells.Count > 1 && string.IsNullOrWhiteSpace(row.Cells[0].ToString()))
                    {
                        continue;
                    }
                    DataRow dr = dt.NewRow();
                    var cellIndex = 0;
                    foreach (ICell item in row.Cells)
                    {
                        if (cellIndex >= dt.Columns.Count)
                        {
                            continue;
                        }
                        switch (item.CellType)
                        {
                            case CellType.Boolean:
                                dr[item.ColumnIndex] = item.BooleanCellValue;
                                break;
                            case CellType.Error:
                                dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                break;
                            case CellType.Formula:
                                switch (item.CachedFormulaResultType)
                                {
                                    case CellType.Boolean:
                                        dr[item.ColumnIndex] = item.BooleanCellValue;
                                        break;
                                    case CellType.Error:
                                        dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(item))
                                        {
                                            dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                        }
                                        else
                                        {
                                            dr[item.ColumnIndex] = item.NumericCellValue;
                                        }
                                        break;
                                    case CellType.String:
                                        string str = item.StringCellValue;
                                        if (!string.IsNullOrEmpty(str))
                                        {
                                            dr[item.ColumnIndex] = str.ToString();
                                        }
                                        else
                                        {
                                            dr[item.ColumnIndex] = null;
                                        }
                                        break;
                                    case CellType.Unknown:
                                    case CellType.Blank:
                                    default:
                                        dr[item.ColumnIndex] = string.Empty;
                                        break;
                                }
                                break;
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(item))
                                {
                                    dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                }
                                else
                                {
                                    dr[item.ColumnIndex] = item.NumericCellValue;
                                }
                                break;
                            case CellType.String:
                                string strValue = item.StringCellValue;
                                if (!string.IsNullOrEmpty(strValue))
                                {
                                    dr[item.ColumnIndex] = strValue.ToString();
                                }
                                else
                                {
                                    dr[item.ColumnIndex] = null;
                                }
                                break;
                            case CellType.Unknown:
                            case CellType.Blank:
                            default:
                                dr[item.ColumnIndex] = string.Empty;
                                break;
                        }
                        cellIndex++;
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex.Message);
            }
            finally
            {
                fileStream.Dispose();
                fileStream.Close();
            }
        }

        private List<GoodsRecordDto> DataToList(DataTable dt)
        {
            var listTempReader = new List<GoodsRecordDto>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var tempReader = new GoodsRecordDto();
                    tempReader.ID = _idGenerator.CreateGuid();
                    tempReader.Name = dt.Rows[i]["Name"].ToString();
                    tempReader.Type = TryGetGoodsType(dt.Rows[i]["Type"].ToString());
                    tempReader.CurrentCount = (int)(DataConverter.ToNullableDecimal(dt.Rows[i]["CurrentCount"].ToString()) ?? 0);
                    tempReader.TotalCount = (int)(DataConverter.ToNullableDecimal(dt.Rows[i]["CurrentCount"].ToString()) ?? 0);
                    tempReader.FreezeCount = 0;
                    tempReader.Score = (int)(DataConverter.ToNullableDecimal(dt.Rows[i]["Score"].ToString()) ?? 0);
                    tempReader.ObtainWay = TryGetObtainWay(dt.Rows[i]["ObtainWay"].ToString());
                    tempReader.ObtainAddress = dt.Rows[i]["ObtainAddress"].ToString();
                    tempReader.ObtainContact = dt.Rows[i]["ObtainContact"].ToString();
                    tempReader.BeginDate = DateTime.Now.Date;
                    tempReader.EndDate = null;
                    tempReader.DetailInfo = "";
                    tempReader.Status = (int)EnumGoodsStatus.下架;
                    if (!string.IsNullOrWhiteSpace(tempReader.Name))
                    {
                        listTempReader.Add(tempReader);
                    }
                }
            }
            return listTempReader;
        }

        /// <summary>
        /// 获取商品类型枚举
        /// </summary>
        /// <param name="goodsType"></param>
        /// <returns></returns>
        private int TryGetGoodsType(string goodsType)
        {
            try
            {
                return (int)Enum.Parse(typeof(EnumGoodsType), goodsType);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取商品获取方式
        /// </summary>
        /// <param name="obtainWay"></param>
        /// <returns></returns>
        private int TryGetObtainWay(string obtainWay)
        {
            try
            {
                return (int)Enum.Parse(typeof(EnumObtainWay), obtainWay);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取待导入数据
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]")]
        public async Task<List<GoodsRecordDto>> QueryImportTempGoodsData(Guid batchId)
        {
            var pageList = await _goodsManageService.GetStoreTempGoods(batchId);
            var targetList = pageList.Adapt<List<GoodsRecordDto>>();
            return targetList;
        }

        /// <summary>
        /// 用户数据导入确认
        /// </summary>
        /// <param name="batchId">用户数据</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        [UnitOfWork]
        public async Task<object> ImportGoodsConfirm(Guid batchId)
        {
            var result = await _goodsManageService.ImportGoodsConfirm(batchId);
            return new { SucCount = result.Item1, ErrCount = result.Item2 };
        }

    }
}
