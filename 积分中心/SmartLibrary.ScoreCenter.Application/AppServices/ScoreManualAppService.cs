/*********************************************************
* 名    称：ScoreManualAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分奖惩处理
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
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
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.Common.Services;
using SmartLibrary.User.RpcService;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.AppServices
{
    /// <summary>
    /// 积分手动处理
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class ScoreManualAppService : BaseAppService
    {
        private readonly IScoreManualService _scoreManualService;
        private readonly IGrpcClientResolver _grpcClientResolver;

        public ScoreManualAppService(IScoreManualService scoreManualService
            , IGrpcClientResolver grpcClientResolver)
        {
            _scoreManualService = scoreManualService;
            _grpcClientResolver = grpcClientResolver;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var initData = await _scoreManualService.GetInitData();
            return initData;
        }

        /// <summary>
        /// 查询表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ScoreManualListItemOutput>> QueryTableData([FromQuery] ScoreManualTableQuery queryFilter)
        {
            var pagedList = await _scoreManualService.QueryTableData(queryFilter);
            var targetList = pagedList.Adapt<PagedList<ScoreManualListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 创建手动调整积分任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> Create([FromBody] ScoreManualProcessInput input)
        {
            var inputData = input.Adapt<ScoreManualProcessDto>();
            inputData.OperatorUserKey = CurrentUser?.UserKey ?? "";
            inputData.OperatorName = CurrentUser?.UserName ?? "";
            var result = await _scoreManualService.Create(inputData);
            return result;
        }

        /// <summary>
        /// 通过用户组获取用户列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedList<SimpleUserListItemDto>> GetUserListByGroups([FromBody] GroupUserTableQuery queryFilter)
        {
            var pagedList = await _scoreManualService.GetUserListByGroups(queryFilter);
            return pagedList;
        }

        /// <summary>
        /// 通过用户类型获取用户列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PagedList<SimpleUserListItemDto>> GetUserListByUserType([FromBody] TypeUserTableQuery queryFilter)
        {
            var pagedList = await _scoreManualService.GetUserListByUserType(queryFilter);
            return pagedList;
        }

        /// <summary>
        /// 通过条件查询用户列表数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemDto>> GetUserListByConditions([FromQuery] ViewModels.UserTableQuery queryFilter)
        {
            var pagedList = await _scoreManualService.GetUserListByConditions(queryFilter);
            return pagedList;
        }

        /// <summary>
        /// 用户数据导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DownloadUserImportTemplate()
        {
            #region 代码生成模板
            var wb = new XSSFWorkbook();
            var sheet = wb.CreateSheet("sheet1");//创建工作簿
            var dateCellStyle = wb.CreateCellStyle();
            var dataFormat = wb.CreateDataFormat();
            dateCellStyle.DataFormat = dataFormat.GetFormat("yyyy-MM-dd");
            //添加表头
            var rowIndex = 0;
            var title = sheet.CreateRow(rowIndex);
            var cell_0 = title.CreateCell(0);
            cell_0.SetCellValue("姓名");
            var cell_1 = title.CreateCell(1);
            cell_1.SetCellValue("身份证号");
            var cell_2 = title.CreateCell(2);
            cell_2.SetCellValue("手机号");
            rowIndex++;
            while (rowIndex < 10000)
            {
                var dataRow = sheet.CreateRow(rowIndex);
                var dataCell_0 = dataRow.CreateCell(0);
                dataCell_0.SetCellType(CellType.String);
                var dataCell_1 = dataRow.CreateCell(1);
                dataCell_1.SetCellType(CellType.String);
                var dataCell_2 = dataRow.CreateCell(2);
                dataCell_2.SetCellType(CellType.String);
                rowIndex++;
            }
            #endregion
            var fileStream = new MemoryStream();
            byte[] content = null;
            try
            {
                wb.Write(fileStream);
                content = fileStream.ToArray();
                return await Task.FromResult(new FileContentResult(content, "application/octet-stream") { FileDownloadName = "用户组数据导入元数据模板.xls" });
            }
            catch (Exception ex)
            {
                throw Oops.Oh("用户组数据模板导出错误");
            }
            finally
            {
                fileStream.Dispose();
                fileStream.Close();
            }
        }

        /// <summary>
        /// 用户组数据导入
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<SimpleUserListItemDto>> ImportGroupUser(IFormFile file)
        {
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
            var simpleUserList = new List<SimpleUserListItemDto>();
            var dataList = DataToList(dt);
            var userGrpcClient = _grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>();
            var dataSearch = new UserListImportSearchRequest();
            dataSearch.Items.AddRange(dataList.Select(x => new UserListImportSearchItem { Name = x.Name, IdCard = x.IdCard, Phone = x.Phone }));
            var userList = await userGrpcClient.GetUserListBySearchInfoAsync(dataSearch);
            foreach (var item in userList.Items)
            {
                var mapItem = item.Adapt<SimpleUserListItemDto>();
                simpleUserList.Add(mapItem);
            }
            return simpleUserList;
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
                    new DataColumn("User_Name",typeof(string)),
                    new DataColumn("User_IdCard",typeof(string)),
                    new DataColumn("User_Phone",typeof(string)),
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

                    DataRow dr = dt.NewRow();
                    foreach (ICell item in row.Cells)
                    {
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

        private List<ManualUserImportSearchDto> DataToList(DataTable dt)
        {
            var listTempReader = new List<ManualUserImportSearchDto>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var tempReader = new ManualUserImportSearchDto();
                    tempReader.Name = dt.Rows[i]["User_Name"].ToString();
                    tempReader.IdCard = dt.Rows[i]["User_IdCard"].ToString();
                    tempReader.Phone = dt.Rows[i]["User_Phone"].ToString();
                    if (!string.IsNullOrWhiteSpace(tempReader.Name))
                    {
                        listTempReader.Add(tempReader);
                    }
                }
            }
            return listTempReader;
        }

    }
}
