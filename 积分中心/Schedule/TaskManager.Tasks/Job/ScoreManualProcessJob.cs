using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.Tasks.Dto;
using TaskManager.Model.Entities;
using TaskManager.Model.Enum;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TaskManager.Tasks.Services;
using SmartLibrary.User.RpcService;

namespace TaskManager.Tasks.Job
{
    /// <summary>
    /// 积分手动调整任务
    /// </summary>
    public class ScoreManualProcessJob : SmartJobBase
    {

        public override string JobName => "积分手动调整任务";
        private int LogId = 0;
        public override void DoWork()
        {
            var db = TenantDb;
            LogId = WriteLog(LogId, "---------执行中--------", 0);
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddScoped<IGrpcClientResolver, GrpcClientResolver>();
            var serviceProvider = services.BuildServiceProvider();

            var manualProcess = db.Queryable<ScoreManualProcess>().Where(x => x.Status == (int)EnumManualProcessStatus.待处理).OrderBy(x => x.CreateTime).Take(5).ToList();
            if (!manualProcess.Any())
            {
                WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}无待处理数据", 1);
                return;
            }

            var index = 1;
            manualProcess.ForEach(item =>
            {
                WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}开始处理第{index}条数据，ID:{item.ID.ToString("N")}", 0);
                switch (item.SourceFrom)
                {
                    case (int)EnumSourceFrom.规则查询:
                        CreateUsersFromRule(item, serviceProvider);
                        DoScoreProcess(item);
                        return;
                    case (int)EnumSourceFrom.直接添加:
                        DoScoreProcess(item);
                        return;
                }
                WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}第{index}条数据处理完成，ID:{item.ID.ToString("N")}", 0);
                index++;
            });
            WriteLog(LogId, "---------执行完成--------", 1);
        }

        /// <summary>
        /// 通过规则查询用户数据到
        /// </summary>
        /// <param name="process"></param>
        private void CreateUsersFromRule(ScoreManualProcess process, ServiceProvider serviceProvider)
        {
            var accessToken = GetAccessToken(serviceProvider, SchedulerCenter.SmartTaskConfig, SchedulerEntity.TenantId);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}获取AccessToken失败", -1);
                return;
            }
            //网关地址后期需要替换为根据应用编码动态获取
            var fabioUrl = SchedulerCenter.SmartTaskConfig["FabioUrl"].ToString();
            if (string.IsNullOrWhiteSpace(fabioUrl))
            {
                WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}获取Fabio网关地址失败", -1);
                return;
            }
            var grpcClientResolver = serviceProvider.GetRequiredService<IGrpcClientResolver>();
            var grpcClient = grpcClientResolver.EnsureClient<UserGrpcService.UserGrpcServiceClient>(fabioUrl, accessToken);
            var db = TenantDb;
            var users = db.Queryable<ScoreRecieveUser>().Where(x => !x.DeleteFlag && x.ProcessID == process.ID).ToList();
            if (users.Any())
            {
                //已关联用户数据，不再做处理
                return;
            }
            var rules = db.Queryable<ScoreRecieveRule>().Where(x => !x.DeleteFlag && x.ProcessID == process.ID).ToList();
            if (!rules.Any())
            {
                return;
            }
            var ruleGroupIds = rules.Where(x => x.RuleType == (int)EnumRuleType.用户组).Select(x => x.PropertyValue).ToList();
            var ruleUserTypes = rules.Where(x => x.RuleType == (int)EnumRuleType.用户类型).Select(x => x.PropertyValue).ToList();
            var pageIndex = 1;
            var pageSize = 10000;
            var pageCount = 10000;
            var recieveUsers = new List<ScoreRecieveUser>();
            //通过用户组添加
            while (pageCount >= pageSize)
            {
                if (ruleGroupIds.Any())
                {
                    var queryFilter = new SimpleTableQuery { PageIndex = pageIndex, PageSize = pageSize };
                    queryFilter.GroupIds.AddRange(ruleGroupIds);
                    var userData = grpcClient.GetUserListByGroups(queryFilter);
                    var userItems = userData.Items.Where(x => !string.IsNullOrWhiteSpace(x.Key)).Select(x => new ScoreRecieveUser
                    {
                        ID = Guid.NewGuid(),
                        ProcessID = process.ID,
                        UserKey = x.Key,
                        SourceFrom = (int)EnumSourceFrom.规则查询,
                        DeleteFlag = false,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    });
                    recieveUsers.AddRange(userItems);
                    pageIndex++;
                    pageCount = userData.Items.Count;
                }
                else
                {
                    pageCount = 0;
                }

            }
            //通过用户类型添加
            pageCount = 10000;
            pageIndex = 1;
            while (pageCount >= pageSize)
            {
                if (ruleUserTypes.Any())
                {
                    var queryFilter = new SimpleTableQuery { PageIndex = pageIndex, PageSize = pageSize };
                    queryFilter.UserTypeCodes.AddRange(ruleUserTypes);
                    var userData = grpcClient.GetUserListByTypes(queryFilter);
                    var userItems = userData.Items.Select(x => new ScoreRecieveUser
                    {
                        ID = Guid.NewGuid(),
                        ProcessID = process.ID,
                        UserKey = x.Key,
                        SourceFrom = (int)EnumSourceFrom.规则查询,
                        DeleteFlag = false,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now
                    });
                    recieveUsers.AddRange(userItems);
                    pageIndex++;
                    pageCount = userData.Items.Count;
                }
                else
                {
                    pageCount = 0;
                }

            }

            var needInsert = recieveUsers.Count();
            var actualInsert = 0;
            if (recieveUsers.Any())
            {
                using (var tran = db.UseTran())
                {
                    try
                    {
                        //标记UserGroup数据预新增
                        db.Utilities.PageEach(recieveUsers, 10000, pageList =>
                        {
                            actualInsert = actualInsert + db.Insertable(pageList).ExecuteCommand();
                        });
                        tran.CommitTran();
                    }
                    catch
                    {
                        tran.RollbackTran();
                    }

                }
            }
            WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}需要新增{needInsert}条数据，实际新增{actualInsert}条数据", 0);
        }

        /// <summary>
        /// 处理读者积分
        /// </summary>
        /// <param name="process"></param>
        private void DoScoreProcess(ScoreManualProcess process)
        {
            WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}开始处理用户积分", 0);

            var db = TenantDb;
            //用户积分记录处理
            var pgSqlInsertUserScore = @$"
                Insert Into ""{SchedulerEntity.TenantId}"".""UserScore""(""ID"",""UserKey"",""AvailableScore"",""FreezeScore"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"")
                SELECT ""SRU"".""ID"",""SRU"".""UserKey"",0,0,@CreateTime,@UpdateTime,false,@Tenant FROM ""{SchedulerEntity.TenantId}"".""ScoreRecieveUser"" AS ""SRU""
                WHERE ""SRU"".""ProcessID"" = @ProcessId AND NOT EXISTS(SELECT ""InnerUserScore"".""ID"" FROM ""{SchedulerEntity.TenantId}"".""UserScore"" AS ""InnerUserScore"" WHERE ""InnerUserScore"".""UserKey"" = ""SRU"".""UserKey"" AND ""InnerUserScore"".""DeleteFlag"" = FALSE)
                ";
            var insertCount = db.Ado.ExecuteCommand(pgSqlInsertUserScore,
                 new
                 {
                     Tenant = SchedulerEntity.TenantId,
                     CreateTime = DateTime.Now,
                     UpdateTime = DateTime.Now,
                     ProcessId = process.ID
                 });
            WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}本次新增用户积分记录{insertCount}条", 0);
            var scoreValidTerm = GetValidTimeRange(process);
            using (var tran = db.UseTran())
            {
                try
                {
                    //用户积分变更事件
                    var pgSqlInsertUserScoreEvent = $@"
                        INSERT INTO ""{SchedulerEntity.TenantId}"".""UserEventScore""(""ID"",""AppCode"",""EventCode"",""EventName"",""FullEventCode"",""Type"",""EventScore"",""UserKey"",""TriggerTime"",""CreateTime"",""UpdateTime"",""DeleteFlag"",""TenantId"",""ScoreExpireDate"",""ScoreObtainDate"")
                        SELECT ""ID"",'scorecenter','ManualProcess',@EventName,'scorecenter:ManualProcess',@Type,@EventScore,""UserKey"",@TriggerTime,@CreateTime,@UpdateTime,false,@Tenant,cast(@ScoreExpireDate as timestamp),cast(@ScoreObtainDate as timestamp)
                        FROM ""{SchedulerEntity.TenantId}"".""ScoreRecieveUser"" WHERE ""ProcessID"" = @ProcessId and ""DeleteFlag"" = FALSE
             ";
                    var insertEventCount = db.Ado.ExecuteCommand(pgSqlInsertUserScoreEvent,
                        new
                        {
                            Tenant = SchedulerEntity.TenantId,
                            EventName = process.Desc,
                            Type = process.Type,
                            EventScore = process.Type * Math.Abs(process.Score),
                            TriggerTime = process.CreateTime,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            ProcessId = process.ID,
                            ScoreObtainDate = scoreValidTerm.Item1,
                            ScoreExpireDate = scoreValidTerm.Item2,
                        });
                    //更新用户积分
                    var pgSqlUpdateUserScore = $@"
                        UPDATE ""{SchedulerEntity.TenantId}"".""UserScore"" AS ""US"" SET ""AvailableScore""=""AvailableScore""+@EventScore,""UpdateTime""=@UpdateTime
                        FROM ""{SchedulerEntity.TenantId}"".""ScoreRecieveUser"" AS ""SRU"" WHERE ""US"".""DeleteFlag"" = FALSE AND ""SRU"".""DeleteFlag"" = FALSE AND
                        ""US"".""UserKey"" = ""SRU"".""UserKey"" AND ""SRU"".""ProcessID"" = @ProcessID
                    ";
                    var updateUserScoreCount = db.Ado.ExecuteCommand(pgSqlUpdateUserScore,
                        new
                        {
                            EventScore = process.Type * Math.Abs(process.Score),
                            ProcessId = process.ID,
                            UpdateTime = DateTime.Now,
                        });
                    //修改Process为已完成
                    db.Updateable<ScoreManualProcess>()
                        .SetColumns(x => x.Status == (int)EnumManualProcessStatus.完成)
                        .SetColumns(x => x.UpdateTime == DateTime.Now)
                        .Where(x => x.ID == process.ID)
                        .ExecuteCommand();
                    tran.CommitTran();
                    WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}本次新增用户积分事件记录{insertEventCount}条,更新读者积分记录{updateUserScoreCount}条", 0);
                }
                catch (Exception ex)
                {
                    WriteLog(LogId, $"{nameof(ScoreManualProcessJob)}积分处理异常:{ex.Message}", -1);
                    tran.RollbackTran();
                }
            }

        }
        private Tuple<DateTime?, DateTime?> GetValidTimeRange(ScoreManualProcess task)
        {
            if (task.Type == (int)EnumScoreType.减积分)
            {
                return new Tuple<DateTime?, DateTime?>(null, null);
            }
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

        private string GetAccessToken(ServiceProvider serviceProvider, IConfigurationRoot configRoot, string tenant)
        {
            try
            {
                var accessTokenUrl = configRoot.GetSection("AccessTokenUrl").Value;
                if (string.IsNullOrWhiteSpace(accessTokenUrl))
                {
                    return "";
                }
                var orgInfoSection = configRoot.GetSection("OrgInfo");
                var orgId = orgInfoSection["Id"] ?? "";
                var orgSecret = orgInfoSection["Secret"] ?? "";

                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var client = httpClientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(new { OrgId = orgId, OrgSecret = orgSecret, OrgCode = tenant, UserKey = "scorecenter" }));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(accessTokenUrl, content).Result;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return "";
                }
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var tokenResult = JsonConvert.DeserializeObject<TokenResultOutput>(responseContent);
                return tokenResult.Data.Token;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}
