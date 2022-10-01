/*********************************************************
* 名    称：UserGroupAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：用户组管理
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
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Dtos.UserGroup;
using SmartLibrary.User.Application.Services.Consts;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 用户组服务
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class UserGroupAppService : BaseAppService
    {
        private readonly IUserGroupService _userGroupService;
        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="userGroupService"></param>
        public UserGroupAppService(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

        /// <summary>
        /// 获取初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _userGroupService.GetInitData();
        }

        /// <summary>
        /// 获取用户组数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<GroupListItemOutput>> QueryTableData([FromQuery] GroupTableQuery queryFilter)
        {
            var pageList = await _userGroupService.QueryTableQuery(queryFilter);
            return pageList;
        }

        /// <summary>
        /// 获取用户组数据
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<GroupInfoOutput> GetByID(Guid groupId)
        {
            var groupInfo = await _userGroupService.GetByID(groupId);
            var targetGroupInfo = groupInfo.Adapt<GroupInfoOutput>();
            return targetGroupInfo;
        }

        /// <summary>
        /// 获取用户组简要信息
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<GroupBriefInfoOutput> GetGroupBriefInfo(Guid groupId)
        {
            var groupBriefInfo = await _userGroupService.GetBriefInfoByID(groupId);
            return groupBriefInfo;
        }

        /// <summary>
        /// 创建用户组
        /// </summary>
        /// <param name="groupData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> Create([FromBody] GroupEditInput groupData)
        {
            if (CurrentUser == null)
            {
                throw Oops.Oh("未找到操作人信息").StatusCode(Consts.ExceptionStatus);
            }
            var groupDto = groupData.Adapt<GroupEditDto>();
            groupDto.CreateUserID = CurrentUser.UserID;
            groupDto.CreateUserName = CurrentUser.UserName;
            var groupId = await _userGroupService.Create(groupDto);
            return groupId;
        }

        /// <summary>
        /// 编辑用户组
        /// </summary>
        /// <param name="groupData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> Update([FromBody] GroupEditInput groupData)
        {
            var groupDto = groupData.Adapt<GroupEditDto>();
            groupDto.CreateUserID = CurrentUser.UserID;
            groupDto.CreateUserName = CurrentUser.UserName;
            var groupId = await _userGroupService.Update(groupDto);
            return groupId;
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Route("{groupId}")]
        [UnitOfWork]
        public async Task<bool> Delete(Guid groupId)
        {
            return await _userGroupService.Delete(groupId);
        }

        /// <summary>
        /// 添加用户到用户组1103
        /// </summary>
        /// <param name="userGroupAddData"></param>
        /// <returns></returns>
        public async Task<bool> AddUserToGroup([FromBody] UserGroupAddInput userGroupAddData)
        {
            var userGroupAddDto = userGroupAddData.Adapt<UserGroupAddDto>();
            var result = await _userGroupService.AddUserToGroup(userGroupAddDto);
            return result;
        }

        /// <summary>
        /// 从用户组删除用户1103
        /// </summary>
        /// <param name="userGroupDelData"></param>
        /// <returns></returns>
        public async Task<bool> DeleteGroupUsers([FromBody] UserGroupDelInput userGroupDelData)
        {
            var userGroupDelDto = userGroupDelData.Adapt<UserGroupDelDto>();
            var result = await _userGroupService.DeleteGroupUsers(userGroupDelDto);
            return result;
        }

        /// <summary>
        /// 用户数据导入模板下载1103
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
        /// 用户组数据导入1103
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<SimpleUserListItemOutput>> ImportGroupUser(IFormFile file)
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
            var dataList = DataToList(dt);
            var dataSearch = dataList.Adapt<List<UserImportSearchDto>>();
            var userList = await _userGroupService.QueryUserListBySearchInfo(dataSearch);
            var userListOutput = userList.Adapt<List<SimpleUserListItemOutput>>();
            return userListOutput;
        }


        /// <summary>
        /// 数据读取
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<GroupUserImportSearchInput> DataToList(DataTable dt)
        {
            var listTempReader = new List<GroupUserImportSearchInput>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var tempReader = new GroupUserImportSearchInput();
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
