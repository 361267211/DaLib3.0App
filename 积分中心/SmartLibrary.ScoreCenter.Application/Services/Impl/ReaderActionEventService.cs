/*********************************************************
* 名    称：ReaderActionEventService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：读者行为事件监听
* 更新历史：
*
* *******************************************************/
using DotNetCore.CAP;
using Furion;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using SmartLibrary.ScoreCenter.Application.Dtos.Cap;
using SmartLibrary.ScoreCenter.Application.Services.Extensions;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Common.Utils;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using System;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 读者行为事件监听
    /// </summary>
    public class ReaderActionEventService : IReaderActionEventService, IScoped
    {
        /// <summary>
        /// 处理读者行为事件监听
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="capHeader"></param>
        /// <returns></returns>
        public async Task<bool> ProcessReaderActionEvent(SubscribeEvent.ActionEventMsg msg, CapHeader capHeader)
        {
            var _idGenerator = App.GetRequiredService<IDistributedIDGenerator>();
            var db = SqlSugarHelper.GetTenantDb(msg.TenantName);
            //var operateToken = string.IsNullOrWhiteSpace(capHeader.Token) ? _idGenerator.CreateGuid().ToString("N") : exchangeInput.Token;
            var operateToken = capHeader["cap-msg-id"] ?? _idGenerator.CreateGuid().ToString("N");
            var scoreOperateKey = $"Score_Event_{operateToken}";
            var medalOperateKey = $"Medal_Event_{operateToken}";

            //查询加分任务
            var nowTime = DateTime.Now.Date;
            var scoreTask = await db.Queryable<ScoreObtainTask>().FirstAsync(x => !x.DeleteFlag && x.AppCode == msg.AppCode && x.EventCode == msg.EventCode && nowTime <= x.EndDate && x.IsActive);
            if (scoreTask != null && scoreTask.ObtainScore > 0)
            {
                var obtainScore = scoreTask.ObtainScore;
                //积分处理逻辑
                var pgSqlInsertUserScore = @$"
                INSERT INTO ""{msg.TenantName}"".""UserScore""(""ID"",""UserKey"",""AvailableScore"",""FreezeScore"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"")
                SELECT @ID, @UserKey, @AvailableScore, @FreezeScore, @CreateTime, @UpdateTime, @DeleteFlag, @Tenant
                WHERE(SELECT COUNT(""ID"") FROM ""{msg.TenantName}"".""UserScore"" WHERE ""DeleteFlag"" = false AND ""UserKey"" = @UserKey) < 1
                ";
                await db.Ado.ExecuteCommandAsync(pgSqlInsertUserScore,
                    new
                    {
                        Tenant = msg.TenantName,
                        ID = _idGenerator.CreateGuid(),
                        UserKey = msg.UserKey,
                        AvailableScore = 0,
                        FreezeScore = 0,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        DeleteFlag = false,
                    });

                //OperateKey需要设置为唯一约束
                var operateRecord = new OperationRecord { Id = _idGenerator.CreateGuid(), OperateKey = scoreOperateKey, TenantId = msg.TenantName };
                using (var tran = db.UseTran())
                {
                    try
                    {
                        var returnId = await db.Insertable(operateRecord).ExecuteCommandAsync();
                        if (returnId <= 0)
                        {
                            throw Oops.Oh("操作重复提交");
                        }

                        //添加积分事件记录
                        var scoreTaskTerm = GetTermTimeRangeAndTimes(scoreTask);
                        var scoreValidTerm = GetValidTimeRange(scoreTask);
                        var pgSqlInsertScoreEvent = @$"
                            Insert Into ""{msg.TenantName}"". ""UserEventScore"" (""ID"",""AppCode"",""EventCode"",""EventName"",""FullEventCode"",""Type"",""EventScore"",""UserKey"",""TriggerTime"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"",""ScoreExpireDate"",""ScoreObtainDate"")
                            SELECT @ID,@AppCode,@EventCode,@EventName,@FullEventCode,@Type,@EventScore,@UserKey,@TriggerTime,@CreateTime,@UpdateTime,false,@Tenant,cast(@ScoreExpireDate as timestamp),cast(@ScoreObtainDate as timestamp)
                            WHERE (SELECT COUNT(""ID"") FROM ""{msg.TenantName}"".""UserEventScore"" WHERE ""DeleteFlag""=false AND ""UserKey""=@UserKey AND ""FullEventCode""=@FullEventCode AND ""TriggerTime"" > @StartTime AND ""TriggerTime"" < @EndTime) < @TriggerCount
                            ";
                        var insertScoreEventCount = await db.Ado.ExecuteCommandAsync(pgSqlInsertScoreEvent,
                            new
                            {
                                Tenant = msg.TenantName,
                                ID = _idGenerator.CreateGuid(),
                                AppCode = msg.AppCode,
                                EventCode = msg.EventCode,
                                EventName = msg.EventName,
                                FullEventCode = $"{msg.AppCode}:{msg.EventCode}",
                                Type = 1,
                                EventScore = obtainScore,
                                UserKey = msg.UserKey,
                                TriggerTime = DateTime.Now,
                                CreateTime = DateTime.Now,
                                UpdateTime = DateTime.Now,
                                StartTime = scoreTaskTerm.Item1,
                                EndTime = scoreTaskTerm.Item2,
                                TriggerCount = scoreTaskTerm.Item3,
                                ScoreObtainDate = scoreValidTerm.Item1,
                                ScoreExpireDate = scoreValidTerm.Item2,
                            });
                        if (insertScoreEventCount <= 0)
                        {
                            throw Oops.Oh("周期内已达成最大可执行任务次数");
                        }

                        //读者加积分
                        await db.Updateable<UserScore>()
                            .SetColumns(x => x.AvailableScore == x.AvailableScore + obtainScore)
                            .Where(x => !x.DeleteFlag && x.UserKey == msg.UserKey)
                            .ExecuteCommandAsync();

                        tran.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        //暂时吞掉异常记录日志
                        tran.RollbackTran();
                    }
                }
            }

            var medalTask = await db.Queryable<MedalObtainTask>().FirstAsync(x => !x.DeleteFlag && x.AppCode == msg.AppCode && x.EventCode == msg.EventCode && nowTime >= x.BeginDate && nowTime <= x.EndDate && x.IsActive);
            if (medalTask != null)
            {
                //勋章处理逻辑
                var pgSqlInsertUserMedal = @$"
                INSERT INTO ""{msg.TenantName}"".""UserMedal""(""ID"",""MedalObtainTaskId"",""UserKey"",""Status"",""TriggerTime"",""TotalTime"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"")
                SELECT @ID, @TaskId, @UserKey, @Status, @TriggerTime, @TotalTime, @CreateTime, @UpdateTime, @DeleteFlag, @Tenant
                WHERE (SELECT COUNT(""ID"") FROM ""{msg.TenantName}"".""UserMedal"" WHERE ""DeleteFlag"" = false AND ""UserKey"" = @UserKey AND ""MedalObtainTaskId"" = @TaskId) < 1
                ";
                await db.Ado.ExecuteCommandAsync(pgSqlInsertUserMedal,
                    new
                    {
                        Tenant = msg.TenantName,
                        ID = _idGenerator.CreateGuid(),
                        TaskId = medalTask.Id,
                        UserKey = msg.UserKey,
                        Status = 0,
                        TriggerTime = 0,
                        TotalTime = medalTask.TotalTime,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        DeleteFlag = false,
                    });
                //OperateKey需要设置为唯一约束
                var operateRecord = new OperationRecord { Id = _idGenerator.CreateGuid(), OperateKey = medalOperateKey, TenantId = msg.TenantName };
                using (var tran = db.UseTran())
                {
                    try
                    {
                        var returnId = await db.Insertable(operateRecord).ExecuteCommandAsync();
                        if (returnId <= 0)
                        {
                            throw Oops.Oh("操作重复提交");
                        }
                        //添加勋章事件
                        var triggerWay = medalTask.TriggerWay;
                        var mustContinue = medalTask.MustContinue;

                        if (triggerWay == 0)
                        {
                            var pgSqlInsertMedalEvent = @$"
                            Insert into ""{msg.TenantName}"".""UserMedalEvent""(""ID"",""MedalObtainTaskId"",""UserKey"",""TriggerTime"",""TriggerReset"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"")
                            SELECT @ID,@TaskId,@UserKey,@TriggerTime,@TriggerReset,@CreateTime,@UpdateTime,@DeleteFlag,@Tenant
                            ";
                            //按次数
                            //插入行为事件
                            await db.Ado.ExecuteCommandAsync(pgSqlInsertMedalEvent,
                            new
                            {
                                Tenant = msg.TenantName,
                                ID = _idGenerator.CreateGuid(),
                                TaskId = medalTask.Id,
                                UserKey = msg.UserKey,
                                TriggerReset = false,
                                TriggerTime = nowTime,
                                CreateTime = DateTime.Now,
                                UpdateTime = DateTime.Now,
                                DeleteFlag = false,
                            });
                            await db.Updateable<UserMedal>()
                                .SetColumns(x => x.TriggerTime == x.TriggerTime + 1)
                                .SetColumns(x => x.TotalTime == medalTask.TotalTime)
                                .Where(x => !x.DeleteFlag && x.UserKey == msg.UserKey && x.MedalObtainTaskId == medalTask.Id && x.TriggerTime < medalTask.TotalTime)
                                .ExecuteCommandAsync();

                            await db.Updateable<UserMedal>()
                                .SetColumns(x => x.Status == 1)
                                .Where(x => !x.DeleteFlag && x.UserKey == msg.UserKey && x.MedalObtainTaskId == medalTask.Id && x.TriggerTime >= x.TotalTime)
                                .ExecuteCommandAsync();
                        }
                        else
                        {
                            //按天数
                            var preday = DateTime.Now.Date.AddDays(-1);
                            var nowday = DateTime.Now.Date;
                            var nextday = DateTime.Now.Date.AddDays(1);
                            var hasRecord = await db.Queryable<UserMedalEvent>().AnyAsync(x => !x.DeleteFlag && x.MedalObtainTaskId == medalTask.Id && x.UserKey == msg.UserKey && x.TriggerTime >= preday && x.TriggerTime < nowday);
                            var triggerReset = !hasRecord && mustContinue;
                            var pgSqlInsertMedalEvent = @$"
                            Insert into ""{msg.TenantName}"".""UserMedalEvent""(""ID"",""MedalObtainTaskId"",""UserKey"",""TriggerTime"",""TriggerReset"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"")
                            SELECT @ID,@TaskId,@UserKey,@TriggerTime,@TriggerReset,@CreateTime,@UpdateTime,@DeleteFlag,@Tenant
                            WHERE(SELECT(""ID"") FROM ""{msg.TenantName}"".""UserMedalEvent"" WHERE ""DeleteFlag"" = false AND ""UserKey"" = @UserKey AND ""MedalObtainTaskId"" = @TaskId AND ""TriggerTime"" > @NowDay AND ""TriggerTime"" < @NextDay) <= 0
                            ";
                            var insertEventCount = await db.Ado.ExecuteCommandAsync(pgSqlInsertMedalEvent,
                            new
                            {
                                ID = _idGenerator.CreateGuid(),
                                TaskId = medalTask.Id,
                                UserKey = msg.UserKey,
                                triggerReset = triggerReset,
                                TriggerTime = nowTime,
                                CreateTime = DateTime.Now,
                                UpdateTime = DateTime.Now,
                                DeleteFlag = false,
                                NowDay = nowday,
                                NextDay = nextday
                            });
                            if (insertEventCount <= 0)
                            {
                                throw Oops.Oh("今日已记录勋章时间");
                            }

                            if (triggerReset)
                            {
                                await db.Updateable<UserMedal>()
                                   .SetColumns(x => x.TriggerTime == 1)
                                   .SetColumns(x => x.TotalTime == medalTask.TotalTime)
                                   .Where(x => !x.DeleteFlag && x.UserKey == msg.UserKey && x.MedalObtainTaskId == medalTask.Id)
                                   .ExecuteCommandAsync();
                            }
                            else
                            {
                                await db.Updateable<UserMedal>()
                                .SetColumns(x => x.TriggerTime == x.TriggerTime + 1)
                                .SetColumns(x => x.TotalTime == medalTask.TotalTime)
                                .Where(x => !x.DeleteFlag && x.UserKey == msg.UserKey && x.MedalObtainTaskId == medalTask.Id && x.TriggerTime < medalTask.TotalTime)
                                .ExecuteCommandAsync();
                            }


                            await db.Updateable<UserMedal>()
                                .SetColumns(x => x.Status == 1)
                                .Where(x => !x.DeleteFlag && x.UserKey == msg.UserKey && x.MedalObtainTaskId == medalTask.Id && x.TriggerTime >= x.TotalTime)
                                .ExecuteCommandAsync();

                        }
                        tran.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        //暂时吞掉异常记录日志
                        tran.RollbackTran();
                    }
                }
            }
            db.Close();
            return true;
        }



        #region QueryScoreObtainTaskHelper

        /// <summary>
        /// 获取时间周期以及周期内可重复触发次数
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private Tuple<DateTime, DateTime, int> GetTermTimeRangeAndTimes(ScoreObtainTask task)
        {
            switch (task.TriggerTerm)
            {
                case 0:
                    return new Tuple<DateTime, DateTime, int>(new DateTime(1990, 1, 1), new DateTime(2100, 1, 1), task.TriggerTime);
                case 1:
                    var dayStartTime = DateTime.Now.Date;
                    var dayEndTime = DateTime.Now.Date.AddDays(1);
                    return new Tuple<DateTime, DateTime, int>(dayStartTime, dayEndTime, task.TriggerTime);
                case 2:
                    var weekStartTime = GetWeekFirstDayMon(DateTime.Now);
                    var weekEndTime = GetWeekLastDaySun(DateTime.Now).AddDays(1);
                    return new Tuple<DateTime, DateTime, int>(weekStartTime, weekEndTime, task.TriggerTime);
                case 3:
                    var monthStartTime = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
                    var monthEndTime = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1);
                    return new Tuple<DateTime, DateTime, int>(monthStartTime, monthEndTime, task.TriggerTime);
                case 4:
                    int year = DateTime.Now.Year;
                    var yearStartTime = new DateTime(year, 1, 1);
                    var yearEndTime = yearStartTime.AddYears(1);
                    return new Tuple<DateTime, DateTime, int>(yearStartTime, yearEndTime, task.TriggerTime);
                default:
                    return new Tuple<DateTime, DateTime, int>(new DateTime(1990, 1, 1), new DateTime(2100, 1, 1), task.TriggerTime);
            }
        }

        private Tuple<DateTime?, DateTime?> GetValidTimeRange(ScoreObtainTask task)
        {
            switch (task.ValidTerm)
            {
                case 0:
                    return new Tuple<DateTime?, DateTime?>(DateTime.Now.Date, null);
                case 1:
                    return new Tuple<DateTime?, DateTime?>(DateTime.Now.Date, DateTime.Now.Date.AddYears(1));
                case 2:
                    return new Tuple<DateTime?, DateTime?>(DateTime.Now.Date, DateTime.Now.Date.AddYears(2));
                case 3:
                    return new Tuple<DateTime?, DateTime?>(DateTime.Now.Date, DateTime.Now.Date.AddYears(3));
                case 4:
                    return new Tuple<DateTime?, DateTime?>(DateTime.Now.Date, DateTime.Now.Date.AddDays(4));
                case 5:
                    return new Tuple<DateTime?, DateTime?>(DateTime.Now.Date, DateTime.Now.Date.AddDays(5));
                default:
                    return new Tuple<DateTime?, DateTime?>(DateTime.Now.Date, null);
            }
        }

        ///// <summary>
        ///// 获取时间周期以及周期内可重复触发次数
        ///// </summary>
        ///// <param name="task"></param>
        ///// <returns></returns>
        //private Tuple<DateTime, DateTime, int> GetTermTimeRangeAndTimes(MedalObtainTask task)
        //{
        //    switch (task.TriggerTerm)
        //    {
        //        case 0:
        //            return new Tuple<DateTime, DateTime, int>(new DateTime(1990, 1, 1), new DateTime(2100, 1, 1), task.TriggerTime);
        //        case 1:
        //            var dayStartTime = DateTime.Now.Date;
        //            var dayEndTime = DateTime.Now.Date.AddDays(1);
        //            return new Tuple<DateTime, DateTime, int>(dayStartTime, dayEndTime, task.TriggerTime);
        //        case 2:
        //            var weekStartTime = GetWeekFirstDayMon(DateTime.Now);
        //            var weekEndTime = GetWeekLastDaySun(DateTime.Now).AddDays(1);
        //            return new Tuple<DateTime, DateTime, int>(weekStartTime, weekEndTime, task.TriggerTime);
        //        case 3:
        //            var monthStartTime = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
        //            var monthEndTime = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1);
        //            return new Tuple<DateTime, DateTime, int>(monthStartTime, monthEndTime, task.TriggerTime);
        //        case 4:
        //            int year = DateTime.Now.Year;
        //            var yearStartTime = new DateTime(year, 1, 1);
        //            var yearEndTime = yearStartTime.AddYears(1);
        //            return new Tuple<DateTime, DateTime, int>(yearStartTime, yearEndTime, task.TriggerTime);
        //        default:
        //            return new Tuple<DateTime, DateTime, int>(new DateTime(1990, 1, 1), new DateTime(2100, 1, 1), task.TriggerTime);
        //    }
        //}

        private DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            //星期一为第一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天  
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }

        private DateTime GetWeekLastDaySun(DateTime datetime)
        {
            //星期天为最后一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天  
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(LastDay);
        }
        #endregion
    }


}
