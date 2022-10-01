/*********************************************************
* 名    称：CardAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：读者卡服务Api
* 更新历史：
*
* *******************************************************/
using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 读者卡服务
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class CardAppService : BaseAppService
    {
        private readonly ICardService _cardService;
        private readonly IUserIoService _userIoService;
        private readonly ILogger<UserAppService> _logger;
        private readonly ISyncCardService _syncCardService;
        private readonly TenantInfo _tenantInfo;

        public CardAppService(ICardService cardService,
                              ILogger<UserAppService> logger,
                              IUserIoService userIoService,
                              ISyncCardService syncCardService,
                              TenantInfo tenantInfo)
        {
            _cardService = cardService;
            _userIoService = userIoService;
            _syncCardService = syncCardService;
            _logger = logger;
            _tenantInfo = tenantInfo;
        }

        /// <summary>
        /// 用户初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            return await _cardService.GetCardInitData();
        }

        /// <summary>
        /// 获取读者卡表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<CardListItemOutput>> QueryTableData([FromQuery] CardTableQuery queryFilter)
        {
            var pageDtoList = await _cardService.QueryTableData(queryFilter);
            var targetPageList = pageDtoList.Adapt<PagedList<CardListItemOutput>>();
            if (CurUserPermission == null || !CurUserPermission.SeeSensitiveInfo)
            {
                targetPageList = AdaptEncoder.SensitiveEncode<CardListItemOutput, SensitiveCardListItemOutput>(targetPageList);
            }
            return targetPageList;
        }

        /// <summary>
        /// 获取用户读者卡信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [Route("[action]/{userId}")]
        public async Task<List<CardListItemOutput>> QueryUserCardListData(Guid userId)
        {
            var cardList = await _cardService.QueryUserCardListData(userId);
            var targetList = cardList.Adapt<List<CardListItemOutput>>();
            return targetList;
        }
        /// <summary>
        /// 获取用户读者卡申请信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [Route("[action]/{userId}")]
        public async Task<List<CardListItemOutput>> QueryUserCardApplyListData(Guid userId)
        {
            var cardList = await _cardService.QueryUserCardApplyListData(userId);
            var targetList = cardList.Adapt<List<CardListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取读者卡信息
        /// </summary>
        /// <param name="cardId">读者卡Id</param>
        /// <returns></returns>
        [Route("{cardId}")]
        public async Task<CardDetailOutput> GetByID(Guid cardId)
        {
            var targetCard = await _cardService.GetByID(cardId);
            return targetCard;
        }

        /// <summary>
        /// 创建卡信息
        /// </summary>
        /// <param name="cardData">卡数据</param>
        /// <returns></returns>
        public async Task<Guid> Create([FromBody] CardInput cardData)
        {
            var cardDto = cardData.Adapt<CardDto>();
            var cardId = await _cardService.Create(cardDto);
            return cardId;
        }

        /// <summary>
        /// 编辑卡信息
        /// </summary>
        /// <param name="cardData">卡数据</param>
        /// <returns></returns>
        public async Task<Guid> Update([FromBody] CardInput cardData)
        {
            var cardDto = cardData.Adapt<CardDto>();
            cardDto = await _cardService.MapPropertyName(cardDto);
            var cardId = await _cardService.Update(cardDto);
            return cardId;
        }

        /// <summary>
        /// 设置卡密码
        /// </summary>
        /// <param name="cardSecret"></param>
        /// <returns></returns>
        public async Task<bool> SetCardSecret([FromBody] CardSecreteInput cardSecret)
        {
            var cardSecretDto = cardSecret.Adapt<CardSecretDto>();
            var result = await _cardService.SetSecret(cardSecretDto);
            return result;
        }

        /// <summary>
        /// 重置卡密码
        /// </summary>
        /// <param name="cardSecret"></param>
        /// <returns></returns>
        public async Task<bool> ResetCardSecret([FromBody] CardSecreteInput cardSecret)
        {
            var cardSecretDto = cardSecret.Adapt<CardSecretDto>();
            var result = await _cardService.ResetSecret(cardSecretDto);
            return result;
        }

        /// <summary>
        /// 删除读者卡数据
        /// </summary>
        /// <param name="cardId">读者卡Id</param>
        /// <returns></returns>
        [Route("{cardId}")]
        public async Task<bool> Delete(Guid cardId)
        {
            return await _cardService.Delete(cardId);
        }

        /// <summary>
        /// 批量修改读者卡数据
        /// </summary>
        /// <param name="cardData">读者卡数据</param>
        /// <returns></returns>
        [HttpPut]
        [Route("[action]")]
        public async Task<bool> BatchUpdate([FromBody] CardBatchEditInput cardData)
        {
            cardData = await _cardService.MapBatchPropertyName(cardData);
            var result = await _cardService.BatchUpdate(cardData);
            return result;
        }
        /// <summary>
        /// 获取待导出简要信息1103
        /// </summary>
        /// <param name="exportFilter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<CardExportBriefOutput> GetExportCardDataBriefInfo([FromBody] CardExportFilter exportFilter)
        {
            var briefInfoDto = await _userIoService.GetExportCardDataBriefInfo(exportFilter);
            var targetBriefInfo = briefInfoDto.Adapt<CardExportBriefOutput>();
            return targetBriefInfo;
        }

        /// <summary>
        /// 卡数据导出
        /// </summary>
        /// <param name="exportFilter">导出筛选</param>
        /// <returns></returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ExportCardData([FromBody] CardExportFilter exportFilter)
        {
            var groupSelects = await _cardService.GetGroupSelectItem();
            var properties = exportFilter.Properties.ToList();
            switch (exportFilter.ExportType)
            {
                case 0:
                    exportFilter.PageSize = 5000;
                    break;
                default:
                    break;
            }
            var wb = new XSSFWorkbook();
            var sheet = wb.CreateSheet("sheet1");//创建工作簿
            var dateCellStyle = wb.CreateCellStyle();
            var dataFormat = wb.CreateDataFormat();
            dateCellStyle.DataFormat = dataFormat.GetFormat("yyyy-MM-dd");
            //查询导出数据
            var cardData = await _userIoService.QueryExportCardTableData(exportFilter);
            var cardDataItems = cardData.Items.ToList();
            var cardList = cardDataItems.Adapt<List<ExportCardListItemOutput>>();
            var rowIndex = 0;
            //添加表头
            var titleRow = sheet.CreateRow(rowIndex);
            var titleRowCells = new Dictionary<int, ICell>();
            for (var cellIndex = 0; cellIndex < properties.Count(); cellIndex++)
            {
                var property = properties[cellIndex];
                titleRowCells.Add(cellIndex, titleRow.CreateCell(cellIndex));
                titleRowCells[cellIndex].SetCellValue(property.PropertyName);
            }
            rowIndex++;
            //添加数据
            foreach (var rowData in cardList)
            {
                var dataRow = sheet.CreateRow(rowIndex);
                var dataRowCells = new Dictionary<int, ICell>();
                for (var cellIndex = 0; cellIndex < properties.Count(); cellIndex++)
                {
                    var property = properties[cellIndex];
                    dataRowCells.Add(cellIndex, dataRow.CreateCell(cellIndex));
                    var cellValue = GetPropertyValue(rowData, property, groupSelects);
                    dataRowCells[cellIndex].SetCellValue(cellValue);
                }
                rowIndex++;
            }

            var fileStream = new MemoryStream();
            byte[] content = null;
            try
            {
                wb.Write(fileStream);
                content = fileStream.ToArray();
                return await Task.FromResult(new FileContentResult(content, "application/octet-stream") { FileDownloadName = $"读者卡导出数据{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"读者卡数据导出错误：{ex.Message}");
                throw Oops.Oh("读者卡数据导出错误");
            }
            finally
            {
                //await fileStream.FlushAsync();
                fileStream.Dispose();
                fileStream.Close();
            }
        }

        /// <summary>
        /// 获取同步设置配置项
        /// </summary>
        /// <returns></returns>
        public async Task<SyncCardConfigItemDto> GetSyncCardConfig()
        {
            var configData = await _syncCardService.GetSyncCardConfig(_tenantInfo.Name);
            return configData;
        }

        /// <summary>
        /// 设置同步任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> SetSyncCardConfig([FromBody] SyncCardConfigItemDto input)
        {
            var result = await _syncCardService.SetSyncCardConfig(_tenantInfo.Name, input);
            return result;
        }

        /// <summary>
        /// 获取同步日志
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SyncCardLogListItemDto>> QuerySyncCardLogTableData([FromQuery] SyncCardLogTableQuery queryFilter)
        {
            var listResult = await _syncCardService.GetSyncCardLogTableData(_tenantInfo.Name, queryFilter);
            return listResult;
        }

        /// <summary>
        /// 添加临时同步读者卡任务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SetSyncCardTask()
        {
            var result = await _syncCardService.SetSyncCardTask(_tenantInfo.Name);
            return result;
        }

        private string ShowAddrName(string addr)
        {
            var addrArray = (addr ?? "").Split('|');
            if (addrArray.Length >= 1)
            {
                return addrArray[0];
            }
            return "";
        }

        private string GetPropertyValue(ExportCardListItemOutput userData, ExportPropertyInput property, List<PropertyGroupSelectDto> groupSelects)
        {
            var fieldProperties = typeof(ExportCardListItemOutput).GetProperties().ToList();
            var extenalProperties = userData.Properties.ToList();
            if (!property.External)
            {
                var fieldCode = property.PropertyCode;
                if (property.PropertyCode.ToLower().Contains("card"))
                {
                    fieldCode = fieldCode.Replace("Card_", "");
                }
                else
                {
                    fieldCode = fieldCode.Replace("_", "");
                }
                var appendNamesFields = new[] { "Type", "UserType" };
                if (appendNamesFields.Contains(fieldCode))
                {
                    fieldCode = $"{fieldCode}Name";
                }
                var field = fieldProperties.FirstOrDefault(x => x.Name.ToLower() == fieldCode.ToLower());
                if (field != null)
                {
                    var fieldValue = DataConverter.ObjectToString(field.GetValue(userData), field.PropertyType);
                    if (property.PropertyType == (int)EnumPropertyType.属性组 && (field.PropertyType == typeof(int) || field.PropertyType == typeof(int?)))
                    {
                        var mapItems = groupSelects.Where(g => g.GroupCode == property.PropertyCode).SelectMany(g => g.GroupItems).ToList();
                        var mapItem = mapItems.FirstOrDefault(i => i.Value == fieldValue);
                        fieldValue = mapItem != null ? mapItem.Key : fieldValue;
                    }
                    if (property.PropertyType == (int)EnumPropertyType.是非)
                    {
                        fieldValue = !string.IsNullOrWhiteSpace(fieldValue) ? ((DataConverter.ToNullableBoolean(fieldValue) ?? false) ? "是" : "否") : "";
                    }
                    if (property.PropertyType == (int)EnumPropertyType.地址)
                    {
                        fieldValue = this.ShowAddrName(fieldValue);
                    }
                    return fieldValue;
                }
                return "";
            }
            else
            {
                var fieldCode = property.PropertyCode;
                var mapProperty = extenalProperties.FirstOrDefault(x => x.PropertyCode == fieldCode);
                if (mapProperty != null)
                {
                    var fieldValue = mapProperty.PropertyValue;
                    if (property.PropertyType == (int)EnumPropertyType.是非)
                    {
                        fieldValue = !string.IsNullOrWhiteSpace(fieldValue) ? ((DataConverter.ToNullableBoolean(fieldValue) ?? false) ? "是" : "否") : "";
                    }
                    if (property.PropertyType == (int)EnumPropertyType.地址)
                    {
                        fieldValue = this.ShowAddrName(fieldValue);
                    }
                    return fieldValue;
                }
                return "";
            }
        }
    }
}
