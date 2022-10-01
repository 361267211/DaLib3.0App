using Newtonsoft.Json.Linq;
using Scheduler.Service;
using Scheduler.Service.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Util;
using TaskManager.Model.Entities;
using TaskManager.Model.Reader;
using TaskManager.Model.Standard;

namespace TaskManager.Adapters
{
    /// <summary>
    /// 重大V2.2读者数据同步适配器
    /// </summary>
    public class CquReaderAdapterV2_2 : IReaderDataAdapter
    {
        private readonly JObject _AdapterParamDic;
        private readonly SqlSugarClient _TenantDb;

        /// <summary>
        /// 分页SQL
        /// </summary>
        private readonly string _PagedSql = "select * from (select CC.*, ROW_NUMBER() Over(Order By CC.id desc) AS rn from({0}) CC) DD where DD.rn >= {1} And DD.rn<={2}";

        /// <summary>
        /// 学院属性组选项
        /// </summary>
        private string _CollegeDataSql = "select 'User_College' GroupCode,UGI.Name Name,UGI.FieldKey Code from UserGroupItem UGI left join UserGroup UG on UGI.UserGroupNo=UG.ID where UGI.DeleteFlag=0 and UG.DeleteFlag= 0 and UG.Field= 'department_name'";

        /// <summary>
        /// 读者属性组选项
        /// </summary>
        private string _ReaderTypeDataSql = "select 'User_Type' GroupCode,UGI.Name Name,UGI.FieldKey Code from UserGroupItem UGI left join UserGroup UG on UGI.UserGroupNo=UG.ID where UGI.DeleteFlag=0 and UG.DeleteFlag= 0 and UG.Field= 'reader_type'";

        /// <summary>
        /// 所有V22读者数据
        /// </summary>
        private string _V22ReaderCountSql = "select count(*) from dbo.reader_info where delete_flag=0 and update_flag>'{0}'";

        /// <summary>
        /// 所有读者数据
        /// </summary>
        private string _V22ReaderDataSql = "select * from dbo.reader_info where delete_flag=0 and update_flag>'{0}'";


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="adapterParamStr"></param>
        /// <param name="tenantDb"></param>
        public CquReaderAdapterV2_2(string adapterParamStr, SqlSugarClient tenantDb)
        {
            _TenantDb = tenantDb;
            if (!string.IsNullOrWhiteSpace(adapterParamStr))
            {
                _AdapterParamDic = JObject.Parse(adapterParamStr);
            }
            else
            {
                _AdapterParamDic = new JObject();
            }
        }

        /// <summary>
        /// 通过名称生成GroupItemCode
        /// </summary>
        /// <param name="groupItems"></param>
        private void RetriveGroupItemCodes(List<SourceGroupItemDto> groupItems)
        {
            var existPy = new HashSet<string>();
            foreach (var item in groupItems)
            {
                if (!string.IsNullOrWhiteSpace(item.Name) && string.IsNullOrWhiteSpace(item.Code))
                {
                    var py = EzPinyin.PinyinHelper.GetInitial(item.Name).ToLower();
                    var index = 0;
                    while (existPy.Contains(py))
                    {
                        index++;
                        py = $"{py}{(index > 0 ? $"_{index}" : "")}";
                    }
                    existPy.Add(py);
                    item.Code = py;
                }
            }
        }

        /// <summary>
        /// 同步用户组编码
        /// </summary>
        /// <returns></returns>
        public List<SourceGroupItemDto> GetGroupCodeItems()
        {
            var sourceListItems = new List<SourceGroupItemDto>();

            using (var db = DataHelper.GetInstance(_AdapterParamDic["ReaderConn"].ToString(), (int)DbType.SqlServer))
            {
                //学院
                var collegeItems = db.Ado.SqlQuery<SourceGroupItemDto>(_CollegeDataSql);
                RetriveGroupItemCodes(collegeItems);
                sourceListItems.AddRange(collegeItems);
                //用户类型
                var readerTypeItems = db.Ado.SqlQuery<SourceGroupItemDto>(_ReaderTypeDataSql);
                RetriveGroupItemCodes(readerTypeItems);
                sourceListItems.AddRange(readerTypeItems);
                //卡类型
                var cardTypeItems = new List<SourceGroupItemDto> { new SourceGroupItemDto { GroupCode = "Card_Type", Name = "一卡通", Code = "cquykt" } };
                sourceListItems.AddRange(cardTypeItems);
            }

            return sourceListItems;
        }

