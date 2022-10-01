/*********************************************************
* 名    称：PropertyAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：属性管理Api
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Dtos.DataApprove;
using SmartLibrary.User.Application.Dtos.Property;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 属性管理
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class PropertyAppService : BaseAppService
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IBasicConfigService _basicConfigService;
        private readonly IPropertyService _propertyService;
        private readonly IDataApproveService _dataApproveService;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="idGenerator"></param>
        /// <param name="basicConfigService"></param>
        /// <param name="propertyService"></param>
        /// <param name="dataApproveService"></param>
        public PropertyAppService(
            IDistributedIDGenerator idGenerator
            , IBasicConfigService basicConfigService
            , IPropertyService propertyService
            , IDataApproveService dataApproveService)
        {
            _idGenerator = idGenerator;
            _basicConfigService = basicConfigService;
            _propertyService = propertyService;
            _dataApproveService = dataApproveService;
        }

        /// <summary>
        /// 属性初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.PropertyConfirm;
            return await Task.FromResult(new
            {
                propertyData = new PropertyDto(),
                PropertyDataStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumPropertyDataStatus)),
                PropertyApproveStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumPropertyApproveStatus)),
                PropertyType = EnumHelper.GetEnumDictionaryItems(typeof(EnumPropertyType)),
                needApprove
            }); ;
        }

        /// <summary>
        /// 获取属性列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyListItemOutput>> QueryListData()
        {
            var groupList = await _propertyService.QueryPropertyList();
            var targetList = groupList.Adapt<List<PropertyListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取属性详情
        /// </summary>
        /// <param name="id">属性Id</param>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<PropertyDetailOutput> GetByID(Guid id)
        {
            var propertyDto = await _propertyService.GetByID(id);
            var targetOutput = propertyDto.Adapt<PropertyDetailOutput>();
            return targetOutput;
        }

        /// <summary>
        /// 创建自定义属性
        /// </summary>
        /// <param name="inputData">属性信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> Create([FromBody] PropertyEditInput inputData)
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.PropertyConfirm;
            inputData.ID = _idGenerator.CreateGuid(inputData.ID);
            var propertyDto = inputData.Adapt<PropertyDto>();
            await _propertyService.PropertyPrecheck(propertyDto);
            propertyDto.Status = (int)EnumPropertyDataStatus.正常;
            propertyDto.ApproveStatus = (int)EnumPropertyApproveStatus.正常;

            var changeLog = ComparePropertyDiff(EnumLogDiffType.新增, null, propertyDto);
            if (needApprove)
            {
                changeLog.Status = (int)EnumPropertyLogStatus.待审批;
                propertyDto.Status = (int)EnumPropertyDataStatus.未激活;
                propertyDto.ApproveStatus = (int)EnumPropertyApproveStatus.待审批;
            }

            //创建属性对象
            var propertyId = await _propertyService.CreateProperty(propertyDto);
            //创建变更日志
            await _dataApproveService.CreatePropertyChangeLog(changeLog);
            return propertyId;
        }

        /// <summary>
        /// 编辑自定义属性
        /// </summary>
        /// <param name="inputData">属性信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> Update([FromBody] PropertyEditInput inputData)
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.PropertyConfirm;
            var propertyDto = inputData.Adapt<PropertyDto>();
            await _propertyService.PropertyPrecheck(propertyDto);
            var preProperty = await _propertyService.GetByID(inputData.ID);
            var changeLog = ComparePropertyDiff(EnumLogDiffType.修改, preProperty, propertyDto);
            if (!changeLog.ItemChangeLogs.Any())
            {
                return inputData.ID;
            }
            propertyDto.Status = (int)EnumPropertyDataStatus.正常;
            propertyDto.ApproveStatus = (int)EnumPropertyApproveStatus.正常;
            if (needApprove)
            {
                propertyDto = preProperty.Adapt<PropertyDto>();
                changeLog.Status = (int)EnumPropertyLogStatus.待审批;
                propertyDto.Status = (int)EnumGroupItemDataStatus.正常;
                propertyDto.ApproveStatus = (int)EnumGroupItemApproveStatus.待审批;
            }

            //修改实体
            var propertyId = await _propertyService.UpdateProperty(propertyDto);
            //创建变更日志
            await _dataApproveService.CreatePropertyChangeLog(changeLog);

            return propertyId;
        }

        /// <summary>
        /// 删除属性
        /// </summary>
        /// <param name="id">属性Id</param>
        /// <returns></returns>
        [Route("{id}")]
        [UnitOfWork]
        public async Task<bool> Delete(Guid id)
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.PropertyConfirm;
            var propertyDto = await _propertyService.GetByID(id);
            var changeLog = ComparePropertyDiff(EnumLogDiffType.删除, propertyDto, null);
            if (needApprove)
            {
                changeLog.Status = (int)EnumPropertyLogStatus.待审批;
                propertyDto.ApproveStatus = (int)EnumPropertyApproveStatus.待审批;
                //修改属性审批状态
                await _propertyService.UpdateProperty(propertyDto);
                //创建变更日志
                await _dataApproveService.CreatePropertyChangeLog(changeLog);
            }
            else
            {
                //逻辑删除可选项
                await _propertyService.DeleteProperty(id);
            }
            return true;
        }

        /// <summary>
        /// 设置是否可搜索
        /// </summary>
        /// <param name="searchSet">是否可检索</param>
        /// <returns></returns>
        [Route("[action]")]
        public async Task<bool> SetCanSearch([FromBody] PropertySearchSetInput searchSet)
        {
            var searchSetDto = searchSet.Adapt<PropertySearchSetDto>();
            var result = await _propertyService.SetCanSearch(searchSetDto);
            return result;
        }

        /// <summary>
        ///设置是否在列表显示
        /// </summary>
        /// <param name="showSet">在列表显示</param>
        /// <returns></returns>
        [Route("[action]")]
        public async Task<bool> SetShowOnTable([FromBody] PropertyShowSetInput showSet)
        {
            var showSetDto = showSet.Adapt<PropertyShowSetDto>();
            var result = await _propertyService.SetShowOnTable(showSetDto);
            return result;
        }

        /// <summary>
        /// 通过反射对比属性变化情况
        /// </summary>
        /// <param name="diffType"></param>
        /// <param name="preData"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        private PropertyChangeLogDto ComparePropertyDiff(EnumLogDiffType diffType, PropertyDto preData, PropertyDto newData)
        {
            var xType = typeof(PropertyDto);
            var xProperties = xType.GetProperties().ToList();
            var logProperties = xType.GetProperties().Where(x => x.GetCustomAttributes(typeof(LogPropertyAttribute), false).Any()).Select(x => (LogPropertyAttribute)(x.GetCustomAttributes(typeof(LogPropertyAttribute), false).FirstOrDefault())).ToList();
            var fieldNames = new List<string>();
            var propertyChangeLog = new PropertyChangeLogDto
            {
                ID = _idGenerator.CreateGuid(),
                PropertyType = (int)EnumLogPropertyType.属性,
                PropertyID = Guid.Empty,
                ChangeType = 0,
                Status = (int)EnumGroupLogStatus.无需审批,
                ChangeTime = DateTime.Now,
                ChangeUserID = CurrentUser?.UserID ?? Guid.Empty,
                ChangeUserName = CurrentUser?.UserName ?? "",
                ChangeUserPhone = CurrentUser?.UserPhone ?? "",
                Content = ""
            };
            var itemChangeLogs = new List<PropertyChangeItemDto>();
            switch (diffType)
            {
                case EnumLogDiffType.新增:
                    if (newData == null)
                    {
                        throw Oops.Oh("属性对象不能为空");
                    }
                    propertyChangeLog.PropertyID = newData.ID;
                    propertyChangeLog.ChangeType = (int)EnumPropertyLogType.新增;
                    logProperties.OrderBy(x => x.Sort).ToList().ForEach(x =>
                    {
                        var mapP = xProperties.FirstOrDefault(p => p.Name.ToLower() == x.Code.ToLower());
                        if (mapP == null)
                        {
                            throw Oops.Oh("属性映射对比失败");
                        }
                        var preValue = "";
                        var newValue = (mapP.GetValue(newData) ?? "").ToString();
                        var itemLog = new PropertyChangeItemDto
                        {
                            LogID = propertyChangeLog.ID,
                            PropertyID = propertyChangeLog.PropertyID,
                            FieldName = x.Name,
                            FieldCode = x.Code,
                            OldValue = preValue,
                            NewValue = (mapP.GetValue(newData) ?? "").ToString()
                        };
                        itemChangeLogs.Add(itemLog);
                    });
                    fieldNames = itemChangeLogs.Select(x => x.FieldName).ToList();
                    propertyChangeLog.Content = $"属性:{newData.Name} 变更字段:{string.Join(";", fieldNames)}";
                    propertyChangeLog.ItemChangeLogs = itemChangeLogs;
                    break;
                case EnumLogDiffType.修改:
                    if (preData == null || newData == null)
                    {
                        throw Oops.Oh("属性对象不能为空");
                    }
                    propertyChangeLog.PropertyID = newData.ID;
                    propertyChangeLog.ChangeType = (int)EnumPropertyLogType.修改;
                    logProperties.OrderBy(x => x.Sort).ToList().ForEach(x =>
                    {
                        var mapP = xProperties.FirstOrDefault(p => p.Name.ToLower() == x.Code.ToLower());
                        if (mapP == null)
                        {
                            throw Oops.Oh("属性映射对比失败");
                        }
                        var preValue = (mapP.GetValue(preData) ?? "").ToString();
                        var newValue = (mapP.GetValue(newData) ?? "").ToString();
                        if (preValue != newValue)
                        {
                            var itemLog = new PropertyChangeItemDto
                            {
                                LogID = propertyChangeLog.ID,
                                PropertyID = propertyChangeLog.PropertyID,
                                FieldName = x.Name,
                                FieldCode = x.Code,
                                OldValue = preValue,
                                NewValue = newValue
                            };
                            itemChangeLogs.Add(itemLog);
                        }

                    });
                    fieldNames = itemChangeLogs.Select(x => x.FieldName).ToList();
                    propertyChangeLog.Content = $"属性:{preData.Name} 变更字段:{string.Join(";", fieldNames)}";
                    propertyChangeLog.ItemChangeLogs = itemChangeLogs;
                    break;
                case EnumLogDiffType.删除:
                    if (preData == null)
                    {
                        throw Oops.Oh("属性对象不能为空");
                    }
                    propertyChangeLog.PropertyID = preData.ID;
                    propertyChangeLog.ChangeType = (int)EnumPropertyLogType.删除;
                    logProperties.OrderBy(x => x.Sort).ToList().ForEach(x =>
                    {
                        var mapP = xProperties.FirstOrDefault(p => p.Name.ToLower() == x.Code.ToLower());
                        if (mapP == null)
                        {
                            throw Oops.Oh("属性映射对比失败");
                        }
                        var preValue = (mapP.GetValue(preData) ?? "").ToString();
                        var newValue = "";
                        if (preValue != newValue)
                        {
                            var itemLog = new PropertyChangeItemDto
                            {
                                LogID = propertyChangeLog.ID,
                                PropertyID = propertyChangeLog.PropertyID,
                                FieldName = x.Name,
                                FieldCode = x.Code,
                                OldValue = preValue,
                                NewValue = newValue
                            };
                            itemChangeLogs.Add(itemLog);
                        }

                    });
                    fieldNames = itemChangeLogs.Select(x => x.FieldName).ToList();
                    propertyChangeLog.Content = $"属性:{preData.Name} 变更字段:{string.Join(";", fieldNames)}";
                    propertyChangeLog.ItemChangeLogs = itemChangeLogs;
                    break;
                default:
                    break;
            }
            return propertyChangeLog;
        }

    }
}
