/*********************************************************
* 名    称：CardService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者卡操作数据
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos;
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vive.Crypto;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 卡数据服务
    /// </summary>
    public class CardService : ICardService
    {
        private readonly Base64Crypt _baseEncrypt;
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly IRepository<CardProperty> _cardPropertyRepository;
        private readonly IRepository<UserProperty> _userPropertyRepository;
        private readonly IRepository<Card> _cardRepository;
        private readonly IRepository<Property> _propertyRepository;
        private readonly IRepository<PropertyGroup> _propertyGroupRepository;
        private readonly IRepository<PropertyGroupItem> _propertyGroupItemRepository;
        private readonly IRepository<UserCardClaim> _cardClaimRepository;
        private readonly IRepository<Region> _regionRepository;
        private readonly ISecurityService _securityService;

        public CardService(IDistributedIDGenerator idGenerator
             , IRepository<EntityFramework.Core.Entitys.User> userRepository
             , IRepository<Card> cardRepository
             , IRepository<CardProperty> cardPropertyRepository
             , IRepository<Property> propertyRepository
             , IRepository<PropertyGroup> propertyGroupRepository
             , IRepository<PropertyGroupItem> propertyGroupItemRepository
             , IRepository<UserCardClaim> cardClaimRepository
            , IRepository<Region> regionRepository
            , IRepository<UserProperty> userPropertyRepository
            , ISecurityService securityService)
        {
            var codeTable = FileHelper.ReadFileToText("EncodeTable.cert");
            _baseEncrypt = new Base64Crypt(codeTable);
            _idGenerator = idGenerator;
            _userRepository = userRepository;
            _cardPropertyRepository = cardPropertyRepository;
            _propertyRepository = propertyRepository;
            _propertyGroupRepository = propertyGroupRepository;
            _propertyGroupItemRepository = propertyGroupItemRepository;
            _cardRepository = cardRepository;
            _cardClaimRepository = cardClaimRepository;
            _userPropertyRepository = userPropertyRepository;
            _regionRepository = regionRepository;
            _securityService = securityService;
        }

        /// <summary>
        /// 获取卡初始信息
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetCardInitData()
        {
            var cardData = new CardDto { IssueDate = DateTime.Now.Date, ExpireDate = DateTime.Now.Date.AddYears(3), Status = (int)EnumCardStatus.正常 };//默认正常状态
            var properties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常).OrderBy(x => x.CreateTime).ToListAsync();

            var userProperties = properties.Where(x => !x.ForReader & x.ForCard && !x.SysBuildIn)
                .Select(propertyForUser => new CardPropertyDto
                {
                    PropertyID = propertyForUser.Id,
                    PropertyName = propertyForUser.Name,
                    PropertyCode = propertyForUser.Code,
                    PropertyType = propertyForUser.Type,
                    PropertyValue = "",
                    Required = propertyForUser.Required,
                    PropertyGroupID = propertyForUser.PropertyGroupID ?? Guid.Empty,
                }).ToList();
            cardData.Properties = userProperties;
            var exportProperties = new List<ExportPropertyInput>();
            exportProperties.AddRange(new[] {
                new ExportPropertyInput{PropertyName = "卡号", PropertyCode = "Card_No", External = false, PropertyType = 0 },
                new ExportPropertyInput{PropertyName = "姓名", PropertyCode = "User_Name", External = false, PropertyType = 0 },
                new ExportPropertyInput{PropertyName = "学号", PropertyCode = "User_StudentNo", External = false, PropertyType = 0 },
                new ExportPropertyInput{PropertyName = "用户类型", PropertyCode = "User_Type", External = false, PropertyType = 4 },
            });
            var exportCardProperties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常 && !x.ForReader && x.ForCard).OrderBy(x => x.CreateTime).Select(x => new ExportPropertyInput
            {
                PropertyCode = x.Code,
                PropertyName = x.Name,
                PropertyType = x.Type,
                External = !x.SysBuildIn
            }).ToListAsync();
            var exportCardNo = exportCardProperties.FirstOrDefault(x => x.PropertyCode == "Card_No");
            if (exportCardNo != null)
            {
                exportCardProperties.Remove(exportCardNo);
            }
            exportProperties.AddRange(exportCardProperties);

            var showOnTableProperties = new List<object> { };
            var showOnTableProperties_All = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常 && x.ShowOnTable).OrderBy(x => x.CreateTime).ToListAsync();
            var showOnTableProperties_ForCardField = showOnTableProperties_All.Where(x => !x.ForReader && x.ForCard && x.SysBuildIn).OrderBy(x => x.CreateTime).Select(x => new { Name = x.Name, Code = x.Code, External = !x.SysBuildIn, Type = x.Type }).ToList();
            var showOnTableProperties_ForCardProperty = showOnTableProperties_All.Where(x => !x.ForReader && x.ForCard && !x.SysBuildIn).OrderBy(x => x.CreateTime).Select(x => new { Name = x.Name, Code = x.Code, External = !x.SysBuildIn, Type = x.Type }).ToList();
            //重拍卡属性
            showOnTableProperties.AddRange(new[] {
                new { Name = "卡号", Code = "Card_No", External = false, Type = 0 },
                new { Name = "姓名", Code = "User_Name", External = false, Type = 0 },
                new { Name = "学号", Code = "User_StudentNo", External = false, Type = 0 },
                new { Name = "用户类型", Code = "User_Type", External = false, Type = 4 }
            });
            var cardNo = showOnTableProperties_ForCardField.FirstOrDefault(x => x.Code == "Card_No");
            if (cardNo != null)
            {
                showOnTableProperties_ForCardField.Remove(cardNo);
            }
            showOnTableProperties.AddRange(showOnTableProperties_ForCardField);
            showOnTableProperties.AddRange(showOnTableProperties_ForCardProperty);

            var canSearchProperties = await _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumPropertyDataStatus.正常 && x.CanSearch).OrderBy(x => x.CreateTime).Select(x => new { Name = x.Name, Code = x.Code, External = !x.SysBuildIn, Type = x.Type }).ToListAsync();
            var groupIds = properties.Select(x => x.PropertyGroupID).ToList();
            var allGroups = _propertyGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.Id)).ToList();
            var allGroupItems = _propertyGroupItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && groupIds.Contains(x.GroupID) && x.Status == (int)EnumGroupItemDataStatus.正常).ToList();
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
            var securityPolicy = await _securityService.GetSecurityPolicy();
            var dynamicData = new
            {
                cardData,
                showOnTableProperties,
                canSearchProperties,
                groupSelect = groupSelect.ToList(),
                exportProperties,
                secretLevel = securityPolicy.SecretLevel,
                SyncCardLogStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumSyncCardLogStatus)),
                SyncCardType = EnumHelper.GetEnumDictionaryItems(typeof(EnumSyncCardType)),
                SyncCardStatus = EnumHelper.GetEnumDictionaryItems(typeof(EnumSyncCardStatus)),
            };
            return await Task.FromResult(dynamicData);
        }

        /// <summary>
        /// 获取数组可选项
        /// </summary>
        /// <returns></returns>
        public async Task<List<PropertyGroupSelectDto>> GetGroupSelectItem()
        {
            var allGroups = _propertyGroupRepository.DetachedEntities.Where(x => !x.DeleteFlag).OrderBy(x => x.CreateTime).ToList();
            var allGroupItems = _propertyGroupItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Status == (int)EnumGroupItemDataStatus.正常).OrderBy(x => x.CreateTime).ToList();
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
            return await Task.FromResult(groupSelect);
        }

        /// <summary>
        /// 查询卡表格数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<CardListItemDto>> QueryTableData(CardTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<CardTableQuery, CardEncodeTableQuery>(queryFilter);
            var cardExternalProperties = from cardProperty in _cardPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                         join propertyForCard in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && !x.ForReader && x.ForCard && !x.SysBuildIn) on cardProperty.PropertyID equals propertyForCard.Id
                                         select new CardPropertyItemDto
                                         {
                                             CardID = cardProperty.CardID,
                                             PropertyID = cardProperty.PropertyID,
                                             PropertyName = propertyForCard.Name,
                                             PropertyCode = propertyForCard.Code,
                                             PropertyType = propertyForCard.Type,
                                             PropertyValue = cardProperty.PropertyValue,
                                             Required = propertyForCard.Required
                                         };

            var cardUserQuery = from card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                                .Where(queryFilter.IsPrincipal.HasValue, x => x.IsPrincipal == queryFilter.IsPrincipal)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.CardType), x => x.Type == queryFilter.CardType)
                                .Where(queryFilter.CardStatus.HasValue, x => x.Status == queryFilter.CardStatus)
                                .Where(queryFilter.CardIssueStartTime.HasValue, x => x.IssueDate > queryFilter.CardIssueStartTime)
                                .Where(queryFilter.CardIssueEndCompareTime.HasValue, x => x.IssueDate <= queryFilter.CardIssueEndCompareTime)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.CardNo), x => x.No != null && x.No.Contains(queryFilter.CardNo))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.PhysicNo), x => x.PhysicNo != null && x.PhysicNo.Contains(queryFilter.PhysicNo))
                                join user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                                .Where(
                                    (!string.IsNullOrWhiteSpace(queryFilter.Keyword), x => x.Name != null && x.Name.Contains(queryFilter.Keyword)),
                                    (!string.IsNullOrWhiteSpace(queryFilter.Keyword), x => x.StudentNo != null && x.StudentNo.Contains(queryFilter.Keyword))
                                )
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Name), x => x.Name != null && x.Name.Contains(queryFilter.Name))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.StudentNo), x => x.StudentNo != null && x.StudentNo.Contains(queryFilter.StudentNo))
                                on card.UserID equals user.Id into users
                                from user in users
                                select new CardListItemDto
                                {
                                    ID = card.Id,
                                    UserID = user.Id,
                                    UserName = user.Name,
                                    UserStudentNo = user.StudentNo,
                                    UserType = user.Type,
                                    UserTypeName = user.TypeName,
                                    No = card.No,
                                    BarCode = card.BarCode,
                                    PhysicNo = card.PhysicNo,
                                    IdentityNo = card.IdentityNo,
                                    Type = card.Type,
                                    TypeName = card.TypeName,
                                    Status = card.Status,
                                    CreateTime = card.CreateTime,
                                    UpdateTime = card.UpdateTime,
                                    Deposit = card.Deposit,
                                    IssueDate = card.IssueDate,
                                    ExpireDate = card.ExpireDate,
                                    IsPrincipal = card.IsPrincipal
                                };
            var pageList = await cardUserQuery.OrderByDescending(x => x.CreateTime).ToPagedListAsync<CardListItemDto>(queryFilter.PageIndex, queryFilter.PageSize);
            var pageCardIds = pageList.Items.Select(x => x.ID).ToArray();
            var cardPropertyList = cardExternalProperties.Where(x => pageCardIds.Contains(x.CardID)).ToList();
            foreach (var item in pageList.Items)
            {
                item.Properties = cardPropertyList.Where(x => x.CardID == item.ID);
            }
            return pageList;

        }

        /// <summary>
        /// 查询卡信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CardListItemDto>> QueryUserCardListData(Guid userId)
        {
            var cardExternalProperties = from cardProperty in _cardPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                         join propertyForCard in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && !x.ForReader && x.ForCard && !x.SysBuildIn) on cardProperty.PropertyID equals propertyForCard.Id
                                         select new CardPropertyItemDto
                                         {
                                             CardID = cardProperty.CardID,
                                             PropertyID = cardProperty.PropertyID,
                                             PropertyName = propertyForCard.Name,
                                             PropertyCode = propertyForCard.Code,
                                             PropertyType = propertyForCard.Type,
                                             PropertyValue = cardProperty.PropertyValue,
                                             Required = propertyForCard.Required
                                         };
            var cardUserQuery = from card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                                join user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.Id == userId)
                                on card.UserID equals user.Id into users
                                from user in users
                                select new CardListItemDto
                                {
                                    ID = card.Id,
                                    UserID = user.Id,
                                    UserName = user.Name,
                                    UserStudentNo = user.StudentNo,
                                    UserType = user.Type,
                                    No = card.No,
                                    BarCode = card.BarCode,
                                    PhysicNo = card.PhysicNo,
                                    IdentityNo = card.IdentityNo,
                                    Type = card.Type,
                                    Status = card.Status,
                                    CreateTime = card.CreateTime,
                                    UpdateTime = card.UpdateTime,
                                    Deposit = card.Deposit,
                                    IssueDate = card.IssueDate,
                                    ExpireDate = card.ExpireDate,
                                    IsPrincipal = card.IsPrincipal,
                                    ApproveStatus = 0,
                                    RecordType = 0,
                                };
            var pageList = cardUserQuery.OrderBy(x => x.Status).ThenByDescending(x => x.CreateTime).ToList();
            var pageCardIds = pageList.Select(x => x.ID).ToArray();
            var cardPropertyList = cardExternalProperties.Where(x => pageCardIds.Contains(x.CardID)).ToList();
            foreach (var item in pageList)
            {
                item.Properties = cardPropertyList.Where(x => x.CardID == item.ID);
            }
            return await Task.FromResult(pageList);
        }

        /// <summary>
        /// 查询读者领卡记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CardListItemDto>> QueryUserCardApplyListData(Guid userId)
        {
            var cardExternalProperties = from cardProperty in _cardPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                         join propertyForCard in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && !x.ForReader && x.ForCard && !x.SysBuildIn) on cardProperty.PropertyID equals propertyForCard.Id
                                         select new CardPropertyItemDto
                                         {
                                             CardID = cardProperty.CardID,
                                             PropertyID = cardProperty.PropertyID,
                                             PropertyName = propertyForCard.Name,
                                             PropertyCode = propertyForCard.Code,
                                             PropertyType = propertyForCard.Type,
                                             PropertyValue = cardProperty.PropertyValue,
                                             Required = propertyForCard.Required
                                         };
            var cardClaimUserQuery = from claim in _cardClaimRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag && x.UserID == userId)
                                     join card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag) on claim.CardID equals card.Id into cards
                                     from card in cards
                                     join user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag) on card.UserID equals user.Id into users
                                     from user in users
                                     select new CardListItemDto
                                     {
                                         ID = claim.Id,
                                         UserID = user.Id,
                                         UserName = user.Name,
                                         UserStudentNo = user.StudentNo,
                                         UserType = user.Type,
                                         No = card.No,
                                         BarCode = card.BarCode,
                                         PhysicNo = card.PhysicNo,
                                         IdentityNo = card.IdentityNo,
                                         Type = card.Type,
                                         Status = card.Status,
                                         CreateTime = card.CreateTime,
                                         UpdateTime = card.UpdateTime,
                                         Deposit = card.Deposit,
                                         IssueDate = card.IssueDate,
                                         ExpireDate = card.ExpireDate,
                                         IsPrincipal = card.IsPrincipal,
                                         ApproveStatus = claim.Status,
                                         RecordType = 1,
                                     };
            var pageList = cardClaimUserQuery.OrderBy(x => x.RecordType).ThenByDescending(x => x.ApproveStatus).ThenByDescending(x => x.CreateTime).ToList();
            var pageCardIds = pageList.Select(x => x.ID).ToArray();
            var cardPropertyList = cardExternalProperties.Where(x => pageCardIds.Contains(x.CardID)).ToList();
            foreach (var item in pageList)
            {
                item.Properties = cardPropertyList.Where(x => x.CardID == item.ID);
            }
            return await Task.FromResult(pageList);
        }

        /// <summary>
        /// 获取卡信息
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public async Task<CardDetailOutput> GetByID(Guid cardId)
        {
            var cardInfo = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardId);
            if (cardInfo == null)
            {
                throw Oops.Oh("未找到卡对象");
            }

            var targetCard = cardInfo.Adapt<CardDetailOutput>();
            var userInfo = await _userRepository.FirstOrDefaultAsync(x => x.Id == cardInfo.UserID);
            if (userInfo != null)
            {
                var userDto = userInfo.Adapt<UserDto>();
                targetCard.UserName = userDto.Name;
                targetCard.UserPhone = userDto.Phone;
                targetCard.UserEmail = userDto.Email;
                targetCard.UserIdCard = userDto.IdCard;
                targetCard.UserStudentNo = userDto.StudentNo;
                targetCard.UserType = userDto.Type;
            }

            var forCardProperties = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForCard && !x.ForReader && !x.SysBuildIn).OrderBy(x => x.CreateTime).Select(x => new CardPropertyItemDto
            {
                Sort = 0,
                CardID = cardId,
                PropertyID = x.Id,
                PropertyName = x.Name,
                PropertyCode = x.Code,
                PropertyType = x.Type,
                Required = x.Required,
                PropertyValue = ""
            }).ToList();
            var sortNo = 1;
            forCardProperties.ForEach(x =>
            {
                x.Sort = sortNo++;
            });

            var forUserProperties = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForCard && x.ForReader && !x.SysBuildIn).OrderBy(x => x.CreateTime).Select(x => new UserPropertyItemDto
            {
                Sort = 0,
                UserID = cardInfo.UserID,
                PropertyID = x.Id,
                PropertyName = x.Name,
                PropertyCode = x.Code,
                PropertyType = x.Type,
                Required = x.Required,
                PropertyValue = ""
            }).ToList();
            var UserSortNo = 1;
            forUserProperties.ForEach(x =>
            {
                x.Sort = UserSortNo++;
            });

            var cardExternalProperties = (from cardProperty in _cardPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.CardID == cardId)
                                          join propertyForCard in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForCard && !x.ForReader && !x.SysBuildIn) on cardProperty.PropertyID equals propertyForCard.Id
                                          select new CardPropertyItemDto
                                          {
                                              CardID = cardProperty.CardID,
                                              PropertyID = cardProperty.PropertyID,
                                              PropertyName = propertyForCard.Name,
                                              PropertyCode = propertyForCard.Code,
                                              PropertyType = propertyForCard.Type,
                                              PropertyValue = cardProperty.PropertyValue,
                                              Required = propertyForCard.Required
                                          }).ToList();
            var userExternalProperties = (from userProperty in _userPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserID == cardInfo.UserID)
                                          join propertyForUser in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForCard && x.ForReader && !x.SysBuildIn) on userProperty.PropertyID equals propertyForUser.Id
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
            forCardProperties.ForEach(x =>
            {
                var mapProperty = cardExternalProperties.FirstOrDefault(p => p.PropertyID == x.PropertyID);
                if (mapProperty != null)
                {
                    x.PropertyValue = mapProperty.PropertyValue;
                }
            });
            forUserProperties.ForEach(x =>
            {
                var mapProperty = userExternalProperties.FirstOrDefault(p => p.PropertyID == x.PropertyID);
                if (mapProperty != null)
                {
                    x.PropertyValue = mapProperty.PropertyValue;
                }
            });
            targetCard.Properties = forCardProperties.Adapt<List<CardPropertyItemOutput>>();
            targetCard.UserProperties = forUserProperties.Adapt<List<UserPropertyItemOutput>>();
            return targetCard;
        }


        /// <summary>
        /// 获取卡详情
        /// </summary>
        /// <param name="no">卡号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public async Task<CardDetailOutput> GetByNoPwd(string no, string pwd)
        {
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var pwdEncode = encryptProvider.Encrypt(pwd, SiteGlobalConfig.SM2Key.PublicKey);
            var cardInfo = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.No == no && x.Secret == pwdEncode);
            if (cardInfo == null)
            {
                cardInfo = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.No == no);
                if (cardInfo == null)
                {
                    throw Oops.Oh("未找到读者卡");
                }
                if (cardInfo.Secret != pwdEncode)
                {
                    throw Oops.Oh("读者卡密码错误");
                }
            }

            var userInfo = await _userRepository.FirstOrDefaultAsync(x => x.Id == cardInfo.UserID);
            var userDto = userInfo.Adapt<UserDto>();
            var targetCard = cardInfo.Adapt<CardDetailOutput>();
            targetCard.UserName = userDto.Name;
            targetCard.UserPhone = userDto.Phone;
            targetCard.UserEmail = userDto.Email;
            targetCard.UserIdCard = userDto.IdCard;
            var forCardProperties = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForCard && !x.ForReader && !x.SysBuildIn).OrderBy(x => x.CreateTime).Select(x => new CardPropertyItemDto
            {
                Sort = 0,
                CardID = cardInfo.Id,
                PropertyID = x.Id,
                PropertyName = x.Name,
                PropertyCode = x.Code,
                PropertyType = x.Type,
                Required = x.Required,
                PropertyValue = ""
            }).ToList();
            var sortNo = 1;
            forCardProperties.ForEach(x =>
            {
                x.Sort = sortNo++;
            });

            var cardExternalProperties = (from cardProperty in _cardPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.CardID == cardInfo.Id)
                                          join propertyForCard in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForCard && !x.ForReader && !x.SysBuildIn) on cardProperty.PropertyID equals propertyForCard.Id
                                          select new CardPropertyItemDto
                                          {
                                              CardID = cardProperty.CardID,
                                              PropertyID = cardProperty.PropertyID,
                                              PropertyName = propertyForCard.Name,
                                              PropertyCode = propertyForCard.Code,
                                              PropertyType = propertyForCard.Type,
                                              PropertyValue = cardProperty.PropertyValue,
                                              Required = propertyForCard.Required
                                          }).ToList();
            forCardProperties.ForEach(x =>
            {
                var mapProperty = cardExternalProperties.FirstOrDefault(p => p.PropertyID == x.PropertyID);
                if (mapProperty != null)
                {
                    x.PropertyValue = mapProperty.PropertyValue;
                }
            });
            targetCard.Properties = forCardProperties.Adapt<List<CardPropertyItemOutput>>();
            return targetCard;
        }

        /// <summary>
        /// 读者卡数据编辑合法性验证
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="isAdd"></param>
        /// <returns></returns>
        public async Task<Tuple<List<CardProperty>, List<CardProperty>>> CardEditValidate(CardDto cardData, bool isAdd)
        {
            if (!cardData.UserID.HasValue)
            {
                throw Oops.Oh("请选择所属读者").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (string.IsNullOrWhiteSpace(cardData.No))
            {
                throw Oops.Oh("卡号必填").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (!cardData.IssueDate.HasValue)
            {
                throw Oops.Oh("发证日期必填").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (!cardData.ExpireDate.HasValue)
            {
                throw Oops.Oh("过期日期必填").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (cardData.ExpireDate < cardData.IssueDate)
            {
                throw Oops.Oh("过期时间不能早于发证时间").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (string.IsNullOrWhiteSpace(cardData.Secret))
            {
                throw Oops.Oh("请填写密码").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (cardData.Secret != "******")
            {
                var securityPolicy = await _securityService.GetSecurityPolicy();
                if (!SecretStrongChecker.Check(cardData.Secret, securityPolicy.SecretLevel).Ok)
                {
                    throw Oops.Oh($"密码校验未通过：{SecretStrongChecker.Check(cardData.Secret, securityPolicy.SecretLevel).Err}").StatusCode(Consts.Consts.ExceptionStatus);
                }
            }

            var insertCardProperties = new List<CardProperty>();
            var updateCardProperties = new List<CardProperty>();
            if (isAdd)
            {
                var forCardProperties = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForCard && !x.ForReader && !x.SysBuildIn).OrderBy(x => x.CreateTime).ToList();
                cardData.Properties.ForEach(x =>
               {
                   var cardProperty = new CardProperty
                   {
                       Id = new Guid(_idGenerator.Create().ToString()),
                       CardID = cardData.ID,
                       PropertyID = x.PropertyID,
                       PropertyValue = x.PropertyValue,
                   };

                   var mapProperty = forCardProperties.FirstOrDefault(p => p.Id == x.PropertyID);
                   if (mapProperty != null)
                   {
                       if (mapProperty.Required && string.IsNullOrWhiteSpace(cardProperty.PropertyValue))
                       {
                           throw Oops.Oh($"{mapProperty.Name}必填").StatusCode(Consts.Consts.ExceptionStatus);
                       }
                       if (mapProperty.Unique)
                       {
                           var pValue = (cardProperty.PropertyValue ?? "").Trim();
                           var isPropertyExist = _cardPropertyRepository.Any(p => !p.DeleteFlag && p.PropertyID == mapProperty.Id && p.PropertyValue != null && p.PropertyValue != "" && p.PropertyValue == pValue);
                           if (isPropertyExist) throw Oops.Oh($"{mapProperty.Name}已存在").StatusCode(Consts.Consts.ExceptionStatus);
                       }
                       MapPropertyValue(cardProperty, mapProperty.Type);
                       insertCardProperties.Add(cardProperty);
                   }
               });
            }
            else
            {
                var forCardProperties = _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.ForCard && !x.ForReader && !x.SysBuildIn).OrderBy(x => x.CreateTime).ToList();
                var cardInputProperties = cardData.Properties;
                var cardProperties = _cardPropertyRepository.Where(x => !x.DeleteFlag && x.CardID == cardData.ID).ToList();
                //添加需要新增的属性值
                cardInputProperties.Where(x => !cardProperties.Any(p => p.PropertyID == x.PropertyID)).ToList()
                .ForEach(x =>
               {
                   var cardProperty = new CardProperty
                   {
                       Id = new Guid(_idGenerator.Create().ToString()),
                       CardID = cardData.ID,
                       PropertyID = x.PropertyID,
                       PropertyValue = x.PropertyValue,
                   };

                   var mapProperty = forCardProperties.FirstOrDefault(p => p.Id == x.PropertyID);
                   if (mapProperty != null)
                   {
                       if (mapProperty.Required && string.IsNullOrWhiteSpace(x.PropertyValue))
                       {
                           throw Oops.Oh($"{mapProperty.Name}必填");
                       }
                       if (mapProperty.Unique)
                       {
                           var pValue = (x.PropertyValue ?? "").Trim();
                           var isPropertyExist = _cardPropertyRepository.Any(p => p.CardID != cardData.ID && !p.DeleteFlag && p.PropertyID == mapProperty.Id && p.PropertyValue != null && p.PropertyValue != "" && p.PropertyValue == pValue);
                           if (isPropertyExist) throw Oops.Oh($"{mapProperty.Name}已存在");
                       }
                       MapPropertyValue(cardProperty, mapProperty.Type);
                       insertCardProperties.Add(cardProperty);
                   }
               });
                //添加需要修改的属性值
                cardInputProperties.Where(x => cardProperties.Any(p => p.PropertyID == x.PropertyID)).ToList()
                .ForEach(x =>
               {
                   var existProperty = cardProperties.FirstOrDefault(up => up.PropertyID == x.PropertyID);
                   var mapProperty = forCardProperties.FirstOrDefault(p => p.Id == x.PropertyID);
                   if (mapProperty != null)
                   {
                       if (mapProperty.Required && string.IsNullOrWhiteSpace(x.PropertyValue))
                       {
                           throw Oops.Oh($"{mapProperty.Name}必填");
                       }
                       if (mapProperty.Unique)
                       {
                           var pValue = (x.PropertyValue ?? "").Trim();
                           var isPropertyExist = _cardPropertyRepository.Any(p => p.CardID != cardData.ID && !p.DeleteFlag && p.PropertyID == mapProperty.Id && p.PropertyValue != null && p.PropertyValue != "" && p.PropertyValue == pValue);
                           if (isPropertyExist) throw Oops.Oh($"{mapProperty.Name}已存在");
                       }
                       //新值不同时为空，并且不同
                       if ((!string.IsNullOrWhiteSpace(x.PropertyValue) || !string.IsNullOrWhiteSpace(existProperty.PropertyValue))
                              && (_baseEncrypt.Encode(x.PropertyValue ?? "") != (existProperty.PropertyValue ?? "")))
                       {
                           existProperty.PropertyValue = x.PropertyValue;
                           MapPropertyValue(existProperty, mapProperty.Type);
                           existProperty.UpdateTime = DateTime.Now;
                           updateCardProperties.Add(existProperty);
                       }
                   }
               });
            }
            var cardNo = (cardData.No ?? "").Trim();
            var barCode = (cardData.BarCode ?? "").Trim();
            var physicNo = (cardData.PhysicNo ?? "").Trim();
            var isCardNoExist = await _cardRepository.AnyAsync(u => u.Id != cardData.ID && u.No != null && u.No != "" && cardNo != "" && u.No == cardNo && !u.DeleteFlag, false, true);
            if (isCardNoExist) throw Oops.Oh("卡号已存在").StatusCode(Consts.Consts.ExceptionStatus);
            var isBarCodeExist = await _cardRepository.AnyAsync(u => u.Id != cardData.ID && u.BarCode != null && u.BarCode != "" && barCode != "" && u.BarCode == barCode && !u.DeleteFlag, false, true);
            if (isBarCodeExist) throw Oops.Oh("条码号已存在").StatusCode(Consts.Consts.ExceptionStatus);
            var isPhysicNoExist = await _cardRepository.AnyAsync(u => u.Id != cardData.ID && u.PhysicNo != null && u.PhysicNo != "" && physicNo != "" && u.PhysicNo == physicNo && !u.DeleteFlag, false, true);
            if (isPhysicNoExist) throw Oops.Oh("物理码已存在").StatusCode(Consts.Consts.ExceptionStatus);
            return new Tuple<List<CardProperty>, List<CardProperty>>(insertCardProperties, updateCardProperties);
        }

        /// <summary>
        /// 映射卡类型名称
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public async Task<CardDto> MapPropertyName(CardDto cardData)
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
            if (!string.IsNullOrWhiteSpace(cardData.Type))
            {
                var mapProperty = groupDatas.Where(x => x.Code == "Card_Type").SelectMany(x => x.Items).FirstOrDefault(x => x.Code == cardData.Type);
                cardData.TypeName = mapProperty?.Name;
            }
            return cardData;
        }

        /// <summary>
        /// 映射卡类型
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public async Task<CardBatchEditInput> MapBatchPropertyName(CardBatchEditInput cardData)
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
            if (!string.IsNullOrWhiteSpace(cardData.Type))
            {
                var mapProperty = groupDatas.Where(x => x.Code == "Card_Type").SelectMany(x => x.Items).FirstOrDefault(x => x.Code == cardData.Type);
                cardData.TypeName = mapProperty?.Name;
            }
            return cardData;
        }

        /// <summary>
        /// 创建卡
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public async Task<Guid> Create(CardDto cardData)
        {
            cardData.ID = _idGenerator.CreateGuid(cardData.ID);
            var validateResult = await this.CardEditValidate(cardData, true);
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var encodePwd = encryptProvider.Encrypt(cardData.Secret, SiteGlobalConfig.SM2Key.PublicKey);
            cardData.Secret = encodePwd;
            cardData.SecretChangeTime = DateTime.Now;
            var cardEntity = cardData.Adapt<Card>();

            var cardEntry = await _cardRepository.InsertNowAsync(cardEntity);
            await _cardPropertyRepository.InsertAsync(validateResult.Item1);
            if (cardData.IsPrincipal)
            {
                await SetPrincipalCard(cardEntity.Id, cardEntity.UserID);
            }
            return cardEntry.Entity.Id;
        }

        /// <summary>
        /// 编辑卡信息
        /// </summary>
        /// <param name="cardData"></param>
        /// <returns></returns>
        public async Task<Guid> Update(CardDto cardData)
        {
            var validateResult = await this.CardEditValidate(cardData, false);

            var cardEntity = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardData.ID);
            if (cardEntity == null)
            {
                throw Oops.Oh("未找到卡对象");
            }
            var oriSecret = cardEntity.Secret;
            if (cardData.Secret != "******")
            {
                var securityPolicy = await _securityService.GetSecurityPolicy();
                if (!SecretStrongChecker.Check(cardData.Secret, securityPolicy.SecretLevel).Ok)
                {
                    throw Oops.Oh($"密码校验未通过：{SecretStrongChecker.Check(cardData.Secret, securityPolicy.SecretLevel).Err}").StatusCode(Consts.Consts.ExceptionStatus);
                }

                var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
                var encodePwd = encryptProvider.Encrypt(cardData.Secret ?? "", SiteGlobalConfig.SM2Key.PublicKey);
                cardData.Secret = encodePwd;
                cardData.SecretChangeTime = DateTime.Now;
            }
            else
            {
                cardData.Secret = oriSecret;
            }
            cardEntity = cardData.Adapt(cardEntity);
            var cardEntityEntry = await _cardRepository.UpdateAsync(cardEntity);
            if (validateResult.Item1.Any())
            {
                await _cardPropertyRepository.InsertAsync(validateResult.Item1);
            }
            if (validateResult.Item2.Any())
            {
                await _cardPropertyRepository.UpdateAsync(validateResult.Item2);
            }
            if (cardData.IsPrincipal)
            {
                await SetPrincipalCard(cardEntity.Id, cardEntity.UserID);
            }
            return cardEntityEntry.Entity.Id;
        }

        /// <summary>
        /// 设置读者卡密码
        /// </summary>
        /// <param name="cardSecret"></param>
        /// <returns></returns>
        public async Task<bool> SetSecret(CardSecretDto cardSecret)
        {
            var cardEntity = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardSecret.CardId);
            if (cardEntity == null)
            {
                throw Oops.Oh("未找到读者卡信息").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var oriSecret = cardEntity.Secret;
            if (cardSecret.Secret != "******")
            {
                var securityPolicy = await _securityService.GetSecurityPolicy();
                if (!SecretStrongChecker.Check(cardSecret.Secret, securityPolicy.SecretLevel).Ok)
                {
                    throw Oops.Oh($"密码校验未通过：{SecretStrongChecker.Check(cardSecret.Secret, securityPolicy.SecretLevel).Err}").StatusCode(Consts.Consts.ExceptionStatus);
                }
                var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
                var encodePwd = encryptProvider.Encrypt(cardSecret.Secret ?? "", SiteGlobalConfig.SM2Key.PublicKey);
                cardEntity.Secret = encodePwd;
                cardEntity.SecretChangeTime = DateTime.Now;
            }
            else
            {
                cardEntity.Secret = oriSecret;
            }
            await _cardRepository.UpdateAsync(cardEntity);
            return true;
        }


        /// <summary>
        /// 重置读者卡密码
        /// </summary>
        /// <param name="cardSecret"></param>
        /// <returns></returns>
        public async Task<bool> ResetSecret(CardSecretDto cardSecret)
        {
            var cardEntity = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardSecret.CardId);
            if (cardEntity == null)
            {
                throw Oops.Oh("未找到读者卡信息").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var encodePwd = encryptProvider.Encrypt(cardEntity.No ?? "", SiteGlobalConfig.SM2Key.PublicKey);
            cardEntity.Secret = encodePwd;
            cardEntity.SecretChangeTime = DateTime.Now;
            await _cardRepository.UpdateAsync(cardEntity);
            return true;
        }

        /// <summary>
        /// 卡数据删除
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid cardId)
        {
            var cardEntity = await _cardRepository.FirstOrDefaultAsync(x => x.Id == cardId);
            if (cardEntity != null)
            {
                cardEntity.DeleteFlag = true;
                cardEntity.UpdateTime = DateTime.Now;
                await _cardRepository.UpdateAsync(cardEntity);
            }
            return true;
        }

        /// <summary>
        /// 批量更新卡信息
        /// </summary>
        /// <param name="batchEditData"></param>
        /// <returns></returns>
        public async Task<bool> BatchUpdate(CardBatchEditInput batchEditData)
        {
            var selectFields = batchEditData.Fields.Select(x => x.ToLower()).ToList();
            var updateBuilder = _userRepository.Context.BatchUpdate<Card>();
            updateBuilder = selectFields.Contains(nameof(batchEditData.Type).ToLower()) ? updateBuilder.Set(b => b.Type, b => batchEditData.Type) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.Type).ToLower()) ? updateBuilder.Set(b => b.TypeName, b => batchEditData.TypeName) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.Status).ToLower()) && batchEditData.Status.HasValue ? updateBuilder.Set(b => b.Status, b => batchEditData.Status.Value) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.IssueDate).ToLower()) && batchEditData.IssueDate.HasValue ? updateBuilder.Set(b => b.IssueDate, b => batchEditData.IssueDate.Value) : updateBuilder;
            updateBuilder = selectFields.Contains(nameof(batchEditData.ExpireDate).ToLower()) && batchEditData.ExpireDate.HasValue ? updateBuilder.Set(b => b.ExpireDate, b => batchEditData.ExpireDate.Value) : updateBuilder;
            updateBuilder = updateBuilder.Set(b => b.UpdateTime, b => DateTime.Now);
            updateBuilder.Where(x => !x.DeleteFlag && batchEditData.CardIDList.Contains(x.Id));
            await updateBuilder.ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 获取待导出读者卡数据
        /// </summary>
        /// <param name="exportFilter"></param>
        /// <returns></returns>
        public Task<PagedList<CardListItemDto>> ExportCardData(CardExportFilter exportFilter)
        {
            throw new NotImplementedException();
        }

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

        private void MapPropertyValue(CardProperty cardProperty, int propertyType)
        {
            if (string.IsNullOrWhiteSpace(cardProperty.PropertyValue))
            {
                return;
            }
            switch (propertyType)
            {
                case (int)EnumPropertyType.数值:
                    cardProperty.NumValue = DataConverter.ToNullableDecimal(cardProperty.PropertyValue);
                    break;
                case (int)EnumPropertyType.时间:
                    cardProperty.TimeValue = DataConverter.ToNumableDateTime(cardProperty.PropertyValue);
                    break;
                case (int)EnumPropertyType.是非:
                    cardProperty.BoolValue = DataConverter.ToNullableBoolean(cardProperty.PropertyValue);
                    break;
                case (int)EnumPropertyType.地址:
                    cardProperty.PropertyValue = this.MapAddrName(cardProperty.PropertyValue);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 修改读者卡密码
        /// </summary>
        /// <param name="cardPwdChange"></param>
        /// <returns></returns>
        public async Task<bool> ChangeCardPwd(ReaderCardPwdChangeInput cardPwdChange)
        {
            var cardEntity = await _cardRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == cardPwdChange.CardID);
            if (cardEntity == null)
            {
                throw Oops.Oh("读者卡不存在").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var prePwdEncode = encryptProvider.Encrypt(cardPwdChange.PrePwd, SiteGlobalConfig.SM2Key.PublicKey);
            if (prePwdEncode != cardEntity.Secret)
            {
                throw Oops.Oh("原读者卡密码不匹配").StatusCode(Consts.Consts.ExceptionStatus);
            }
            if (cardPwdChange.NewPwd != "******")
            {
                var securityPolicy = await _securityService.GetSecurityPolicy();
                if (!SecretStrongChecker.Check(cardPwdChange.NewPwd, securityPolicy.SecretLevel).Ok)
                {
                    throw Oops.Oh($"密码校验未通过：{SecretStrongChecker.Check(cardPwdChange.NewPwd, securityPolicy.SecretLevel).Err}").StatusCode(Consts.Consts.ExceptionStatus);
                }
                var encodePwd = encryptProvider.Encrypt(cardPwdChange.NewPwd ?? "", SiteGlobalConfig.SM2Key.PublicKey);
                cardEntity.Secret = encodePwd;
                cardEntity.SecretChangeTime = DateTime.Now;
                cardEntity.UpdateTime = DateTime.Now;
            }
            await _cardRepository.UpdateAsync(cardEntity);
            return true;
        }

        /// <summary>
        /// 设置读者主卡
        /// </summary>
        /// <param name="cardId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> SetPrincipalCard(Guid cardId, Guid userId)
        {
            var card = await _cardRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardId);
            if (card == null)
            {
                throw Oops.Oh("未找到读者卡信息");
            }
            if (userId != card.UserID)
            {
                throw Oops.Oh("当前读者不是卡所有人");
            }
            var updateBuilder = _userRepository.Context.BatchUpdate<Card>();
            updateBuilder = updateBuilder.Set(b => b.IsPrincipal, b => false);
            updateBuilder.Where(x => !x.DeleteFlag && x.UserID == userId && x.Id != cardId);
            await updateBuilder.ExecuteAsync();
            var updateBuilder1 = _userRepository.Context.BatchUpdate<Card>();
            updateBuilder1 = updateBuilder1.Set(b => b.IsPrincipal, b => true);
            updateBuilder1.Where(x => !x.DeleteFlag && x.UserID == userId && x.Id == cardId);
            await updateBuilder1.ExecuteAsync();
            return true;
        }
    }
}
