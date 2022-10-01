using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Tasks.Job
{
    /// <summary>
    /// 积分过期结算任务
    /// </summary>
    public class ScoreExpireSettleJob : SmartJobBase
    {
        public override string JobName => "积分过期结算任务";
        private int LogId = 0;
        public override void DoWork()
        {
            var db = TenantDb;
            LogId = WriteLog(LogId, "---------执行中--------", 0);
            //创建本次需要处理的结算数据
            var checkExpireDate = DateTime.Now.Date.AddDays(-1);
            //生成本次读者清算记录
            //检查条件
            //存在UserEventScore过期时间>该读者上次清算过期时间并且<=本次过期时间的读者
            var pgSqlOverdueClear = @$"
            INSERT INTO ""{SchedulerEntity.TenantId}"".""OverdueScoreClear""(""UserKey"",""ExpireTime"",""OverScore"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"",""Status"")
            select DISTINCT ""UES"".""UserKey"",@ExpireDate,0,@CreateTime,@UpdateTime,false,@Tenant,0 FROM ""{SchedulerEntity.TenantId}"".""UserEventScore"" AS ""UES"" WHERE COALESCE((select ""max""(""IOSC"".""ExpireTime"") FROM ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" AS ""IOSC"" WHERE ""IOSC"".""UserKey"" = ""UES"".""UserKey""),@DefaultPreExpireDate)< ""UES"". ""ScoreExpireDate"" AND ""UES"".""ScoreExpireDate"" <= @ExpireDate
            AND ""UES"".""Type""=1 --AND NOT EXISTS (select ""IOSC"".""ID"" From ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" AS ""IOSC"" Where ""IOSC"".""UserKey""=""UES"".""UserKey"" AND ""IOSC"".""ExpireTime""=@ExpireDate)
            ";
            var insertOverdueClearCount = db.Ado.ExecuteCommand(pgSqlOverdueClear,
                new
                {
                    ExpireDate = checkExpireDate,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    Tenant = SchedulerEntity.TenantId,
                    DefaultPreExpireDate = new DateTime(2020, 1, 1)
                });

            //计算是否有需要流转到下个周期的消费积分
            //当前周期消费的积分+上次消费流转到这个周期的-本周期获得的且过期的积分=流转到下个周期的消费积分
            var pgSqlCheckOverScore = @$"
            update ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" AS ""OSC"" SET ""OverScore""=
            (
            COALESCE((select ""sum""(""IUES"".""EventScore"") FROM ""{SchedulerEntity.TenantId}"".""UserEventScore"" AS ""IUES"" WHERE ""IUES"".""UserKey"" = ""OSC"".""UserKey"" AND ""IUES"".""TriggerTime"" > COALESCE((select ""max""(""IOSC"".""ExpireTime"") FROM ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" AS ""IOSC"" WHERE ""IOSC"".""UserKey"" = ""OSC"".""UserKey"" AND ""IOSC"".""Status""=1), @DefaultPreExpireDate) AND ""IUES"".""TriggerTime"" <= @ExpireDate AND ""IUES"".""Type"" = -1),0)
            +
            COALESCE((SELECT ""IOSC"".""OverScore"" FROM ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" AS ""IOSC"" WHERE ""IOSC"".""UserKey"" = ""OSC"".""UserKey"" AND ""IOSC"".""ExpireTime"" < @ExpireDate AND ""IOSC"".""Status""=1 Order By ""IOSC"".""ExpireTime"" desc LIMIT 1), 0)
            -
            COALESCE((SELECT ""sum""(""IUES"".""EventScore"") FROM ""{SchedulerEntity.TenantId}"".""UserEventScore"" AS ""IUES"" WHERE ""IUES"".""UserKey"" = ""OSC"".""UserKey"" AND ""IUES"".""ScoreObtainDate"" > COALESCE((select ""max""(""IOSC"".""ExpireTime"") FROM ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" AS ""IOSC"" WHERE ""IOSC"".""UserKey"" = ""OSC"".""UserKey"" AND ""IOSC"".""Status""=1),@DefaultPreExpireDate) AND ""IUES"".""ScoreObtainDate"" <= @ExpireDate AND ""IUES"".""ScoreExpireDate""<=@ExpireDate AND ""IUES"".""Type"" = 1),0)
            )
            WHERE ""ExpireTime"" = @ExpireDate
            ";
            var updateOverScoreCount = db.Ado.ExecuteCommand(pgSqlCheckOverScore,
                new
                {
                    ExpireDate = checkExpireDate,
                    DefaultPreExpireDate = new DateTime(2020, 1, 1)
                });

            using (var tran = db.UseTran())
            {
                try
                {
                    //创建积分过期扣除事件
                    var pgSqlInsertExpireScoreEvent = $@"
                        INSERT INTO ""{SchedulerEntity.TenantId}"".""UserEventScore""(""ID"",""AppCode"",""EventCode"",""EventName"",""FullEventCode"",""Type"",""EventScore"",""UserKey"",""TriggerTime"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"",""ScoreExpireDate"",""ScoreObtainDate"")
                        SELECT gen_random_uuid(),'scorecenter','ExpireScoreClear','积分过期','scorecenter:ExpireScoreClear',-1,""OverScore"",""UserKey"",@TriggerTime,@CreateTime,@UpdateTime,FALSE,@Tenant,cast(@ScoreExpireDate as timestamp),cast(@ScoreObtainDate as timestamp)
                        FROM ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" WHERE ""ExpireTime"" = @ExpireDate AND ""OverScore"" < 0 AND ""Status""=0
                        ";
                    var insertExpireScoreEventCount = db.Ado.ExecuteCommand(pgSqlInsertExpireScoreEvent,
                        new
                        {
                            ExpireDate = checkExpireDate,
                            TriggerTime = DateTime.Now,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            Tenant = SchedulerEntity.TenantId,
                            ScoreObtainDate = (DateTime?)null,
                            ScoreExpireDate = (DateTime?)null,
                        });

                    //扣除用户积分
                    var pgSqlUpdateUserScore = $@"
                        UPDATE ""{SchedulerEntity.TenantId}"".""UserScore"" AS ""US"" SET ""AvailableScore"" =""AvailableScore"" +""OSC"".""OverScore"",""UpdateTime"" =@UpdateTime
                        FROM ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" AS ""OSC"" WHERE ""OSC"".""ExpireTime"" = @ExpireDate AND ""US"".""UserKey"" = ""OSC"".""UserKey"" AND ""OSC"".""OverScore"" < 0 AND ""Status""=0
                        ";
                    var updateUserScoreCount = db.Ado.ExecuteCommand(pgSqlUpdateUserScore,
                        new
                        {
                            ExpireDate = checkExpireDate,
                            UpdateTime = DateTime.Now
                        });

                    //将流转到下一周期的消费计费为负数的清理记录设置为0
                    var pgSqlUpdateOverScoreClear = @$"
                        update ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" SET ""OverScore""=0
                        WHERE ""ExpireTime"" = @ExpireDate AND ""OverScore""<0 AND ""Status""=0
                        ";
                    var updateOverScoreClearCount = db.Ado.ExecuteCommand(pgSqlUpdateOverScoreClear,
                        new
                        {
                            ExpireDate = checkExpireDate
                        });

                    //将本次生成的清理记录设置为已处理
                    var pgSqlUpdateStatus = @$"
                        update ""{SchedulerEntity.TenantId}"".""OverdueScoreClear"" SET ""Status""=1
                        WHERE ""ExpireTime"" = @ExpireDate AND ""Status""=0 
                        ";
                    var updateOverScoreClearStatus = db.Ado.ExecuteCommand(pgSqlUpdateStatus, new
                    {
                        ExpireDate = checkExpireDate
                    });
                    tran.CommitTran();
                    WriteLog(LogId, $"新增结算记录{insertOverdueClearCount}条,计算结算记录{updateOverScoreCount}条,积分过期记录{insertExpireScoreEventCount}条,积分扣除{updateUserScoreCount}条，无需流转到下一周期{updateOverScoreClearCount}条", 0);
                }
                catch (Exception ex)
                {
                    tran.RollbackTran();
                    WriteLog(LogId, ex.Message, 0);
                }
            }
            WriteLog(LogId, "---------执行完成--------", 1);
        }
    }
}
