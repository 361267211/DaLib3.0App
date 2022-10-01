/*********************************************************
* 名    称：UserIoService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户数据导入导出服务
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
using SmartLibrary.User.Application.Dtos.Card;
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.Dtos.User;
using SmartLibrary.User.Application.Services.Enum;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Utils;
using SmartLibrary.User.EntityFramework.Core.DbContexts;
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
    /// 用户数据导入导出服务
    /// </summary>
    public class UserIoService : IUserIoService, IScoped
    {
        private readonly UserDbContext _dbContext;
        private readonly IRepository<UserImportTempData> _userTempRepository;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly IRepository<Card> _cardRepository;
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<UserProperty> _userPropertyRepository;
        private readonly IRepository<Property> _propertyRepository;
        private readonly IRepository<CardProperty> _cardPropertyRepository;
        private readonly IRepository<UserGroup> _userGroupRepository;
        private readonly IRepository<PropertyGroup> _propertyGroupRepository;
        private readonly IRepository<PropertyGroupItem> _propertyGroupItemRepository;
        private readonly TenantInfo _tenantInfo;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userTempRepository"></param>
        /// <param name="idGenerator"></param>
        /// <param name="userPropertyRepository"></param>
        /// <param name="propertyRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="cardRepository"></param>
        /// <param name="cardPropertyRepository"></param>
        /// <param name="userGroupRepository"></param>
        /// <param name="propertyGroupRepository"></param>
        /// <param name="propertyGroupItemRepository"></param>
        /// <param name="tenantInfo"></param>
        public UserIoService(UserDbContext dbContext
            , IRepository<UserImportTempData> userTempRepository
            , IDistributedIDGenerator idGenerator
            , IRepository<UserProperty> userPropertyRepository
            , IRepository<Property> propertyRepository
            , IRepository<EntityFramework.Core.Entitys.User> userRepository
            , IRepository<Card> cardRepository
            , IRepository<CardProperty> cardPropertyRepository
            , IRepository<UserGroup> userGroupRepository
            , IRepository<PropertyGroup> propertyGroupRepository
            , IRepository<PropertyGroupItem> propertyGroupItemRepository
            , TenantInfo tenantInfo)
        {
            _dbContext = dbContext;
            _userTempRepository = userTempRepository;
            _idGenerator = idGenerator;
            _userPropertyRepository = userPropertyRepository;
            _propertyRepository = propertyRepository;
            _userRepository = userRepository;
            _cardRepository = cardRepository;
            _cardPropertyRepository = cardPropertyRepository;
            _userGroupRepository = userGroupRepository;
            _propertyGroupRepository = propertyGroupRepository;
            _propertyGroupItemRepository = propertyGroupItemRepository;
            _tenantInfo = tenantInfo;
        }

        /// <summary>
        /// 验证手机号、身份证号
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public List<UserImportTempDataDto> ValidateData(List<UserImportTempDataDto> userData)
        {
            userData.ForEach(x =>
            {

                if (!x.UserPhone.TryValidate(ValidationPattern.AtLeastOne, ExtensionValidationTypes.Empty, ValidationTypes.PhoneNumber).IsValid)
                {
                    x.Error = true;
                    x.ErrorMsg += "手机号格式错误;";
                }
                if (!x.UserPhone.TryValidate(ValidationPattern.AtLeastOne, ExtensionValidationTypes.Empty, ValidationTypes.IDCard, ExtensionValidationTypes.Passport, ExtensionValidationTypes.HKCard, ExtensionValidationTypes.TWCard).IsValid)
                {
                    x.Error = true;
                    x.ErrorMsg += "身份证号格式错误;";
                }
                x.UserKey = $"{_tenantInfo.Name}_{x.StudentNo}";
            });
            return userData;
        }
        /// <summary>
        /// 验证编码
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public List<UserImportTempDataDto> ValidateUserCode(List<UserImportTempDataDto> userData)
        {
            userData.ForEach(x =>
            {

                if (string.IsNullOrWhiteSpace(x.UserType) && !string.IsNullOrWhiteSpace(x.UserTypeName))
                {
                    x.Error = true;
                    x.ErrorMsg += "系统中未找到所填读者类型;";
                }
                if (string.IsNullOrWhiteSpace(x.CardType) && !string.IsNullOrWhiteSpace(x.CardTypeName))
                {
                    x.Error = true;
                    x.ErrorMsg += "系统中未找到所填读者卡类型;";
                }
            });
            return userData;
        }

        /// <summary>
        /// 映射编码
        /// </summary>
        /// <param name="userDatas"></param>
        /// <returns></returns>
        public async Task<List<UserImportTempDataDto>> MapPropertyCode(List<UserImportTempDataDto> userDatas)
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
            userDatas.ForEach(userData =>
            {
                if (!string.IsNullOrWhiteSpace(userData.CollegeName))
                {
                    var mapProperty = groupDatas.Where(x => x.Code == "User_College").SelectMany(x => x.Items).FirstOrDefault(x => x.Name == userData.CollegeName);
                    userData.College = mapProperty?.Code;
                }
                if (!string.IsNullOrWhiteSpace(userData.CollegeDepartName))
                {
                    var mapProperty = groupDatas.Where(x => x.Code == "User_CollegeDepart").SelectMany(x => x.Items).FirstOrDefault(x => x.Name == userData.CollegeDepartName);
                    userData.CollegeDepart = mapProperty?.Code;
                }
                if (!string.IsNullOrWhiteSpace(userData.UserTypeName))
                {
                    var mapProperty = groupDatas.Where(x => x.Code == "User_Type").SelectMany(x => x.Items).FirstOrDefault(x => x.Name == userData.UserTypeName);
                    userData.UserType = mapProperty?.Code;
                }
                if (!string.IsNullOrWhiteSpace(userData.CardTypeName))
                {
                    var mapProperty = groupDatas.Where(x => x.Code == "Card_Type").SelectMany(x => x.Items).FirstOrDefault(x => x.Name == userData.CardTypeName);
                    userData.CardType = mapProperty?.Code;
                }
            });
            return userDatas;
        }

        /// <summary>
        /// 数据导入
        /// </summary>
        /// <param name="importTempData"></param>
        /// <returns></returns>
        public async Task<bool> ImportUserTempData(List<UserImportTempData> importTempData)
        {
            var nowTime = DateTime.Now;
            var updateBuilder = _dbContext.BatchUpdate<UserImportTempData>();
            await updateBuilder.Set(s => s.DeleteFlag, s => true)
                .Where(s => !s.DeleteFlag && s.ExpireTime < nowTime)
                .ExecuteAsync();
            await _dbContext.BulkInsertAsync(importTempData);
            return true;
        }

        /// <summary>
        /// 检查临时导入数据
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserTempData(Guid batchId)
        {
            var updateBuilder1 = _dbContext.BatchUpdate<UserImportTempData>();//性别必填
            var updateBuilder2 = _dbContext.BatchUpdate<UserImportTempData>();//手机号必填
            var updateBuilder3 = _dbContext.BatchUpdate<UserImportTempData>();//读者类型必填
            var updateBuilder4 = _dbContext.BatchUpdate<UserImportTempData>();//卡号必填
            var updateBuilder5 = _dbContext.BatchUpdate<UserImportTempData>();//读者卡类型必填
            var updateBuilder6 = _dbContext.BatchUpdate<UserImportTempData>();//学工号必填
            var updateBuilder7 = _dbContext.BatchUpdate<UserImportTempData>();//卡号是否重复
            var updateBuilder8 = _dbContext.BatchUpdate<UserImportTempData>();//新增读者手机号重复
            var updateBuilder9 = _dbContext.BatchUpdate<UserImportTempData>();//新增读者学号重复
            // 必填项验证
            await updateBuilder1
              .Set(s => s.Error, s => true)
              .Set(s => s.ErrorMsg, s => s.ErrorMsg + "性别必填;")
              .Where(s => !s.DeleteFlag && s.BatchId == batchId && (s.UserGender == null || s.UserGender == ""))
              .ExecuteAsync();
            await updateBuilder2
              .Set(s => s.Error, s => true)
              .Set(s => s.ErrorMsg, s => s.ErrorMsg + "手机号必填;")
              .Where(s => !s.DeleteFlag && s.BatchId == batchId && (s.UserPhone == null || s.UserPhone == ""))
              .ExecuteAsync();
            await updateBuilder3
              .Set(s => s.Error, s => true)
              .Set(s => s.ErrorMsg, s => s.ErrorMsg + "读者类型必填;")
              .Where(s => !s.DeleteFlag && s.BatchId == batchId && (s.UserType == null || s.UserType == ""))
              .ExecuteAsync();
            await updateBuilder4
              .Set(s => s.Error, s => true)
              .Set(s => s.ErrorMsg, s => s.ErrorMsg + "卡号必填;")
              .Where(s => !s.DeleteFlag && s.BatchId == batchId && (s.CardNo == null || s.CardNo == ""))
              .ExecuteAsync();
            await updateBuilder5
              .Set(s => s.Error, s => true)
              .Set(s => s.ErrorMsg, s => s.ErrorMsg + "读者卡类型必填;")
              .Where(s => !s.DeleteFlag && s.BatchId == batchId && (s.CardType == null || s.CardType == ""))
              .ExecuteAsync();
            await updateBuilder6
              .Set(s => s.Error, s => true)
              .Set(s => s.ErrorMsg, s => s.ErrorMsg + "学工号必填;")
              .Where(s => !s.DeleteFlag && s.BatchId == batchId && (s.StudentNo == null || s.StudentNo == ""))
              .ExecuteAsync();

            //检查卡号是否已存在
            var sameExistCardNoQuery = from tempData in _dbContext.Set<UserImportTempData>().Where(x => !x.DeleteFlag && x.BatchId == batchId)
                                       where _dbContext.Set<Card>().Any(x => !x.DeleteFlag && x.No != null && x.No != "" && x.No == tempData.CardNo)
                                       select tempData.Id;
            var sameExistCardNoIds = sameExistCardNoQuery.ToList();


            await updateBuilder7
             .Set(s => s.Error, s => true)
             .Set(s => s.ErrorMsg, s => s.ErrorMsg + "卡号已存在;")
             .Where(s => !s.DeleteFlag && s.BatchId == batchId && sameExistCardNoIds.Any(x => x == s.Id))
             .ExecuteAsync();

            //检查需新增读者手机号是否重复
            var sameExistPhoneQuery = from tempData in _dbContext.Set<UserImportTempData>().Where(x => !x.DeleteFlag && x.BatchId == batchId)
                                      where _dbContext.Set<EntityFramework.Core.Entitys.User>().Any(x => !x.DeleteFlag && (x.Phone != null && x.Phone != "" && x.Phone == tempData.UserPhone)
                                      && ((x.IdCard != null && x.IdCard != "" && x.IdCard != tempData.IdCard) && (x.Phone != null && x.Phone != "" && !(x.Name == tempData.UserName && x.Phone == tempData.UserPhone))))
                                      select tempData.Id;
            var sameExistPhoneIds = sameExistPhoneQuery.ToList();
            await updateBuilder8
                .Set(s => s.Error, s => true)
                .Set(s => s.ErrorMsg, s => s.ErrorMsg + "手机号已存在;")
                .Where(s => !s.DeleteFlag && s.BatchId == batchId && sameExistPhoneIds.Any(x => x == s.Id))
                .ExecuteAsync();

            //检查需新增读者学号是否重复
            var sameExistStudentNoQuery = from tempData in _dbContext.Set<UserImportTempData>().Where(x => !x.DeleteFlag && x.BatchId == batchId)
                                          where _dbContext.Set<EntityFramework.Core.Entitys.User>().Any(x => !x.DeleteFlag && (x.StudentNo != null && x.StudentNo != "" && x.StudentNo == tempData.StudentNo)
                                           && ((x.IdCard != null && x.IdCard != "" && x.IdCard != tempData.IdCard) && (x.Phone != null && x.Phone != "" && !(x.Name == tempData.UserName && x.Phone == tempData.UserPhone))))
                                          select tempData.Id;
            var sameExistStudentNoIds = sameExistStudentNoQuery.ToList();
            await updateBuilder9
                .Set(s => s.Error, s => true)
                .Set(s => s.ErrorMsg, s => s.ErrorMsg + "学号已存在;")
                .Where(s => !s.DeleteFlag && s.BatchId == batchId && sameExistStudentNoIds.Any(x => x == s.Id))
                .ExecuteAsync();
            return true;
        }

        /// <summary>
        /// 查询临时导入数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<UserImportTempData>> QueryImportTempUserData(UserTempDataTableQuery queryFilter)
        {
            var nowTime = DateTime.Now;
            var tempUserQuery = from tempUser in _userTempRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.BatchId == queryFilter.BatchId)
                                .Where(x => x.ExpireTime > nowTime)
                                .Where(queryFilter.IsError.HasValue, x => x.Error == queryFilter.IsError)
                                select tempUser;
            var pageList = await tempUserQuery.OrderBy(x => x.CreateTime).ThenBy(x => x.Id).ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 数据导入确认
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public async Task<UserImportResultDto> ImportUserConfirm(Guid batchId)
        {
            var result = new UserImportResultDto();
            var nowTime = DateTime.Now;
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var readerValidateInfos = await _userRepository.DetachedEntities.Where(x => !x.DeleteFlag).Select(x => new ReaderValidateInfo
            {
                ID = x.Id,
                Name = x.Name,
                StudentNo = x.StudentNo,
                Phone = x.Phone,
                IdCard = x.IdCard,
            }).ToListAsync();
            var cardValidateInfos = await _cardRepository.DetachedEntities.Where(x => !x.DeleteFlag).Select(x => new CardValidateInfo
            {
                UserID = x.UserID,
                No = x.No,
            }).ToListAsync();
            //使用HashSet，查重检索效率高
            var idCardSets = new HashSet<string>();
            var phoneSets = new HashSet<string>();
            var studentNoSets = new HashSet<string>();
            var userKeySets = new HashSet<string>();
            var cardNoSets = new HashSet<string>();
            var cardUserIdSets = new HashSet<Guid>();
            //将需要用于验证重复的字段装载到hash表中
            readerValidateInfos.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.IdCard))
                {
                    idCardSets.Add(x.IdCard.Trim());
                }
                if (!string.IsNullOrWhiteSpace(x.Phone))
                {
                    phoneSets.Add($"{(x.Name ?? "").Trim()}_{x.Phone.Trim()}");
                }
                if (!string.IsNullOrWhiteSpace(x.StudentNo))
                {
                    studentNoSets.Add(x.StudentNo.Trim());
                }
            });

            cardValidateInfos.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.No))
                {
                    cardNoSets.Add(x.No.Trim());
                }
                if (x.UserID != Guid.Empty)
                {
                    cardUserIdSets.Add(x.UserID);
                }
            });

            var userTempData = await _userTempRepository.DetachedEntities.Where(x => !x.DeleteFlag && !x.Error && x.ExpireTime > nowTime && x.BatchId == batchId).ToListAsync();
            var errorCount = await _userTempRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Error && x.ExpireTime > nowTime && x.BatchId == batchId).CountAsync();
            var needInsertUser = new List<EntityFramework.Core.Entitys.User>();
            var needInsertCard = new List<Card>();
            userTempData.ForEach(x =>
            {
                var addUser = true;
                var addCard = true;
                var newUser = new EntityFramework.Core.Entitys.User
                {
                    Id = x.Id,
                    Name = x.UserName,
                    Gender = x.UserGender,
                    Phone = x.UserPhone,
                    Type = x.UserType,
                    StudentNo = x.StudentNo,
                    Unit = x.Unit,
                    Edu = x.Edu,
                    College = x.College,
                    Major = x.Major,
                    Grade = x.Grade,
                    Class = x.Class,
                    IdCard = x.IdCard,
                    Email = x.Email,
                    Birthday = x.Birthday,
                    Addr = x.Addr,
                    AddrDetail = x.AddrDetail,
                    SourceFrom = (int)EnumUserSourceFrom.后台导入,
                    Status = (int)EnumUserStatus.正常,
                    UserKey = x.UserKey,
                };
                var newCard = new Card
                {
                    Id = _idGenerator.CreateGuid(),
                    UserID = Guid.Empty,
                    No = x.CardNo,
                    Type = x.CardType,
                    Status = (int)EnumCardStatus.正常,
                    IsPrincipal = false,
                    IssueDate = DateTime.Now.Date,
                    ExpireDate = DateTime.Now.Date.AddYears(3),
                    Deposit = 0,
                    Secret = GenerateSecret(x.UserPhone, encryptProvider),
                    SecretChangeTime = DateTime.Now
                };
                if (idCardSets.Contains((x.IdCard ?? "").Trim()))
                {
                    addUser = false;
                }
                if (phoneSets.Contains($"{(x.UserName ?? "").Trim()}_{(x.UserPhone ?? "").Trim()}"))
                {
                    addUser = false;
                }
                if (addUser)
                {
                    needInsertUser.Add(newUser);
                    if (!string.IsNullOrWhiteSpace(newUser.IdCard))
                    {
                        idCardSets.Add(newUser.IdCard.Trim());
                    }
                    if (!string.IsNullOrWhiteSpace(newUser.Phone))
                    {
                        phoneSets.Add($"{(newUser.Name ?? "").Trim()}_{newUser.Phone.Trim()}");
                    }
                    readerValidateInfos.Add(new ReaderValidateInfo
                    {
                        ID = newUser.Id,
                        Name = newUser.Name.Trim(),
                        StudentNo = newUser.StudentNo.Trim(),
                        Phone = newUser.Phone.Trim(),
                        IdCard = newUser.IdCard.Trim(),
                        IsNew = true,
                    });
                }
                if (cardNoSets.Contains((x.CardNo ?? "").Trim()))
                {
                    addCard = false;
                }
                if (addCard)
                {
                    if (addUser)
                    {
                        newCard.UserID = newUser.Id;
                    }
                    else
                    {
                        var mapUser = readerValidateInfos.FirstOrDefault(r => (r.IdCard ?? "").Trim() == (x.IdCard ?? "").Trim());
                        if (mapUser == null)
                        {
                            mapUser = readerValidateInfos.FirstOrDefault(r => (r.Name ?? "").Trim() == (x.UserName ?? "").Trim() && (r.Phone ?? "").Trim() == (x.UserPhone ?? "").Trim());
                        }
                        newCard.UserID = mapUser != null ? mapUser.ID : newCard.UserID;
                    }
                    if (newCard.UserID != Guid.Empty)
                    {
                        newCard.IsPrincipal = !cardUserIdSets.Contains(newCard.UserID);
                        needInsertCard.Add(newCard);
                        if (!string.IsNullOrWhiteSpace(newCard.No))
                        {
                            cardNoSets.Add(newCard.No.Trim());
                        }
                        cardUserIdSets.Add(newCard.UserID);
                        cardValidateInfos.Add(new CardValidateInfo
                        {
                            No = newCard.No.Trim(),
                            UserID = newCard.UserID,
                            IsNew = true
                        });
                    }
                }
            });
            result.SucCount = needInsertCard.Count;
            result.ErrCount = errorCount;
            await _dbContext.BulkInsertAsync(needInsertUser);
            await _dbContext.BulkInsertAsync(needInsertCard);

            return result;
        }

        /// <summary>
        /// 导出读者数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ExportUserListItemDto>> QueryExportUserTableData(UserExportFilter queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<UserExportFilter, UserEncodeExportFilter>(queryFilter);
            var userExternalProperties = from userProperty in _userPropertyRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                         join propertyForUser in _propertyRepository.DetachedEntities.Where(x => !x.DeleteFlag && !x.SysBuildIn && x.Status == (int)EnumPropertyDataStatus.正常) on userProperty.PropertyID equals propertyForUser.Id into propertyForUsers
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
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Depart), x => x.Depart != null && x.Depart.Equals(queryFilter.Depart))
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
                               .Where(queryFilter.GroupID.HasValue, x => _userGroupRepository.DetachedEntities.Any(u => !u.DeleteFlag && u.UserID == x.Id && u.GroupID == queryFilter.GroupID))

                                join card in _cardRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.IsPrincipal)
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
                                select new ExportUserListItemDto
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
                                    CardStatus = card != null ? card.Status : null,
                                    CardType = card != null ? card.Type : "",
                                    CardTypeName = card != null ? card.TypeName : "",
                                    CardBarCode = card != null ? card.BarCode : "",
                                    CardPhysicNo = card != null ? card.PhysicNo : "",
                                    CardIdentityNo = card != null ? card.IdentityNo : "",
                                    CardIssueDate = card != null ? card.IssueDate : null,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                    CardDeposit = card != null ? card.Deposit : null,
                                    CardIsPrincipal = card != null ? card.IsPrincipal : null,
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
        /// 获取简要信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<UserExportBriefDto> GetExportUserDataBriefInfo(UserExportFilter queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<UserExportFilter, UserEncodeExportFilter>(queryFilter);
            var userCardQuery = from user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Name), x => x.Name.Contains(queryFilter.Name))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.NickName), x => x.NickName != null && x.NickName.Contains(queryFilter.NickName))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.StudentNo), x => x.StudentNo != null && x.StudentNo.Contains(queryFilter.StudentNo))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Unit), x => x.Unit != null && x.Unit.Contains(queryFilter.Unit))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Edu), x => x.Edu != null && x.Edu.Equals(queryFilter.Edu))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Title), x => x.Title != null && x.Title.Equals(queryFilter.Title))
                               .Where(!string.IsNullOrWhiteSpace(queryFilter.Depart), x => x.Depart != null && x.Depart.Equals(queryFilter.Depart))
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
                               .Where(queryFilter.GroupID.HasValue, x => _userGroupRepository.DetachedEntities.Any(u => !u.DeleteFlag && u.UserID == x.Id && u.GroupID == queryFilter.GroupID))
                                join card in _cardRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.IsPrincipal)
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
                                select new ExportUserListItemDto
                                {
                                    ID = user.Id,
                                    Name = user.Name,
                                    NickName = user.NickName,
                                    StudentNo = user.StudentNo,
                                    CardNo = card != null ? card.No : "",
                                    CardStatus = card != null ? card.Status : null,
                                    CardType = card != null ? card.Type : "",
                                    CardTypeName = card != null ? card.TypeName : "",
                                    CardBarCode = card != null ? card.BarCode : "",
                                    CardPhysicNo = card != null ? card.PhysicNo : "",
                                    CardIdentityNo = card != null ? card.IdentityNo : "",
                                    CardIssueDate = card != null ? card.IssueDate : null,
                                    CardExpireDate = card != null ? card.ExpireDate : null,
                                    CardDeposit = card != null ? card.Deposit : null,
                                    CardIsPrincipal = card != null ? card.IsPrincipal : null,
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
            var totalCount = await userCardQuery.CountAsync();
            var pageSize = 5000;
            var totalPage = (totalCount / pageSize) + 1;

            return new UserExportBriefDto { TotalCount = totalCount, PageSize = pageSize, TotalPages = totalPage };
        }

        /// <summary>
        /// 国密SM2生成密码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="encryptProvider"></param>
        /// <returns></returns>
        private string GenerateSecret(string phone, IAsymmetricProvider encryptProvider)
        {
            var secret = "";
            if ((phone ?? "").Length >= 6)
            {
                secret = phone.Substring(phone.Length - 6, 6);
            }
            else
            {
                secret = phone ?? "";
            }

            var encodePwd = encryptProvider.Encrypt(secret, SiteGlobalConfig.SM2Key.PublicKey);
            return encodePwd;
        }

        /// <summary>
        /// 获取待查询读者卡数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<ExportCardListItemDto>> QueryExportCardTableData(CardExportFilter queryFilter)
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
                                .Where(queryFilter.IsPrincipal.HasValue, x => x.IsPrincipal == queryFilter.IsPrincipal)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.CardType), x => x.Type == queryFilter.CardType)
                                .Where(queryFilter.CardStatus.HasValue, x => x.Status == queryFilter.CardStatus)
                                .Where(queryFilter.CardIssueStartTime.HasValue, x => x.IssueDate > queryFilter.CardIssueStartTime)
                                .Where(queryFilter.CardIssueEndCompareTime.HasValue, x => x.IssueDate <= queryFilter.CardIssueEndCompareTime)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.CardNo), x => x.No != null && x.No.Contains(queryFilter.CardNo))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.PhysicNo), x => x.PhysicNo != null && x.PhysicNo.Contains(queryFilter.PhysicNo))
                                join user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Name), x => x.Name != null && x.Name.Contains(queryFilter.Name))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.StudentNo), x => x.StudentNo != null && x.StudentNo.Contains(queryFilter.StudentNo))
                                on card.UserID equals user.Id into users
                                from user in users
                                select new ExportCardListItemDto
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
                                    ExpireDate = card.IssueDate,
                                    IsPrincipal = card.IsPrincipal
                                };
            var pageList = await cardUserQuery.OrderByDescending(x => x.CreateTime).ToPagedListAsync<ExportCardListItemDto>(queryFilter.PageIndex, queryFilter.PageSize);
            var pageCardIds = pageList.Items.Select(x => x.ID).ToArray();
            var cardPropertyList = cardExternalProperties.Where(x => pageCardIds.Contains(x.CardID)).ToList();
            foreach (var item in pageList.Items)
            {
                item.Properties = cardPropertyList.Where(x => x.CardID == item.ID);
            }
            return pageList;
        }

        /// <summary>
        /// 获取导出简要信息
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<CardExportBriefDto> GetExportCardDataBriefInfo(CardExportFilter queryFilter)
        {

            var cardUserQuery = from card in _cardRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                                .Where(queryFilter.IsPrincipal.HasValue, x => x.IsPrincipal == queryFilter.IsPrincipal)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.CardType), x => x.Type == queryFilter.CardType)
                                .Where(queryFilter.CardStatus.HasValue, x => x.Status == queryFilter.CardStatus)
                                .Where(queryFilter.CardIssueStartTime.HasValue, x => x.IssueDate > queryFilter.CardIssueStartTime)
                                .Where(queryFilter.CardIssueEndCompareTime.HasValue, x => x.IssueDate <= queryFilter.CardIssueEndCompareTime)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.CardNo), x => x.No != null && x.No.Contains(queryFilter.CardNo))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.PhysicNo), x => x.PhysicNo != null && x.PhysicNo.Contains(queryFilter.PhysicNo))
                                join user in _userRepository.DetachedEntities.AsQueryable().Where(x => !x.DeleteFlag)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.Name), x => x.Name != null && x.Name.Contains(queryFilter.Name))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.StudentNo), x => x.StudentNo != null && x.StudentNo.Contains(queryFilter.StudentNo))
                                on card.UserID equals user.Id into users
                                from user in users
                                select user.Id;
            var totalCount = await cardUserQuery.CountAsync();
            var pageSize = 5000;
            var totalPage = (totalCount / pageSize) + 1;

            return new CardExportBriefDto { TotalCount = totalCount, PageSize = pageSize, TotalPages = totalPage };
        }

        /// <summary>
        /// 获取用户合并信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<MergeUserDto>> GetMergeInfo(Guid userId)
        {
            var mergeUserInfos = new List<MergeUserDto>();

            var userInfo = await _userRepository.DetachedEntities.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == userId);
            if (userInfo == null)
            {
                throw Oops.Oh("未找到用户信息").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var userCardInfos = await _cardRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.UserID == userInfo.Id).ToListAsync();
            var sort = 1;
            //如果读者有卡
            if (userCardInfos.Any())
            {
                userCardInfos.ForEach(card =>
                {
                    var mergeUserInfo = new MergeUserDto
                    {
                        Sort = sort++,
                        CardId = card.Id,
                        CardNo = card.No,
                        CardType = card.Type,
                        CardTypeName = card.TypeName,
                        CardStatus = card.Status,
                        CardStatusName = ((EnumCardStatus)card.Status).ToString(),
                        ID = userInfo.Id,
                        Name = userInfo.Name,
                        Type = userInfo.Type,
                        TypeName = userInfo.TypeName,
                        Title = userInfo.Title,
                        College = userInfo.College,
                        CollegeName = userInfo.CollegeName,
                        CollegeDepart = userInfo.CollegeDepart,
                        CollegeDepartName = userInfo.CollegeDepartName,
                        Major = userInfo.Major,
                        Grade = userInfo.Grade,
                        Class = userInfo.Class,
                        Birthday = userInfo.Birthday,
                        Gender = userInfo.Gender,
                        Edu = userInfo.Edu,
                        Depart = userInfo.Depart,
                        DepartName = userInfo.DepartName,
                        Phone = userInfo.Phone,
                        IdCard = userInfo.IdCard,
                        Addr = userInfo.Addr,
                        AddrDetail = userInfo.AddrDetail,
                        Status = userInfo.Status,
                        StatusName = ((EnumUserStatus)userInfo.Status).ToString()
                    };
                    mergeUserInfos.Add(mergeUserInfo);
                });
            }
            //只有读者信息，但是没有关联卡信息
            else
            {
                var mergeUserInfo = new MergeUserDto
                {
                    Sort = sort++,
                    CardId = null,
                    CardNo = "",
                    CardType = "",
                    CardTypeName = "",
                    CardStatus = (int)EnumCardStatus.停用,
                    CardStatusName = EnumCardStatus.停用.ToString(),
                    ID = userInfo.Id,
                    Name = userInfo.Name,
                    Type = userInfo.Type,
                    TypeName = userInfo.TypeName,
                    Title = userInfo.Title,
                    College = userInfo.College,
                    CollegeName = userInfo.CollegeName,
                    CollegeDepart = userInfo.CollegeDepart,
                    CollegeDepartName = userInfo.CollegeDepartName,
                    Major = userInfo.Major,
                    Grade = userInfo.Grade,
                    Class = userInfo.Class,
                    Birthday = userInfo.Birthday,
                    Gender = userInfo.Gender,
                    Edu = userInfo.Edu,
                    Depart = userInfo.Depart,
                    DepartName = userInfo.DepartName,
                    Phone = userInfo.Phone,
                    IdCard = userInfo.IdCard,
                    Addr = userInfo.Addr,
                    AddrDetail = userInfo.AddrDetail,
                    Status = userInfo.Status,
                    StatusName = ((EnumUserStatus)userInfo.Status).ToString()
                };
                mergeUserInfos.Add(mergeUserInfo);
            }

            var userIds = new List<Guid>();
            var users = new List<User.EntityFramework.Core.Entitys.User>();
            var sameIdUsers = await _userRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.IdCard == userInfo.IdCard && x.Id != userInfo.Id).ToListAsync();
            users.AddRange(sameIdUsers);
            var sameIdUserIds = sameIdUsers.Select(x => x.Id).ToList();
            if (sameIdUserIds.Any())
            {
                sameIdUserIds.ForEach(x =>
                {
                    if (!userIds.Contains(x))
                    {
                        userIds.Add(x);
                    }
                });
            }
            var samePhoneUsers = await _userRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.Phone == userInfo.Phone && x.Id != userInfo.Id).ToListAsync();
            users.AddRange(samePhoneUsers);
            var samePhoneUserIds = samePhoneUsers.Select(x => x.Id).ToList();
            if (samePhoneUserIds.Any())
            {
                samePhoneUserIds.ForEach(x =>
                {
                    if (!userIds.Contains(x))
                    {
                        userIds.Add(x);
                    }
                });
            }
            //使用读者信息做循环匹配，因为存在只有读者没有关联卡的情况
            var allUserCards = await _cardRepository.DetachedEntities.Where(x => !x.DeleteFlag && userIds.Contains(x.UserID)).ToListAsync();
            users.ForEach(matchUser =>
            {
                var matchUserCards = allUserCards.Where(x => x.UserID == matchUser.Id).ToList();
                if (matchUserCards.Any())
                {
                    matchUserCards.ForEach(card =>
                    {
                        var mergeUserInfo = new MergeUserDto
                        {
                            Sort = sort++,
                            CardId = card.Id,
                            CardNo = card.No,
                            CardType = card.Type,
                            CardTypeName = card.TypeName,
                            CardStatus = card.Status,
                            CardStatusName = ((EnumCardStatus)card.Status).ToString(),
                            ID = userInfo.Id,
                            Name = userInfo.Name,
                            Type = userInfo.Type,
                            TypeName = userInfo.TypeName,
                            Title = userInfo.Title,
                            College = userInfo.College,
                            CollegeName = userInfo.CollegeName,
                            CollegeDepart = userInfo.CollegeDepart,
                            CollegeDepartName = userInfo.CollegeDepartName,
                            Major = userInfo.Major,
                            Grade = userInfo.Grade,
                            Class = userInfo.Class,
                            Birthday = userInfo.Birthday,
                            Gender = userInfo.Gender,
                            Edu = userInfo.Edu,
                            Depart = userInfo.Depart,
                            DepartName = userInfo.DepartName,
                            Phone = userInfo.Phone,
                            IdCard = userInfo.IdCard,
                            Addr = userInfo.Addr,
                            AddrDetail = userInfo.AddrDetail,
                            Status = userInfo.Status,
                            StatusName = ((EnumUserStatus)userInfo.Status).ToString()
                        };
                        mergeUserInfos.Add(mergeUserInfo);

                    });
                }
                else
                {
                    var mergeUserInfo = new MergeUserDto
                    {
                        Sort = sort++,
                        CardId = null,
                        CardNo = "",
                        CardType = "",
                        CardTypeName = "",
                        CardStatus = (int)EnumCardStatus.停用,
                        CardStatusName = EnumCardStatus.停用.ToString(),
                        ID = userInfo.Id,
                        Name = userInfo.Name,
                        Type = userInfo.Type,
                        TypeName = userInfo.TypeName,
                        Title = userInfo.Title,
                        College = userInfo.College,
                        CollegeName = userInfo.CollegeName,
                        CollegeDepart = userInfo.CollegeDepart,
                        CollegeDepartName = userInfo.CollegeDepartName,
                        Major = userInfo.Major,
                        Grade = userInfo.Grade,
                        Class = userInfo.Class,
                        Birthday = userInfo.Birthday,
                        Gender = userInfo.Gender,
                        Edu = userInfo.Edu,
                        Depart = userInfo.Depart,
                        DepartName = userInfo.DepartName,
                        Phone = userInfo.Phone,
                        IdCard = userInfo.IdCard,
                        Addr = userInfo.Addr,
                        AddrDetail = userInfo.AddrDetail,
                        Status = userInfo.Status,
                        StatusName = ((EnumUserStatus)userInfo.Status).ToString()
                    };
                    mergeUserInfos.Add(mergeUserInfo);
                }

            });

            return mergeUserInfos;
        }

        /// <summary>
        /// 获取用户合并信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<List<MergeUserDto>> GetMergeInfo(List<Guid> userIds)
        {
            var mergeUserInfos = new List<MergeUserDto>();
            var allUserCards = await _cardRepository.DetachedEntities.Where(x => !x.DeleteFlag && userIds.Contains(x.UserID)).ToListAsync();
            var allUsers = await _userRepository.DetachedEntities.Where(x => !x.DeleteFlag && userIds.Contains(x.Id)).ToListAsync();
            var sort = 1;

            allUsers.ForEach(userInfo =>
            {
                var matchUserCards = allUserCards.Where(x => x.UserID == userInfo.Id).ToList();
                if (matchUserCards.Any())
                {
                    matchUserCards.ForEach(card =>
                    {
                        var mergeUserInfo = new MergeUserDto
                        {
                            Sort = sort++,
                            CardId = card.Id,
                            CardNo = card.No,
                            CardType = card.Type,
                            CardTypeName = card.TypeName,
                            CardStatus = card.Status,
                            CardStatusName = ((EnumCardStatus)card.Status).ToString(),
                            ID = userInfo.Id,
                            Name = userInfo.Name,
                            Type = userInfo.Type,
                            TypeName = userInfo.TypeName,
                            Title = userInfo.Title,
                            College = userInfo.College,
                            CollegeName = userInfo.CollegeName,
                            CollegeDepart = userInfo.CollegeDepart,
                            CollegeDepartName = userInfo.CollegeDepartName,
                            Major = userInfo.Major,
                            Grade = userInfo.Grade,
                            Class = userInfo.Class,
                            Birthday = userInfo.Birthday,
                            Gender = userInfo.Gender,
                            Edu = userInfo.Edu,
                            Depart = userInfo.Depart,
                            DepartName = userInfo.DepartName,
                            Phone = userInfo.Phone,
                            IdCard = userInfo.IdCard,
                            Addr = userInfo.Addr,
                            AddrDetail = userInfo.AddrDetail,
                            Status = userInfo.Status,
                            StatusName = ((EnumUserStatus)userInfo.Status).ToString()
                        };
                        mergeUserInfos.Add(mergeUserInfo);

                    });
                }
                else
                {
                    var mergeUserInfo = new MergeUserDto
                    {
                        Sort = sort++,
                        CardId = null,
                        CardNo = "",
                        CardType = "",
                        CardTypeName = "",
                        CardStatus = (int)EnumCardStatus.停用,
                        CardStatusName = EnumCardStatus.停用.ToString(),
                        ID = userInfo.Id,
                        Name = userInfo.Name,
                        Type = userInfo.Type,
                        TypeName = userInfo.TypeName,
                        Title = userInfo.Title,
                        College = userInfo.College,
                        CollegeName = userInfo.CollegeName,
                        CollegeDepart = userInfo.CollegeDepart,
                        CollegeDepartName = userInfo.CollegeDepartName,
                        Major = userInfo.Major,
                        Grade = userInfo.Grade,
                        Class = userInfo.Class,
                        Birthday = userInfo.Birthday,
                        Gender = userInfo.Gender,
                        Edu = userInfo.Edu,
                        Depart = userInfo.Depart,
                        DepartName = userInfo.DepartName,
                        Phone = userInfo.Phone,
                        IdCard = userInfo.IdCard,
                        Addr = userInfo.Addr,
                        AddrDetail = userInfo.AddrDetail,
                        Status = userInfo.Status,
                        StatusName = ((EnumUserStatus)userInfo.Status).ToString()
                    };
                    mergeUserInfos.Add(mergeUserInfo);
                }

            });

            return mergeUserInfos;
        }

        /// <summary>
        /// 合并读者卡
        /// </summary>
        /// <param name="mergeInfo"></param>
        /// <returns></returns>
        public async Task<Guid> MergeUserInfo(MergeUserInfo mergeInfo)
        {
            var allUsers = await _userRepository.Where(x => !x.DeleteFlag && mergeInfo.UserIds.Contains(x.Id)).ToListAsync();
            var allUserCards = await _cardRepository.Where(x => !x.DeleteFlag && mergeInfo.UserIds.Contains(x.UserID)).ToListAsync();
            var allUserCardIds = allUserCards.Select(x => x.Id).ToList();
            var principalCard = allUserCards.FirstOrDefault(x => x.Id == mergeInfo.CardId);
            if (principalCard == null)
            {
                throw Oops.Oh("未找到读者卡");
            }
            var selectUser = allUsers.FirstOrDefault(x => x.Id == principalCard.UserID);
            if (selectUser == null)
            {
                throw Oops.Oh("未找到读者");
            }
            selectUser.Name = mergeInfo.Name;
            selectUser.Type = mergeInfo.Type;
            selectUser.TypeName = mergeInfo.TypeName;
            selectUser.Title = mergeInfo.Title;
            selectUser.College = mergeInfo.College;
            selectUser.CollegeName = mergeInfo.CollegeName;
            selectUser.CollegeDepart = mergeInfo.CollegeDepart;
            selectUser.CollegeDepartName = mergeInfo.CollegeDepartName;
            selectUser.Major = mergeInfo.Major;
            selectUser.Grade = mergeInfo.Grade;
            selectUser.Class = mergeInfo.Class;
            selectUser.Birthday = mergeInfo.Birthday;
            selectUser.Gender = mergeInfo.Gender;
            selectUser.Edu = mergeInfo.Edu;
            selectUser.Depart = mergeInfo.Depart;
            selectUser.DepartName = mergeInfo.DepartName;
            selectUser.Phone = mergeInfo.Phone;
            selectUser.IdCard = mergeInfo.IdCard;
            selectUser.Addr = mergeInfo.Addr;
            selectUser.AddrDetail = mergeInfo.AddrDetail;
            selectUser.Status = mergeInfo.Status;
            selectUser.DeleteFlag = false;

            //设置所有卡为次卡，修改关联读者信息
            await _dbContext.BatchUpdate<Card>()
                .Set(dest => dest.UserID, src => principalCard.UserID)
                .Set(dest => dest.IsPrincipal, scr => false)
                .Set(dest => dest.UpdateTime, src => DateTime.Now)
                .Where(x => !x.DeleteFlag && allUserCardIds.Contains(x.Id))
                .ExecuteAsync();
            //设置主卡
            await _dbContext.BatchUpdate<Card>()
                .Set(dest => dest.IsPrincipal, src => true)
                .Set(dest => dest.UpdateTime, src => DateTime.Now)
                .Where(x => x.Id == mergeInfo.CardId)
                .ExecuteAsync();
            //标记删除所有读者
            await _dbContext.BatchUpdate<EntityFramework.Core.Entitys.User>()
                .Set(dest => dest.DeleteFlag, src => true)
                .Set(dest => dest.UpdateTime, src => DateTime.Now)
                .Where(x => mergeInfo.UserIds.Contains(x.Id))
                .ExecuteAsync();
            //修改读者信息
            await _userRepository.UpdateAsync(selectUser);
            return principalCard.UserID;
        }
    }
}