        /// <summary>
        /// 分页获取读者待同步数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="readerSource"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<ReaderInfo> GetReaderInfoList(int pageIndex, int pageSize, ReaderSource readerSource, DateTime updateFlag, out MessageHand message)
        {
            var readerList = new List<ReaderInfo>();
            message = new MessageHand { Code = CODE.SUCCED, Context = typeof(CquReaderAdapterV2_2).FullName.ToString() };
            var groupCodeItems = GetGroupCodeItems();
            try
            {
                //var readerValidateInfos = GetLocalUserValidateInfo();
                //var cardValidateInfos = GetLocalCardValidateInfo();

                switch (readerSource.Source)
                {
                    case "V22Reader":
                        readerList = GetV22ReaderInfoList(pageIndex, pageSize, updateFlag);
                        break;
                }
                //处理年级和专业中文对应值
                if (readerList.Count > 0)
                {
                    readerList.ForEach(reader =>
                    {
                        //院系
                        if (!string.IsNullOrWhiteSpace(reader.CollegeName))
                        {
                            reader.College = groupCodeItems.FirstOrDefault(x => x.GroupCode == "User_College" && x.Name == reader.CollegeName)?.Code ?? "";
                        }
                        //类型
                        if (!string.IsNullOrWhiteSpace(reader.TypeName))
                        {
                            reader.Type = groupCodeItems.FirstOrDefault(x => x.GroupCode == "User_Type" && x.Name == reader.TypeName)?.Code ?? "";
                        }
                        //卡类型
                        if (!string.IsNullOrWhiteSpace(reader.CardTypeName))
                        {
                            reader.CardType = groupCodeItems.FirstOrDefault(x => x.GroupCode == "Card_Type" && x.Code == reader.CardTypeName)?.Code ?? "";
                        }
                    });
                }
                return readerList;
            }
            catch (Exception ex)
            {
                message.ex = ex;
                return readerList;
            }
        }

        /// <summary>
        /// 分页获取2.2本地读者数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private List<ReaderInfo> GetV22ReaderInfoList(int pageIndex, int pageSize, DateTime updateFlag)
        {
            var readerList = new List<ReaderInfo>();
            using (var db = DataHelper.GetInstance(_AdapterParamDic["ReaderConn"].ToString(), (int)DbType.SqlServer))
            {
                var sql = string.Format(_PagedSql, string.Format(_V22ReaderDataSql, updateFlag), pageIndex * pageSize + 1, (pageIndex + 1) * pageSize);
                var dt = db.Ado.GetDataTable(sql);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return new List<ReaderInfo>();
                }
                var list = dt.DtStringAllToList<V22CquReader>();
                foreach (var item in list)
                {
                    if (string.IsNullOrEmpty(item.login_name))
                    {
                        continue;
                    }
                    var reader = HandReaderV22(item);

                    readerList.Add(reader);
                }
            }
            return readerList;
        }

        /// <summary>
        /// 获取待同步数据总量
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<ReaderSource> GetReaderInfoListTotalNum(DateTime updateFlag, out MessageHand message)
        {
            var readerSourceList = new List<ReaderSource>();
            try
            {
                var V22ReaderSource = new ReaderSource { Source = "V22Reader", Count = 0 };
                using (var db = DataHelper.GetInstance(_AdapterParamDic["ReaderConn"].ToString(), (int)DbType.SqlServer))
                {
                    V22ReaderSource.Count = Convert.ToInt32(db.Ado.GetScalar(string.Format(_V22ReaderCountSql, updateFlag)));
                }
                readerSourceList.AddRange(new[] { V22ReaderSource });
                message = new MessageHand { Code = CODE.SUCCED, Context = typeof(CquReaderAdapterV2_2).FullName.ToString() };
            }
            catch (Exception ex)
            {
                message = new MessageHand { Code = CODE.FAIED, Context = typeof(CquReaderAdapterV2_2).FullName.ToString(), ex = ex, };
            }
            return readerSourceList;
        }




        public List<ReaderInfo> GetReaderPasswordInfo()
        {
            throw new NotImplementedException();
        }





        #region 私有方法

        /// <summary>
        /// 获取3.0所有用户
        /// </summary>
        /// <returns></returns>
        private List<ReaderValidateInfo> GetLocalUserValidateInfo()
        {
            var readerValidateInfos = _TenantDb.Queryable<User>().Where(x => x.DeleteFlag == false).Select(x => new ReaderValidateInfo
            {
                UserKey = x.UserKey,
                StudentNo = x.StudentNo,
                Phone = x.Phone,
                IdCard = x.IdCard,
            }).ToList();
            return readerValidateInfos;
        }

        /// <summary>
        /// 获取3.0所有卡
        /// </summary>
        /// <returns></returns>
        private List<CardValidateInfo> GetLocalCardValidateInfo()
        {
            var readerValidateInfos = _TenantDb.Queryable<Card>().Where(x => x.DeleteFlag == false).Select(x => new CardValidateInfo
            {
                No = x.No,
                BarCode = x.BarCode,
                PhysicNo = x.PhysicNo,
            }).ToList();
            return readerValidateInfos;
        }


