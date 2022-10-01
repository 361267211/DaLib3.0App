using Scheduler.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Adapters;
using TaskManager.Adapters.Util;
using TaskManager.Model.Entities;
using TaskManager.Tasks.Extension;

namespace TaskManager.Tasks.Job
{
    /// <summary>
    ///修正用户过期时间
    /// </summary>
    public class SyncReaderStopTimeJob : SmartJobBase
    {
        public override string JobName => "修正用户过期时间";


        public override void DoWork()
        {
            //获取2.2的数据
            LogManager.Log(JobName, "开始获取2.2原始数据");
            var dataOperator = GetDataOperater<IReaderInfoAdapter>();
            var readerList = dataOperator.GetReaderInfos().ToConcurrentDictionary(c => c.AsyncReaderId, c => c.ExpireDate);

            //获取3.0的数据
            var cardList = TenantDb.Queryable<Card>().Where(c => !c.DeleteFlag && !string.IsNullOrWhiteSpace(c.AsyncReaderId)).ToList();

            //开始修正
            if (cardList != null && cardList.Any())
            {
                var updateList = new List<Card>();
                foreach (var item in cardList)
                {
                    var tempItem = DataConverter.Clone(item);
                    tempItem.ExpireDate = readerList.ContainsKey(item.AsyncReaderId) ? readerList[item.AsyncReaderId] : DateTime.Now;
                    updateList.Add(tempItem);
                }
                //更新数据库
                TenantDb.Fastest<Card>().BulkUpdate(updateList, new string[] { "ID" }, new string[] { "ExpireDate" });
            }

            LogManager.Log(JobName, "修正用户过期时间完成");
        }
    }
}
