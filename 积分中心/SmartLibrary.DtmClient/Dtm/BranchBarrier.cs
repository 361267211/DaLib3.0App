/*********************************************************
* 名    称：BranchBarrier.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：分支事务执行屏障信息，主要用于确保分支调用的先后顺序
* 更新历史：
*
* *******************************************************/
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// 分支事务执行屏障信息
    /// </summary>
    public class BranchBarrier
    {
        /// <summary>
        /// 全局事务id
        /// </summary>
        private string gid;
        /// <summary>
        /// 事务类型
        /// </summary>
        private string transType;
        /// <summary>
        /// 分支id
        /// </summary>
        private string branchId;
        /// <summary>
        /// 屏障id
        /// </summary>
        private int barrierId;
        /// <summary>
        /// 操作
        /// </summary>
        private string op;
        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool Inited;

        private readonly IDataOperator _dataOperator;

        public BranchBarrier(IDataOperator dataOperator)
        {
            _dataOperator = dataOperator;
        }

        /// <summary>
        /// 配置初始化
        /// </summary>
        /// <param name="paramDict"></param>
        /// <returns></returns>
        public bool InitConfig(IDictionary<string, object> paramDict)
        {
            if (paramDict == null)
            {
                throw new Exception("BranchBarrier参数为空");
            }
            //事务类型
            if (paramDict.ContainsKey("trans_type"))
            {
                this.transType = paramDict["trans_type"].ToString() ?? "";
            }
            //全局事务id
            if (paramDict.ContainsKey("gid"))
            {
                this.gid = paramDict["gid"].ToString() ?? "";
            }
            //分支id
            if (paramDict.ContainsKey("branch_id"))
            {
                this.branchId = paramDict["branch_id"].ToString() ?? "";
            }
            //操作行为
            if (paramDict.ContainsKey("op"))
            {
                this.op = paramDict["op"].ToString() ?? "";
            }
            this.Inited = true;
            return true;
        }

        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="dbClient"></param>
        /// <param name="func"></param>
        public async Task<bool> Call(SqlSugarClient dbClient, Func<BranchBarrier, Task> func)
        {
            if (!this.Inited)
            {
                throw new Exception("未初始化配置");
            }
            this.barrierId++;
            using (var tran = dbClient.UseTran())
            {
                try
                {
                    var result = await InsertBarrier(dbClient);
                    if (result)
                    {
                        await func(this);
                    }
                    dbClient.CommitTran();
                }
                catch (Exception ex)
                {
                    dbClient.RollbackTran();
                    throw new Exception(ex.Message);
                }
            }
            return true;
        }
        /// <summary>
        /// 插入事务屏障信息
        /// </summary>
        /// <param name="dbClient"></param>
        /// <returns></returns>
        private async Task<bool> InsertBarrier(SqlSugarClient dbClient)
        {
            var paramObj = new
            {
                TransType = this.transType,
                Gid = this.gid,
                BranchId = this.branchId,
                Op = this.op,
                BarrierId = this.barrierId.ToString().PadLeft(2, '0'),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            var insertCount = await _dataOperator.InsertBarrier(dbClient, paramObj);
            if (insertCount <= 0)
            {
                return false;
            }
            if (this.op.Equals("cancel"))
            {
                var tryParamObj = new
                {
                    TransType = this.transType,
                    Gid = this.gid,
                    BranchId = this.branchId,
                    Op = "try",
                    BarrierId = this.barrierId.ToString().PadLeft(2, '0'),
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                var innerExecCount = await _dataOperator.InsertBarrier(dbClient, tryParamObj);
                if (innerExecCount > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
