using Mapster;
using Newtonsoft.Json;
using Quartz;
using Scheduler.Service.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters;
using TaskManager.Adapters.Util;
using TaskManager.Model.Entities;
using TaskManager.Model.Reader;
using TaskManager.Model.Standard;
using TaskManager.Tasks.Dto;
using Vive.Crypto;

namespace TaskManager.Tasks.Job
{
    /// <summary>
    /// 从2.2版本同步读者数据
    /// </summary>
    public class SyncReaderCardJobV2_2 : SmartJobBase
    {
        public override string JobName => "同步V2.2读者数据";
        private Base64Crypt _Encryptor = null;
        private List<ReaderValidateInfo> _ReaderValidateInfos = new();
        private List<CardValidateInfo> _CardValidateInfos = new();
        private DateTime _UpdateFlag;

        public override void DoWork()
        {
            var tableCode = FileHelper.ReadFileToText("EncodeTable.cert");
            _Encryptor = new Base64Crypt(tableCode);
            LogManager.Log(JobName, $"--------------------------开始同步学院，用户类型，卡类型---------------------------");
            GroupCodeAsync();
            LogManager.Log(JobName, $"--------------------------同步学院，用户类型，卡类型完成---------------------------");

            LogManager.Log(JobName, $"--------------------------开始同步用户数据---------------------------------------");
            ReaderAsync();
            LogManager.Log(JobName, $"--------------------------同步用户数据完成---------------------------------------");
        }

        /// <summary>
        /// 同步属性组，学院，用户类型，卡类型
        /// </summary>
        private void GroupCodeAsync()
        {
            //获取2.2数据
            var sourceGroupItems = GetDataOperater<IReaderDataAdapter>().GetGroupCodeItems();
            //获取3.0数据
            var localGroupItems = GetGroupDataList();
            //学院同步;读者类型，卡类型
            var asyncCodes = new[] { "User_College", "User_Type", "Card_Type" };
            foreach (var code in asyncCodes)
            {
                var localGroup = localGroupItems.FirstOrDefault(x => x.Code == code);
                if (localGroup != null)
                {
                    var groupId = localGroup.ID;
                    var localItems = localGroup.Items.Select(x => x).ToList();
                    var sourceItems = sourceGroupItems.Where(x => x.GroupCode == code).ToList();
                    //需要新增的数据
                    var needInsertCollegeItems = sourceItems.Where(x => !localItems.Any(l => l.Code == x.Code))
                        .Select(x => new PropertyGroupItem
                        {
                            ID = Guid.NewGuid(),
                            GroupID = groupId,
                            Name = x.Name,
                            Code = x.Code,
                            Status = 1,
                            ApproveStatus = 1,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            DeleteFlag = false
                        }).ToList();
                    //执行插入
                    TenantDb.Insertable(needInsertCollegeItems).ExecuteCommand();
                    LogManager.Log(JobName, $"新增数量：{needInsertCollegeItems.Count}，新增类型：{localGroup.Name}+{localGroup.Code}");
                }
            }
        }

        /// <summary>
        /// 获取3.0现有属性组
        /// </summary>
        /// <returns></returns>
        private List<PropertyGroupDto> GetGroupDataList()
        {
            var groupDataList = TenantDb.Queryable<PropertyGroup>().Where(x => !x.DeleteFlag).OrderBy(x => x.CreateTime).ToList();
            var groupDataIds = groupDataList.Select(x => x.ID).ToList();
            var groupDataItemList = TenantDb.Queryable<PropertyGroupItem>().Where(x => !x.DeleteFlag && groupDataIds.Contains(x.GroupID)).ToList();
            var groupDatas = groupDataList.Adapt<List<PropertyGroupDto>>();
            var groupDataItems = groupDataItemList.Adapt<List<PropertyGroupItemDto>>();
            groupDatas.ForEach(x =>
            {
                x.Items = groupDataItems.Where(i => i.GroupID == x.ID).ToList();
            });

            return groupDatas;
        }

