using Newtonsoft.Json.Linq;
using Scheduler.Service;
using SqlSugar;
using System.Collections.Generic;
using TaskManager.Adapters.Util;
using TaskManager.Model.Reader;

namespace TaskManager.Adapters
{
    /// <summary>
    /// 修正读者过期时间
    /// </summary>
    public class ReaderStopTimeAdapter : IReaderInfoAdapter
    {
        private readonly JObject _AdapterParamDic;
        private readonly SqlSugarClient _TenantDb;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adapterParamStr"></param>
        /// <param name="tenantDb"></param>
        public ReaderStopTimeAdapter(string adapterParamStr, SqlSugarClient tenantDb)
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
        /// 获取读者信息
        /// </summary>
        /// <returns></returns>
        public List<ReaderInfo> GetReaderInfos()
        {
            var readerList = new List<ReaderInfo>();
            using (var db = DataHelper.GetInstance(_AdapterParamDic["ReaderConn"].ToString(), (int)DbType.SqlServer))
            {
                var sql = "select user_key,reader_id,stop_time from dbo.reader_info where delete_flag=0 and reader_id <>'' and reader_id is not null";
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
                        ExpireDate = item.stop_time
                    });
                }
            }
            return readerList;
        }
    }
}
