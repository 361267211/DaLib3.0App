/*********************************************************
* 名    称：DataOperatorForPgSql.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：数据操作
* 更新历史：
*
* *******************************************************/
using SqlSugar;
using System.Threading.Tasks;

namespace SmartLibrary.DtmClient.Dtm
{
    /// <summary>
    /// 数据操作
    /// </summary>
    public class DataOperatorForPgSql : IDataOperator
    {
        /// <summary>
        /// 初始化数据结构
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitData(SqlSugarClient dbClient)
        {
            var pgSqlCreateBarrierTable = $@"
                create schema if not exists dtm_barrier;
                CREATE SEQUENCE if not EXISTS dtm_barrier.barrier_seq;
                create table if not exists dtm_barrier.barrier(
                  id bigint NOT NULL DEFAULT NEXTVAL ('dtm_barrier.barrier_seq'),
                  trans_type varchar(45) default '',
                  gid varchar(128) default '',
                  branch_id varchar(128) default '',
                  op varchar(45) default '',
                  barrier_id varchar(45) default '',
                  reason varchar(45) default '',
                  create_time timestamp(0) with time zone DEFAULT NULL,
                  update_time timestamp(0) with time zone DEFAULT NULL,
                  PRIMARY KEY(id),
                  CONSTRAINT uniq_barrier unique(gid, branch_id, op, barrier_id)
                );
            ";
            await dbClient.Ado.ExecuteCommandAsync(pgSqlCreateBarrierTable, new { });
            return true;
        }

        /// <summary>
        /// 插入屏障数据
        /// </summary>
        /// <param name="dbClient"></param>
        /// <param name="paramObj"></param>
        /// <returns></returns>
        public async Task<int> InsertBarrier(SqlSugarClient dbClient, dynamic paramObj)
        {
            var pgSqlInsertBarrier = $@"
                Insert Into ""dtm_barrier"".""barrier""(""trans_type"",""gid"",""branch_id"",""op"",""barrier_id"",""reason"",""create_time"",""update_time"")
                Select @TransType,@Gid,@BranchId,@Op,@BarrierId,@Op,@CreateTime,@UpdateTime
                Where (Select Count(""id"") from ""dtm_barrier"".""barrier"" Where ""gid""=@Gid AND ""branch_id""=@BranchId AND ""op""=@Op AND ""barrier_id""=@BarrierId)<1
                ";
            var insertCount = await dbClient.Ado.ExecuteCommandAsync(pgSqlInsertBarrier, paramObj);
            return insertCount;
        }
    }
}