        /// <summary>
        /// V22读者转化为3.0读者数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private ReaderInfo HandReaderV22(V22CquReader item)
        {
            #region 转化为智图标准读者数据
            var reader = new ReaderInfo
            {
                ID = Guid.NewGuid(),
                SourceFrom = 3,//数据同步
                Name = string.IsNullOrEmpty(item.reader_name) ? "" : item.reader_name,
                NickName = string.IsNullOrEmpty(item.nickname) ? "" : item.nickname,
                StudentNo = string.IsNullOrEmpty(item.reader_number) ? item.login_name ?? "" : item.reader_number.ToLower(),
                Unit = "",
                Edu = string.IsNullOrEmpty(item.edu_background) ? "" : item.edu_background,
                Title = string.IsNullOrEmpty(item.reader_title) ? "" : item.reader_title,
                Depart = "",
                DepartName = "",
                College = "",
                CollegeName = string.IsNullOrEmpty(item.department_name) ? "" : item.department_name,
                CollegeDepart = "",
                CollegeDepartName = "",
                Major = string.IsNullOrEmpty(item.specialty_name) ? "" : item.specialty_name,
                Grade = string.IsNullOrEmpty(item.grade_name) ? "" : item.grade_name,
                Class = string.IsNullOrEmpty(item.class_name) ? "" : item.class_name,
                Type = "",
                TypeName = string.IsNullOrEmpty(item.reader_type) ? "" : item.reader_type,
                Status = MapReaderStatus(item.is_active, item.is_stoped, item.status),//读者状态映射
                IdCard = string.IsNullOrEmpty(item.identity_card) ? "" : item.identity_card,
                Phone = string.IsNullOrEmpty(item.reader_phone) ? "" : item.reader_phone,
                Email = string.IsNullOrEmpty(item.reader_email) ? "" : item.reader_email,
                Birthday = item.reader_birthday,
                Gender = string.IsNullOrEmpty(item.reader_gender) ? "" : item.reader_gender,
                Addr = "",
                AddrDetail = string.IsNullOrEmpty(item.reader_address) ? "" : item.reader_address,
                Photo = string.IsNullOrEmpty(item.image_url) ? "" : item.image_url,
                LeaveTime = item.GraduationLeavingTime,
                AsyncReaderId = string.IsNullOrEmpty(item.reader_id) ? "" : item.reader_id,
                IsStaff = item.backreadertype > 0,
                StaffStatus = item.backreadertype > 0 ? 1 : 0,//默认正式馆员
                FirstLoginTime = item.active_time,
                LastLoginTime = item.last_login_time,
                DeleteFlag = item.delete_flag > 0,
                UserKey = string.IsNullOrEmpty(item.user_key) ? "" : item.user_key,
                CardNo = string.IsNullOrEmpty(item.reader_number) ? "" : item.reader_number.ToLower(),
                CardBarCode = string.IsNullOrEmpty(item.reader_barcode) ? "" : item.reader_barcode,
                CardPhysicNo = "",
                CardIdentityNo = "",
                CardType = "cquykt",
                CardTypeName = "一卡通",
                CardStatus = MapCardStatus(item.is_active, item.is_stoped, item.status),
                IssueDate = item.create_time,
                ExpireDate = item.stop_time,
                Deposit = item.deposit,
                Secret = string.IsNullOrEmpty(item.reader_number) ? "" : item.reader_number.ToLower(),//通过学号编码
                OriSecret = item.virgin_password ?? "",
                ExtFields = new List<ExtFieldInfo>
                {
                    new ExtFieldInfo
                    {
                        FieldName="thesis_status",
                        FieldType=(int)EnumFieldType.数值,
                        FieldValue=item.thesis_status.ToString()
                    },
                    new ExtFieldInfo
                    {
                        FieldName="library_branch",
                        FieldType=(int)EnumFieldType.文本,
                        FieldValue=(item.library_branch??"")
                    }
                }
            };

            reader.CreateTime = item.create_time;
            reader.UpdateTime = item.update_flag;

            #endregion
            return reader;
        }

        /// <summary>
        /// 映射状态
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="isStoped"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private int MapReaderStatus(int isActive, int isStoped, int status)
        {
            //未激活
            if (isActive == 0)
            {
                return 0;
            }
            //停用
            if (isStoped == 1)
            {
                return 2;
            }
            //正常
            if (status == 0)
            {
                return 1;
            }
            //注销
            if (status == 1)
            {
                return 3;
            }
            //违规=>禁用
            if (status == 2)
            {
                return 2;
            }
            //正常
            return 1;
        }

        /// <summary>
        /// 映射卡状态
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="isStoped"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private int MapCardStatus(int isActive, int isStoped, int status)
        {
            //未激活
            if (isActive == 0)
            {
                return 3;
            }
            //停用
            if (isStoped == 1)
            {
                return 3;
            }
            //正常
            if (status == 0)
            {
                return 1;
            }
            //注销
            if (status == 1)
            {
                return 3;
            }
            //违规=>禁用
            if (status == 2)
            {
                return 3;
            }
            //正常
            return 1;
        }

        #endregion
    }
}
