/*********************************************************
* 名    称：UserService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户管理服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DataValidation;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.GrpcService.Enum;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Extensions;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vive.Crypto;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : IUserService, IScoped
    {
        private readonly Base64Crypt _baseEncrypt;
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly IRepository<UserProperty> _userPropertyRepository;
        private readonly IRepository<Card> _cardRepository;
        private readonly IRepository<Property> _propertyRepository;
        private readonly IRepository<PropertyGroup> _propertyGroupRepository;
        private readonly IRepository<PropertyGroupItem> _propertyGroupItemRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<UserRegister> _userRegisterRepository;
        private readonly IRepository<UserGroup> _userGroupRepository;
        private readonly IRepository<ReaderEditProperty> _editPropertyRepository;
        private readonly IRepository<SysOrg> _orgRepository;
        private readonly IRepository<Region> _regionRepository;
        private readonly TenantInfo _tenantInfo;
        private readonly ISecurityService _securityService;
        private readonly IBasicConfigService _basicConfigService;

        public UserService(IDistributedIDGenerator idGenerator
            , IRepository<EntityFramework.Core.Entitys.User> userRepository
            , IRepository<Card> cardRepository
            , IRepository<UserProperty> userPropertyRepository
            , IRepository<Property> propertyRepository
            , IRepository<PropertyGroup> propertyGroupRepository
            , IRepository<PropertyGroupItem> propertyGroupItemRepository
            , IRepository<Group> groupRepository
            , IRepository<UserRegister> userRegisterRepository
            , IRepository<UserGroup> userGroupRepository
            , IRepository<ReaderEditProperty> editPropertyRepository
            , IRepository<SysOrg> orgRepository
            , IRepository<Region> regionRepository
            , TenantInfo tenantInfo
            , ISecurityService securityService
            , IBasicConfigService basicConfigService)
        {
            var codeTable = FileHelper.ReadFileToText("EncodeTable.cert");
            _baseEncrypt = new Base64Crypt(codeTable);
            _idGenerator = idGenerator;
            _userRepository = userRepository;
            _userPropertyRepository = userPropertyRepository;
            _propertyRepository = propertyRepository;
            _propertyGroupRepository = propertyGroupRepository;
            _propertyGroupItemRepository = propertyGroupItemRepository;
            _cardRepository = cardRepository;
            _groupRepository = groupRepository;
            _userRegisterRepository = userRegisterRepository;
            _userGroupRepository = userGroupRepository;
            _editPropertyRepository = editPropertyRepository;
            _orgRepository = orgRepository;
            _regionRepository = regionRepository;
            _tenantInfo = tenantInfo;
            _securityService = securityService;
            _basicConfigService = basicConfigService;
        }

        /// <summary>
        /// 获取用户模块初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetUserInitData()
        {
            var configSet = await _basicConfigService.GetBasicConfigSet();
            var needApprove = configSet.UserInfoConfirm;
            var codeProperties = new List<string> { "User_College", "User_CollegeDepart", "User_Type", "Card_Type" };
            var userData = new UserDto { Gender = "男", Status = (int)EnumUserStatus.正常 };
            var cardData = new CardDto { IssueDate = DateTime.Now.Date, ExpireDate = DateTime.Now.Date.AddYears(3), Status = (int)EnumCardStatus.正常 };
            var properties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).ToListAsync();

            var userProperties = properties.Where(x => x.ForReader && !x.SysBuildIn)
                .Select(propertyForUser => new UserPropertyDto
                {
                    PropertyID = propertyForUser.Id,
                    PropertyName = propertyForUser.Name,
                    PropertyCode = propertyForUser.Code,
                    PropertyType = propertyForUser.Type,
                    PropertyValue = "",
                    Required = propertyForUser.Required,
                    PropertyGroupID = propertyForUser.PropertyGroupID ?? Guid.Empty,
                }).ToList();
            userData.Properties = userProperties;
            var exportProperties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常 && !(x.ForCard && !x.ForReader && !x.SysBuildIn)).OrderBy(x => x.CreateTime).Select(x => new ExportPropertyInput
            {
                PropertyCode = x.Code,
                PropertyName = x.Name,
                PropertyType = x.Type,
                External = !x.SysBuildIn
            }).ToListAsync();
            var showOnTableProperties = new List<object> { };
            var showOnTableProperties_All = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常 && x.ShowOnTable).OrderBy(x => x.CreateTime).ToListAsync();
            var showOnTableProperties_ForUserField = showOnTableProperties_All.Where(x => x.ForReader && x.SysBuildIn).OrderBy(x => x.CreateTime).Select(x => new { Name = x.Name, Code = x.Code, External = !x.SysBuildIn, Type = x.Type }).ToList();
            var showOnTableProperties_ForUserProperty = showOnTableProperties_All.Where(x => x.ForReader && !x.SysBuildIn).OrderBy(x => x.CreateTime).Select(x => new { Name = x.Name, Code = x.Code, External = !x.SysBuildIn, Type = x.Type }).ToList();
            //重新排列属性
            showOnTableProperties.AddRange(showOnTableProperties_ForUserField);
            showOnTableProperties.AddRange(new[] {
                new {Name="卡号",Code="Card_No", External = false, Type = 0 },
                new {Name="卡状态",Code="Card_Status", External = false, Type = 4 },
                new {Name="注册日期",Code="User_CreateTime", External = false, Type = 2 },
            });
            showOnTableProperties.AddRange(showOnTableProperties_ForUserProperty);

            var canSearchProperties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常 && x.CanSearch).OrderBy(x => x.CreateTime).Select(x => new { Name = x.Name, Code = x.Code, External = !x.SysBuildIn, Type = x.Type }).ToListAsync();
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
            var sourceFromItem = groupSelect.FirstOrDefault(x => x.GroupCode == "User_SourceFrom");
            if (sourceFromItem != null)
            {
                groupSelect.Remove(sourceFromItem);
            }
            groupSelect.Add(new { GroupCode = "User_SourceFrom", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserSourceFrom)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            groupSelect.Add(new { GroupCode = "User_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            groupSelect.Add(new { GroupCode = "Card_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            var groups = await _groupRepository.DetachedEntities.Where(x => !x.DeleteFlag).OrderBy(x => x.CreateTime).Select(x => new DictItem<string, string> { Key = x.Name, Value = x.Id.ToString() }).ToListAsync();
            groupSelect.Add(new { GroupCode = "User_Groups", GroupItems = groups });
            var securityPolicy = await _securityService.GetSecurityPolicy();
            var dynamicData = new
            {
                userData,
                cardData,
                showOnTableProperties,
                canSearchProperties,
                groupSelect = groupSelect.ToList(),
                userAuthAppType = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserAuthAppType)),
                exportProperties,
                CardClaimStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardClaimStatus)),
                secretLevel = securityPolicy.SecretLevel,
                needApprove
            };
            return dynamicData;
        }
        /// <summary>
        /// 获取读者高级查询条件
        /// </summary>
        /// <returns></returns>
        public async Task<List<SearchPropertyDto>> GetCanSearchPropertyList()
        {
            var canSearchProperties = await _propertyRepository.DetachedEntities
                .Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常 && x.CanSearch).OrderBy(x => x.CreateTime)
                .Select(x => new SearchPropertyDto { Name = x.Name, Code = x.Code, External = !x.SysBuildIn, Type = x.Type }).ToListAsync();
            return canSearchProperties;
        }
        /// <summary>
        /// 获取读者模块初始数据
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetReaderInitData()
        {
            var userData = new UserDto();
            var properties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).ToListAsync();
            var groupIds = properties.Select(x => x.PropertyGroupID).ToList();
            var allGroups = _propertyGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.Id)).ToList();
            var allGroupItems = _propertyGroupItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.GroupID) && x.Status == (int)EnumGroupItemDataStatus.正常).OrderBy(x => x.CreateTime).ToList();
            var groupSelect = allGroups.Select(x => new { GroupCode = x.Code, GroupItems = allGroupItems.Where(i => i.GroupID == x.Id).Select(i => new DictItem<string, string> { Key = i.Name, Value = x.RequiredCode ? i.Code : i.Name }).ToList() }).ToList();
            //添加读者数据来源
            groupSelect.Add(new { GroupCode = "User_SourceFrom", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserSourceFrom)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
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
            groupSelect.Add(new { GroupCode = "User_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            groupSelect.Add(new { GroupCode = "Card_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            var readerEditProperties = _editPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.IsCheck).Select(x => new { x.PropertyCode }).ToList();
            var dynamicData = new
            {
                userData,
                groupSelect = groupSelect,
                readerEditProperties,
                CardClaimStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardClaimStatus)),
            };
            return dynamicData;
        }

        /// <summary>
        /// 获取数组可选项
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyGroupSelectDto>> GetGroupSelectItem()
        {
            var allGroups = _propertyGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag).ToList();
            var allGroupItems = _propertyGroupItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumGroupItemDataStatus.正常).ToList();
            var groupSelect = allGroups.Select(x => new PropertyGroupSelectDto { GroupCode = x.Code, GroupItems = allGroupItems.Where(i => i.GroupID == x.Id).Select(i => new DictItem<string, string> { Key = i.Name, Value = x.RequiredCode ? i.Code : i.Name }).ToList() }).ToList();
            //添加读者数据来源
            groupSelect.Add(new PropertyGroupSelectDto { GroupCode = "User_SourceFrom", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserSourceFrom)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
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
            groupSelect.Add(new PropertyGroupSelectDto { GroupCode = "User_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            groupSelect.Add(new PropertyGroupSelectDto { GroupCode = "Card_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            var groups = await _groupRepository.DetachedEntities.Where(x => !x.DeleteFlag).OrderBy(x => x.CreateTime).Select(x => new DictItem<string, string> { Key = x.Name, Value = x.Id.ToString() }).ToListAsync();
            groupSelect.Add(new PropertyGroupSelectDto { GroupCode = "User_Groups", GroupItems = groups });
            return groupSelect;
        }

        /// <summary>
        /// 馆员数据初始化
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetStaffInitData()
        {
            var staffStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumStaffStatus));
            var properties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).ToListAsync();
            var groupIds = properties.Select(x => x.PropertyGroupID).ToList();
            var allGroups = _propertyGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.Id)).ToList();
            var allGroupItems = _propertyGroupItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.GroupID) && x.Status == (int)EnumGroupItemDataStatus.正常).OrderBy(x => x.CreateTime).ToList();
            var groupSelect = allGroups.Select(x => new { GroupCode = x.Code, GroupItems = allGroupItems.Where(i => i.GroupID == x.Id).Select(i => new DictItem<string, string> { Key = i.Name, Value = x.RequiredCode ? i.Code : i.Name }).ToList() }).ToList();
            //添加读者数据来源
            groupSelect.Add(new { GroupCode = "User_SourceFrom", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserSourceFrom)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
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
            groupSelect.Add(new { GroupCode = "User_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumUserStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });
            groupSelect.Add(new { GroupCode = "Card_Status", GroupItems = EnumHelper.GetEnumDictionaryItems(typeof(EnumCardStatus)).Select(i => new DictItem<string, string> { Key = i.Key, Value = i.Value.ToString() }).ToList() });

            var dynamicData = new
            {
                staffStatus,
                groupSelect
            };
            return dynamicData;
        }
        /// <summary>
        /// 查询读者表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<UserListItemDto>> QueryTableData(UserTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<UserTableQuery, UserEncodeTableQuery>(queryFilter);
            var userExternalProperties = from userProperty in _userPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                         join propertyForUser in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForReader && !x.SysBuildIn && x.Status == (int)EnumPropertyDataStatus.正常) on userProperty.PropertyID equals propertyForUser.Id into propertyForUsers
                                         from propertyForUser in propertyForUsers
                                         select new UserPropertyItemDto
                                         {
                                             UserID = userProperty.UserID,
                                             PropertyID = userProperty.PropertyID,
                                             PropertyName = propertyForUser.Name,
                                             PropertyCode = propertyForUser.Code,
                                             PropertyType = propertyForUser.Type,
                                             PropertyValue = userProperty.PropertyValue,
                                             Required = propertyForUser.Required
                                         };


            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Name), x => x.Name.Contains(queryFilter.Name))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.NickName), x => x.NickName != null && x.NickName.Contains(queryFilter.NickName))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.StudentNo), x => x.StudentNo != null && x.StudentNo.Contains(queryFilter.StudentNo))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Unit), x => x.Unit != null && x.Unit.Contains(queryFilter.Unit))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Edu), x => x.Edu != null && x.Edu.Equals(queryFilter.Edu))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Title), x => x.Title != null && x.Title.Equals(queryFilter.Title))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Depart), x => x.Depart != null && x.Depart.StartsWith(queryFilter.Depart))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.College), x => x.College != null && x.College.Equals(queryFilter.College))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.CollegeDepart), x => x.CollegeDepart != null && x.CollegeDepart.Equals(queryFilter.CollegeDepart))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Major), x => x.Major != null && x.Major.Equals(queryFilter.Major))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Grade), x => x.Grade != null && x.Grade.Equals(queryFilter.Grade))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Class), x => x.Class != null && x.Class.Equals(queryFilter.Class))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Type), x => x.Type != null && x.Type.Equals(queryFilter.Type))
                                .Where(queryFilter.Status.HasValue, x => x.Status == queryFilter.Status)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.IDCard), x => x.IdCard != null && x.IdCard.Contains(queryFilter.IDCard))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Phone), x => x.Phone != null && x.Phone.Contains(queryFilter.Phone))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Email), x => x.Email != null && x.Email.Contains(queryFilter.Email))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Gender), x => x.Gender != null && x.Gender.Equals(queryFilter.Gender))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Addr), x => x.Addr != null && x.Addr.Equals(queryFilter.Addr))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.AddrDetail), x => x.AddrDetail != null && x.AddrDetail.Equals(queryFilter.AddrDetail))
                                .Where(queryFilter.SourceFrom.HasValue, x => x.SourceFrom == queryFilter.SourceFrom)
                                .Where(queryFilter.LastLoginStartTime.HasValue, x => x.LastLoginTime >= queryFilter.LastLoginStartTime)
                                .Where(queryFilter.LastLoginEndCompareTime.HasValue, x => x.LastLoginTime < queryFilter.LastLoginEndCompareTime)
                                .Where(queryFilter.LeaveStartTime.HasValue, x => x.LeaveTime >= queryFilter.LeaveStartTime)
                                .Where(queryFilter.LeaveEndCompareTime.HasValue, x => x.LeaveTime < queryFilter.LeaveEndCompareTime)
                                .Where(queryFilter.BirthdayStartTime.HasValue, x => x.Birthday >= queryFilter.BirthdayStartTime)
                                .Where(queryFilter.BirthdayEndCompareTime.HasValue, x => x.Birthday < queryFilter.BirthdayEndCompareTime)
                                .Where(queryFilter.GroupID.HasValue, x => _userGroupRepository.DetachedEntities.Any(g => !g.DeleteFlag && g.GroupID == queryFilter.GroupID && x.Id == g.UserID))
                                join card in _cardRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.IsPrincipal)
                                // .Where(!string.IsNullOrWhiteSpace(queryFilter.CardNo), x => x.No != null && x.No.Contains(queryFilter.CardNo))
                                //.Where(!string.IsNullOrWhiteSpace(queryFilter.CardBarCode), x => x.BarCode != null && x.BarCode.Contains(queryFilter.CardBarCode))
                                //.Where(!string.IsNullOrWhiteSpace(queryFilter.CardPhysicNo), x => x.PhysicNo != null && x.PhysicNo.Contains(queryFilter.CardPhysicNo))
                                //.Where(!string.IsNullOrWhiteSpace(queryFilter.CardIdentityNo), x => x.IdentityNo != null && x.IdentityNo.Contains(queryFilter.CardIdentityNo))
                                //.Where(!string.IsNullOrWhiteSpace(queryFilter.CardType), x => x.Type == queryFilter.CardType)
                                //.Where(queryFilter.CardStatus.HasValue, x => x.Status == queryFilter.CardStatus)
                                //.Where(queryFilter.CardIsPrincipal.HasValue, x => x.IsPrincipal == queryFilter.CardIsPrincipal)
                                //.Where(queryFilter.CardIssueStartTime.HasValue, x => x.IssueDate >= queryFilter.CardIssueStartTime)
                                //.Where(queryFilter.CardIssueEndCompareTime.HasValue, x => x.IssueDate < queryFilter.CardIssueEndCompareTime)
                                //.Where(queryFilter.CardExpireStartTime.HasValue, x => x.ExpireDate >= queryFilter.CardExpireStartTime)
                                //.Where(queryFilter.CardExpireEndCompareTime.HasValue, x => x.ExpireDate < queryFilter.CardExpireEndCompareTime)
                                on user.Id equals card.UserID into cards
                                from card in cards.DefaultIfEmpty()
                                select new UserListItemDto
                                {
                                    ID = user.Id,
                                    Name = user.Name,
                                    NickName = user.NickName,
                                    StudentNo = user.StudentNo,
                                    Unit = user.Unit,
                                    Edu = user.Edu,
                                    Title = user.Title,
                                    Depart = user.Depart,
                                    DepartName = user.DepartName,
                                    College = user.College,
                                    CollegeName = user.CollegeName,
                                    CollegeDepart = user.CollegeDepart,
                                    CollegeDepartName = user.CollegeDepartName,
                                    Major = user.Major,
                                    Grade = user.Grade,
                                    Class = user.Class,
                                    Type = user.Type,
                                    TypeName = user.TypeName,
                                    Status = user.Status,
                                    IdCard = user.IdCard,
                                    Phone = user.Phone,
                                    Email = user.Email,
                                    Birthday = user.Birthday,
                                    Gender = user.Gender,
                                    Addr = user.Addr,
                                    AddrDetail = user.AddrDetail,
                                    Photo = user.Photo,
                                    LeaveTime = user.LeaveTime,
                                    FirstLoginTime = user.FirstLoginTime,
                                    LastLoginTime = user.LastLoginTime,
                                    SourceFrom = user.SourceFrom,
                                    CreateTime = user.CreateTime,
                                    UpdateTime = user.UpdateTime,
                                    CardNo = card != null ? card.No : "",
                                    CardBarCode = card != null ? card.BarCode : "",
                                    CardPhysicNo = card != null ? card.PhysicNo : "",
                                    CardIdentityNo = card != null ? card.IdentityNo : "",
                                    CardStatus = card != null ? card.Status : null,
                                    CardType = card != null ? card.Type : "",
                                    CardIssueDate = card != null ? card.IssueDate : null,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                };
            userCardQuery = userCardQuery.Where(!string.IsNullOrWhiteSpace(queryFilter.CardNo), x => x.CardNo != null && x.CardNo.Contains(queryFilter.CardNo))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.CardBarCode), x => x.CardBarCode != null && x.CardBarCode.Contains(queryFilter.CardBarCode))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.CardPhysicNo), x => x.CardPhysicNo != null && x.CardPhysicNo.Contains(queryFilter.CardPhysicNo))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.CardIdentityNo), x => x.CardIdentityNo != null && x.CardIdentityNo.Contains(queryFilter.CardIdentityNo))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.CardType), x => x.CardType == queryFilter.CardType)
                               .Where(queryFilter.CardStatus.HasValue, x => x.CardStatus == queryFilter.CardStatus)
                               .Where(queryFilter.CardIssueStartTime.HasValue, x => x.CardIssueDate >= queryFilter.CardIssueStartTime)
                               .Where(queryFilter.CardIssueEndCompareTime.HasValue, x => x.CardIssueDate < queryFilter.CardIssueEndCompareTime)
                               .Where(queryFilter.CardExpireStartTime.HasValue, x => x.CardExpireDate >= queryFilter.CardExpireStartTime)
                               .Where(queryFilter.CardExpireEndCompareTime.HasValue, x => x.CardExpireDate < queryFilter.CardExpireEndCompareTime);
            var pageList = await userCardQuery.OrderByDescending(x => x.CreateTime).ThenByDescending(x => x.ID).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            var pageUserIds = pageList.Items.Select(x => x.ID).ToArray();
            var userPropertyList = userExternalProperties.Where(x => pageUserIds.Contains(x.UserID)).ToList();
            foreach (var item in pageList.Items)
            {
                item.Properties = userPropertyList.Where(x => x.UserID == item.ID).ToList();
            }
            return pageList;
        }

        /// <summary>
        /// 获取基础用户列表数据信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemDto>> QuerySimpleInfoTableData(UserTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<UserTableQuery, UserEncodeTableQuery>(queryFilter);
            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Name), x => x.Name.Contains(queryFilter.Name))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.NickName), x => x.NickName != null && x.NickName.Contains(queryFilter.NickName))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.StudentNo), x => x.StudentNo != null && x.StudentNo.Contains(queryFilter.StudentNo))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Unit), x => x.Unit != null && x.Unit.Contains(queryFilter.Unit))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Edu), x => x.Edu != null && x.Edu.Equals(queryFilter.Edu))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Title), x => x.Title != null && x.Title.Equals(queryFilter.Title))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Depart), x => x.Depart != null && x.Depart.StartsWith(queryFilter.Depart))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.College), x => x.College != null && x.College.Equals(queryFilter.College))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Major), x => x.Major != null && x.Major.Equals(queryFilter.Major))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Grade), x => x.Grade != null && x.Grade.Equals(queryFilter.Grade))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Class), x => x.Class != null && x.Class.Equals(queryFilter.Class))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Type), x => x.Type != null && x.Type.Equals(queryFilter.Type))
                               .Where(queryFilter.Status.HasValue, x => x.Status == queryFilter.Status)
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.IDCard), x => x.IdCard != null && x.IdCard.Contains(queryFilter.IDCard))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Phone), x => x.Phone != null && x.Phone.Contains(queryFilter.Phone))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Email), x => x.Email != null && x.Email.Contains(queryFilter.Email))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Gender), x => x.Gender != null && x.Gender.Equals(queryFilter.Gender))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Addr), x => x.Addr != null && x.Addr.Equals(queryFilter.Addr))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.AddrDetail), x => x.AddrDetail != null && x.AddrDetail.Equals(queryFilter.AddrDetail))
                               .Where(queryFilter.SourceFrom.HasValue, x => x.SourceFrom == queryFilter.SourceFrom)
                               .Where(queryFilter.LastLoginStartTime.HasValue, x => x.LastLoginTime >= queryFilter.LastLoginStartTime)
                               .Where(queryFilter.LastLoginEndCompareTime.HasValue, x => x.LastLoginTime < queryFilter.LastLoginEndCompareTime)
                               .Where(queryFilter.LeaveStartTime.HasValue, x => x.LeaveTime >= queryFilter.LeaveStartTime)
                               .Where(queryFilter.LeaveEndCompareTime.HasValue, x => x.LeaveTime < queryFilter.LeaveEndCompareTime)
                               .Where(queryFilter.BirthdayStartTime.HasValue, x => x.Birthday >= queryFilter.BirthdayStartTime)
                               .Where(queryFilter.BirthdayEndCompareTime.HasValue, x => x.Birthday < queryFilter.BirthdayEndCompareTime)
                               .Where(queryFilter.IsStaff.HasValue, x => x.IsStaff == queryFilter.IsStaff)
                               .Where(queryFilter.GroupID.HasValue, x => _userGroupRepository.DetachedEntities.Any(u => !u.DeleteFlag && u.UserID == x.Id && u.GroupID == queryFilter.GroupID))
                                join card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsPrincipal)
                                //.Where(!string.IsNullOrWhiteSpace(queryFilter.CardNo), x => x.No != null && x.No.Contains(queryFilter.CardNo))
                                //.Where(!string.IsNullOrWhiteSpace(queryFilter.CardBarCode), x => x.BarCode != null && x.BarCode.Contains(queryFilter.CardBarCode))
                                //.Where(!string.IsNullOrWhiteSpace(queryFilter.CardPhysicNo), x => x.PhysicNo != null && x.PhysicNo.Contains(queryFilter.CardPhysicNo))
                                //.Where(!string.IsNullOrWhiteSpace(queryFilter.CardIdentityNo), x => x.IdentityNo != null && x.IdentityNo.Contains(queryFilter.CardIdentityNo))
                                //.Where(!string.IsNullOrWhiteSpace(queryFilter.CardType), x => x.Type == queryFilter.CardType)
                                //.Where(queryFilter.CardStatus.HasValue, x => x.Status == queryFilter.CardStatus)
                                //.Where(queryFilter.CardIsPrincipal.HasValue, x => x.IsPrincipal == queryFilter.CardIsPrincipal)
                                //.Where(queryFilter.CardIssueStartTime.HasValue, x => x.IssueDate >= queryFilter.CardIssueStartTime)
                                //.Where(queryFilter.CardIssueEndCompareTime.HasValue, x => x.IssueDate < queryFilter.CardIssueEndCompareTime)
                                //.Where(queryFilter.CardExpireStartTime.HasValue, x => x.ExpireDate >= queryFilter.CardExpireStartTime)
                                //.Where(queryFilter.CardExpireEndCompareTime.HasValue, x => x.ExpireDate < queryFilter.CardExpireEndCompareTime)
                                on user.Id equals card.UserID into cards
                                from card in cards.DefaultIfEmpty()
                                select new SimpleUserListItemDto
                                {
                                    UserKey = user.UserKey,
                                    ID = user.Id,
                                    Name = user.Name,
                                    NickName = user.NickName,
                                    StudentNo = user.StudentNo,
                                    Unit = user.Unit,
                                    Edu = user.Edu,
                                    Title = user.Title,
                                    Depart = user.Depart,
                                    DepartName = user.DepartName,
                                    College = user.College,
                                    CollegeName = user.CollegeName,
                                    CollegeDepart = user.CollegeDepart,
                                    CollegeDepartName = user.CollegeDepartName,
                                    Major = user.Major,
                                    Grade = user.Grade,
                                    Class = user.Class,
                                    Type = user.Type,
                                    TypeName = user.TypeName,
                                    Status = user.Status,
                                    IdCard = user.IdCard,
                                    Phone = user.Phone,
                                    Email = user.Email,
                                    Birthday = user.Birthday,
                                    Gender = user.Gender,
                                    Photo = user.Photo,
                                    LeaveTime = user.LeaveTime,
                                    FirstLoginTime = user.FirstLoginTime,
                                    LastLoginTime = user.LastLoginTime,
                                    SourceFrom = user.SourceFrom,
                                    CreateTime = user.CreateTime,
                                    UpdateTime = user.UpdateTime,
                                    CardNo = card != null ? card.No : "",
                                    CardBarCode = card != null ? card.BarCode : "",
                                    CardPhysicNo = card != null ? card.PhysicNo : "",
                                    CardIdentityNo = card != null ? card.IdentityNo : "",
                                    CardStatus = card != null ? card.Status : null,
                                    CardType = card != null ? card.Type : "",
                                    CardIssueDate = card != null ? card.IssueDate : null,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                };
            userCardQuery = userCardQuery.Where(!string.IsNullOrWhiteSpace(queryFilter.CardNo), x => x.CardNo != null && x.CardNo.Contains(queryFilter.CardNo))
                              .Where(!string.IsNullOrWhiteSpace(queryFilter.CardBarCode), x => x.CardBarCode != null && x.CardBarCode.Contains(queryFilter.CardBarCode))
                              .Where(!string.IsNullOrWhiteSpace(queryFilter.CardPhysicNo), x => x.CardPhysicNo != null && x.CardPhysicNo.Contains(queryFilter.CardPhysicNo))
                              .Where(!string.IsNullOrWhiteSpace(queryFilter.CardIdentityNo), x => x.CardIdentityNo != null && x.CardIdentityNo.Contains(queryFilter.CardIdentityNo))
                              .Where(!string.IsNullOrWhiteSpace(queryFilter.CardType), x => x.CardType == queryFilter.CardType)
                              .Where(queryFilter.CardStatus.HasValue, x => x.CardStatus == queryFilter.CardStatus)
                              .Where(queryFilter.CardIssueStartTime.HasValue, x => x.CardIssueDate >= queryFilter.CardIssueStartTime)
                              .Where(queryFilter.CardIssueEndCompareTime.HasValue, x => x.CardIssueDate < queryFilter.CardIssueEndCompareTime)
                              .Where(queryFilter.CardExpireStartTime.HasValue, x => x.CardExpireDate >= queryFilter.CardExpireStartTime)
                              .Where(queryFilter.CardExpireEndCompareTime.HasValue, x => x.CardExpireDate < queryFilter.CardExpireEndCompareTime);
            var pageList = await userCardQuery.OrderByDescending(x => x.CreateTime).ThenByDescending(x => x.ID).ToPagedListAsync<SimpleUserListItemDto>(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 通过关键字查询用户数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemDto>> QuerySimpleInfoByKeyword(UserTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<UserTableQuery, UserEncodeTableQuery>(queryFilter);
            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Keyword),
                                x => x.Name.Contains(queryFilter.Keyword)
                                || (x.NickName.Contains(queryFilter.Keyword))
                                || (x.IdCard.Contains(queryFilter.Keyword))
                                || (x.Phone.Contains(queryFilter.Keyword)))
                               .Where(queryFilter.IsStaff.HasValue, x => x.IsStaff == queryFilter.IsStaff)
                                join card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsPrincipal)
                                on user.Id equals card.UserID into cards
                                from card in cards.DefaultIfEmpty()
                                select new SimpleUserListItemDto
                                {
                                    ID = user.Id,
                                    Name = user.Name,
                                    NickName = user.NickName,
                                    StudentNo = user.StudentNo,
                                    Unit = user.Unit,
                                    Edu = user.Edu,
                                    Title = user.Title,
                                    Depart = user.Depart,
                                    DepartName = user.DepartName,
                                    College = user.College,
                                    CollegeName = user.CollegeName,
                                    CollegeDepart = user.CollegeDepart,
                                    CollegeDepartName = user.CollegeDepartName,
                                    Major = user.Major,
                                    Grade = user.Grade,
                                    Class = user.Class,
                                    Type = user.Type,
                                    TypeName = user.TypeName,
                                    Status = user.Status,
                                    IdCard = user.IdCard,
                                    Phone = user.Phone,
                                    Email = user.Email,
                                    Birthday = user.Birthday,
                                    Gender = user.Gender,
                                    Photo = user.Photo,
                                    LeaveTime = user.LeaveTime,
                                    FirstLoginTime = user.FirstLoginTime,
                                    LastLoginTime = user.LastLoginTime,
                                    SourceFrom = user.SourceFrom,
                                    CreateTime = user.CreateTime,
                                    UpdateTime = user.UpdateTime,
                                    UserKey = user.UserKey,
                                    CardNo = card != null ? card.No : "",
                                    CardStatus = card != null ? card.Status : null,
                                    CardIssueDate = card != null ? card.IssueDate : null,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                    Usage = card != null ? card.Usage : 0
                                };
            var pageList = await userCardQuery.OrderByDescending(x => x.CreateTime)
                                              .ThenByDescending(x => x.ID)
                                              .ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 通过用户Id获取用户信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<List<SimpleUserListItemDto>> QuerySimpleInfoListByIds(List<Guid> userIds)
        {
            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && userIds.Contains(x.Id))
                                join card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsPrincipal)
                                on user.Id equals card.UserID into cards
                                from card in cards.DefaultIfEmpty()
                                select new SimpleUserListItemDto
                                {
                                    UserKey = user.UserKey,
                                    ID = user.Id,
                                    Name = user.Name,
                                    NickName = user.NickName,
                                    StudentNo = user.StudentNo,
                                    Unit = user.Unit,
                                    Edu = user.Edu,
                                    Title = user.Title,
                                    Depart = user.Depart,
                                    DepartName = user.DepartName,
                                    College = user.College,
                                    CollegeName = user.CollegeName,
                                    CollegeDepart = user.CollegeDepart,
                                    CollegeDepartName = user.CollegeDepartName,
                                    Major = user.Major,
                                    Grade = user.Grade,
                                    Class = user.Class,
                                    Type = user.Type,
                                    TypeName = user.TypeName,
                                    Status = user.Status,
                                    IdCard = user.IdCard,
                                    Phone = user.Phone,
                                    Email = user.Email,
                                    Birthday = user.Birthday,
                                    Gender = user.Gender,
                                    Photo = user.Photo,
                                    LeaveTime = user.LeaveTime,
                                    FirstLoginTime = user.FirstLoginTime,
                                    LastLoginTime = user.LastLoginTime,
                                    SourceFrom = user.SourceFrom,
                                    CreateTime = user.CreateTime,
                                    UpdateTime = user.UpdateTime,
                                    CardNo = card != null ? card.No : "",
                                    CardStatus = card != null ? card.Status : null,
                                    CardIssueDate = card != null ? card.IssueDate : null,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                };
            var userList = await userCardQuery.OrderByDescending(x => x.CreateTime).ThenByDescending(x => x.ID).Take(100).ToListAsync();
            return userList;
        }

        /// <summary>
        /// 通过用户UserKey获取用户信息
        /// </summary>
        /// <param name="userKeys"></param>
        /// <returns></returns>
        public async Task<List<SimpleUserListItemDto>> QuerySimpleInfoListByUserKeys(List<string> userKeys)
        {
            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && userKeys.Contains(x.UserKey))
                                join card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsPrincipal)
                                on user.Id equals card.UserID into cards
                                from card in cards.DefaultIfEmpty()
                                select new SimpleUserListItemDto
                                {
                                    ID = user.Id,
                                    UserKey = user.UserKey,
                                    Name = user.Name,
                                    NickName = user.NickName,
                                    StudentNo = user.StudentNo,
                                    Unit = user.Unit,
                                    Edu = user.Edu,
                                    Title = user.Title,
                                    Depart = user.Depart,
                                    DepartName = user.DepartName,
                                    College = user.College,
                                    CollegeName = user.CollegeName,
                                    CollegeDepart = user.CollegeDepart,
                                    CollegeDepartName = user.CollegeDepartName,
                                    Major = user.Major,
                                    Grade = user.Grade,
                                    Class = user.Class,
                                    Type = user.Type,
                                    TypeName = user.TypeName,
                                    Status = user.Status,
                                    IdCard = user.IdCard,
                                    Phone = user.Phone,
                                    Email = user.Email,
                                    Birthday = user.Birthday,
                                    Gender = user.Gender,
                                    Photo = user.Photo,
                                    LeaveTime = user.LeaveTime,
                                    FirstLoginTime = user.FirstLoginTime,
                                    LastLoginTime = user.LastLoginTime,
                                    SourceFrom = user.SourceFrom,
                                    CreateTime = user.CreateTime,
                                    UpdateTime = user.UpdateTime,
                                    CardNo = card != null ? card.No : "",
                                    CardStatus = card != null ? card.Status : null,
                                    CardIssueDate = card != null ? card.IssueDate : null,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                };
            var userList = await userCardQuery.OrderByDescending(x => x.CreateTime).ThenByDescending(x => x.ID).Take(100).ToListAsync();
            return userList;
        }

        /// <summary>
        /// 查询馆员表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<StaffListItemDto>> QueryStaffTableData(StaffTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<StaffTableQuery, StaffEncodeTableQuery>(queryFilter);
            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsStaff)
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Name), x => x.Name.Contains(queryFilter.Name))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Depart), x => x.Depart.StartsWith(queryFilter.Depart))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.StudentNo), x => x.StudentNo != null && x.StudentNo.Contains(queryFilter.StudentNo))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Phone), x => x.Phone != null && x.Phone.Contains(queryFilter.Phone))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.IdCard), x => x.IdCard != null && x.IdCard.Contains(queryFilter.IdCard))
                                join card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsPrincipal)
                                on user.Id equals card.UserID into cards
                                from card in cards.DefaultIfEmpty()
                                select new StaffListItemDto
                                {
                                    ID = user.Id,
                                    Name = user.Name,
                                    StudentNo = user.StudentNo,
                                    Title = user.Title,
                                    Depart = user.Depart,
                                    DepartName = user.DepartName,
                                    Phone = user.Phone,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                    CardNo = card != null ? card.No : "",
                                    CardStatus = card != null ? card.Status : null,
                                    StaffBeginTime = user.StaffBeginTime,
                                };
            userCardQuery = userCardQuery.Where(!string.IsNullOrWhiteSpace(queryFilter.CardNo), x => x.CardNo != null && x.CardNo.Contains(queryFilter.CardNo));

            var pageList = await userCardQuery.OrderByDescending(x => x.StaffBeginTime)
                                              .ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);

            return pageList;
        }

        /// <summary>
        /// 获取读者详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDetailOutput> GetByID(Guid userId)
        {
            var userInfo = await _userRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == userId);
            if (userInfo == null)
            {
                throw Oops.Oh("未找到用户对象");
            }
            var targetUser = userInfo.Adapt<UserDetailOutput>();
            var cardInfo = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserID == userInfo.Id && x.IsPrincipal);
            if (cardInfo != null)
            {
                targetUser.CardId = cardInfo.Id;
                targetUser.CardNo = cardInfo.No;
                targetUser.CardStatus = cardInfo.Status;
                targetUser.CardType = cardInfo.Type;
                targetUser.CardTypeName = cardInfo.TypeName;
                targetUser.CardIssueDate = cardInfo.IssueDate;
                targetUser.CardExpireDate = cardInfo.ExpireDate;
                targetUser.AsyncReaderId = cardInfo.AsyncReaderId;
            }
            var forUserProperties = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForReader && !x.SysBuildIn && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).Select(x => new UserPropertyItemDto
            {
                Sort = 0,
                UserID = userId,
                PropertyID = x.Id,
                PropertyName = x.Name,
                PropertyCode = x.Code,
                PropertyType = x.Type,
                Required = x.Required,
                PropertyValue = ""
            }).ToList();
            var sortNo = 1;
            forUserProperties.ForEach(x =>
            {
                x.Sort = sortNo++;
            });

            var userExternalProperties = (from userProperty in _userPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserID == userId)
                                          join propertyForUser in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForReader && !x.SysBuildIn) on userProperty.PropertyID equals propertyForUser.Id
                                          select new UserPropertyItemDto
                                          {
                                              UserID = userProperty.UserID,
                                              PropertyID = userProperty.PropertyID,
                                              PropertyName = propertyForUser.Name,
                                              PropertyCode = propertyForUser.Code,
                                              PropertyType = propertyForUser.Type,
                                              PropertyValue = userProperty.PropertyValue,
                                              Required = propertyForUser.Required
                                          }).ToList();
            forUserProperties.ForEach(x =>
            {
                var mapProperty = userExternalProperties.FirstOrDefault(p => p.PropertyID == x.PropertyID);
                if (mapProperty != null)
                {
                    x.PropertyValue = mapProperty.PropertyValue;
                }
            });
            targetUser.Properties = forUserProperties.Adapt<List<UserPropertyItemOutput>>();
            var userGroupIds = await _userGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserID == userId).Select(x => x.GroupID).ToListAsync();
            targetUser.GroupIds = userGroupIds.Distinct().ToList();
            return targetUser;
        }

        /// <summary>
        /// 通过UserKey获取读者详情
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<UserDetailOutput> GetByUserKey(string userKey)
        {
            var userInfo = await _userRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserKey == userKey);
            if (userInfo == null)
            {
                throw Oops.Oh("未找到用户对象");
            }
            var targetUser = userInfo.Adapt<UserDetailOutput>();
            var cardInfo = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserID == userInfo.Id && x.IsPrincipal);
            if (cardInfo != null)
            {
                targetUser.CardId = cardInfo.Id;
                targetUser.CardNo = cardInfo.No;
                targetUser.CardStatus = cardInfo.Status;
                targetUser.CardType = cardInfo.Type;
                targetUser.CardTypeName = cardInfo.TypeName;
                targetUser.CardIssueDate = cardInfo.IssueDate;
                targetUser.CardExpireDate = cardInfo.ExpireDate;
                targetUser.AsyncReaderId = cardInfo.AsyncReaderId;
            }
            var forUserProperties = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForReader && !x.SysBuildIn && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).Select(x => new UserPropertyItemDto
            {
                Sort = 0,
                UserID = userInfo.Id,
                PropertyID = x.Id,
                PropertyName = x.Name,
                PropertyCode = x.Code,
                PropertyType = x.Type,
                Required = x.Required,
                PropertyValue = ""
            }).ToList();
            var sortNo = 1;
            forUserProperties.ForEach(x =>
            {
                x.Sort = sortNo++;
            });

            var userExternalProperties = (from userProperty in _userPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserID == userInfo.Id)
                                          join propertyForUser in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForReader && !x.SysBuildIn) on userProperty.PropertyID equals propertyForUser.Id
                                          select new UserPropertyItemDto
                                          {
                                              UserID = userProperty.UserID,
                                              PropertyID = userProperty.PropertyID,
                                              PropertyName = propertyForUser.Name,
                                              PropertyCode = propertyForUser.Code,
                                              PropertyType = propertyForUser.Type,
                                              PropertyValue = userProperty.PropertyValue,
                                              Required = propertyForUser.Required
                                          }).ToList();
            forUserProperties.ForEach(x =>
            {
                var mapProperty = userExternalProperties.FirstOrDefault(p => p.PropertyID == x.PropertyID);
                if (mapProperty != null)
                {
                    x.PropertyValue = mapProperty.PropertyValue;
                }
            });
            targetUser.Properties = forUserProperties.Adapt<List<UserPropertyItemOutput>>();
            var userGroupIds = await _userGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserID == userInfo.Id).Select(x => x.GroupID).ToListAsync();
            targetUser.GroupIds = userGroupIds.Distinct().ToList();
            return targetUser;
        }

        /// <summary>
        /// 用户数据编辑合法性验证
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public async Task<Tuple<List<UserProperty>, List<UserProperty>>> UserEditValidate(UserDto userData, bool isAdd)
        {
            if (string.IsNullOrWhiteSpace(userData.Name))
            {
                throw Oops.Oh("用户名称必填");
            }
            if (string.IsNullOrWhiteSpace(userData.Phone))
            {
                throw Oops.Oh("联系电话必填");
            }
            if (string.IsNullOrWhiteSpace(userData.StudentNo))
            {
                throw Oops.Oh("学工号必填");
            }
            var userPropertyQuery = from userProperty in _userPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                    join user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag) on userProperty.UserID equals user.Id into users
                                    from user in users
                                    select userProperty;
            //需要新增的属性
            var insertUserProperties = new List<UserProperty>();
            //需要更新的属性
            var updateUserProperties = new List<UserProperty>();
            if (isAdd)
            {
                var forUserProperties = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForReader && !x.SysBuildIn && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).ToList();
                userData.Properties.ForEach(x =>
                {
                    var userProperty = new UserProperty
                    {
                        Id = _idGenerator.CreateGuid(),
                        UserID = userData.ID,
                        PropertyID = x.PropertyID,
                        PropertyValue = x.PropertyValue,
                    };
                    var mapProperty = forUserProperties.FirstOrDefault(p => p.Id == x.PropertyID);
                    if (mapProperty != null)
                    {
                        if (mapProperty.Required && string.IsNullOrWhiteSpace(userProperty.PropertyValue))
                        {
                            throw Oops.Oh($"{mapProperty.Name}必填");
                        }
                        if (mapProperty.Unique)
                        {
                            var pValue = (userProperty.PropertyValue ?? "").Trim();
                            var encodeValue = _baseEncrypt.Encode(pValue);
                            var isPropertyExist = userPropertyQuery.Any(p => p.PropertyID == mapProperty.Id && p.PropertyValue != null && p.PropertyValue != "" && p.PropertyValue == encodeValue);
                            if (isPropertyExist) throw Oops.Oh($"{mapProperty.Name}已存在");
                        }
                        MapPropertyValue(userProperty, mapProperty.Type);
                        userProperty.PropertyValue = _baseEncrypt.Encode(userProperty.PropertyValue);
                        insertUserProperties.Add(userProperty);
                    }
                });
            }
            else
            {
                var forUserProperties = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForReader && !x.SysBuildIn && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).ToList();
                var userInputProperties = userData.Properties;
                var userProperties = userPropertyQuery.Where(x => !x.DeleteFlag && x.UserID == userData.ID).ToList();
                //添加需要新增的属性值
                userInputProperties.Where(x => !userProperties.Any(p => p.PropertyID == x.PropertyID)).ToList()
                .ForEach(x =>
               {
                   var userProperty = new UserProperty
                   {
                       Id = _idGenerator.CreateGuid(),
                       UserID = userData.ID,
                       PropertyID = x.PropertyID,
                       PropertyValue = x.PropertyValue,
                   };

                   var mapProperty = forUserProperties.FirstOrDefault(p => p.Id == x.PropertyID);
                   if (mapProperty != null)
                   {
                       if (mapProperty.Required && string.IsNullOrWhiteSpace(x.PropertyValue))
                       {
                           throw Oops.Oh($"{mapProperty.Name}必填");
                       }
                       if (mapProperty.Unique)
                       {
                           var pValue = (x.PropertyValue ?? "").Trim();
                           var encodeValue = _baseEncrypt.Encode(pValue);
                           var isPropertyExist = userPropertyQuery.Any(p => p.UserID != userData.ID && !p.DeleteFlag && p.PropertyID == mapProperty.Id && p.PropertyValue != null && p.PropertyValue != "" && p.PropertyValue == encodeValue);
                           if (isPropertyExist) throw Oops.Oh($"{mapProperty.Name}已存在");
                       }
                       MapPropertyValue(userProperty, mapProperty.Type);
                       userProperty.PropertyValue = _baseEncrypt.Encode(userProperty.PropertyValue);
                       insertUserProperties.Add(userProperty);
                   }
               });
                //添加需要修改的属性值
                userInputProperties.Where(x => userProperties.Any(p => p.PropertyID == x.PropertyID)).ToList()
                .ForEach(x =>
               {
                   var existProperty = userProperties.FirstOrDefault(up => up.PropertyID == x.PropertyID);
                   var mapProperty = forUserProperties.FirstOrDefault(p => p.Id == x.PropertyID);
                   if (mapProperty != null)
                   {
                       if (mapProperty.Required && string.IsNullOrWhiteSpace(x.PropertyValue))
                       {
                           throw Oops.Oh($"{mapProperty.Name}必填");
                       }
                       if (mapProperty.Unique)
                       {
                           var pValue = (x.PropertyValue ?? "").Trim();
                           var encodeValue = _baseEncrypt.Encode(pValue);
                           var isPropertyExist = userPropertyQuery.Any(p => p.UserID != userData.ID && !p.DeleteFlag && p.PropertyID == mapProperty.Id && p.PropertyValue != null && p.PropertyValue != "" && p.PropertyValue == encodeValue);
                           if (isPropertyExist) throw Oops.Oh($"{mapProperty.Name}已存在");
                       }
                       //新值不同时为空，并且不同
                       if ((!string.IsNullOrWhiteSpace(x.PropertyValue) || !string.IsNullOrWhiteSpace(existProperty.PropertyValue))
                              && (_baseEncrypt.Encode(x.PropertyValue ?? "") != (existProperty.PropertyValue ?? "")))
                       {
                           existProperty.PropertyValue = x.PropertyValue;
                           MapPropertyValue(existProperty, mapProperty.Type);
                           existProperty.PropertyValue = _baseEncrypt.Encode(existProperty.PropertyValue);
                           existProperty.UpdateTime = DateTime.Now;
                           updateUserProperties.Add(existProperty);
                       }
                   }
               });
            }
            var idCard = _baseEncrypt.Encode((userData.IdCard ?? "").Trim());
            var phone = _baseEncrypt.Encode((userData.Phone ?? "").Trim());
            var studentNo = _baseEncrypt.Encode((userData.StudentNo ?? "").Trim());
            var isIdCardExist = await _userRepository.AnyAsync(u => u.Id != userData.ID && u.IdCard != null && u.IdCard != "" && idCard != "" && u.IdCard == idCard && !u.DeleteFlag, false, true);
            if (isIdCardExist) throw Oops.Oh("读者身份证号已存在");
            var isPhoneExist = await _userRepository.AnyAsync(u => u.Id != userData.ID && u.Phone != null && u.Phone != "" && phone != "" && u.Phone == phone && !u.DeleteFlag, false, true);
            if (isPhoneExist) throw Oops.Oh("联系电话已存在");
            var isStudentNoExist = await _userRepository.AnyAsync(u => u.Id != userData.ID && u.StudentNo != null && u.StudentNo != "" && studentNo != "" && u.StudentNo == studentNo && !u.DeleteFlag, false, true);
            if (isStudentNoExist) throw Oops.Oh("学工号已存在");
            return new Tuple<List<UserProperty>, List<UserProperty>>(insertUserProperties, updateUserProperties);
        }

        public async Task<UserBatchEditInput> MapBatchPropertyName(UserBatchEditInput userData)
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
            if (!string.IsNullOrWhiteSpace(userData.College))
            {
                var mapProperty = groupDatas.Where(x => x.Code == "User_College").SelectMany(x => x.Items).FirstOrDefault(x => x.Code == userData.College);
                userData.CollegeName = mapProperty?.Name;
            }
            if (!string.IsNullOrWhiteSpace(userData.CollegeDepart))
            {
                var mapProperty = groupDatas.Where(x => x.Code == "User_CollegeDepart").SelectMany(x => x.Items).FirstOrDefault(x => x.Code == userData.CollegeDepart);
                userData.CollegeDepartName = mapProperty?.Name;
            }
            if (!string.IsNullOrWhiteSpace(userData.Type))
            {
                var mapProperty = groupDatas.Where(x => x.Code == "User_Type").SelectMany(x => x.Items).FirstOrDefault(x => x.Code == userData.Type);
                userData.TypeName = mapProperty?.Name;
            }
            return userData;
        }

        public async Task<StaffDepartEditInput> MapStaffPropertyName(StaffDepartEditInput staffData)
        {
            if (!string.IsNullOrWhiteSpace(staffData.Depart))
            {
                var mapProperty = await _orgRepository.DetachedEntities.FirstOrDefaultAsync(x => x.FullPath == staffData.Depart);
                staffData.DepartName = mapProperty?.FullName;
            }
            return staffData;
        }

        /// <summary>
        /// 映射用户信息编码和名称，学院，所在系，用户类型，部门
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public async Task<UserDto> MapPropertyName(UserDto userData)
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
            if (!string.IsNullOrWhiteSpace(userData.College))
            {
                var mapProperty = groupDatas.Where(x => x.Code == "User_College").SelectMany(x => x.Items).FirstOrDefault(x => x.Code == userData.College);
                userData.CollegeName = mapProperty?.Name;
            }
            if (!string.IsNullOrWhiteSpace(userData.CollegeDepart))
            {
                var mapProperty = groupDatas.Where(x => x.Code == "User_CollegeDepart").SelectMany(x => x.Items).FirstOrDefault(x => x.Code == userData.CollegeDepart);
                userData.CollegeDepartName = mapProperty?.Name;
            }
            if (!string.IsNullOrWhiteSpace(userData.Type))
            {
                var mapProperty = groupDatas.Where(x => x.Code == "User_Type").SelectMany(x => x.Items).FirstOrDefault(x => x.Code == userData.Type);
                userData.TypeName = mapProperty?.Name;
            }
            if (!string.IsNullOrWhiteSpace(userData.Depart))
            {
                var mapProperty = await _orgRepository.DetachedEntities.FirstOrDefaultAsync(x => x.FullPath == userData.Depart);
                userData.DepartName = mapProperty?.FullName;
            }
            userData.Addr = this.MapAddrName(userData.Addr);
            return userData;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public async Task<Guid> Create(UserDto userData)
        {
            userData.ID = _idGenerator.CreateGuid(userData.ID);
            var validateResult = await this.UserEditValidate(userData, true);
            var userEntity = userData.Adapt<EntityFramework.Core.Entitys.User>();
            if (string.IsNullOrWhiteSpace(userEntity.UserKey))
            {
                var userKey = $"{_tenantInfo.Name}_{userData.StudentNo}";
                var isUserKeyExist = await _userRepository.AnyAsync(u => u.Id != userData.ID && u.UserKey != null && u.UserKey != "" && userKey != "" && u.UserKey == userKey && !u.DeleteFlag, false, true);
                if (isUserKeyExist) throw Oops.Oh("用户标识已处在");
                userEntity.UserKey = userKey;
            }
            var userEntry = await _userRepository.InsertNowAsync(userEntity);
            await _userPropertyRepository.InsertNowAsync(validateResult.Item1);
            return userEntry.Entity.Id;
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public async Task<Guid> Update(UserDto userData)
        {
            var validateResult = await this.UserEditValidate(userData, false);
            var userEntity = await _userRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == userData.ID);
            if (userEntity == null)
            {
                throw Oops.Oh("未找到用户对象");
            }
            var userEntityDto = userEntity.Adapt<UserDto>();
            userEntity = userData.Adapt(userEntity);
            //编辑时暂不处理UserKey
            //if (string.IsNullOrWhiteSpace(userEntity.UserKey))
            //{
            //    var userKey = $"{_tenantInfo.Name}-{userData.StudentNo}";
            //    var isUserKeyExist = await _userRepository.AnyAsync(u => u.Id != userData.ID && u.UserKey != null && u.UserKey != "" && userKey != "" && u.UserKey == userKey && !u.DeleteFlag, false, true);
            //    if (isUserKeyExist) throw Oops.Oh("用户标识已处在");
            //    userEntity.UserKey = userKey;
            //}
            if (userData.IdCard != userEntityDto.IdCard)
            {
                userEntity.IdCardIdentity = false;
            }
            if (userData.Phone != userEntityDto.Phone)
            {
                userEntity.IdCardIdentity = false;
            }
            if (userData.Email != userEntityDto.Email)
            {
                userEntity.EmailIdentity = false;
            }
            userEntity.SourceFrom = userEntityDto.SourceFrom;
            var userEntityEntry = await _userRepository.UpdateNowAsync(userEntity);
            if (validateResult.Item1.Any())
            {
                await _userPropertyRepository.InsertNowAsync(validateResult.Item1);
            }
            if (validateResult.Item2.Any())
            {
                await _userPropertyRepository.UpdateNowAsync(validateResult.Item2);
            }
            return userEntityEntry.Entity.Id;
        }

        /// <summary>
        /// 转换地址
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        private string MapAddrName(string addr)
        {
            if (!string.IsNullOrWhiteSpace(addr))
            {
                var addrCode = DataConverter.ToInt(addr);
                var mapProperty = _regionRepository.DetachedEntities.FirstOrDefault(x => x.ID == addrCode);
                return $"{mapProperty?.MerName}|{mapProperty.ID}";
            }
            return addr;
        }

        private void MapPropertyValue(UserProperty userProperty, int propertyType)
        {
            if (string.IsNullOrWhiteSpace(userProperty.PropertyValue))
            {
                return;
            }
            switch (propertyType)
            {
                case (int)EnumPropertyType.数值:
                    userProperty.NumValue = DataConverter.ToNullableDecimal(userProperty.PropertyValue);
                    break;
                case (int)EnumPropertyType.时间:
                    userProperty.TimeValue = DataConverter.ToNumableDateTime(userProperty.PropertyValue);
                    break;
                case (int)EnumPropertyType.是非:
                    userProperty.BoolValue = DataConverter.ToNullableBoolean(userProperty.PropertyValue);
                    break;
                case (int)EnumPropertyType.地址:
                    userProperty.PropertyValue = this.MapAddrName(userProperty.PropertyValue);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 读者数据删除
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid userId)
        {
            var userEntity = await _userRepository.FirstOrDefaultAsync(x => x.Id == userId);
            if (userEntity != null)
            {
                //标记删除读者
                userEntity.DeleteFlag = true;
                userEntity.UpdateTime = DateTime.Now;

                var userCards = _cardRepository.Where(x => !x.DeleteFlag && x.UserID == userId).ToList();
                var userCardIds = userCards.Select(x => x.Id).ToList();

                //标记删除读者卡
                userCards.ForEach(x =>
                {
                    x.DeleteFlag = true;
                    x.UpdateTime = DateTime.Now;
                });
                await _userRepository.UpdateNowAsync(userEntity);
                await _cardRepository.UpdateNowAsync(userCards);
            }
            return true;
        }

        /// <summary>
        /// 批量修改用户信息
        /// </summary>
        /// <param name="batchEditData"></param>
        /// <returns></returns>
        public async Task<bool> BatchUpdate(UserBatchEditDto batchEditData)
        {
            var selectFields = batchEditData.Fields.Select(x => x.ToLower()).ToList();
            var updateBuilder = _userRepository.Context.BatchUpdate<EntityFramework.Core.Entitys.User>();
            updateBuilder = selectFields.Contains(nameof(batchEditData.Edu).ToLower()) ? updateBuilder.Set(b => b.Edu, b => batchEditData.Edu) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.College).ToLower()) ? updateBuilder.Set(b => b.College, b => batchEditData.College) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.College).ToLower()) ? updateBuilder.Set(b => b.CollegeName, b => batchEditData.CollegeName) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.CollegeDepart).ToLower()) ? updateBuilder.Set(b => b.CollegeDepart, b => batchEditData.CollegeDepart) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.CollegeDepart).ToLower()) ? updateBuilder.Set(b => b.CollegeDepartName, b => batchEditData.CollegeDepartName) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.Major).ToLower()) ? updateBuilder.Set(b => b.Major, b => batchEditData.Major) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.Grade).ToLower()) ? updateBuilder.Set(b => b.Grade, b => batchEditData.Grade) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.Class).ToLower()) ? updateBuilder.Set(b => b.Class, b => batchEditData.Class) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.Gender).ToLower()) ? updateBuilder.Set(b => b.Gender, b => batchEditData.Gender) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.Type).ToLower()) ? updateBuilder.Set(b => b.Type, b => batchEditData.Type) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.Type).ToLower()) ? updateBuilder.Set(b => b.TypeName, b => batchEditData.TypeName) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.Status).ToLower()) ? updateBuilder.Set(b => b.Status, b => batchEditData.Status ?? 0) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.LeaveTime).ToLower()) ? updateBuilder.Set(b => b.LeaveTime, b => batchEditData.LeaveTime) : updateBuilder;
            updateBuilder = updateBuilder.Set(b => b.UpdateTime, b => DateTime.Now);
            updateBuilder.Where(x => !x.DeleteFlag && batchEditData.UserIDList.Contains(x.Id));
            await updateBuilder.ExecuteAsync();
            return true;
        }
        /// <summary>
        /// 批量设置为馆员
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<bool> BatchSetUserAsStaff(List<Guid> userIds)
        {
            var updateBuilder = _userRepository.Context.BatchUpdate<EntityFramework.Core.Entitys.User>();
            updateBuilder
            .Set(b => b.IsStaff, b => true)
            .Set(b => b.StaffStatus, b => (int)EnumStaffStatus.正式)
            .Set(b => b.StaffBeginTime, b => DateTime.Now)
            .Set(b => b.UpdateTime, b => DateTime.Now);
            updateBuilder.Where(x => !x.DeleteFlag && userIds.Contains(x.Id));
            await updateBuilder.ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 批量移除馆员身份
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<bool> BatchSetUserAsReader(List<Guid> userIds)
        {
            var updateBuilder = _userRepository.Context.BatchUpdate<EntityFramework.Core.Entitys.User>();
            updateBuilder
            .Set(b => b.IsStaff, b => false)
            .Set(b => b.StaffStatus, b => null)
            .Set(b => b.UpdateTime, b => DateTime.Now);
            updateBuilder.Where(x => !x.DeleteFlag && userIds.Contains(x.Id));
            await updateBuilder.ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 批量设置馆员部门
        /// </summary>
        /// <param name="staffDepartSetData"></param>
        /// <returns></returns>
        public async Task<bool> BatchSetDepartment(StaffDepartSetDto staffDepartSetData)
        {
            var updateBuilder = _userRepository.Context.BatchUpdate<EntityFramework.Core.Entitys.User>();
            updateBuilder
            .Set(b => b.Depart, b => staffDepartSetData.Depart)
            .Set(b => b.DepartName, b => staffDepartSetData.DepartName)
            .Set(b => b.UpdateTime, b => DateTime.Now);
            updateBuilder.Where(x => !x.DeleteFlag && staffDepartSetData.UserIds.Contains(x.Id));
            await updateBuilder.ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 编辑读者信息
        /// </summary>
        /// <param name="readerData"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateReaderInfo(ReaderEditDto readerData, Guid userId)
        {
            var readerEditProperties = _editPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.IsCheck).Select(x => x.PropertyCode).ToList();
            var selectFields = readerEditProperties.Select(x => x.Replace("User_", "").ToLower()).ToList();
            var updateBuilder = _userRepository.Context.BatchUpdate<EntityFramework.Core.Entitys.User>();
            updateBuilder = selectFields.Contains(nameof(readerData.Name).ToLower()) ? updateBuilder.Set(b => b.Name, b => _baseEncrypt.Encode(readerData.Name)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.NickName).ToLower()) ? updateBuilder.Set(b => b.NickName, b => _baseEncrypt.Encode(readerData.NickName)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.Gender).ToLower()) ? updateBuilder.Set(b => b.Gender, b => _baseEncrypt.Encode(readerData.Gender)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.Birthday).ToLower()) ? updateBuilder.Set(b => b.Birthday, b => readerData.Birthday) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.Addr).ToLower()) ? updateBuilder.Set(b => b.Addr, b => _baseEncrypt.Encode(readerData.Addr)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.AddrDetail).ToLower()) ? updateBuilder.Set(b => b.AddrDetail, b => _baseEncrypt.Encode(readerData.AddrDetail)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.Unit).ToLower()) ? updateBuilder.Set(b => b.Unit, b => _baseEncrypt.Encode(readerData.Unit)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.Edu).ToLower()) ? updateBuilder.Set(b => b.Edu, b => _baseEncrypt.Encode(readerData.Edu)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.College).ToLower()) ? updateBuilder.Set(b => b.College, b => _baseEncrypt.Encode(readerData.College)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.Major).ToLower()) ? updateBuilder.Set(b => b.Major, b => _baseEncrypt.Encode(readerData.Major)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.Grade).ToLower()) ? updateBuilder.Set(b => b.Grade, b => _baseEncrypt.Encode(readerData.Grade)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.Class).ToLower()) ? updateBuilder.Set(b => b.Class, b => _baseEncrypt.Encode(readerData.Class)) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(readerData.Photo).ToLower()) ? updateBuilder.Set(b => b.Photo, b => _baseEncrypt.Encode(readerData.Photo)) : updateBuilder;

            updateBuilder = updateBuilder.Set(b => b.UpdateTime, b => DateTime.Now);
            updateBuilder.Where(x => !x.DeleteFlag && x.Id == userId);
            await updateBuilder.ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 通过账号密码登录
        /// </summary>
        /// <param name="accountInfo"></param>
        /// <returns></returns>
        public async Task<LoginResultDto> LoginByAccountPwd(AccountInfoDto accountInfo)
        {
            var result = new LoginResultDto
            {
                Code = EnumResultCode.成功.GetHashCode().ToString(),
                ErrMsg = ""
            };
            if (accountInfo.Account.IsNullOrWhiteSpace() || accountInfo.Password.IsNullOrWhiteSpace())
            {
                result.Code = EnumResultCode.账号密码必填.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.账号密码必填.ToString();
                return result;
            }
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var encodePwd = encryptProvider.Encrypt(accountInfo.Password, SiteGlobalConfig.SM2Key.PublicKey);
            var cardQuery = from card in _cardRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                            join user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag) on card.UserID equals user.Id into users
                            from user in users
                            select new
                            {
                                UserId = user.Id,
                                IdCard = user.IdCard,
                                Phone = user.Phone,
                                CardId = card.Id,
                                CardNo = card.No,
                                CardSecret = card.Secret,
                            };

            var mapCard = await cardQuery.FirstOrDefaultAsync(x => x.CardNo != null && x.CardNo != "" && x.CardNo == accountInfo.Account && x.CardSecret == encodePwd);
            if (mapCard == null)
            {
                var encodeAccount = _baseEncrypt.Encode(accountInfo.Account);
                mapCard = await cardQuery.FirstOrDefaultAsync(x => x.IdCard != null && x.IdCard != "" && x.IdCard == encodeAccount && x.CardSecret == encodePwd);
            }
            if (mapCard == null)
            {
                result.Code = EnumResultCode.账号密码不匹配.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.账号密码不匹配.ToString();
                return result;
            }
            var cardInfo = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == mapCard.CardId);
            if (cardInfo == null)
            {
                result.Code = EnumResultCode.账号密码不匹配.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.账号密码不匹配.ToString();
                return result;
            }
            //临时卡校验是否过期
            if (cardInfo.Usage == (int)EnumCardUsage.临时馆员登陆)
            {
                if (cardInfo.ExpireDate < DateTime.Now)
                {
                    result.Code = EnumResultCode.临时馆员卡过期.GetHashCode().ToString();
                    result.ErrMsg = EnumResultCode.临时馆员卡过期.ToString();
                    return result;
                }
            }
            var userInfo = await _userRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardInfo.UserID);
            if (userInfo == null)
            {
                result.Code = EnumResultCode.未找到读者信息.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.未找到读者信息.ToString();
                return result;
            }
            //校验读者状态
            if (userInfo.Status != (int)EnumUserStatus.正常)
            {
                result.Code = EnumResultCode.用户状态异常.GetHashCode().ToString();
                switch (userInfo.Status)
                {
                    case (int)EnumUserStatus.未激活:
                        result.ErrMsg = "用户未激活";
                        break;
                    case (int)EnumUserStatus.注销:
                        result.ErrMsg = "用户已注销";
                        break;
                    case (int)EnumUserStatus.禁用:
                        result.ErrMsg = "用户已禁用";
                        break;
                    default:
                        result.ErrMsg = "用户账号异常";
                        break;
                }
                return result;
            }
            //登录成功更新最近登陆时间
            var minDate = new DateTime(1970, 1, 1);
            await _userRepository.Context.BatchUpdate<EntityFramework.Core.Entitys.User>()
            .Set(b => b.LastLoginTime, b => DateTime.Now)
            .Set(b => b.FirstLoginTime, b => (b.FirstLoginTime == null || b.FirstLoginTime < minDate) ? DateTime.Now : b.FirstLoginTime)
            .Where(b => b.Id == userInfo.Id)
            .ExecuteAsync();
            result.UserKey = userInfo.UserKey.ToString();
            result.IsStaff = userInfo.IsStaff;

            return result;
        }

        /// <summary>
        /// 通过身份证号密码登录
        /// </summary>
        /// <param name="idCardInfo"></param>
        /// <returns></returns>
        public async Task<LoginResultDto> LoginByIdCardPwd(IdCardInfoDto idCardInfo)
        {
            var result = new LoginResultDto
            {
                Code = EnumResultCode.成功.GetHashCode().ToString(),
                ErrMsg = ""
            };
            if (idCardInfo.IdCard.IsNullOrWhiteSpace() || idCardInfo.Password.IsNullOrWhiteSpace())
            {
                result.Code = EnumResultCode.身份证号密码必填.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.身份证号密码必填.ToString();
                return result;
            }
            var userInfo = await _userRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.IdCard == idCardInfo.IdCard);
            if (userInfo == null)
            {
                result.Code = EnumResultCode.未找到读者信息.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.未找到读者信息.ToString();
                return result;
            }
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var encodePwd = encryptProvider.Encrypt(idCardInfo.Password, SiteGlobalConfig.SM2Key.PublicKey);
            var cardInfo = await _cardRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.UserID == userInfo.Id && x.Secret == encodePwd);
            if (cardInfo == null)
            {
                result.Code = EnumResultCode.账号密码不匹配.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.账号密码不匹配.ToString();
                return result;
            }
            //临时卡校验是否过期
            if (cardInfo.Usage == (int)EnumCardUsage.临时馆员登陆)
            {
                if (cardInfo.ExpireDate < DateTime.Now)
                {
                    result.Code = EnumResultCode.临时馆员卡过期.GetHashCode().ToString();
                    result.ErrMsg = EnumResultCode.临时馆员卡过期.ToString();
                    return result;
                }
            }
            //校验读者状态
            if (userInfo.Status != (int)EnumUserStatus.正常)
            {
                result.Code = EnumResultCode.用户状态异常.GetHashCode().ToString();
                switch (userInfo.Status)
                {
                    case (int)EnumUserStatus.未激活:
                        result.ErrMsg = "用户未激活";
                        break;
                    case (int)EnumUserStatus.注销:
                        result.ErrMsg = "用户已注销";
                        break;
                    case (int)EnumUserStatus.禁用:
                        result.ErrMsg = "用户已禁用";
                        break;
                    default:
                        result.ErrMsg = "用户账号异常";
                        break;
                }
                return result;
            }
            result.UserKey = userInfo.UserKey.ToString();
            result.IsStaff = userInfo.IsStaff;
            return result;
        }

        /// <summary>
        /// 通过手机号码
        /// </summary>
        /// <param name="phoneInfo"></param>
        /// <returns></returns>
        public async Task<LoginResultDto> LoginByPhone(PhoneInfoDto phoneInfo)
        {
            var result = new LoginResultDto
            {
                Code = EnumResultCode.成功.GetHashCode().ToString(),
                ErrMsg = ""
            };
            if (phoneInfo.Phone.IsNullOrWhiteSpace())
            {
                result.Code = EnumResultCode.手机号码必填.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.手机号码必填.ToString();
                return result;
            }
            var userInfo = await _userRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Phone == phoneInfo.Phone);
            if (userInfo == null)
            {
                result.Code = EnumResultCode.未找到读者信息.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.未找到读者信息.ToString();
                return result;
            }
            //校验读者状态
            if (userInfo.Status != (int)EnumUserStatus.正常)
            {
                result.Code = EnumResultCode.用户状态异常.GetHashCode().ToString();
                switch (userInfo.Status)
                {
                    case (int)EnumUserStatus.未激活:
                        result.ErrMsg = "用户未激活";
                        break;
                    case (int)EnumUserStatus.注销:
                        result.ErrMsg = "用户已注销";
                        break;
                    case (int)EnumUserStatus.禁用:
                        result.ErrMsg = "用户已禁用";
                        break;
                    default:
                        result.ErrMsg = "用户账号异常";
                        break;
                }
                return result;
            }
            result.UserKey = userInfo.UserKey.ToString();
            result.IsStaff = userInfo.IsStaff;
            return result;
        }

        /// <summary>
        /// 通过卡号查询用户卡
        /// </summary>
        /// <param name="cardSearch"></param>
        /// <returns></returns>
        public async Task<CardSearchResultDto> SearchCardByNo(CardSearchDto cardSearch)
        {
            var result = new CardSearchResultDto
            {
                Code = EnumResultCode.成功.GetHashCode().ToString(),
                ErrMsg = ""
            };
            if (cardSearch.No.IsNullOrWhiteSpace())
            {
                result.Code = EnumResultCode.卡号必填.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.卡号必填.ToString();
                return result;
            }
            var cardInfo = await _cardRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.No == cardSearch.No);
            if (cardInfo == null)
            {
                result.Code = EnumResultCode.未找到卡信息.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.未找到卡信息.ToString();
                return result;
            }
            var userInfo = await _userRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardInfo.UserID);
            if (userInfo == null || userInfo.Phone.IsNullOrWhiteSpace())
            {
                result.Code = EnumResultCode.未找到读者信息.GetHashCode().ToString();
                result.ErrMsg = $"读者卡未关联手机";
                return result;
            }
            result.CardId = cardInfo.Id.ToString();
            result.Phone = userInfo.Phone;
            return result;
        }

        /// <summary>
        /// 变更卡密码
        /// </summary>
        /// <param name="CardToken"></param>
        /// <returns></returns>
        public async Task<SimpleResultDto> ChangeCardPwd(CardTokenInfoDto CardToken)
        {
            var result = new SimpleResultDto
            {
                Code = EnumResultCode.成功.GetHashCode().ToString(),
                ErrMsg = ""
            };
            var cardId = new Guid(CardToken.CardId);
            var cardInfo = await _cardRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardId);
            if (cardInfo == null)
            {
                result.Code = EnumResultCode.未找到卡信息.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.未找到卡信息.ToString();
                return result;
            }
            var securityPolicy = await _securityService.GetSecurityPolicy();
            if (!SecretStrongChecker.Check(CardToken.Password, securityPolicy.SecretLevel).Ok)
            {
                result.Code = EnumResultCode.密码较弱.GetHashCode().ToString();
                result.ErrMsg = $"密码校验未通过：{SecretStrongChecker.Check(CardToken.Password, securityPolicy.SecretLevel).Err}";
                return result;
            }

            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var encodePwd = encryptProvider.Encrypt(CardToken.Password ?? "", SiteGlobalConfig.SM2Key.PublicKey);

            cardInfo.Secret = encodePwd;
            cardInfo.UpdateTime = DateTimeOffset.Now;
            cardInfo.SecretChangeTime = DateTime.Now;
            await _cardRepository.UpdateNowAsync(cardInfo);

            return result;
        }

        /// <summary>
        /// 修改卡密码，需要验证旧密码
        /// </summary>
        /// <param name="cardChangePwd"></param>
        /// <returns></returns>
        public async Task<SimpleResultDto> ChangeCardPwdEx(CardChangePwdDto cardChangePwd)
        {
            var result = new SimpleResultDto
            {
                Code = EnumResultCode.成功.GetHashCode().ToString(),
                ErrMsg = ""
            };
            var cardId = new Guid(cardChangePwd.CardId);
            var cardInfo = await _cardRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardId);
            if (cardInfo == null)
            {
                result.Code = EnumResultCode.未找到卡信息.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.未找到卡信息.ToString();
                return result;
            }
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var encodeOldPwd = encryptProvider.Encrypt(cardChangePwd.OldPwd ?? "", SiteGlobalConfig.SM2Key.PublicKey);
            if (encodeOldPwd != cardInfo.Secret)
            {
                result.Code = EnumResultCode.账号密码不匹配.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.账号密码不匹配.ToString();
                return result;
            }

            var securityPolicy = await _securityService.GetSecurityPolicy();
            if (!SecretStrongChecker.Check(cardChangePwd.Password, securityPolicy.SecretLevel).Ok)
            {
                result.Code = EnumResultCode.密码较弱.GetHashCode().ToString();
                result.ErrMsg = $"密码校验未通过：{SecretStrongChecker.Check(cardChangePwd.Password, securityPolicy.SecretLevel).Err}";
                return result;
            }

            var encodePwd = encryptProvider.Encrypt(cardChangePwd.Password ?? "", SiteGlobalConfig.SM2Key.PublicKey);

            cardInfo.Secret = encodePwd;
            cardInfo.UpdateTime = DateTimeOffset.Now;
            cardInfo.SecretChangeTime = DateTime.Now;
            await _cardRepository.UpdateNowAsync(cardInfo);

            return result;
        }

        /// <summary>
        /// 根据userkey获取卡列表
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<List<CardSingleInfo>> GetCardListByUserKey(string userKey)
        {
            var result = new List<CardSingleInfo>();

            var user = await _userRepository.DetachedEntities.FirstOrDefaultAsync(c => c.UserKey == userKey && !c.DeleteFlag);
            if (user != null)
            {
                var cardList = await _cardRepository.DetachedEntities.Where(c => !c.DeleteFlag && c.UserID == user.Id).ToListAsync();
                cardList?.ForEach(c =>
                {
                    result.Add(new CardSingleInfo
                    {
                        CardId = c.Id.ToString(),
                        CardNo = c.No,
                        IsPrincipal = c.IsPrincipal
                    });
                });
            }

            return result;
        }

        /// <summary>
        /// 检查手机号是否唯一
        /// </summary>
        /// <param name="phoneInfo"></param>
        /// <returns></returns>
        public async Task<SimpleResultDto> CheckUniquePhone(PhoneInfoDto phoneInfo)
        {
            var result = new SimpleResultDto
            {
                Code = EnumResultCode.成功.GetHashCode().ToString(),
                ErrMsg = ""
            };
            if (phoneInfo.Phone.IsNullOrWhiteSpace())
            {
                result.Code = EnumResultCode.手机号码必填.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.手机号码必填.ToString();
                return result;
            }
            if (!phoneInfo.Phone.TryValidate(ValidationTypes.PhoneNumber).IsValid)
            {
                result.Code = EnumResultCode.手机号格式错误.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.手机号格式错误.ToString();
                return result;
            }
            var isExist = !await _userRepository.DetachedEntities.AnyAsync(x => !x.DeleteFlag && x.Phone == phoneInfo.Phone);
            if (isExist)
            {
                result.Code = EnumResultCode.手机号已存在.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.手机号已存在.ToString();
                return result;
            }
            return result;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public async Task<RegisterResultDto> RegisterUser(RegisterUserInfoDto userInfo)
        {
            var result = new RegisterResultDto
            {
                Code = EnumResultCode.成功.GetHashCode().ToString(),
                ErrMsg = ""
            };
            #region 验证信息
            if (userInfo.UserData == null)
            {
                result.Code = EnumResultCode.未找到读者信息.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.未找到读者信息.ToString();
                return result;
            }
            if (userInfo.UserData.Name.IsNullOrWhiteSpace())
            {
                result.Code = EnumResultCode.用户名称必填.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.用户名称必填.ToString();
                return result;
            }
            if (userInfo.UserData.Phone.IsNullOrWhiteSpace())
            {
                result.Code = EnumResultCode.手机号码必填.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.手机号码必填.ToString();
                return result;
            }

            if (!userInfo.UserData.Phone.TryValidate(ValidationTypes.PhoneNumber).IsValid)
            {
                result.Code = EnumResultCode.手机号格式错误.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.手机号格式错误.ToString();
                return result;
            }
            var isExist = !await _userRepository.DetachedEntities.AnyAsync(x => !x.DeleteFlag && x.Phone == userInfo.UserData.Phone);
            if (isExist)
            {
                result.Code = EnumResultCode.手机号已存在.GetHashCode().ToString();
                result.ErrMsg = EnumResultCode.手机号已存在.ToString();
                return result;
            }

            if (!userInfo.UserData.Email.IsNullOrWhiteSpace())
            {
                if (!userInfo.UserData.Email.TryValidate(ValidationTypes.EmailAddress).IsValid)
                {
                    result.Code = EnumResultCode.邮箱格式错误.GetHashCode().ToString();
                    result.ErrMsg = EnumResultCode.邮箱格式错误.ToString();
                    return result;
                }
                var isEmailExist = !await _userRepository.DetachedEntities.AnyAsync(x => !x.DeleteFlag && x.Email == userInfo.UserData.Email);
                if (isEmailExist)
                {
                    result.Code = EnumResultCode.邮箱已存在.GetHashCode().ToString();
                    result.ErrMsg = EnumResultCode.邮箱已存在.ToString();
                    return result;
                }
            }

            if (!userInfo.UserData.IdCard.IsNullOrWhiteSpace())
            {
                if (!userInfo.UserData.IdCard.TryValidate(ValidationPattern.AtLeastOne, new { ValidationTypes.IDCard, ExtensionValidationTypes.Passport }).IsValid)
                {
                    result.Code = EnumResultCode.身份证格式错误.GetHashCode().ToString();
                    result.ErrMsg = EnumResultCode.身份证格式错误.ToString();
                    return result;
                }
                var isIdCardExist = !await _userRepository.DetachedEntities.AnyAsync(x => !x.DeleteFlag && x.IdCard == userInfo.UserData.IdCard);
                if (isIdCardExist)
                {
                    result.Code = EnumResultCode.身份证号已存在.GetHashCode().ToString();
                    result.ErrMsg = EnumResultCode.身份证号已存在.ToString();
                    return result;
                }
            }
            #endregion
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var encodePwd = encryptProvider.Encrypt("123456", SiteGlobalConfig.SM2Key.PublicKey);
            var userData = userInfo.UserData.Adapt<UserDto>();
            userData.ID = _idGenerator.CreateGuid(userData.ID);
            userData.Status = userInfo.NeedConfirm ? (int)EnumUserStatus.未激活 : (int)EnumUserStatus.正常;
            using (var tran = await _userRepository.Database.BeginTransactionAsync())
            {
                try
                {
                    var userEntity = userData.Adapt<EntityFramework.Core.Entitys.User>();
                    var userEntry = await _userRepository.InsertNowAsync(userEntity);
                    var cardData = new Card
                    {
                        Id = _idGenerator.CreateGuid(),
                        UserID = userEntity.Id,
                        No = _idGenerator.CreateGuid().ToString("N"),
                        Type = "用户注册",
                        Status = userInfo.NeedConfirm ? (int)EnumCardStatus.停用 : (int)EnumCardStatus.正常,
                        IsPrincipal = true,
                        IssueDate = DateTime.Now.Date,
                        ExpireDate = DateTime.Now.Date.AddYears(3),
                        Secret = encodePwd,
                        SecretChangeTime = DateTime.Now
                    };
                    var cardEntry = await _cardRepository.InsertNowAsync(cardData);

                    if (userInfo.NeedConfirm)
                    {
                        var userRegister = new UserRegister
                        {
                            Id = _idGenerator.CreateGuid(),
                            UserID = userData.ID,
                            Status = (int)EnumUserRegisterStatus.待审批,
                        };
                        await _userRegisterRepository.InsertNowAsync(userRegister);
                    }
                    tran.Commit();
                    result.CardNo = cardEntry.Entity.Id.ToString();

                }
                catch (Exception ex)
                {
                    //记录异常日志
                    result.Code = EnumResultCode.未知异常.GetHashCode().ToString();
                    result.ErrMsg = $"未知异常{ex.Message}";
                    tran.Rollback();
                }
            }
            return result;
        }

        /// <summary>
        /// 通过用户组获取用户数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemDto>> QuerySimpleUserByGroupIds(SimpleUserTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<SimpleUserTableQuery, SimpleUserEncodeTableQuery>(queryFilter);
            var groupIds = queryFilter.GroupIds.ToList();
            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                                .Where(groupIds.Any(), x => _userGroupRepository.DetachedEntities.AsQueryable().Any(g => !g.DeleteFlag && g.UserID == x.Id && groupIds.Contains(g.GroupID)))
                                join card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsPrincipal)
                                on user.Id equals card.UserID into cards
                                from card in cards.DefaultIfEmpty()
                                select new SimpleUserListItemDto
                                {
                                    UserKey = user.UserKey,
                                    ID = user.Id,
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
                                    IdCard = user.IdCard,
                                    Phone = user.Phone,
                                    Email = user.Email,
                                    Birthday = user.Birthday,
                                    Gender = user.Gender,
                                    Photo = user.Photo,
                                    LeaveTime = user.LeaveTime,
                                    FirstLoginTime = user.FirstLoginTime,
                                    LastLoginTime = user.LastLoginTime,
                                    SourceFrom = user.SourceFrom,
                                    CreateTime = user.CreateTime,
                                    UpdateTime = user.UpdateTime,
                                    CardNo = card != null ? card.No : "",
                                    CardStatus = card != null ? card.Status : null,
                                    CardIssueDate = card != null ? card.IssueDate : null,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                };
            var pageList = await userCardQuery.OrderByDescending(x => x.CreateTime).ThenByDescending(x => x.ID).ToPagedListAsync<SimpleUserListItemDto>(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 通过用户类型获取用户数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<SimpleUserListItemDto>> QuerySimpleUserByUserTypes(SimpleUserTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<SimpleUserTableQuery, SimpleUserEncodeTableQuery>(queryFilter);
            var typeCodes = queryFilter.UserTypeCodes.ToList();
            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                               .Where(typeCodes.Any(), x => typeCodes.Contains(x.Type))
                                join card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.IsPrincipal)
                                on user.Id equals card.UserID into cards
                                from card in cards.DefaultIfEmpty()
                                select new SimpleUserListItemDto
                                {
                                    UserKey = user.UserKey,
                                    ID = user.Id,
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
                                    IdCard = user.IdCard,
                                    Phone = user.Phone,
                                    Email = user.Email,
                                    Birthday = user.Birthday,
                                    Gender = user.Gender,
                                    Photo = user.Photo,
                                    LeaveTime = user.LeaveTime,
                                    FirstLoginTime = user.FirstLoginTime,
                                    LastLoginTime = user.LastLoginTime,
                                    SourceFrom = user.SourceFrom,
                                    CreateTime = user.CreateTime,
                                    UpdateTime = user.UpdateTime,
                                    CardNo = card != null ? card.No : "",
                                    CardStatus = card != null ? card.Status : null,
                                    CardIssueDate = card != null ? card.IssueDate : null,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                };
            var pageList = await userCardQuery.OrderByDescending(x => x.CreateTime).ThenByDescending(x => x.ID).ToPagedListAsync<SimpleUserListItemDto>(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }
    }
}
