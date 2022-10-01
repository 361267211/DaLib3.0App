using Scheduler.Service.Utils;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Adapters;
using TaskManager.Adapters.Util;
using TaskManager.Model.Entities;
using TaskManager.Tasks.Extension;
using Vive.Crypto;

namespace TaskManager.Tasks.Job
{
    /// <summary>
    /// 修正密码
    /// </summary>
    public class SyncReaderPasswordJob : SmartJobBase
    {
        public override string JobName => "修正用户密码";

        public override void DoWork()
        {
            //加密提供器
            var encryptProvider = CryptoFactory.CreateAsymmetric(AsymmetricProviderType.SM2);
            var publicKey = ConfigHelper.TaskConfig["PublicKey"] ?? "";

            //获取2.2的数据
            LogManager.Log(JobName, "开始获取2.2原始数据");
            var dataOperator = GetDataOperater<IReaderDataAdapter>();
            var readerDic = dataOperator.GetReaderPasswordInfo().ToConcurrentDictionary(c => c.AsyncReaderId, c => c.OriSecret);

            //获取3.0的数据
            var cardList = TenantDb.Queryable<Card>().Where(c => !c.DeleteFlag && !string.IsNullOrWhiteSpace(c.AsyncReaderId)).ToList();

            if (cardList != null && cardList.Any())
            {
                var updateList = new List<Card>();
                foreach (var item in cardList)
                {
                    var tempItem = DataConverter.Clone(item);
                    var tempSecret = readerDic.ContainsKey(tempItem.AsyncReaderId) ? readerDic[tempItem.AsyncReaderId] : "cqu123456789";
                    tempItem.Secret = encryptProvider.Encrypt(tempSecret, publicKey);
                    updateList.Add(tempItem);
                }
                //更新数据库
                TenantDb.Fastest<Card>().BulkUpdate(updateList, new string[] { "ID" }, new string[] { "Secret" });
            }

            LogManager.Log(JobName, "修正密码完成");
        }
    }
}
