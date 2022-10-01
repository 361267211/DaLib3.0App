/*********************************************************
* 名    称：PropertyGroupService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性组管理
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Extensions;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 属性组服务
    /// </summary>
    public class PropertyGroupService : IPropertyGroupService
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<PropertyGroupItem> _propertyGroupItemRepository;
        private readonly IRepository<PropertyGroup> _propertyGroupRepository;
        private readonly IRepository<Property> _propertyRepository;

        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="idGenerator"></param>
        /// <param name="propertyGroupItemRepository"></param>
        /// <param name="propertyGroupRepository"></param>
        /// <param name="propertyRepository"></param>
        public PropertyGroupService(
            IDistributedIDGenerator idGenerator
            , IRepository<PropertyGroupItem> propertyGroupItemRepository
            , IRepository<PropertyGroup> propertyGroupRepository
            , IRepository<Property> propertyRepository)
        {
            _idGenerator = idGenerator;
            _propertyGroupItemRepository = propertyGroupItemRepository;
            _propertyGroupRepository = propertyGroupRepository;
            _propertyRepository = propertyRepository;
        }

        #region 属性组
        /// <summary>
        /// 获取属性组列表数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyGroupListItemDto>> QueryListData()
        {
            var groupQuery = _propertyGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag).OrderBy(x => x.CreateTime).Select(x => new PropertyGroupListItemDto
            {
                ID = x.Id,
                Sort = 0,
                Name = x.Name,
                Code = x.Code,
                Type = x.Type,
                SysBuildIn = x.SysBuildIn,
                Count = _propertyGroupItemRepository.DetachedEntities.Count(i => !i.DeleteFlag && i.Status == (int)EnumGroupItemDataStatus.正常 && i.GroupID == x.Id)
            });

            var groupList = groupQuery.ToList();
            var sortNo = 1;
            groupList.ForEach(x =>
            {
                x.Sort = sortNo++;
            });
            return await Task.FromResult(groupList);
        }
        /// <summary>
        /// 获取属性组对象
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<PropertyGroupListItemDto> GetGroupById(Guid groupId)
        {
            var groupEntity = await _propertyGroupRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == groupId);
            if (groupEntity == null)
            {
                throw Oops.Oh("未找到属性对象");
            }
            var groupDto = groupEntity.Adapt<PropertyGroupListItemDto>();
            return groupDto;
        }

        #endregion

        #region 属性组选项
        /// <summary>
        /// 获取属性组可选项列表
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<PropertyGroupDto> QueryGroupItemList(Guid groupId)
        {
            var propertyGroup = await _propertyGroupRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == groupId);
            if (propertyGroup == null)
            {
                throw Oops.Oh("未找到属性组对象");
            }

            var groupItemList = await _propertyGroupItemRepository.DetachedEntities.Where(i => !i.DeleteFlag && i.GroupID == groupId)
                .OrderBy(x => x.CreateTime).ProjectToType<PropertyGroupItemDto>().ToListAsync();
            var groupDto = propertyGroup.Adapt<PropertyGroupDto>();
            var groupItems = groupItemList.Adapt<List<PropertyGroupItemDto>>();
            groupDto.Items = groupItems;
            return groupDto;
        }

        /// <summary>
        /// 获取属性组选项
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<PropertyGroupItemDto>> QueryGroupItemListByKeyword(PropertyGroupItemTableQuery queryFilter)
        {
            var propertyGroup = _propertyGroupRepository.DetachedEntities.FirstOrDefault(x => !x.DeleteFlag && x.Code == queryFilter.GroupCode);
            if (propertyGroup == null)
            {
                return new PagedList<PropertyGroupItemDto>();
            }
            var groupItemQuery = _propertyGroupItemRepository.DetachedEntities.Where(i => !i.DeleteFlag && i.GroupID == propertyGroup.Id)
                .Where(!queryFilter.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(queryFilter.Keyword))
                .OrderBy(x => x.CreateTime).ProjectToType<PropertyGroupItemDto>();
            var groupItemList = await groupItemQuery.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return groupItemList;
        }

        /// <summary>
        /// 查询所有属性组选项
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyGroupSelectDto>> GetPropertyGroupSelect()
        {
            var properties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).ToListAsync();
            var groupIds = properties.Select(x => x.PropertyGroupID).ToList();
            var allGroups = await _propertyGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.Id)).OrderBy(x => x.CreateTime).ToListAsync();
            var allGroupItems = await _propertyGroupItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.GroupID) && x.Status == (int)EnumGroupItemDataStatus.正常).OrderBy(x => x.CreateTime).ToListAsync();
            var groupSelect = allGroups.Select(x => new PropertyGroupSelectDto { GroupCode = x.Code, GroupItems = allGroupItems.Where(i => i.GroupID == x.Id).Select(i => new DictItem<string, string> { Key = i.Name, Value = x.RequiredCode ? i.Code : i.Name }).ToList() }).ToList();
            //特殊处理读者状态和卡状态
            var userStatusItem = groupSelect.FirstOrDefault(x => x.GroupCode == "User_Status");
            if (userStatusItem != null)
            {
                groupSelect.Remove(userStatusItem);
            }
            var cardStatusItem = groupSelect.FirstOrDefault(x => x.GroupCode == "Card_Status");
            if (cardStatusItem != null)
            {
                groupSelect.Remove(cardStatusItem);
            }
            var sourceFromItem = groupSelect.FirstOrDefault(x => x.GroupCode == "User_SourceFrom");
            if (sourceFromItem != null)
            {
                groupSelect.Remove(sourceFromItem);
            }
            groupSelect.Add(new PropertyGroupSelectDto { GroupCode = "User_SourceFrom", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserSourceFrom)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            groupSelect.Add(new PropertyGroupSelectDto { GroupCode = "User_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            groupSelect.Add(new PropertyGroupSelectDto { GroupCode = "Card_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });

            return groupSelect;
        }

        /// <summary>
        /// 获取可选项数据
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<PropertyGroupItemDto> GetGroupItemById(Guid itemId)
        {
            var groupItemEntity = await _propertyGroupItemRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == itemId);
            if (groupItemEntity == null)
            {
                throw Oops.Oh("未找到属性选项对象");
            }
            var groupItemDto = groupItemEntity.Adapt<PropertyGroupItemDto>();
            return groupItemDto;
        }

        /// <summary>
        /// 创建属性组选项
        /// </summary>
        /// <param name="groupItem"></param>
        /// <returns></returns>
        public async Task<Guid> CreateGroupItem(PropertyGroupItemDto groupItem)
        {
            groupItem.ID = groupItem.ID == Guid.Empty ? new Guid(_idGenerator.Create().ToString()) : groupItem.ID;
            var groupItemEntity = groupItem.Adapt<PropertyGroupItem>();
            var entityEntry = await _propertyGroupItemRepository.InsertAsync(groupItemEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 编辑属性组选项
        /// </summary>
        /// <param name="groupItem"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateGroupItem(PropertyGroupItemDto groupItem)
        {
            var groupItemEntity = await _propertyGroupItemRepository.FirstOrDefaultAsync(x => x.Id == groupItem.ID);
            if (groupItemEntity == null)
            {
                throw Oops.Oh("未找到属性组选项对象");
            }
            if (groupItemEntity.DeleteFlag)
            {
                throw Oops.Oh("属性组选项已删除，不能修改");
            }
            if (groupItemEntity.ApproveStatus == (int)EnumGroupItemApproveStatus.待审批)
            {
                throw Oops.Oh("属性组选项审批中，不能编辑");
            }
            groupItemEntity = groupItem.Adapt(groupItemEntity);
            var entityEntry = await _propertyGroupItemRepository.UpdateAsync(groupItemEntity);
            return entityEntry.Entity.Id;
        }

        /// <summary>
        /// 删除属性组选项
        /// </summary>
        /// <param name="groupItemId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteGroupItem(Guid groupItemId)
        {
            var groupItemEntity = await _propertyGroupItemRepository.FirstOrDefaultAsync(x => x.Id == groupItemId);
            if (groupItemEntity == null)
            {
                throw Oops.Oh("未找到属性组选项对象");
            }
            groupItemEntity.DeleteFlag = true;
            var entityEntry = await _propertyGroupItemRepository.UpdateAsync(groupItemEntity);
            return true;
        }

        /// <summary>
        /// 获取需要编码映射的选项
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyGroupDto>> GetMapCodePropertyGroupList()
        {
            var groupDataList = await _propertyGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.RequiredCode).OrderBy(x => x.CreateTime).ToListAsync();
            var groupDataIds = groupDataList.Select(x => x.Id).ToList();
            var groupDataItemList = await _propertyGroupItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupDataIds.Contains(x.GroupID)).ToListAsync();
            var groupDatas = groupDataList.Adapt<List<PropertyGroupDto>>();
            var groupDataItems = groupDataItemList.Adapt<List<PropertyGroupItemDto>>();
            groupDatas.ForEach(x =>
            {
                x.Items = groupDataItems.Where(i => i.GroupID == x.Id).ToList();
            });
            return groupDatas;
        }

        #endregion
    }
}
