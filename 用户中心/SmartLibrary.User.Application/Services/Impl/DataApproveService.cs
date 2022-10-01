/*********************************************************
* 名    称：DataApproveService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：数据变更审批
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos.DataApprove;
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
    /// 数据变更审批服务
    /// </summary>
    public class DataApproveService : IDataApproveService
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<PropertyGroup> _propertyGroupRepository;
        private readonly IRepository<PropertyGroupItem> _propertyGroupItemRepository;

        private readonly IRepository<PropertyChangeLog> _propertyChangeLogRepository;
        private readonly IRepository<PropertyChangeItem> _propertyChangeLogItemRepository;
        private readonly IRepository<Property> _propertyRepository;

        private readonly IRepository<UserPropertyChangeLog> _userChangeLogRepository;
        private readonly IRepository<UserPropertyChangeItem> _userChangeLogItemRepository;
        private readonly IRepository<EntityFramework.Core.Entitys.User> _userRepository;
        private readonly IRepository<UserProperty> _userPropertyRepository;

        private readonly IRepository<UserRegister> _userRegisterRepository;
        private readonly IRepository<UserCardClaim> _cardClaimRepository;
        private readonly IRepository<Card> _cardRepository;

        public DataApproveService(IDistributedIDGenerator idGenerator
            , IRepository<PropertyGroup> propertyGroupRepository
            , IRepository<PropertyGroupItem> propertyGroupItemRepository
            , IRepository<PropertyChangeLog> propertyChangeLogRepository
            , IRepository<PropertyChangeItem> propertyChangeLogItemRepository
            , IRepository<Property> propertyRepository
            , IRepository<UserPropertyChangeLog> userChangeLogRepository
            , IRepository<UserPropertyChangeItem> userChangeLogItemRepository
            , IRepository<EntityFramework.Core.Entitys.User> userRepository
            , IRepository<UserProperty> userPropertyRepository
            , IRepository<UserRegister> userRegisterRepository
            , IRepository<UserCardClaim> cardClaimRepository
            , IRepository<Card> cardRepository)
        {
            _idGenerator = idGenerator;
            _propertyGroupRepository = propertyGroupRepository;
            _propertyGroupItemRepository = propertyGroupItemRepository;
            _propertyChangeLogRepository = propertyChangeLogRepository;
            _propertyChangeLogItemRepository = propertyChangeLogItemRepository;
            _propertyRepository = propertyRepository;
            _userChangeLogRepository = userChangeLogRepository;
            _userChangeLogItemRepository = userChangeLogItemRepository;
            _userRepository = userRepository;
            _userPropertyRepository = userPropertyRepository;
            _userRegisterRepository = userRegisterRepository;
            _cardClaimRepository = cardClaimRepository;
            _cardRepository = cardRepository;
        }

        #region Property
        /// <summary>
        /// 创建属性变更日志
        /// </summary>
        /// <param name="changeLogDto"></param>
        /// <returns></returns>
        public async Task<Guid> CreatePropertyChangeLog(PropertyChangeLogDto changeLogDto)
        {
            changeLogDto.ID = _idGenerator.CreateGuid(changeLogDto.ID);
            var changeLogEntity = changeLogDto.Adapt<PropertyChangeLog>();
            var changeLogEntry = await _propertyChangeLogRepository.InsertAsync(changeLogEntity);
            var changeLogItemEntities = changeLogDto.ItemChangeLogs.Adapt<List<PropertyChangeItem>>();
            changeLogItemEntities.ForEach(x =>
            {
                x.LogID = changeLogDto.ID;
                x.Id = new Guid(_idGenerator.Create().ToString());
            });
            await _propertyChangeLogItemRepository.InsertAsync(changeLogItemEntities);
            return changeLogEntry.Entity.Id;
        }

        /// <summary>
        /// 获取属性变更日志记录
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<PropertyChangeListDto>> QueryPropertyChangeLogTableData(PropertyChangeLogTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<PropertyChangeLogTableQuery, EncodePropertyChangeLogTableQuery>(queryFilter);
            var changeLogQuery = from changeLog in _propertyChangeLogRepository.DetachedEntities
                                 .Where(x => !x.DeleteFlag)
                                 .Where(!string.IsNullOrWhiteSpace(queryFilter.Keyword), x => x.ChangeUserName.Contains(queryFilter.Keyword)
                                  || x.Content.Contains(queryFilter.Keyword))
                                 .Where(queryFilter.LogStatus.Any(), x => queryFilter.LogStatus.Contains(x.Status))
                                 .Where(queryFilter.Type.HasValue, x => x.ChangeType == queryFilter.Type)
                                 .Where(queryFilter.Status.HasValue, x => x.Status == queryFilter.Status)
                                 .Where(queryFilter.ChangeStartTime.HasValue, x => x.ChangeTime >= queryFilter.ChangeStartTime)
                                 .Where(queryFilter.ChangeEndTime.HasValue, x => x.ChangeTime < queryFilter.ChangeEndCompareTime)
                                 .Where(!string.IsNullOrWhiteSpace(queryFilter.PropertyName), x => x.Content.Contains(queryFilter.PropertyName))
                                 .Where(queryFilter.PropertyID.HasValue, x => x.PropertyID == queryFilter.PropertyID)
                                 .Where(queryFilter.PropertyType.HasValue, x => x.PropertyType == queryFilter.PropertyType)
                                 join user in _userRepository.DetachedEntities
                                 .Where(!string.IsNullOrWhiteSpace(queryFilter.ChangeUserName), x => x.Name != null && x.Name.Contains(queryFilter.ChangeUserName))
                                 .Where(!string.IsNullOrWhiteSpace(queryFilter.ChangeUserPhone), x => x.Phone != null && x.Phone.Contains(queryFilter.ChangeUserPhone))
                                 on changeLog.ChangeUserID equals user.Id into users
                                 from user in users
                                 orderby changeLog.ChangeTime descending
                                 select new PropertyChangeListDto
                                 {
                                     ID = changeLog.Id,
                                     PropertyType = changeLog.PropertyType,
                                     PropertyID = changeLog.PropertyID,
                                     ChangeType = changeLog.ChangeType,
                                     ChangeTime = changeLog.ChangeTime,
                                     ChangeUserID = changeLog.ChangeUserID,
                                     ChangeUserName = user != null ? user.Name : "",
                                     Status = changeLog.Status,
                                     Content = changeLog.Content
                                 };
            var pageList = await changeLogQuery.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 查询属性变更详情记录
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        public async Task<List<PropertyChangeLogDetailItemDto>> GetPropertyChangeLogDetailItems(Guid logId)
        {
            var changeLog = await _propertyChangeLogRepository.FirstOrDefaultAsync(x => x.Id == logId);
            if (changeLog == null)
            {
                throw Oops.Oh("未找到属性变更日志记录");
            }
            var propertyName = "";
            switch (changeLog.PropertyType)
            {
                case (int)EnumLogPropertyType.属性:
                    var property = await _propertyRepository.FirstOrDefaultAsync(x => x.Id == changeLog.PropertyID);
                    propertyName = property != null ? property.Name : "";
                    break;
                case (int)EnumLogPropertyType.属性组:
                    var group = await _propertyGroupRepository.FirstOrDefaultAsync(x => x.Id == changeLog.PropertyID);
                    propertyName = group != null ? group.Name : "";
                    break;
            }
            var changeDetails = await _propertyChangeLogItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.LogID == changeLog.Id).OrderBy(x => x.PropertyID).ThenBy(x => x.Id).Select(x => new PropertyChangeLogDetailItemDto
            {
                LogId = changeLog.Id,
                PropertyName = propertyName,
                FieldName = x.FieldName,
                FieldCode = x.FieldCode,
                ChangeType = changeLog.ChangeType,
                OldValue = x.OldValue,
                NewValue = x.NewValue,
            }).ToListAsync();
            return changeDetails;
        }

        /// <summary>
        /// 查询属性变更详情记录
        /// </summary>
        /// <param name="logIds"></param>
        /// <returns></returns>
        public async Task<List<PropertyChangeLogDetailItemDto>> GetPropertyGroupChangeLogDetailItems(List<Guid> logIds)
        {
            var changeLogs = await _propertyChangeLogRepository.Where(x => logIds.Contains(x.Id)).ToListAsync();
            if (changeLogs == null || !changeLogs.Any())
            {
                return new List<PropertyChangeLogDetailItemDto>();
            }
            var changeDetails = await _propertyChangeLogItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && logIds.Contains(x.LogID)).OrderBy(x => x.PropertyID).ThenBy(x => x.Id).Select(x => new PropertyChangeLogDetailItemDto
            {
                LogId = x.LogID,
                PropertyName = "",
                FieldName = x.FieldName,
                FieldCode = x.FieldCode,
                ChangeType = 0,
                OldValue = x.OldValue,
                NewValue = x.NewValue,
            }).ToListAsync();
            return changeDetails;
        }

        /// <summary>
        /// 属性变更日志审批
        /// </summary>
        /// <param name="approvePropertyChange"></param>
        /// <returns></returns>
        public async Task<bool> ApprovePropertyChange(ApproveLogChangeInput approvePropertyChange)
        {
            var logEntity = await _propertyChangeLogRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == approvePropertyChange.LogID);
            if (logEntity == null)
            {
                throw Oops.Oh("未找到日志记录");
            }
            if (logEntity.Status != (int)EnumGroupLogStatus.待审批)
            {
                throw Oops.Oh("不能重复审批变更日志");
            }
            var approveResult = false;
            switch (logEntity.PropertyType)
            {
                case (int)EnumLogPropertyType.属性:
                    approveResult = await ApprovePropertyChangeLog(approvePropertyChange, logEntity);
                    break;
                case (int)EnumLogPropertyType.属性组:
                    approveResult = await ApprovePropertyGroupChange(approvePropertyChange, logEntity);
                    break;
                default:
                    break;
            }
            return approveResult;
        }

        /// <summary>
        /// 用户属性组变更日志
        /// </summary>
        /// <param name="approveGroupChange"></param>
        /// <param name="logEntity"></param>
        /// <returns></returns>
        private async Task<bool> ApprovePropertyGroupChange(ApproveLogChangeInput approveGroupChange, PropertyChangeLog logEntity)
        {
            var logItems = _propertyChangeLogItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.LogID == logEntity.Id).ToList();
            var groupItemIds = logItems.Select(x => x.PropertyID).ToArray();
            var groupItems = _propertyGroupItemRepository.Where(x => !x.DeleteFlag && groupItemIds.Contains(x.Id)).ToList();
            logEntity.Remark = approveGroupChange.Remark;
            if (approveGroupChange.Passed)
            {
                logEntity.Status = (int)EnumGroupLogStatus.通过;
                switch (logEntity.ChangeType)
                {
                    case (int)EnumGroupLogType.新增:
                        groupItems.ForEach(x =>
                        {
                            x.ApproveStatus = (int)EnumGroupItemApproveStatus.正常;
                            x.Status = (int)EnumGroupItemDataStatus.正常;
                        });
                        break;
                    case (int)EnumGroupLogType.删除:
                        groupItems.ForEach(x =>
                        {
                            x.ApproveStatus = (int)EnumGroupItemApproveStatus.正常;
                            x.DeleteFlag = true;
                        });
                        break;
                    case (int)EnumGroupLogType.修改:
                        groupItems.ForEach(x =>
                        {
                            x.ApproveStatus = (int)EnumGroupItemApproveStatus.正常;
                            var mapItemChanges = logItems.Where(i => i.PropertyID == x.Id).ToList();
                            var xType = x.GetType();
                            mapItemChanges.ForEach(i =>
                            {
                                var pInfo = xType.GetProperty(i.FieldCode);
                                if (pInfo != null)
                                {
                                    var pValue = DataConverter.StringToObject(i.NewValue, pInfo.PropertyType);
                                    pInfo.SetValue(x, pValue);
                                }
                            });

                        });
                        break;
                    default:
                        break;
                }
            }
            else
            {
                logEntity.Status = (int)EnumGroupLogStatus.驳回;
                groupItems.ForEach(x =>
                {
                    x.ApproveStatus = (int)EnumGroupItemApproveStatus.正常;
                    x.DeleteFlag = x.Status == (int)EnumGroupItemDataStatus.未激活;
                });
            }
            await _propertyChangeLogRepository.UpdateAsync(logEntity);
            await _propertyGroupItemRepository.UpdateAsync(groupItems);
            return true;
        }

        /// <summary>
        /// 用户属性变更日志
        /// </summary>
        /// <param name="approvePropertyChange"></param>
        /// <param name="logEntity"></param>
        /// <returns></returns>
        private async Task<bool> ApprovePropertyChangeLog(ApproveLogChangeInput approvePropertyChange, PropertyChangeLog logEntity)
        {
            var logItems = _propertyChangeLogItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.LogID == logEntity.Id).ToList();
            var propertyIds = logItems.Select(x => x.PropertyID).ToArray();
            var properties = _propertyRepository.Where(x => !x.DeleteFlag && propertyIds.Contains(x.Id)).ToList();
            logEntity.Remark = approvePropertyChange.Remark;
            if (approvePropertyChange.Passed)
            {
                logEntity.Status = (int)EnumPropertyLogStatus.通过;
                switch (logEntity.ChangeType)
                {
                    case (int)EnumGroupLogType.新增:
                        properties.ForEach(x =>
                        {
                            x.ApproveStatus = (int)EnumPropertyApproveStatus.正常;
                            x.Status = (int)EnumPropertyDataStatus.正常;
                        });
                        break;
                    case (int)EnumGroupLogType.删除:
                        properties.ForEach(x =>
                        {
                            x.ApproveStatus = (int)EnumPropertyApproveStatus.正常;
                            x.DeleteFlag = true;
                        });
                        break;
                    case (int)EnumGroupLogType.修改:
                        properties.ForEach(x =>
                        {
                            x.ApproveStatus = (int)EnumPropertyApproveStatus.正常;
                            var mapItemChanges = logItems.Where(i => i.PropertyID == x.Id).ToList();
                            var xType = x.GetType();
                            mapItemChanges.ForEach(i =>
                            {
                                var pInfo = xType.GetProperty(i.FieldCode);
                                if (pInfo != null)
                                {
                                    var pValue = DataConverter.StringToObject(i.NewValue, pInfo.PropertyType);
                                    pInfo.SetValue(x, pValue);
                                }
                            });

                        });
                        break;
                    default:
                        break;
                }
            }
            else
            {
                logEntity.Status = (int)EnumPropertyLogStatus.驳回;
                properties.ForEach(x =>
                {
                    x.ApproveStatus = (int)EnumPropertyApproveStatus.正常;
                    x.DeleteFlag = x.Status == (int)EnumPropertyDataStatus.未激活;
                });
            }
            await _propertyChangeLogRepository.UpdateAsync(logEntity);
            await _propertyRepository.UpdateAsync(properties);
            return true;
        }
        #endregion

        #region User
        /// <summary>
        /// 创建用户信息变更记录
        /// </summary>
        /// <param name="changeLogDto"></param>
        /// <returns></returns>
        public async Task<Guid> CreateUserChangeLog(UserChangeLogDto changeLogDto)
        {
            changeLogDto.ID = _idGenerator.CreateGuid(changeLogDto.ID);
            var changeLogEntity = changeLogDto.Adapt<UserPropertyChangeLog>();
            var changeLogEntry = await _userChangeLogRepository.InsertAsync(changeLogEntity);
            var changeLogItemEntities = changeLogDto.ItemChangeLogs.Adapt<List<UserPropertyChangeItem>>();
            changeLogItemEntities.ForEach(x =>
            {
                x.LogID = changeLogDto.ID;
                x.Id = _idGenerator.CreateGuid();
            });
            await _userChangeLogItemRepository.InsertAsync(changeLogItemEntities);
            return changeLogEntry.Entity.Id;
        }

        /// <summary>
        /// 获取用户变更日志记录
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<UserChangeListDto>> QueryUserChangeLogTableData(UserChangeLogTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<UserChangeLogTableQuery, EncodeUserChangeLogTableQuery>(queryFilter);
            var changeLogQuery = from changeLog in _userChangeLogRepository.DetachedEntities
                                .Where(x => !x.DeleteFlag)
                                .Where(queryFilter.Type.HasValue, x => x.ChangeType == queryFilter.Type)
                                .Where(queryFilter.Status.HasValue, x => x.Status == queryFilter.Status)
                                .Where(queryFilter.ChangeStartTime.HasValue, x => x.ChangeTime >= queryFilter.ChangeStartTime)
                                .Where(queryFilter.ChangeEndTime.HasValue, x => x.ChangeTime < queryFilter.ChangeEndCompareTime)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.UserName), x => x.Content.Contains(queryFilter.UserName))
                                 join user in _userRepository.DetachedEntities
                                 .Where(!string.IsNullOrWhiteSpace(queryFilter.ChangeUserName), x => x.Name != null && x.Name.Contains(queryFilter.ChangeUserName))
                                 .Where(!string.IsNullOrWhiteSpace(queryFilter.ChangeUserPhone), x => x.Phone != null && x.Phone.Contains(queryFilter.ChangeUserPhone))
                                 on changeLog.ChangeUserID equals user.Id into users
                                 from user in users
                                 orderby changeLog.ChangeTime descending
                                 select new UserChangeListDto
                                 {
                                     ID = changeLog.Id,
                                     ChangeType = changeLog.ChangeType,
                                     ChangeTime = changeLog.ChangeTime,
                                     ChangeUserID = changeLog.ChangeUserID,
                                     ChangeUserName = user != null ? user.Name : "",
                                     ChangeUserPhone = user != null ? user.Phone : "",
                                     Status = changeLog.Status,
                                     Remark = changeLog.Remark,
                                     Content = changeLog.Content
                                 };
            var pageList = await changeLogQuery.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 审批用户变更记录
        /// </summary>
        /// <param name="approveUserChange"></param>
        /// <returns></returns>
        public async Task<bool> ApproveUserChange(ApproveLogChangeInput approveUserChange)
        {

            var logEntity = await _userChangeLogRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == approveUserChange.LogID);
            if (logEntity == null)
            {
                throw Oops.Oh("未找到日志记录").StatusCode(Consts.Consts.ExceptionStatus);
            }
            var forUserProperties = await _propertyRepository.Where(x => !x.DeleteFlag && x.ForReader).ToListAsync();
            var logItems = await _userChangeLogItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.LogID == logEntity.Id).ToListAsync();
            var userIds = logItems.Select(x => x.UserID).ToArray();
            var users = await _userRepository.Where(x => !x.DeleteFlag && userIds.Contains(x.Id)).ToListAsync();
            var cards = await _cardRepository.Where(x => !x.DeleteFlag && userIds.Contains(x.UserID)).ToListAsync();
            logEntity.Remark = approveUserChange.Remark;
            //需要新增的属性
            var insertUserProperties = new List<UserProperty>();
            //需要更新的属性
            var updateUserProperties = new List<UserProperty>();
            if (approveUserChange.Passed)
            {
                logEntity.Status = (int)EnumUserLogStatus.通过;
                switch (logEntity.ChangeType)
                {
                    case (int)EnumUserLogType.新增:
                        users.ForEach(x =>
                        {
                            x.Status = (int)EnumPropertyDataStatus.正常;
                            cards.ForEach(c =>
                            {
                                if (c.UserID == x.Id)
                                {
                                    c.Status = (int)EnumCardStatus.正常;
                                }
                            });
                        });
                        break;
                    case (int)EnumUserLogType.删除:
                        users.ForEach(x =>
                        {
                            x.DeleteFlag = true;
                            cards.ForEach(c =>
                            {
                                if (c.UserID == x.Id)
                                {
                                    c.DeleteFlag = true;
                                }
                            });
                        });

                        break;
                    case (int)EnumUserLogType.修改:
                    case (int)EnumUserLogType.批量修改:
                        var codeTable = FileHelper.ReadFileToText("EncodeTable.cert");
                        var _baseEncrypt = new Base64Crypt(codeTable);
                        var userPropertyQuery = from userProperty in _userPropertyRepository.Where(x => !x.DeleteFlag && userIds.Contains(x.UserID))
                                                join user in _userRepository.Where(x => !x.DeleteFlag) on userProperty.UserID equals user.Id into userInfos
                                                from user in userInfos
                                                select userProperty;
                        var allUserProperties = _userPropertyRepository.Where(x => !x.DeleteFlag && userIds.Contains(x.UserID)).ToList();
                        users.ForEach(x =>
                        {
                            var userProperties = allUserProperties.Where(i => i.UserID == x.Id).ToList();
                            var mapFieldItemChanges = logItems.Where(i => i.UserID == x.Id && i.IsField).ToList();
                            var xType = x.GetType();
                            mapFieldItemChanges.ForEach(i =>
                           {
                               var filedCode = (i.PropertyCode ?? "").Replace("User_", "");
                               if (filedCode == nameof(x.IdCard))
                               {
                                   var idCard = _baseEncrypt.Encode((i.NewValue ?? "").Trim());
                                   var isIdCardExist = _userRepository.Any(u => u.Id != x.Id && u.IdCard != null && u.IdCard != "" && idCard != "" && u.IdCard == idCard && !u.DeleteFlag, false, true);
                                   if (isIdCardExist) throw Oops.Oh("读者身份证号已存在").StatusCode(Consts.Consts.ExceptionStatus);
                               }
                               if (filedCode == nameof(x.Phone))
                               {
                                   var phone = _baseEncrypt.Encode((i.NewValue ?? "").Trim());
                                   var isPhoneExist = _userRepository.Any(u => u.Id != x.Id && u.Phone != null && u.Phone != "" && phone != "" && u.Phone == phone && !u.DeleteFlag, false, true);
                                   if (isPhoneExist) throw Oops.Oh("联系电话已存在").StatusCode(Consts.Consts.ExceptionStatus);
                               }
                               if (filedCode == nameof(x.StudentNo))
                               {
                                   var studentNo = _baseEncrypt.Encode((i.NewValue ?? "").Trim());
                                   var isStudentNoExist = _userRepository.Any(u => u.Id != x.Id && u.StudentNo != null && u.StudentNo != "" && studentNo != "" && u.StudentNo == studentNo && !u.DeleteFlag, false, true);
                                   if (isStudentNoExist) throw Oops.Oh("学工号已存在").StatusCode(Consts.Consts.ExceptionStatus);
                               }
                               var pInfo = xType.GetProperty(filedCode);
                               if (pInfo != null)
                               {
                                   var pValue = DataConverter.StringToObject(i.NewValue, pInfo.PropertyType);
                                   pValue = pInfo.PropertyType.FullName.Contains("String") ? _baseEncrypt.Encode(pValue.ToString()) : pValue;
                                   pInfo.SetValue(x, pValue);
                                   //身份证号变更，认证失效
                                   if (filedCode == nameof(x.IdCard))
                                   {
                                       var identityPInfo = xType.GetProperty(nameof(x.IdCardIdentity));
                                       if (identityPInfo != null)
                                       {
                                           identityPInfo.SetValue(x, false);
                                       }
                                   }
                                   //联系电话变更，手机认证失效
                                   if (filedCode == nameof(x.Phone))
                                   {
                                       var identityPInfo = xType.GetProperty(nameof(x.MobileIdentity));
                                       if (identityPInfo != null)
                                       {
                                           identityPInfo.SetValue(x, false);
                                       }
                                   }
                                   //电子邮件变更，手机认证失效
                                   if (filedCode == nameof(x.Email))
                                   {
                                       var identityPInfo = xType.GetProperty(nameof(x.EmailIdentity));
                                       if (identityPInfo != null)
                                       {
                                           identityPInfo.SetValue(x, false);
                                       }
                                   }
                               }
                           });
                            var mapPropertyItemChanges = logItems.Where(i => i.UserID == x.Id && !i.IsField).ToList();
                            mapPropertyItemChanges.ForEach(i =>
                           {
                               var mapProperty = forUserProperties.FirstOrDefault(up => up.Code == i.PropertyCode);
                               if (mapProperty != null)
                               {
                                   if (mapProperty.Required && string.IsNullOrWhiteSpace(i.NewValue))
                                   {
                                       throw Oops.Oh($"{mapProperty.Name}必填").StatusCode(Consts.Consts.ExceptionStatus);
                                   }
                                   if (mapProperty.Unique)
                                   {
                                       var pValue = (i.NewValue ?? "").Trim();
                                       var isPropertyExist = userPropertyQuery.Any(p => p.UserID != x.Id && !p.DeleteFlag && p.PropertyID == mapProperty.Id && p.PropertyValue != null && p.PropertyValue != "" && p.PropertyValue == pValue);
                                       if (isPropertyExist) throw Oops.Oh($"{mapProperty.Name}已存在").StatusCode(Consts.Consts.ExceptionStatus);
                                   }
                                   var updateUserProperty = userProperties.FirstOrDefault(up => up.PropertyID == mapProperty.Id);
                                   //编辑属性
                                   if (updateUserProperty != null)
                                   {
                                       updateUserProperty.PropertyValue = (i.NewValue ?? "").Trim();
                                       MapPropertyValue(updateUserProperty, mapProperty.Type);
                                       updateUserProperty.PropertyValue = _baseEncrypt.Encode(updateUserProperty.PropertyValue);
                                       updateUserProperty.UpdateTime = DateTime.Now;
                                       updateUserProperties.Add(updateUserProperty);
                                   }
                                   //新增属性
                                   else
                                   {
                                       var insertUserProperty = new UserProperty
                                       {
                                           Id = _idGenerator.CreateGuid(),
                                           UserID = x.Id,
                                           PropertyID = mapProperty.Id,
                                           PropertyValue = (i.NewValue ?? "").Trim()
                                       };
                                       MapPropertyValue(insertUserProperty, mapProperty.Type);
                                       insertUserProperty.PropertyValue = _baseEncrypt.Encode(insertUserProperty.PropertyValue);
                                       insertUserProperties.Add(insertUserProperty);
                                   }
                               }

                           });
                        });
                        break;
                    default:
                        break;
                }
            }
            else
            {
                logEntity.Status = (int)EnumUserLogStatus.驳回;
                users.ForEach(x =>
                {
                    x.DeleteFlag = x.Status == (int)EnumUserStatus.未激活;
                });
            }
            await _userChangeLogRepository.UpdateAsync(logEntity);
            await _userRepository.UpdateAsync(users);
            await _cardRepository.UpdateAsync(cards);
            if (insertUserProperties.Any())
            {
                await _userPropertyRepository.InsertAsync(insertUserProperties);
            }
            if (updateUserProperties.Any())
            {
                await _userPropertyRepository.UpdateAsync(updateUserProperties);
            }
            return true;
        }

        /// <summary>
        /// 获取用户信息变更详情
        /// </summary>
        /// <param name="logId"></param>
        /// <returns></returns>
        public async Task<UserChangeLogDetailInfoDto> GetUserChangeLogDetailInfo(Guid logId)
        {
            var logEntity = await _userChangeLogRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == logId);
            if (logEntity == null)
            {
                throw Oops.Oh("未找到日志信息");
            }
            var logItems = await _userChangeLogItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.LogID == logEntity.Id).ToListAsync();
            var userIds = logItems.Select(x => x.UserID).ToList();
            var detailItemUsers = await _userRepository.DetachedEntities.Where(x => userIds.Contains(x.Id)).Select(x => new UserChangeLogDetailUserDto
            {
                ID = x.Id,
                Name = x.Name,
                StudentNo = x.StudentNo,
                College = x.College,
                CollegeName = x.CollegeName,
            }).OrderBy(x => x.ID).ToListAsync();

            var firstUser = detailItemUsers.FirstOrDefault();
            var detailItems = new List<UserChangeLogDetailItemDto>();
            if (firstUser != null)
            {
                detailItems = logItems.Where(x => x.UserID == firstUser.ID).OrderBy(x => x.PropertyCode).Select(x => new UserChangeLogDetailItemDto
                {
                    FieldCode = x.PropertyCode,
                    FieldName = x.PropertyName,
                    ChangeType = logEntity.ChangeType,
                    OldValue = x.OldValue,
                    NewValue = x.NewValue
                }).ToList();
            }
            return new UserChangeLogDetailInfoDto
            {
                Users = detailItemUsers,
                Details = detailItems
            };
        }

        /// <summary>
        /// 获取用户修改日志详情
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserChangeLogDetailItemDto>> GetUserChangeLogDetailItems(Guid logId, Guid userId)
        {
            var logEntity = await _userChangeLogRepository.FirstOrDefaultAsync(x => x.Id == logId);
            if (logEntity == null)
            {
                throw Oops.Oh("未找到日志信息");
            }
            var firstUser = await _userRepository.DetachedEntities.Where(x => x.Id == userId).Select(x => new UserChangeLogDetailUserDto
            {
                ID = x.Id,
                Name = x.Name,
                StudentNo = x.StudentNo,
                College = x.College
            }).OrderBy(x => x.ID).FirstOrDefaultAsync();
            var logItems = await _userChangeLogItemRepository.DetachedEntities.Where(x => !x.DeleteFlag && x.LogID == logEntity.Id && x.UserID == userId).ToListAsync();
            var detailItems = new List<UserChangeLogDetailItemDto>();
            if (firstUser != null)
            {
                detailItems = logItems.Where(x => x.UserID == firstUser.ID).OrderBy(x => x.PropertyCode).Select(x => new UserChangeLogDetailItemDto
                {
                    FieldCode = x.PropertyCode,
                    FieldName = x.PropertyName,
                    ChangeType = logEntity.ChangeType,
                    OldValue = x.OldValue,
                    NewValue = x.NewValue
                }).ToList();
            }
            return detailItems;

        }
        #endregion

        #region Register
        /// <summary>
        /// 用户注册信息查询
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<UserRegisterListItemDto>> QueryUserRegisterTableData(UserRegisterTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<UserRegisterTableQuery, UserRegisterEncodeTableQuery>(queryFilter);
            var registerQuery = from userRegister in _userRegisterRepository.DetachedEntities
                                .Where(x => !x.DeleteFlag)
                                .Where(queryFilter.Status.HasValue, x => x.Status == queryFilter.Status)
                                .Where(queryFilter.RegisterStartTime.HasValue, x => x.RegisterTime >= queryFilter.RegisterStartTime)
                                .Where(queryFilter.RegisterEndTime.HasValue, x => x.RegisterTime < queryFilter.RegisterEndCompareTime)
                                join user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.UserName), x => x.Name.Contains(queryFilter.UserName))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.UserPhone), x => x.Phone != null && x.Phone.Contains(queryFilter.UserPhone)) on userRegister.UserID equals user.Id into users
                                from user in users
                                orderby userRegister.RegisterTime descending
                                select new UserRegisterListItemDto
                                {
                                    ID = userRegister.Id,
                                    RegisterTime = userRegister.RegisterTime,
                                    UserID = userRegister.UserID,
                                    UserName = user.Name,
                                    UserPhone = user.Phone,
                                    Status = user.Status
                                };
            var pageList = await registerQuery.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }

        /// <summary>
        /// 审批用户注册
        /// </summary>
        /// <param name="approveChange"></param>
        /// <returns></returns>
        public async Task<bool> ApproveUserRegister(ApproveLogChangeInput approveChange)
        {
            var register = await _userRegisterRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == approveChange.LogID);
            if (register == null)
            {
                throw Oops.Oh("未找到用户注册记录");
            }
            var user = await _userRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == register.UserID);
            if (user == null)
            {
                throw Oops.Oh("未找到用户信息");
            }
            if (register.Status != (int)EnumUserRegisterStatus.待审批)
            {
                throw Oops.Oh("不能重复审批注册记录");
            }
            if (approveChange.Passed)
            {
                register.Status = (int)EnumUserRegisterStatus.通过;
                register.UpdateTime = DateTime.Now;
                user.Status = (int)EnumUserStatus.正常;
                user.UpdateTime = DateTime.Now;
                await _userRepository.UpdateAsync(user);
            }
            else
            {
                register.Status = (int)EnumUserRegisterStatus.驳回;
                register.UpdateTime = DateTime.Now;
                register.Remark = approveChange.Remark;
            }
            await _userRegisterRepository.UpdateAsync(register);
            return true;
        }

        /// <summary>
        /// 获取用户注册详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserRegisterDetailDto> GetUserRegisterDetailInfo(Guid id)
        {
            var register = await _userRegisterRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (register == null)
            {
                throw Oops.Oh("未找到用户注册记录");
            }
            var userEntity = await _userRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == register.UserID);
            if (userEntity == null)
            {
                throw Oops.Oh("未找到用户信息");
            }
            var result = new UserRegisterDetailDto
            {
                ID = register.Id,
                RegisterTime = register.RegisterTime,
                Name = userEntity.Name,
                NickName = userEntity.NickName,
                Unit = userEntity.Unit,
                Edu = userEntity.Edu,
                Title = userEntity.Title,
                Depart = userEntity.Depart,
                College = userEntity.College,
                Major = userEntity.Major,
                Grade = userEntity.Grade,
                Class = userEntity.Class,
                Type = userEntity.Type,
                Email = userEntity.Email,
                Birthday = userEntity.Birthday,
                Gender = userEntity.Gender,
                Addr = userEntity.Addr,
                AddrDetail = userEntity.AddrDetail,
                Photo = userEntity.Photo
            };
            return result;
        }
        #endregion

        #region CardCliam
        /// <summary>
        /// 查询读者领卡数据
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <returns></returns>
        public async Task<PagedList<CardClaimListItemDto>> QueryCardClaimTableData(CardClaimTableQuery queryFilter)
        {
            queryFilter = AdaptEncoder.EncodedFilter<CardClaimTableQuery, CardClaimEncodeTableQuery>(queryFilter);
            var registerQuery = from cardClaim in _cardClaimRepository.DetachedEntities
                                .Where(x => !x.DeleteFlag)
                                .Where(queryFilter.Status.HasValue, x => x.Status == queryFilter.Status)
                                .Where(queryFilter.ApplyStartTime.HasValue, x => x.ApplyTime >= queryFilter.ApplyStartTime)
                                .Where(queryFilter.ApplyEndTime.HasValue, x => x.ApplyTime < queryFilter.ApplyEndCompareTime)
                                join user in _userRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.UserName), x => x.Name.Contains(queryFilter.UserName))
                                .Where(!string.IsNullOrWhiteSpace(queryFilter.UserPhone), x => x.Phone != null && x.Phone.Contains(queryFilter.UserPhone)) on cardClaim.UserID equals user.Id into users
                                from user in users
                                join card in _cardRepository.DetachedEntities.Where(x => !x.DeleteFlag)
                                on cardClaim.CardID equals card.Id into cards
                                from card in cards
                                orderby cardClaim.ApplyTime descending
                                select new CardClaimListItemDto
                                {
                                    ID = cardClaim.Id,
                                    ApplyTime = cardClaim.ApplyTime,
                                    CardID = cardClaim.CardID,
                                    CardNo = card.No,
                                    UserID = cardClaim.UserID,
                                    UserName = user.Name,
                                    UserPhone = user.Phone,
                                    UserCollege = user.College,
                                    Status = cardClaim.Status
                                };
            var pageList = await registerQuery.ToPagedListAsync(queryFilter.PageIndex, queryFilter.PageSize);
            return pageList;
        }
        /// <summary>
        /// 审批读者领卡
        /// </summary>
        /// <param name="approveChange"></param>
        /// <returns></returns>
        public async Task<bool> ApproveCardClaim(ApproveLogChangeInput approveChange)
        {
            var cardClaim = await _cardClaimRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == approveChange.LogID);
            if (cardClaim == null)
            {
                throw Oops.Oh("未找到读者领卡记录");
            }
            if (cardClaim.Status != (int)EnumCardClaimStatus.待审批)
            {
                throw Oops.Oh("不能重复审批领卡申请");
            }
            if (approveChange.Passed)
            {
                cardClaim.Status = (int)EnumCardClaimStatus.通过;
                cardClaim.UpdateTime = DateTime.Now;
                var user = await _userRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardClaim.UserID);
                if (user == null)
                {
                    throw Oops.Oh("未找到用户信息");
                }
                var card = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardClaim.CardID);
                if (card == null)
                {
                    throw Oops.Oh("未找到卡信息");
                }
                var userExistPrincipalCard = await _cardRepository.AnyAsync(x => !x.DeleteFlag && x.UserID == cardClaim.UserID && x.IsPrincipal);
                //如果领卡人已有主卡，那么领过来的卡为非主卡，否则自动变为主卡
                if (userExistPrincipalCard)
                {
                    card.IsPrincipal = false;
                }
                else
                {
                    card.IsPrincipal = true;
                }
                card.UserID = user.Id;
                card.UpdateTime = DateTime.Now;
                await _cardRepository.UpdateAsync(card);
            }
            else
            {
                cardClaim.Status = (int)EnumCardClaimStatus.驳回;
                cardClaim.UpdateTime = DateTime.Now;
                cardClaim.Remark = approveChange.Remark;
            }
            await _cardClaimRepository.UpdateAsync(cardClaim);
            return true;
        }

        /// <summary>
        /// 取消读者领卡审核
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        public async Task<bool> CancelCardConfirm(Guid claimId)
        {
            var cardClaim = await _cardClaimRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == claimId);
            if (cardClaim == null)
            {
                throw Oops.Oh("未找到领卡申请");
            }
            if (cardClaim.Status != (int)EnumCardClaimStatus.待审批)
            {
                throw Oops.Oh($"审批记录状态为{((EnumCardClaimStatus)cardClaim.Status).ToString()},不能取消审核");
            }
            cardClaim.Status = (int)EnumCardClaimStatus.取消;
            cardClaim.UpdateTime = DateTime.Now;
            await _cardClaimRepository.UpdateAsync(cardClaim);
            return true;
        }

        /// <summary>
        /// 取消读者领卡审核
        /// </summary>
        /// <param name="claimId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCardClaim(Guid claimId)
        {
            var cardClaim = await _cardClaimRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == claimId);
            if (cardClaim == null)
            {
                throw Oops.Oh("未找到领卡申请");
            }
            if (cardClaim.Status != (int)EnumCardClaimStatus.取消 && cardClaim.Status != (int)EnumCardClaimStatus.驳回)
            {
                throw Oops.Oh($"审批记录状态为{((EnumCardClaimStatus)cardClaim.Status).ToString()},不能删除");
            }
            cardClaim.DeleteFlag = true;
            cardClaim.UpdateTime = DateTime.Now;
            await _cardClaimRepository.UpdateAsync(cardClaim);
            return true;
        }

        /// <summary>
        /// 读者卡领取
        /// </summary>
        /// <param name="cardClaim"></param>
        /// <returns></returns>
        public async Task<int> ClaimReaderCard(CardCliamEditDto cardClaim)
        {
            var cardInfo = await _cardRepository.FirstOrDefaultAsync(x => x.Id == cardClaim.CardID);
            if (cardInfo == null)
            {
                throw Oops.Oh("未找到读者卡");
            }
            if (cardClaim.NeedConfirm)
            {
                var confirmInfo = new UserCardClaim
                {
                    Id = _idGenerator.CreateGuid(),
                    UserID = cardClaim.UserID,
                    CardID = cardClaim.CardID,
                    ApplyTime = DateTime.Now,
                    Remark = "",
                };
                await _cardClaimRepository.InsertAsync(confirmInfo);
                return (int)EnumCardClaimWay.提交审核;
            }
            else
            {
                var userExistPrincipalCard = await _cardRepository.AnyAsync(x => !x.DeleteFlag && x.UserID == cardClaim.UserID && x.IsPrincipal);
                //如果领卡人已有主卡，那么领过来的卡为非主卡，否则自动变为主卡
                if (userExistPrincipalCard)
                {
                    cardInfo.IsPrincipal = false;
                }
                else
                {
                    cardInfo.IsPrincipal = true;
                }
                cardInfo.UserID = cardClaim.UserID;
                cardInfo.UpdateTime = DateTime.Now;
                await _cardRepository.UpdateAsync(cardInfo);
                return (int)EnumCardClaimWay.直接领卡;
            }
        }

        /// <summary>
        /// 读者领卡详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CardClaimDetailDto> GetCardClaimDetailInfo(Guid id)
        {
            var cardClaim = await _cardClaimRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == id);
            if (cardClaim == null)
            {
                throw Oops.Oh("未找到读者领卡记录");
            }
            var user = await _userRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardClaim.UserID);
            if (user == null)
            {
                throw Oops.Oh("未找到用户信息");
            }
            var card = await _cardRepository.FirstOrDefaultAsync(x => !x.DeleteFlag && x.Id == cardClaim.CardID);
            if (card == null)
            {
                throw Oops.Oh("未找到读者卡信息");
            }
            var result = new CardClaimDetailDto
            {
                ID = cardClaim.Id,
                No = card.No,
                UserName = user.Name,
                IssueDate = card.IssueDate,
                ExpireDate = card.ExpireDate,
                Status = card.Status,
                BarCode = card.BarCode,
                PhysicNo = card.PhysicNo
            };
            return result;
        }
        #endregion


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
                default:
                    break;
            }
        }
    }
}
