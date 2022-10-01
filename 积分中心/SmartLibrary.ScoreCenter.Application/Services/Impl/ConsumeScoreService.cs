/*********************************************************
* 名    称：ConsumeScoreService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：积分消费服务
* 更新历史：
*
* *******************************************************/
using Furion.DependencyInjection;
using Furion.FriendlyException;
using SmartLibrary.ScoreCenter.Application.Services.Interface;
using SmartLibrary.ScoreCenter.Application.ViewModels;
using SmartLibrary.ScoreCenter.EntityFramework.Core.Entitys;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Application.Services.Impl
{
    /// <summary>
    /// 积分消费服务
    /// </summary>
    public class ConsumeScoreService : IConsumeScoreService, IScoped
    {
        public ConsumeScoreService()
        {
        }
        public async Task<bool> ConsumeTry(SqlSugarClient db, ConsumeScoreInput scoreInput)
        {
            if (string.IsNullOrWhiteSpace(scoreInput.Tenant))
            {
                throw Oops.Oh("租户信息不能为空");
            }
            if (scoreInput.EventID == Guid.Empty)
            {
                throw Oops.Oh("消费事件ID不能为空");
            }
            //pgSql的检查语句，如果后续需要扩展其他数据库，需要进行配置
            var schemaCheckSql = "SELECT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = @TenantName)";

            var existSchema = (bool)db.Ado.GetScalar(schemaCheckSql, new { TenantName = scoreInput.Tenant });
            if (!existSchema)
            {
                //如果不存在该租户数据，直接返回不处理
                return true;
            }
            //查询扣分任务
            var nowTime = DateTime.Now;
            var consumeTask = await db.Queryable<ScoreConsumeTask>().FirstAsync(x => !x.DeleteFlag && x.AppCode == scoreInput.AppCode && x.EventCode == scoreInput.EventCode && x.IsActive);
            if (consumeTask == null || consumeTask.ConsumeScore <= 0)
            {
                //没有相应扣分任务，无需扣分，直接返回
                return true;
            }
            var consumeScore = consumeTask.ConsumeScore;
            //积分冻结逻辑
            var userScore = await db.Queryable<UserScore>().FirstAsync(x => !x.DeleteFlag && x.UserKey == scoreInput.UserKey);
            if (userScore == null)
            {
                throw Oops.Oh("积分不足");
            }
            //添加积分记录实践
            var pgSqlInsertScoreEvent = @$"
                            Insert Into ""{scoreInput.Tenant}"". ""UserEventScore"" (""ID"",""AppCode"",""EventCode"",""EventName"",""FullEventCode"",""Type"",""EventScore"",""UserKey"",""TriggerTime"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"")
                            SELECT @ID,@AppCode,@EventCode,@EventName,@FullEventCode,@Type,@EventScore,@UserKey,@TriggerTime,@CreateTime,@UpdateTime,false,@Tenant
                            ";
            var insertConsumeEventCount = await db.Ado.ExecuteCommandAsync(pgSqlInsertScoreEvent,
                new
                {
                    Tenant = scoreInput.Tenant,
                    ID = scoreInput.EventID,
                    AppCode = scoreInput.AppCode,
                    EventCode = scoreInput.EventCode,
                    EventName = scoreInput.EventName,
                    FullEventCode = $"{scoreInput.AppCode}:{scoreInput.EventCode}",
                    Type = -1,
                    EventScore = -consumeScore,
                    UserKey = scoreInput.UserKey,
                    TriggerTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                });
            if (insertConsumeEventCount <= 0)
            {
                throw Oops.Oh("积分不足");
            }
            var updateRowCount = await db.Updateable<UserScore>()
                .SetColumns(x => x.AvailableScore == x.AvailableScore - consumeScore)
                .SetColumns(x => x.FreezeScore == x.FreezeScore + consumeScore)
                .Where(x => !x.DeleteFlag && x.UserKey == scoreInput.UserKey && x.AvailableScore >= consumeScore)
                .ExecuteCommandAsync();
            if (updateRowCount <= 0)
            {
                throw Oops.Oh("积分不足");
            }
            return true;
        }

        /// <summary>
        /// 扣减锁定积分
        /// </summary>
        /// <param name="db"></param>
        /// <param name="scoreInput"></param>
        /// <returns></returns>
        public async Task<bool> ConsumeConfirm(SqlSugarClient db, ConsumeScoreInput scoreInput)
        {
            //pgSql的检查语句，如果后续需要扩展其他数据库，需要进行配置
            var schemaCheckSql = "SELECT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = @TenantName)";

            var existSchema = (bool)db.Ado.GetScalar(schemaCheckSql, new { TenantName = scoreInput.Tenant });
            if (!existSchema)
            {
                //如果不存在该租户数据，直接返回不处理
                return true;
            }

            var consumeEvent = await db.Queryable<UserEventScore>().FirstAsync(x => !x.DeleteFlag && x.Id == scoreInput.EventID);
            if (consumeEvent == null || Math.Abs(consumeEvent.EventScore) == 0)
            {
                //如果没有找到消费记录，直接返回不处理
                return true;
            }

            var consumeScore = Math.Abs(consumeEvent.EventScore);
            //积分冻结逻辑
            var userScore = await db.Queryable<UserScore>().FirstAsync(x => !x.DeleteFlag && x.UserKey == scoreInput.UserKey);
            if (userScore == null)
            {
                throw Oops.Oh("锁定积分不足");
            }
            var updateRowCount = await db.Updateable<UserScore>()
              .SetColumns(x => x.FreezeScore == x.FreezeScore - consumeScore)
              .Where(x => !x.DeleteFlag && x.UserKey == scoreInput.UserKey && x.FreezeScore >= consumeScore)
              .ExecuteCommandAsync();
            if (updateRowCount <= 0)
            {
                throw Oops.Oh("锁定积分不足");
            }
            return true;

        }

        /// <summary>
        /// 退回锁定积分
        /// </summary>
        /// <param name="db"></param>
        /// <param name="scoreInput"></param>
        /// <returns></returns>
        public async Task<bool> ConsumeCancel(SqlSugarClient db, ConsumeScoreInput scoreInput)
        {
            //pgSql的检查语句，如果后续需要扩展其他数据库，需要进行配置
            var schemaCheckSql = "SELECT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = @TenantName)";

            var existSchema = (bool)db.Ado.GetScalar(schemaCheckSql, new { TenantName = scoreInput.Tenant });
            if (!existSchema)
            {
                //如果不存在该租户数据，直接返回不处理
                return true;
            }
            var consumeEvent = await db.Queryable<UserEventScore>().FirstAsync(x => !x.DeleteFlag && x.Id == scoreInput.EventID);
            if (consumeEvent == null || Math.Abs(consumeEvent.EventScore) == 0)
            {
                //如果没有找到消费记录，直接返回不处理
                return true;
            }
            var consumeScore = Math.Abs(consumeEvent.EventScore);
            //标记删除积分记录
            var updateDeleteCount = await db.Updateable<UserEventScore>()
                .SetColumns(x => x.DeleteFlag == true)
                .Where(x => x.Id == scoreInput.EventID)
                .ExecuteCommandAsync();
            //积分冻结逻辑
            var userScore = await db.Queryable<UserScore>().FirstAsync(x => !x.DeleteFlag && x.UserKey == scoreInput.UserKey);
            if (userScore == null)
            {
                throw Oops.Oh("锁定积分不足");
            }
            var updateRowCount = await db.Updateable<UserScore>()
              .SetColumns(x => x.AvailableScore == x.AvailableScore + consumeScore)
              .SetColumns(x => x.FreezeScore == x.FreezeScore - consumeScore)
              .Where(x => !x.DeleteFlag && x.UserKey == scoreInput.UserKey && x.FreezeScore >= consumeScore)
              .ExecuteCommandAsync();
            if (updateRowCount <= 0)
            {
                throw Oops.Oh("锁定积分不足");
            }
            return true;
        }

    }
}