        /// <summary>
        /// 同步读者数据处理
        /// </summary>
        private void ReaderAsync()
        {
            _ReaderValidateInfos = TenantDb.Queryable<User>().Where(x => x.DeleteFlag == false).Select(x => new ReaderValidateInfo
            {
                ID = x.ID,
                Name = x.Name,
                UserKey = x.UserKey,
                StudentNo = x.StudentNo,
                Phone = x.Phone,
                IdCard = x.IdCard,
            }).ToList();
            _CardValidateInfos = TenantDb.Queryable<Card>().Where(x => x.DeleteFlag == false).Select(x => new CardValidateInfo
            {
                UserID = x.UserID,
                No = x.No,
                BarCode = x.BarCode,
                PhysicNo = x.PhysicNo,
            }).ToList();

            LogManager.Log(JobName, $"本地读者总数：{ _ReaderValidateInfos.Count}；本地读者卡总数：{_CardValidateInfos.Count}");
            //获取上次同步时间
            var logEntity = schedulerLogEntityService.GetSchedulerLogEntityByJobId(SchedulerEntity.Id);
            DateTime updateFlag = new DateTime(1900, 1, 1, 0, 0, 0);
            if (logEntity != null)
            {
                if (!DateTime.TryParse(logEntity.Context, out updateFlag))
                {
                    updateFlag = new DateTime(1900, 1, 1, 0, 0, 0);
                }
            }
            var dataOperator = GetDataOperater<IReaderDataAdapter>();
            var readerSourceList = dataOperator.GetReaderInfoListTotalNum(updateFlag, out MessageHand message);
            if (message.ex != null)
                throw new JobExecutionException(message.ex);

            readerSourceList.ForEach(source =>
            {
                LogManager.Log(JobName, $"{MapReaderType(source.Source)}类型原始数据总条数：{source.Count}");
                var pageSize = 10000;
                var totalPage = source.Count % pageSize == 0 ? source.Count / pageSize : source.Count / pageSize + 1;
                for (var i = 0; i < totalPage; i++)
                {
                    var readerInfoList = dataOperator.GetReaderInfoList(i, pageSize, source, updateFlag, out message);
                    if (message.ex != null)
                        throw message.ex;

                    //更新或修改用户
                    var syncResult = AddReaders(readerInfoList);
                    LogManager.Log(JobName, $"{MapReaderType(source.Source)}：总共{totalPage}页，当前处理第{i + 1}页,每页{pageSize}数据 新增读者数量{syncResult.Item1}，新增卡数量{syncResult.Item2}，修改读者数量{syncResult.Item3}");
                    //同步其他属性组数据
                    var insertCount = StandardGroupHand(readerInfoList);
                    var tempUpdateFlag = readerInfoList.Max(c => c.UpdateTime).DateTime;
                    if (tempUpdateFlag > _UpdateFlag)
                    {
                        _UpdateFlag = tempUpdateFlag;
                    }
                }
            });

            // 记录最近同步时间
            WriteLog(0, _UpdateFlag.ToString("yyyy-MM-dd HH:mm:ss"), 1);
        }

