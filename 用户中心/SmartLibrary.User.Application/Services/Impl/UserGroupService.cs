/*********************************************************
* 名    称：UserGroupService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组操作服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Dtos.UserGroup;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 用户组服务
    /// </summary>
    public class UserGroupService : IUserGroupService
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<UserGroup> _userGroupRepository;
        private readonly IRepository<PropertyGroupRule> _groupRuleRepository;
        private readonly IRepository<Property> _propertyRepository;
        private readonly IRepository<PropertyGroup> _propertyGroupRepository;
        private readonly IRepository<PropertyGroupItem> _propertyGroupItemRepository;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly IRepository<SchedulerEntity> _scheduleRepository;
        private readonly TenantInfo _tenantInfo;
        private readonly IBasicConfigService _basicConfigService;


        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="idGenerator"></param>
        /// <param name="groupRepository"></param>
        /// <param name="userGroupRepository"></param>
        /// <param name="groupRuleRepository"></param>
        /// <param name="propertyRepository"></param>
        /// <param name="propertyGroupRepository"></param>
        /// <param name="propertyGroupItemRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="scheduleRepository"></param>
        /// <param name="tenantInfo"></param>
        /// <param name="basicConfigService"></param>
        public UserGroupService(IDistributedIDGenerator idGenerator
            , IRepository<Group> groupRepository
            , IRepository<UserGroup> userGroupRepository
            , IRepository<PropertyGroupRule> groupRuleRepository
            , IRepository<Property> propertyRepository
            , IRepository<PropertyGroup> propertyGroupRepository
            , IRepository<PropertyGroupItem> propertyGroupItemRepository
            , IRepository<EntityFramework.Core.Entitys.User> userRepository
            , IRepository<SchedulerEntity> scheduleRepository
            , TenantInfo tenantInfo
            , IBasicConfigService basicConfigService)
        {
            _idGenerator = idGenerator;
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
            _groupRuleRepository = groupRuleRepository;
            _propertyRepository = propertyRepository;
            _propertyGroupRepository = propertyGroupRepository;
            _propertyGroupItemRepository = propertyGroupItemRepository;
            _userRepository = userRepository;
            _scheduleRepository = scheduleRepository;
            _tenantInfo = tenantInfo;
            _basicConfigService = basicConfigService;
        }

        /// <summary>
        /// 获取初始化数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInitData()
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.UserInfoConfirm;
            var groupData = new GroupEditDto();
            var sourceFrom = EnumHelper.GetEnumDictionaryItems(typeof(EnumGroupSourceFrom));
            var properties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).ToListAsync();
            var groupIds = properties.Select(x => x.PropertyGroupID).ToList();
            var allGroups = _propertyGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.Id)).OrderBy(x => x.CreateTime).ToList();
            var allGroupItems = _propertyGroupItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.GroupID) && x.Status == (int)EnumGroupItemDataStatus.正常).OrderBy(x => x.CreateTime).ToList();
            var groupSelect = allGroups.Select(x => new { GroupCode = x.Code, GroupItems = allGroupItems.Where(i => i.GroupID == x.Id).Select(i => new DictItem<string, string> { Key = i.Name, Value = x.RequiredCode ? i.Code : i.Name }).ToList() }).ToList();

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
            var userSourceFrom = groupSelect.FirstOrDefault(x => x.GroupCode == "User_SourceFrom");
            if (userSourceFrom != null)
            {
                groupSelect.Remove(userSourceFrom);
            }
            //添加读者数据来源
            groupSelect.Add(new { GroupCode = "User_SourceFrom", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserSourceFrom)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            groupSelect.Add(new { GroupCode = "User_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            groupSelect.Add(new { GroupCode = "Card_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            var properties_All = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常).ToListAsync();
            var properties_ForUserField = properties_All.Where(x => x.ForReader && x.SysBuildIn && x.Type == (int)EnumPropertyType.属性组).OrderBy(x => x.CreateTime).Select(x => new { Id = x.Id, Name = x.Name, Code = x.Code, External = !x.SysBuildIn, Type = x.Type }).ToList();

            var dynamicData = new
            {
                groupData,
                sourceFrom,
                userProperties = properties_ForUserField,
                groupSelect,
                needApprove
            };
            return dynamicData;
        }

        /// <summary>
        /// 查询用户组定义列表
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<GroupListItemOutput>> QueryTableQuery(GroupTableQuery queryFilter)
        {
            var userGroupQuery = from userGroup in _userGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                 join user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag) on userGroup.UserID equals user.Id into users
                                 from user in users
                                 select new
                                 {
                                     GroupID = userGroup.GroupID,
                                     UserID = userGroup.UserID
                                 };
            var groupQuery = _groupRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                            .Where(!string.IsNullOrWhiteSpace(queryFilter.Keyword), x => x.Name != "" && x.Name.Contains(queryFilter.Keyword))
                            .Where(queryFilter.CreateStartTime.HasValue, x => x.CreateTime > queryFilter.CreateStartTime)
                            .Where(queryFilter.CreateEndTime.HasValue, x => x.CreateTime <= queryFilter.CreateEndCompareTime)
                            .Select(x => new GroupListItemOutput
                            {
                                ID = x.Id,
                                Name = x.Name,
                                SourceFrom = x.SourceFrom,
                                CreateUserName = x.CreateUserName,
                                CreateTime = x.CreateTime,
                                UserCount = userGroupQuery.Where(u => u.GroupID == x.Id).Select(u => u.UserID).Distinct().Count()
                            });

            var pageList = await groupQuery.OrderByDescending(x => x.CreateTime).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }
        /// <summary>
        /// 获取用户组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GroupInfoDto> GetByID(Guid id)
        {
            var groupInfo = await _groupRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (groupInfo == null)
            {
                throw Oops.Oh("未找到用户组信息");
            }
            var groupDto = groupInfo.Adapt<GroupInfoDto>();
            if (groupDto.SourceFrom == (int)EnumGroupSourceFrom.规则创建)
            {
                var groupRules = await _groupRuleRepository.Where(x => !x.DeleteFlag && x.GroupID == groupInfo.Id).ToListAsync();
                groupDto.Rules = groupRules.Adapt<List<PropertyGroupRuleDto>>().OrderBy(x => x.Sort).ToList();
            }
            if (groupDto.SourceFrom == (int)EnumGroupSourceFrom.导入)
            {
                var userInfoQuery = from userGroup in _userGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.GroupID == groupInfo.Id)
                                    join user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag) on userGroup.UserID equals user.Id into users
                                    from user in users
                                    select new GroupUserInfoDto
                                    {
                                        ID = userGroup.Id,
                                        UserId = user.Id,
                                        UserName = user.Name,
                                        SourceFrom = user.SourceFrom,
                                        Phone = user.Phone
                                    };
                groupDto.UserInfos = await userInfoQuery.OrderBy(x => x.ID).ToListAsync();
            }
            return groupDto;
        }

        /// <summary>
        /// 获取用户组简要信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GroupBriefInfoOutput> GetBriefInfoByID(Guid id)
        {
            var groupInfo = await _groupRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (groupInfo == null)
            {
                throw Oops.Oh("未找到用户组信息");
            }
            var groupDto = groupInfo.Adapt<GroupBriefInfoOutput>();
            if (groupDto.SourceFrom == (int)EnumGroupSourceFrom.规则创建)
            {
                var groupRules = await _groupRuleRepository.Where(x => !x.DeleteFlag && x.GroupID == groupInfo.Id).ToListAsync();
                var propertyIds = groupRules.Select(x => x.PropertyId).ToList();
                var properties = await _propertyRepository.Where(x => propertyIds.Contains(x.Id)).Select(x => new { x.Id, x.Name }).ToListAsync();
                groupDto.Rules = groupRules.Adapt<List<PropertyGroupRuleDto>>().OrderBy(x => x.Sort).ToList();
                groupDto.Rules.ForEach(x =>
                {
                    var mapProperty = properties.FirstOrDefault(d => d.Id == x.PropertyId);
                    x.PropertyName = mapProperty != null ? mapProperty.Name : "";
                });

            }
            groupDto.LastSyncTime = groupDto.LastSyncTime.HasValue && groupDto.LastSyncTime > new DateTime(1970, 1, 1) ? groupDto.LastSyncTime.Value : groupInfo.UpdateTime.DateTime;
            groupDto.TotalCount = await _userRepository.DetachedEntities.Where(x => !x.DeleteFlag).CountAsync();
            var userInfoQuery = from userGroup in _userGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.GroupID == groupInfo.Id)
                                join user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag) on userGroup.UserID equals user.Id into users
                                from user in users
                                select new GroupUserInfoDto
                                {
                                    ID = userGroup.Id,
                                    UserId = user.Id,
                                    UserName = user.Name,
                                    SourceFrom = user.SourceFrom,
                                    Phone = user.Phone
                                };
            groupDto.Count = await userInfoQuery.Select(x => x.UserId).Distinct().CountAsync();
            return groupDto;
        }

        /// <summary>
        /// 通过导入用户数据获取用户
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <returns></returns>
        public async Task<List<SimpleUserListItemDto>> QueryUserListBySearchInfo(List<UserImportSearchDto> searchInfo)
        {
            var IdCards = searchInfo.Select(x => x.IdCard.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var phones = searchInfo.Select(x => x.Phone.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var userQuery = from user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag && ((x.Phone != null && x.Phone != "") || (x.IdCard != null && x.IdCard != "")))
                            .Where(x => IdCards.Contains(x.IdCard) || phones.Contains(x.Phone))
                            select new SimpleUserListItemDto
                            {
                                UserKey = user.UserKey,
                                ID = user.Id,
                                IdCard = user.IdCard,
                                Phone = user.Phone,
                                Name = user.Name,
                                NickName = user.NickName,
                                StudentNo = user.StudentNo,
                                Unit = user.Unit,
                                Edu = user.Edu,
                                Title = user.Title,
                                Depart = user.Depart,
                                College = user.College,
                                Major = user.Major,
                                Grade = user.Grade,
                                Class = user.Class,
                                Type = user.Type,
                                Status = user.Status,
                            };
            var userList = await userQuery.ToListAsync();

            var outUserList = new List<SimpleUserListItemDto>();
            searchInfo.ForEach(x =>
            {

                var mapUser = userList.FirstOrDefault(d => !string.IsNullOrWhiteSpace(d.IdCard) && d.IdCard == x.IdCard);
                if (mapUser == null)
                {
                    mapUser = userList.FirstOrDefault(d => !string.IsNullOrWhiteSpace(d.Phone) && !string.IsNullOrWhiteSpace(d.Name) && d.Phone == x.Phone && d.Name == x.Name);
                }
                if (mapUser != null && (!outUserList.Any(d => d.ID == mapUser.ID)))
                {
                    outUserList.Add(mapUser);
                }
            });
            return outUserList;
        }
        /// <summary>
        /// 创建用户组
        /// </summary>
        /// <param name="groupEditData"></param>
        /// <returns></returns>
        public async Task<Guid> Create(GroupEditDto groupEditData)
        {
            groupEditData.ID = _idGenerator.CreateGuid();
            var groupEntity = groupEditData.Adapt<Group>();
            var groupEntry = await _groupRepository.InsertNowAsync(groupEntity);
            var groupId = groupEntry.Entity.Id;
            if (groupEditData.Rules.Any() && groupEditData.SourceFrom == (int)EnumGroupSourceFrom.规则创建)
            {
                var propertyIds = groupEditData.Rules.Select(x => x.PropertyId).ToList();
                var properties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && propertyIds.Contains(x.Id)).ToListAsync();
                groupEditData.Rules.ForEach(x =>
                {
                    var mapProperty = properties.FirstOrDefault(p => p.Id == x.PropertyId);
                    if (mapProperty != null)
                    {
                        x.PropertyCode = mapProperty.Code;
                    }
                });
                var groupRules = groupEditData.Rules.Adapt<List<PropertyGroupRule>>();
                groupRules.ForEach(x =>
                {
                    x.GroupID = groupId;
                    x.Id = _idGenerator.CreateGuid();
                });
                await _groupRuleRepository.InsertAsync(groupRules);
                //创建后台任务
                var scheduleEntity = new SchedulerEntity
                {
                    TenantId = _tenantInfo.Name,
                    JobName = SyncUserGroupConst.JobName,
                    JobGroup = SyncUserGroupConst.GroupName,
                    Cron = SyncUserGroupConst.Cron,
                    AssemblyFullName = SyncUserGroupConst.AssemblyFullName,
                    ClassFullName = SyncUserGroupConst.ClassFullName,
                    TaskParam = JsonConvert.SerializeObject(new SyncUserGroupParamsDto { GroupID = groupId }),
                    AdapterAssemblyFullName = "",
                    AdapterClassFullName = "",
                    AdapterParm = "",
                    JobStatus = (int)EnumScheduleJobStatus.RUN,
                    CreatedTime = DateTime.Now,
                    BeginTime = DateTime.Now,
                    IntervalSecond = 0,
                    IsRepeat = true,
                };
                var scheduleEntry = await _scheduleRepository.InsertNowAsync(scheduleEntity);
                var scheduleId = scheduleEntry.Entity.Id;
                groupEntity.RefTaskKey = scheduleId.ToString();
                await _groupRepository.UpdateAsync(groupEntity);
            }
            if (groupEditData.UserIds.Any() && groupEditData.SourceFrom == (int)EnumGroupSourceFrom.导入)
            {
                var groupUsers = groupEditData.UserIds.Select(x => new UserGroup
                {
                    Id = _idGenerator.CreateGuid(),
                    UserID = x,
                    GroupID = groupId,
                });
                await _userGroupRepository.Context.BulkInsertAsync(groupUsers);
            }
            return groupId;
        }

        /// <summary>
        /// 修改用户组
        /// </summary>
        /// <param name="groupEditData"></param>
        /// <returns></returns>
        public async Task<Guid> Update(GroupEditDto groupEditData)
        {
            var groupEntity = await _groupRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == groupEditData.ID);
            if (groupEntity == null)
            {
                throw Oops.Oh("未找到用户组信息");
            }
            if (groupEditData.Rules.Any() && groupEditData.SourceFrom == (int)EnumGroupSourceFrom.规则创建)
            {
                var updateBuilder = _groupRuleRepository.Context.BatchUpdate<PropertyGroupRule>();
                updateBuilder
                    .Set(b => b.DeleteFlag, b => true)
                    .Set(b => b.UpdateTime, b => DateTime.Now)
                    .Where(x => !x.DeleteFlag && x.GroupID == groupEntity.Id);
                await updateBuilder.ExecuteAsync();
                var propertyIds = groupEditData.Rules.Select(x => x.PropertyId).ToList();
                var properties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && propertyIds.Contains(x.Id)).ToListAsync();
                groupEditData.Rules.ForEach(x =>
                {
                    var mapProperty = properties.FirstOrDefault(p => p.Id == x.PropertyId);
                    if (mapProperty != null)
                    {
                        x.PropertyCode = mapProperty.Code;
                    }
                });
                var groupRules = groupEditData.Rules.Adapt<List<PropertyGroupRule>>();
                groupRules.ForEach(x =>
                {
                    x.GroupID = groupEntity.Id;
                    x.Id = _idGenerator.CreateGuid();
                });
                await _groupRuleRepository.InsertAsync(groupRules);
                if (groupEntity.SourceFrom == (int)EnumGroupSourceFrom.导入)//以前导入，现在规则，创建任务
                {
                    //创建后台任务
                    var scheduleEntity = new SchedulerEntity
                    {
                        TenantId = _tenantInfo.Name,
                        JobName = SyncUserGroupConst.JobName,
                        JobGroup = SyncUserGroupConst.GroupName,
                        Cron = SyncUserGroupConst.Cron,
                        AssemblyFullName = SyncUserGroupConst.AssemblyFullName,
                        ClassFullName = SyncUserGroupConst.ClassFullName,
                        TaskParam = JsonConvert.SerializeObject(new SyncUserGroupParamsDto { GroupID = groupEntity.Id }),
                        AdapterAssemblyFullName = "",
                        AdapterClassFullName = "",
                        AdapterParm = "",
                        JobStatus = (int)EnumScheduleJobStatus.RUN,
                        CreatedTime = DateTime.Now,
                        BeginTime = DateTime.Now,
                        IntervalSecond = 0,
                        IsRepeat = true,
                    };
                    var scheduleEntry = await _scheduleRepository.InsertNowAsync(scheduleEntity);
                    var scheduleId = scheduleEntry.Entity.Id;
                    groupEntity.RefTaskKey = scheduleId.ToString();
                    await _groupRepository.UpdateAsync(groupEntity);
                }
            }
            if (groupEditData.UserIds.Any() && groupEditData.SourceFrom == (int)EnumGroupSourceFrom.导入)
            {
                var updateBuilder = _userGroupRepository.Context.BatchUpdate<UserGroup>();
                updateBuilder
                    .Set(b => b.DeleteFlag, b => true)
                    .Set(b => b.UpdateTime, b => DateTime.Now)
                    .Where(x => !x.DeleteFlag && x.GroupID == groupEntity.Id);
                await updateBuilder.ExecuteAsync();
                var groupUsers = groupEditData.UserIds.Select(x => new UserGroup
                {
                    Id = _idGenerator.CreateGuid(),
                    UserID = x,
                    GroupID = groupEntity.Id
                });
                await _userGroupRepository.Context.BulkInsertAsync(groupUsers);
                var refTaskId = (int)DataConverter.ToInt(groupEntity.RefTaskKey);
                var refTaskEntity = await _scheduleRepository.FirstOrDefaultAsync(x => x.IsDelete == 0 && x.Id == refTaskId);
                if (refTaskEntity != null)
                {
                    refTaskEntity.IsDelete = 1;
                    await _scheduleRepository.UpdateAsync(refTaskEntity);
                }
            }
            return groupEntity.Id;
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid groupId)
        {
            var groupEntity = await _groupRepository.FirstOrDefaultAsync(x => x.Id == groupId);
            if (groupEntity == null)
            {
                throw Oops.Oh("未找到用户组");
            }
            if (groupEntity.DeleteFlag)
            {
                return true;
            }
            groupEntity.DeleteFlag = true;
            if (groupEntity.SourceFrom == (int)EnumGroupSourceFrom.规则创建)
            {
                var groupRules = await _groupRuleRepository.Where(x => x.GroupID == groupEntity.Id).ToListAsync();
                groupRules.ForEach(x =>
                {
                    x.DeleteFlag = true;
                    x.UpdateTime = DateTime.Now;
                });
                await _groupRuleRepository.UpdateAsync(groupRules);
                //标记删除后台任务
                var refTaskId = (int)DataConverter.ToInt(groupEntity.RefTaskKey);
                var refTaskEntity = await _scheduleRepository.FirstOrDefaultAsync(x => x.IsDelete == 0 && x.Id == refTaskId);
                if (refTaskEntity != null)
                {
                    refTaskEntity.IsDelete = 1;
                    await _scheduleRepository.UpdateAsync(refTaskEntity);
                }
            }

            var updateBuilder = _userGroupRepository.Context.BatchUpdate<UserGroup>();
            updateBuilder
                .Set(b => b.DeleteFlag, b => true)
                .Set(b => b.UpdateTime, b => DateTime.Now)
                .Where(x => !x.DeleteFlag && x.GroupID == groupId);
            await updateBuilder.ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 添加用户到用户组
        /// </summary>
        /// <param name="userGroupAddData"></param>
        /// <returns></returns>
        public async Task<bool> AddUserToGroup(UserGroupAddDto userGroupAddData)
        {
            var userIds = await _userRepository.DetachedEntities.Where(x => !x.DeleteFlag && userGroupAddData.UserIds.Contains(x.Id)).Select(x => x.Id).ToListAsync();
            var existIds = await _userGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && userIds.Contains(x.UserID) && x.GroupID == userGroupAddData.GroupId).Select(x => x.UserID).ToListAsync();
            userIds = userIds.Except(existIds).ToList();
            var userGroups = userIds.Select(x => new UserGroup
            {
                Id = _idGenerator.CreateGuid(),
                GroupID = userGroupAddData.GroupId,
                UserID = x
            }).ToList();
            await _userGroupRepository.InsertAsync(userGroups);
            return true;
        }
        /// <summary>
        /// 从用户组删除用户
        /// </summary>
        /// <param name="userGroupDelData"></param>
        /// <returns></returns>
        public async Task<bool> DeleteGroupUsers(UserGroupDelDto userGroupDelData)
        {
            var updateBuilder = _userGroupRepository.Context.BatchUpdate<UserGroup>();

            await updateBuilder
                .Set(s => s.DeleteFlag, s => true)
                .Set(s => s.UpdateTime, s => DateTime.Now)
                .Where(x => !x.DeleteFlag && x.GroupID == userGroupDelData.GroupId && userGroupDelData.UserIds.Contains(x.UserID))
                .ExecuteAsync();

            return true;
        }

    }
}
