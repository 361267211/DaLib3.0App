/*********************************************************
* 名    称：PropertyGroupAppService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：属性组管理Api
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
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.Services.Consts;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.AppServices
{
    /// <summary>
    /// 属性组数据服务
    /// </summary>
    [Authorize(Policy = PolicyKey.StaffAuth)]
    public class PropertyGroupAppService : BaseAppService
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IBasicConfigService _basicConfigService;
        private readonly IPropertyGroupService _propertyGroupService;
        private readonly IDataApproveService _dataApproveService;
        private readonly ISysOrgService _sysOrgService;
        private readonly IRegionService _regionService;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="idGenerator"></param>
        /// <param name="basicConfigService"></param>
        /// <param name="propertyGroupService"></param>
        /// <param name="dataApproveService"></param>
        /// <param name="sysOrgService"></param>
        /// <param name="regionService"></param>
        public PropertyGroupAppService(
            IDistributedIDGenerator idGenerator
            , IBasicConfigService basicConfigService
            , IPropertyGroupService propertyGroupService
            , IDataApproveService dataApproveService
            , ISysOrgService sysOrgService
            , IRegionService regionService)
        {
            _idGenerator = idGenerator;
            _basicConfigService = basicConfigService;
            _propertyGroupService = propertyGroupService;
            _dataApproveService = dataApproveService;
            _sysOrgService = sysOrgService;
            _regionService = regionService;
        }

        /// <summary>
        /// 属性组初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.PropertyConfirm;
            return await Task.FromResult(new
            {
                GroupItemData = new PropertyGroupItemDto(),
                GroupItemDataStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumGroupItemDataStatus)),
                GroupItemApproveStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumGroupItemApproveStatus)),
                GroupLogTypes = EnumHelper.GetEnumDictionaryItems(typeof(EnumGroupLogType)),
                needApprove
            });
        }

        /// <summary>
        /// 获取属性组列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyGroupListItemOutput>> QueryListData()
        {
            var groupList = await _propertyGroupService.QueryListData();
            var targetList = groupList.Adapt<List<PropertyGroupListItemOutput>>();
            return targetList;
        }

        /// <summary>
        /// 获取属性组明细项
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<PropertyGroupOutput> QueryListItemData(Guid groupId)
        {
            var groupItemList = await _propertyGroupService.QueryGroupItemList(groupId);
            var targetList = groupItemList.Adapt<PropertyGroupOutput>();
            return targetList;
        }

        /// <summary>
        /// 创建属性组明细项
        /// </summary>
        /// <param name="groupItemData">属性组明细项</param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> CreateGroupItem([FromBody] PropertyGroupItemInput groupItemData)
        {
            var groupData = await _propertyGroupService.GetGroupById(groupItemData.GroupID);
            if (groupData == null)
            {
                throw Oops.Oh("未找到属性组数据").StatusCode(Consts.ExceptionStatus);
            }
            var requiredCode = groupData.RequiredCode;
            if (string.IsNullOrWhiteSpace(groupItemData.Code) && requiredCode)
            {
                throw Oops.Oh("请填写选项编码").StatusCode(Consts.ExceptionStatus);
            }
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.PropertyConfirm;
            groupItemData.ID = _idGenerator.CreateGuid(groupItemData.ID);
            var groupItemDto = groupItemData.Adapt<PropertyGroupItemDto>();
            groupItemDto.Status = (int)EnumGroupItemDataStatus.正常;
            groupItemDto.ApproveStatus = (int)EnumGroupItemApproveStatus.正常;
            var groupChangeLog = new PropertyChangeLogDto
            {
                ID = _idGenerator.CreateGuid(),
                PropertyType = (int)EnumLogPropertyType.属性组,
                PropertyID = groupItemData.GroupID,
                ChangeType = (int)EnumGroupLogType.新增,
                Status = (int)EnumGroupLogStatus.无需审批,
                ChangeTime = DateTime.Now,
                ChangeUserID = CurrentUser?.UserID ?? Guid.Empty,
                ChangeUserName = CurrentUser?.UserName ?? "",
                ChangeUserPhone = CurrentUser?.UserPhone ?? "",
                Content = $"属性组:{groupItemData.GroupName}-{groupItemData.Name} 变更字段:{Consts.GroupItem_Name};{Consts.GroupItem_Code}",
            };
            var itemChangeLogs = new List<PropertyChangeItemDto>
            {
                new PropertyChangeItemDto
                {
                    LogID=groupChangeLog.ID,
                    PropertyID=groupItemData.ID,
                    FieldName=Consts.GroupItem_Name,
                    FieldCode=nameof(groupItemData.Name),
                    OldValue="",
                    NewValue=groupItemData.Name
                },
                new PropertyChangeItemDto
                {
                    LogID=groupChangeLog.ID,
                    PropertyID=groupItemData.ID,
                    FieldName=Consts.GroupItem_Code,
                    FieldCode=nameof(groupItemData.Code),
                    OldValue="",
                    NewValue=groupItemData.Code
                }
            };
            groupChangeLog.ItemChangeLogs = itemChangeLogs;
            if (needApprove)
            {
                groupChangeLog.Status = (int)EnumGroupLogStatus.待审批;
                groupItemDto.Status = (int)EnumGroupItemDataStatus.未激活;
                groupItemDto.ApproveStatus = (int)EnumGroupItemApproveStatus.待审批;
            }
            //创建实体对象
            var entityId = await _propertyGroupService.CreateGroupItem(groupItemDto);
            //创建变更日志
            await _dataApproveService.CreatePropertyChangeLog(groupChangeLog);
            return entityId;
        }

        /// <summary>
        /// 编辑属性明细项
        /// </summary>
        /// <param name="groupItemData">属性组明细项</param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> UpdateGroupItem([FromBody] PropertyGroupItemInput groupItemData)
        {
            var groupData = await _propertyGroupService.GetGroupById(groupItemData.GroupID);
            if (groupData == null)
            {
                throw Oops.Oh("未找到属性组数据").StatusCode(Consts.ExceptionStatus);
            }
            var requiredCode = groupData.RequiredCode;
            if (string.IsNullOrWhiteSpace(groupItemData.Code) && requiredCode)
            {
                throw Oops.Oh("请填写选项编码").StatusCode(Consts.ExceptionStatus);
            }
            var preGroupItem = await _propertyGroupService.GetGroupItemById(groupItemData.ID);
            if (preGroupItem.SysBuildIn)
            {
                throw Oops.Oh("系统内置不可修改").StatusCode(Consts.ExceptionStatus);
            }
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.PropertyConfirm;
            var groupItemDto = groupItemData.Adapt<PropertyGroupItemDto>();

            var groupChangeLogID = _idGenerator.CreateGuid();
            var itemChangeLogs = new List<PropertyChangeItemDto>();
            if (preGroupItem.Name != groupItemDto.Name)
            {
                itemChangeLogs.Add(new PropertyChangeItemDto
                {
                    LogID = groupChangeLogID,
                    PropertyID = groupItemData.ID,
                    FieldName = Consts.GroupItem_Name,
                    FieldCode = nameof(groupItemData.Name),
                    OldValue = preGroupItem.Name,
                    NewValue = groupItemData.Name
                });
            };
            if (preGroupItem.Code != groupItemDto.Code)
            {
                itemChangeLogs.Add(new PropertyChangeItemDto
                {
                    LogID = groupChangeLogID,
                    PropertyID = groupItemData.ID,
                    FieldName = Consts.GroupItem_Code,
                    FieldCode = nameof(groupItemData.Code),
                    OldValue = preGroupItem.Code,
                    NewValue = groupItemData.Code
                });
            };
            var fieldNames = itemChangeLogs.Select(x => x.FieldName).ToList();
            if (!fieldNames.Any())
            {
                return groupItemDto.ID;
            }
            groupItemDto.Status = (int)EnumGroupItemDataStatus.正常;
            groupItemDto.ApproveStatus = (int)EnumGroupItemApproveStatus.正常;
            var groupChangeLog = new PropertyChangeLogDto
            {
                ID = groupChangeLogID,
                PropertyType = (int)EnumLogPropertyType.属性组,
                PropertyID = groupItemData.GroupID,
                ChangeType = (int)EnumGroupLogType.修改,
                Status = (int)EnumGroupLogStatus.无需审批,
                ChangeTime = DateTime.Now,
                ChangeUserID = CurrentUser?.UserID ?? Guid.Empty,
                ChangeUserName = CurrentUser?.UserName ?? "",
                ChangeUserPhone = CurrentUser?.UserPhone ?? "",
                Content = $"属性组:{groupItemData.GroupName}-{groupItemData.Name} 变更字段:{string.Join(";", fieldNames)}",
            };
            groupChangeLog.ItemChangeLogs = itemChangeLogs;
            if (needApprove)
            {
                groupItemDto = preGroupItem.Adapt<PropertyGroupItemDto>();
                groupChangeLog.Status = (int)EnumGroupLogStatus.待审批;
                groupItemDto.Status = (int)EnumGroupItemDataStatus.正常;
                groupItemDto.ApproveStatus = (int)EnumGroupItemApproveStatus.待审批;
            }

            //修改实体
            var entityId = await _propertyGroupService.UpdateGroupItem(groupItemDto);
            //创建变更日志
            await _dataApproveService.CreatePropertyChangeLog(groupChangeLog);
            return entityId;
        }


        /// <summary>
        /// 删除属性组明细项
        /// </summary>
        /// <param name="groupItemId">属性组明细Id</param>
        /// <returns></returns>
        [Route("[action]/{groupItemId}")]
        [UnitOfWork]
        public async Task<bool> DeleteGroupItem(Guid groupItemId)
        {
            var groupItemData = await _propertyGroupService.GetGroupItemById(groupItemId);
            if (groupItemData.SysBuildIn)
            {
                throw Oops.Oh("系统内置不可删除").StatusCode(Consts.ExceptionStatus);
            }
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.PropertyConfirm;
            var groupData = await _propertyGroupService.GetGroupById(groupItemData.GroupID);
            var groupChangeLog = new PropertyChangeLogDto
            {
                ID = _idGenerator.CreateGuid(),
                PropertyType = (int)EnumLogPropertyType.属性组,
                PropertyID = groupItemData.GroupID,
                ChangeType = (int)EnumGroupLogType.删除,
                Status = (int)EnumGroupLogStatus.无需审批,
                ChangeTime = DateTime.Now,
                ChangeUserID = CurrentUser?.UserID ?? Guid.Empty,
                ChangeUserName = CurrentUser?.UserName ?? "",
                ChangeUserPhone = CurrentUser?.UserPhone ?? "",
                Content = $"属性组:{groupData.Name}-{groupItemData.Name} 变更字段:{Consts.GroupItem_Name};{Consts.GroupItem_Code}",
            };
            var itemChangeLogs = new List<PropertyChangeItemDto>
            {
                new PropertyChangeItemDto
                {
                    LogID=groupChangeLog.ID,
                    PropertyID=groupItemData.ID,
                    FieldName=Consts.GroupItem_Name,
                    FieldCode=nameof(groupItemData.Name),
                    OldValue=groupItemData.Name,
                    NewValue=""
                },
                new PropertyChangeItemDto
                {
                    LogID=groupChangeLog.ID,
                    PropertyID=groupItemData.ID,
                    FieldName=Consts.GroupItem_Code,
                    FieldCode=nameof(groupItemData.Code),
                    OldValue=groupItemData.Code,
                    NewValue=""
                }
            };
            groupChangeLog.ItemChangeLogs = itemChangeLogs;
            if (needApprove)
            {
                groupChangeLog.Status = (int)EnumGroupLogStatus.待审批;
                groupItemData.ApproveStatus = (int)EnumGroupItemApproveStatus.待审批;
                //修改可选项审批状态
                await _propertyGroupService.UpdateGroupItem(groupItemData);
                //创建变更日志
                await _dataApproveService.CreatePropertyChangeLog(groupChangeLog);
            }
            else
            {
                //逻辑删除可选项
                await _propertyGroupService.DeleteGroupItem(groupItemId);

            }
            return true;
        }

        /// <summary>
        /// 获取属性组变更日志
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PropertyChangeLogOutput> GetPropertyChangeLog([FromQuery] GroupChangeLogTableQuery queryFilter)
        {
            var resultData = new PropertyChangeLogOutput();
            var newQueryFilter = new PropertyChangeLogTableQuery
            {
                PropertyID = queryFilter.GroupId,
                PropertyType = (int)EnumLogPropertyType.属性组,
                LogStatus = new List<int> { (int)EnumGroupLogStatus.通过, (int)EnumGroupLogStatus.无需审批 },
                Keyword = queryFilter.Keyword
            };
            var pagedList = await _dataApproveService.QueryPropertyChangeLogTableData(newQueryFilter);
            resultData.Logs = pagedList.Adapt<PagedList<ChangeLogMain>>();
            var logIds = resultData.Logs.Items.Select(x => x.ID).ToList();
            var logItems = await _dataApproveService.GetPropertyGroupChangeLogDetailItems(logIds);
            foreach (var log in resultData.Logs.Items)
            {
                var mapItems = logItems.Where(x => x.LogId == log.ID).ToList();
                var detailMessage = mapItems.Select(x => $"{x.FieldName}：{this.FormatShowVal(x.OldValue)}=>{this.FormatShowVal(x.NewValue)}").ToList();
                log.Content = $"{((EnumPropertyLogType)log.ChangeType).ToString()}分组，{string.Join("，", detailMessage)}";
            }
            return resultData;
        }

        private string FormatShowVal(string val)
        {
            return string.IsNullOrWhiteSpace(val) ? "无" : val;
        }

        /// <summary>
        /// 获取属性组变更详情
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        public async Task<List<ChangeLogItem>> GetPropertyChangeLogItems(Guid logId)
        {
            var logItems = await _dataApproveService.GetPropertyChangeLogDetailItems(logId);
            var targetItems = logItems.Adapt<List<ChangeLogItem>>();
            return targetItems;
        }

        /// <summary>
        /// 获取组织机构
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysOrgOutput>> GetOrgList()
        {
            var orgList = await _sysOrgService.GetOrgTree();
            return orgList;
        }

        /// <summary>
        /// 创建组织机构
        /// </summary>
        /// <param name="orgData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> CreateOrg([FromBody] OrgEditInput orgData)
        {
            var orgDto = orgData.Adapt<OrgEditDto>();
            var orgId = await _sysOrgService.CreateOrg(orgDto);
            return orgId;
        }

        /// <summary>
        /// 修改组织机构
        /// </summary>
        /// <param name="orgData"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<Guid> UpdateOrg([FromBody] OrgEditInput orgData)
        {
            var orgDto = orgData.Adapt<OrgEditDto>();
            var orgId = await _sysOrgService.UpdateOrg(orgDto);
            return orgId;
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<bool> DeleteOrg(Guid id)
        {
            var result = await _sysOrgService.DeleteOrg(id);
            return result;
        }

        #region 地区获取
        /// <summary>
        /// 获取地区
        /// </summary>
        /// <returns></returns>
        public async Task<List<RegionOutput>> GetRegionList()
        {
            var orgList = await _regionService.GetRegionTree();
            return orgList;
        }
        #endregion
    }
}