        /// <summary>
        /// 添加和更新
        /// </summary>
        /// <param name="srcReader"></param>
        /// <param name="localReader"></param>
        public Tuple<int, int, int> AddReaders(List<ReaderInfo> srcReaders)
        {
            //确保thesis_status及library_branch附加属性存在
            var thesis_status_id = EnsureProperty(new ExtFieldInfo { FieldName = "论文提交", FieldValue = "thesis_status", FieldType = (int)EnumFieldType.数值 });
            var library_branch_id = EnsureProperty(new ExtFieldInfo { FieldName = "内部分馆", FieldValue = "library_branch", FieldType = (int)EnumFieldType.文本 });
            var idCardSets = new HashSet<string>();
            var phoneSets = new HashSet<string>();
            var studentNoSets = new HashSet<string>();
            var userKeySets = new HashSet<string>();
            var cardNoSets = new HashSet<string>();
            var barCodeSets = new HashSet<string>();
            var physicNoSets = new HashSet<string>();
            var cardUserIdSets = new HashSet<Guid>();

            _ReaderValidateInfos = _ReaderValidateInfos.Where(x => !x.IsDeleted).ToList();
            _CardValidateInfos = _CardValidateInfos.Where(x => !x.IsDeleted).ToList();
            _ReaderValidateInfos.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.IdCard))
                    idCardSets.Add(x.IdCard.Trim());
                if (!string.IsNullOrWhiteSpace(x.Phone))
                    phoneSets.Add($"{(x.Name ?? "").Trim()}_{x.Phone.Trim()}");
                if (!string.IsNullOrWhiteSpace(x.StudentNo))
                    studentNoSets.Add(x.StudentNo.Trim());
                if (!string.IsNullOrWhiteSpace(x.UserKey))
                    userKeySets.Add(x.UserKey.Trim());
            });
            _CardValidateInfos.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x.No))
                    cardNoSets.Add(x.No.Trim());
                if (!string.IsNullOrWhiteSpace(x.BarCode))
                    barCodeSets.Add(x.BarCode.Trim());
                if (!string.IsNullOrWhiteSpace(x.PhysicNo))
                    physicNoSets.Add(x.PhysicNo.Trim());
                if (x.UserID != Guid.Empty)
                    cardUserIdSets.Add(x.UserID);
            });

            srcReaders = ValidateReader(srcReaders);

            var needInsertUser = 0;
            var needInsertCard = 0;
            var needUpdateUser = 0;
            var needInsertUserProperty = 0;
            var insertUsers = new List<User>();
            var insertCards = new List<Card>();
            var insertUserProperties = new List<UserProperty>();
            var updateUsers = new List<User>();
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var publicKey = ConfigHelper.TaskConfig["PublicKey"] ?? "";

            if (srcReaders.Any())
            {
                srcReaders.ForEach(x =>
                {
                    var addUser = true;
                    var addCard = true;
                    var reader = new User
                    {
                        ID = Guid.NewGuid(),
                        Name = _Encryptor.Encode(x.Name),
                        NickName = _Encryptor.Encode(x.NickName),
                        StudentNo = _Encryptor.Encode(x.StudentNo),
                        Unit = _Encryptor.Encode(x.Unit),
                        Edu = _Encryptor.Encode(x.Edu),
                        Title = _Encryptor.Encode(x.Title),
                        Depart = _Encryptor.Encode(x.Depart),
                        DepartName = _Encryptor.Encode(x.DepartName),
                        College = _Encryptor.Encode(x.College),
                        CollegeDepart = _Encryptor.Encode(x.CollegeDepart),
                        CollegeName = _Encryptor.Encode(x.CollegeName),
                        CollegeDepartName = _Encryptor.Encode(x.CollegeDepartName),
                        Major = _Encryptor.Encode(x.Major),
                        Grade = _Encryptor.Encode(x.Grade),
                        Class = _Encryptor.Encode(x.Class),
                        Type = _Encryptor.Encode(x.Type),
                        TypeName = _Encryptor.Encode(x.TypeName),
                        Status = x.Status,
                        IdCard = _Encryptor.Encode(x.IdCard),
                        Phone = _Encryptor.Encode(x.Phone),
                        Email = _Encryptor.Encode(x.Email),
                        Birthday = x.Birthday,
                        Gender = _Encryptor.Encode(x.Gender),
                        Addr = _Encryptor.Encode(x.Addr),
                        AddrDetail = _Encryptor.Encode(x.AddrDetail),
                        Photo = _Encryptor.Encode(x.Photo),
                        LeaveTime = x.LeaveTime,
                        IsStaff = x.IsStaff,
                        StaffStatus = x.StaffStatus,
                        SourceFrom = x.SourceFrom,
                        FirstLoginTime = x.FirstLoginTime,
                        LastLoginTime = x.LastLoginTime,
                        CreateTime = x.CreateTime,
                        UpdateTime = x.UpdateTime,
                        UserKey = x.UserKey,
                    };
                    var card = new Card
                    {
                        ID = Guid.NewGuid(),
                        UserID = Guid.Empty,
                        No = x.CardNo,
                        BarCode = x.CardBarCode,
                        PhysicNo = x.CardPhysicNo,
                        IdentityNo = x.CardIdentityNo,
                        Type = x.CardType,
                        TypeName = x.CardTypeName,
                        Status = x.CardStatus,
                        IsPrincipal = false,
                        IssueDate = x.IssueDate,
                        ExpireDate = x.ExpireDate,
                        Deposit = x.Deposit,
                        Secret = encryptProvider.Encrypt(x.Secret, publicKey),
                        Usage = 0,
                        CreateTime = x.CreateTime,
                        UpdateTime = x.UpdateTime,
                        AsyncReaderId = x.AsyncReaderId,
                    };
                    if (idCardSets.Contains((reader.IdCard ?? "").Trim())
                    || phoneSets.Contains($"{(reader.Name ?? "").Trim()}_{(reader.Phone ?? "").Trim()}")
                    || studentNoSets.Contains((reader.StudentNo ?? "").Trim())
                    || userKeySets.Contains((reader.UserKey ?? "").Trim()))
                    {
                        addUser = false;
                        var existUser = _ReaderValidateInfos.FirstOrDefault(c => c.UserKey == reader.UserKey);
                        if (existUser != null)
                        {
                            reader.ID = existUser.ID;
                            updateUsers.Add(reader);
                        }
                    }

                    if (addUser)
                    {
                        insertUsers.Add(reader);

                        if (x.ExtFields.Any())
                        {
                            foreach (var ef in x.ExtFields)
                            {
                                var readerProperty = new UserProperty
                                {
                                    ID = Guid.NewGuid(),
                                    UserID = reader.ID,
                                    PropertyID = Guid.Empty,
                                    PropertyValue = "",
                                    CreateTime = DateTime.Now,
                                    UpdateTime = DateTime.Now,
                                    TenantId = SchedulerEntity.TenantId
                                };
                                if (ef.FieldName == "thesis_status")
                                {
                                    readerProperty.PropertyID = thesis_status_id;
                                    readerProperty.PropertyValue = _Encryptor.Encode(ef.FieldValue ?? "");
                                    readerProperty.NumValue = DataConverter.ToNullableDecimal(ef.FieldValue);
                                }
                                if (ef.FieldName == "library_branch")
                                {
                                    readerProperty.PropertyID = library_branch_id;
                                    readerProperty.PropertyValue = _Encryptor.Encode(ef.FieldValue ?? "");
                                }
                                if (readerProperty.PropertyID != Guid.Empty)
                                {
                                    insertUserProperties.Add(readerProperty);
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(reader.IdCard))
                            idCardSets.Add(reader.IdCard.Trim());
                        if (!string.IsNullOrWhiteSpace(reader.Phone))
                            phoneSets.Add($"{(reader.Name ?? "").Trim()}_{reader.Phone.Trim()}");
                        if (!string.IsNullOrWhiteSpace(reader.StudentNo))
                            studentNoSets.Add(reader.StudentNo.Trim());
                        if (!string.IsNullOrWhiteSpace(reader.UserKey))
                            userKeySets.Add(reader.UserKey.Trim());

                        _ReaderValidateInfos.Add(new ReaderValidateInfo
                        {
                            ID = reader.ID,
                            Name = reader.Name.Trim(),
                            UserKey = reader.UserKey.Trim(),
                            StudentNo = reader.StudentNo.Trim(),
                            Phone = reader.Phone.Trim(),
                            IdCard = reader.IdCard.Trim(),
                            IsNew = true,
                        });
                    }

                    if (cardNoSets.Contains((x.CardNo ?? "").Trim())
                    || barCodeSets.Contains((x.CardBarCode ?? "").Trim())
                    || physicNoSets.Contains((x.CardPhysicNo ?? "").Trim()))
                    {
                        addCard = false;
                    }
                    if (addCard)
                    {
                        if (addUser)
                            card.UserID = reader.ID;
                        else
                        {
                            var mapUser = _ReaderValidateInfos.FirstOrDefault(r => (r.StudentNo ?? "").Trim() == (reader.StudentNo ?? "").Trim()
                            || (r.IdCard ?? "").Trim() == (reader.IdCard ?? "").Trim()
                            || ((r.Name ?? "").Trim() == (reader.Name ?? "").Trim() && (r.Phone ?? "").Trim() == (reader.Phone ?? "").Trim()));
                            card.UserID = mapUser != null ? mapUser.ID : card.UserID;
                        }

                        if (card.UserID != Guid.Empty)
                        {
                            card.IsPrincipal = !cardUserIdSets.Contains(card.UserID);
                            insertCards.Add(card);

                            if (!string.IsNullOrWhiteSpace(card.No))
                                cardNoSets.Add(card.No.Trim());
                            if (!string.IsNullOrWhiteSpace(card.BarCode))
                                barCodeSets.Add(card.BarCode.Trim());
                            if (!string.IsNullOrWhiteSpace(card.PhysicNo))
                                physicNoSets.Add(card.PhysicNo.Trim());

                            cardUserIdSets.Add(card.UserID);

                            _CardValidateInfos.Add(new CardValidateInfo
                            {
                                UserID = card.UserID,
                                No = card.No.Trim(),
                                BarCode = card.BarCode.Trim(),
                                PhysicNo = card.PhysicNo.Trim(),
                                IsNew = true
                            });
                        }
                    }
                });
                using (var tran = TenantDb.UseTran())
                {
                    try
                    {
                        if (insertUsers.Any())
                        {
                            TenantDb.Utilities.PageEach(insertUsers, 10000, pageList =>
                            {
                                needInsertUser += TenantDb.Insertable(pageList).ExecuteCommand();
                            });
                        }

                        if (insertUserProperties.Any())
                        {
                            TenantDb.Utilities.PageEach(insertUserProperties, 10000, pageList =>
                            {
                                needInsertUserProperty += TenantDb.Insertable(pageList).ExecuteCommand();
                            });
                        }

                        if (insertCards.Any())
                        {
                            TenantDb.Utilities.PageEach(insertCards, 10000, pageList =>
                            {
                                needInsertCard += TenantDb.Insertable(pageList).ExecuteCommand();
                            });
                        }

                        if (updateUsers.Any())
                        {
                            needUpdateUser += TenantDb.Updateable(updateUsers).IgnoreColumns(c => new { c.CreateTime })
                                                      .WhereColumns(c => c.ID).ExecuteCommand();
                        }

                        tran.CommitTran();
                        foreach (var item in _ReaderValidateInfos)
                        {
                            item.IsNew = false;
                        }
                        foreach (var item in _CardValidateInfos)
                        {
                            item.IsNew = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        foreach (var item in _CardValidateInfos)
                        {
                            item.IsDeleted = item.IsNew;
                        }
                        LogManager.Log(JobName, $"同步用户数据插入失败:{ex.Message}");
                        tran.RollbackTran();
                    }
                }
            }

            return new Tuple<int, int, int>(needInsertUser, needInsertCard, needUpdateUser);
        }

        /// <summary>
        /// 将用户转为可插入
        /// </summary>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public List<ReaderInfo> ValidateReader(List<ReaderInfo> sourceList)
        {
            foreach (var item in sourceList)
            {
                item.Birthday = (item.Birthday == DateTime.MinValue || item.Birthday == null) ? DateTime.Now.AddYears(-20) : item.Birthday;
                if (item.Birthday.HasValue && (item.Birthday.Value.Year < 1900 || item.Birthday.Value.Year > DateTime.Now.Year))
                {
                    item.Birthday = DateTime.Now.AddYears(-20);
                }
                item.LeaveTime = item.LeaveTime == DateTime.MinValue ? DateTime.Now.AddYears(4) : item.LeaveTime;
                item.LastLoginTime = (item.LastLoginTime == DateTime.MinValue || item.LastLoginTime == null) ? DateTime.Now : item.LastLoginTime;
                if (item.LastLoginTime.HasValue && (item.LastLoginTime.Value.Year < 1900 || item.LastLoginTime.Value.Year > DateTime.Now.Year))
                {
                    item.LastLoginTime = DateTime.Now;
                }
                item.CreateTime = (item.CreateTime.LocalDateTime == DateTime.MinValue) ? new DateTimeOffset(DateTime.Now) : item.CreateTime;
                if (item.CreateTime.Year < 1900 || item.CreateTime.Year > DateTime.Now.Year)
                {
                    item.CreateTime = DateTime.Now;
                }
                item.FirstLoginTime = (item.FirstLoginTime == DateTime.MinValue || item.FirstLoginTime == null) ? DateTime.Now : item.FirstLoginTime;
                if (item.FirstLoginTime.HasValue && (item.FirstLoginTime.Value.Year < 1900 || item.FirstLoginTime.Value.Year > DateTime.Now.Year))
                {
                    item.FirstLoginTime = DateTime.Now;
                }
                item.Name = string.IsNullOrWhiteSpace(item.Name) ? "" : item.Name;
                item.NickName = string.IsNullOrWhiteSpace(item.NickName) ? "" : item.NickName;
                item.StudentNo = string.IsNullOrWhiteSpace(item.StudentNo) ? "" : item.StudentNo;
                item.Unit = string.IsNullOrWhiteSpace(item.Unit) ? "" : item.Unit;
                item.Edu = string.IsNullOrWhiteSpace(item.Edu) ? "" : item.Edu;
                item.Title = string.IsNullOrWhiteSpace(item.Title) ? "" : item.Title;
                item.Depart = string.IsNullOrWhiteSpace(item.Depart) ? "" : item.Depart;
                item.DepartName = string.IsNullOrWhiteSpace(item.DepartName) ? "" : item.DepartName;
                item.College = string.IsNullOrWhiteSpace(item.College) ? "" : item.College;
                item.CollegeName = string.IsNullOrWhiteSpace(item.CollegeName) ? "" : item.CollegeName;
                item.CollegeDepart = string.IsNullOrWhiteSpace(item.CollegeDepart) ? "" : item.CollegeDepart;
                item.CollegeDepartName = string.IsNullOrWhiteSpace(item.CollegeDepartName) ? "" : item.CollegeDepartName;
                item.Major = string.IsNullOrWhiteSpace(item.Major) ? "" : item.Major;
                item.Grade = string.IsNullOrWhiteSpace(item.Grade) ? "" : item.Grade;
                item.Class = string.IsNullOrWhiteSpace(item.Class) ? "" : item.Class;
                item.Type = string.IsNullOrWhiteSpace(item.Type) ? "" : item.Type;
                item.TypeName = string.IsNullOrWhiteSpace(item.TypeName) ? "" : item.TypeName;
                item.IdCard = string.IsNullOrWhiteSpace(item.IdCard) ? "" : item.IdCard;
                item.Photo = string.IsNullOrWhiteSpace(item.Photo) ? "" : item.Photo;
                item.Phone = string.IsNullOrWhiteSpace(item.Phone) ? "" : item.Phone;
                item.Email = string.IsNullOrWhiteSpace(item.Email) ? "" : item.Email;
                item.Gender = string.IsNullOrWhiteSpace(item.Gender) ? "" : item.Gender;
                item.AddrDetail = string.IsNullOrWhiteSpace(item.AddrDetail) ? "" : item.AddrDetail;
                item.Secret = string.IsNullOrWhiteSpace(item.Secret) ? "" : item.Secret;
                item.OriSecret = string.IsNullOrWhiteSpace(item.OriSecret) ? "" : item.OriSecret;
                item.CardNo = string.IsNullOrWhiteSpace(item.CardNo) ? "" : item.CardNo;
                item.CardBarCode = string.IsNullOrWhiteSpace(item.CardBarCode) ? "" : item.CardBarCode;
                item.CardPhysicNo = string.IsNullOrWhiteSpace(item.CardPhysicNo) ? "" : item.CardPhysicNo;
                item.CardIdentityNo = string.IsNullOrWhiteSpace(item.CardIdentityNo) ? "" : item.CardIdentityNo;
                item.CardType = string.IsNullOrWhiteSpace(item.CardType) ? "" : item.CardType;
                item.CardTypeName = string.IsNullOrWhiteSpace(item.CardType) ? "" : item.CardType;
                item.IssueDate = (item.IssueDate == DateTime.MinValue) ? DateTime.Now : item.IssueDate;
                if (item.IssueDate.Year < 1900 || item.IssueDate.Year > DateTime.Now.Year)
                {
                    item.IssueDate = DateTime.Now;
                }
                item.ExpireDate = (item.ExpireDate == DateTime.MinValue) ? DateTime.Now : item.ExpireDate;
                if (item.ExpireDate.Year < 1900)
                {
                    item.ExpireDate = DateTime.Now;
                }
                item.AsyncReaderId = string.IsNullOrWhiteSpace(item.AsyncReaderId) ? "" : item.AsyncReaderId;
            }
            return sourceList;
        }

        /// <summary>
        /// 从读者数据中处理标准化属性
        /// </summary>
        /// <param name="readers"></param>
        private int StandardGroupHand(List<ReaderInfo> readers)
        {
            var insertCount = 0;
            var localGroupItems = GetGroupDataList();
            var standardParmDic = new[] { "Class", "Grade", "Title", "Edu", "Major" };
            var asyncCodes = new[] { "User_Class", "User_Grade", "User_Title", "User_Edu", "User_Major" };
            var enums = standardParmDic.GetEnumerator();
            var standardList = new Dictionary<string, List<string>>();
            while (enums.MoveNext())
            {
                string columnName = enums.Current.ToString();
                var values = readers.GroupBy(c => typeof(ReaderInfo).GetProperty(columnName).GetValue(c))
                                                    .Select(c => c.Key)
                                                    .Where(c => c != null && !string.IsNullOrEmpty(c.ToString()))
                                                    .Select(c => c.ToString()).ToList();
                var columnKey = $"User_{columnName}";
                if (standardList.ContainsKey(columnKey))
                {
                    var valueList = standardList[columnKey];
                    values.ForEach(c =>
                    {
                        if (!valueList.Contains(c))
                        {
                            valueList.Add(c);
                        }
                    });
                    standardList[columnKey] = valueList;
                }
                else
                {
                    standardList.Add(columnKey, values);
                }
            }

            foreach (var code in asyncCodes)
            {
                var localGroup = localGroupItems.FirstOrDefault(x => x.Code == code);
                if (localGroup != null)
                {
                    var groupId = localGroup.ID;
                    var localItems = localGroup.Items.Select(x => x).ToList();
                    var sourceItems = standardList.Where(x => x.Key == code).SelectMany(x => x.Value).ToList();
                    var needInsertCollegeItems = sourceItems.Where(x => !localItems.Any(l => l.Name == x)).Select(x => new PropertyGroupItem
                    {
                        ID = Guid.NewGuid(),
                        GroupID = groupId,
                        Name = x,
                        Code = "",
                        Status = 1,
                        ApproveStatus = 1,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    }).ToList();
                    insertCount += TenantDb.Insertable(needInsertCollegeItems).ExecuteCommand();
                    LogManager.Log(JobName, $"新增数量：{insertCount},新增类型：{localGroup.Name}");
                }
            }

            return insertCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string MapReaderType(string code)
        {
            switch (code)
            {
                case "V22Reader":
                    return "V22版本读者数据";
            }
            return "";
        }

        /// <summary>
        /// 确保附件属性已被创建
        /// </summary>
        /// <param name="extField"></param>
        /// <returns></returns>
        private Guid EnsureProperty(ExtFieldInfo extField)
        {
            if (!TenantDb.Queryable<Property>().Any(x => x.Code == extField.FieldValue))
            {
                TenantDb.Insertable(new Property
                {
                    ID = Guid.NewGuid(),
                    Name = extField.FieldName,
                    Code = extField.FieldValue,
                    Type = extField.FieldType,
                    ForReader = true,
                    PropertyGroupID = Guid.Empty,
                    Status = 1,
                    ApproveStatus = 1,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                });
            }
            var property = TenantDb.Queryable<Property>().First(x => x.Code == extField.FieldValue);

            return property != null ? property.ID : Guid.Empty;
        }
    }
}
