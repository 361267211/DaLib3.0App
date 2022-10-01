using Newtonsoft.Json.Linq;
using Scheduler.Service;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Util;
using TaskManager.Model.Reader;
using TaskManager.Model.Standard;

namespace TaskManager.Adapters
{
    /// <summary>
    /// 修正密码
    /// </summary>
    public class ReaderPasswordAdapter : IReaderDataAdapter
    {
        private readonly JObject _AdapterParamDic;
        private readonly SqlSugarClient _TenantDb;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adapterParamStr"></param>
        /// <param name="tenantDb"></param>
        public ReaderPasswordAdapter(string adapterParamStr, SqlSugarClient tenantDb)
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
        /// 获取所有userkey和原始密码
        /// </summary>
        /// <returns></returns>
        public List<ReaderInfo> GetReaderPasswordInfo()
        {
            var readerList = new List<ReaderInfo>();
            using (var db = DataHelper.GetInstance(_AdapterParamDic["ReaderConn"].ToString(), (int)DbType.SqlServer))
            {
                var sql = "select user_key,reader_id,password from dbo.reader_info where delete_flag=0 and reader_id <>'' and reader_id is not null and password<>'' and password is not null";
                var dt = db.Ado.GetDataTable(sql);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return new List<ReaderInfo>();
                }
                var list = dt.DtStringAllToList<V22CquReader>();
                foreach (var item in list)
                {
                    readerList.Add(new ReaderInfo
                    {
                        UserKey = item.user_key,
                        AsyncReaderId = item.reader_id,
                        OriSecret = DES.ToDESDecrypt(item.password, _AdapterParamDic["SecretKey"].ToString()),
                    });
                }
            }
            return readerList;
        }




        public List<SourceGroupItemDto> GetGroupCodeItems()
        {
            throw new NotImplementedException();
        }

        public List<ReaderInfo> GetReaderInfoList(int pageIndex, int pageSize, ReaderSource readerSource, DateTime updateFlag, out MessageHand message)
        {
            throw new NotImplementedException();
        }

        public List<ReaderSource> GetReaderInfoListTotalNum(DateTime updateFlag, out MessageHand message)
        {
            throw new NotImplementedException();
        }
    }
}
