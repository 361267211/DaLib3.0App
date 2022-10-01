using Newtonsoft.Json;
using System;
using TaskManager.Tasks.Dto;
using TaskManager.Adapters;
using TaskManager.Model.Standard;
using Quartz;
using SmartLibrary.Search.EsSearchProxy.Core.Interfaces;
using System.Linq;
using SmartLibrary.Search.EsSearchProxy.Core.Dto;
using SmartLibrary.Search.EsSearchProxy.Core;
using TaskManager.Tasks.Utils;
using System.Collections.Generic;
using TaskManager.Model.Entities;
using TaskManager.Adapters.Util;
using Scheduler.Service.Utils;
using TaskManager.Tasks.Services;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.AppCenter;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

using TaskManager.Model.Dtos;
using Scheduler.Service.Entity;

namespace TaskManager.Tasks.Job
{
    /// <summary>
    /// 同步新闻服务
    /// </summary>
    public class SyncDatabaseJob : SmartJobBase
    {
        public override string JobName => "同步新闻服务";
        public async override void DoWork()
        {


            //注册grpc服务
            var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            services.AddHttpClient();
            services.AddScoped<IGrpcClientResolver, GrpcClientResolver>();
            var serviceProvider = services.AddEsSearchProxy(x =>
            {
                x.SiteId = 1;
                x.SitePassword = "SmartCqu_2020";
                x.SiteUserName = "cqu";
                x.EsApiBase = new Uri("http://essmartapi.cqvip.com");
                x.ConnectionTimeOut = TimeSpan.FromSeconds(45);
            }).BuildServiceProvider();

            var accessToken = GetAccessToken(serviceProvider, SchedulerCenter.SmartTaskConfig, "cqu");
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                WriteLog($"{nameof(SyncDatabaseJob)}获取AccessToken失败");
                return;
            }

            //网关地址后期需要替换为根据应用编码动态获取
            var fabioUrl = SchedulerCenter.SmartTaskConfig["FabioUrl"].ToString();
            if (string.IsNullOrWhiteSpace(fabioUrl))
            {
                WriteLog($"{nameof(SyncDatabaseJob)}获取Fabio网关地址失败");
                return;
            }

            var grpcClientResolver = serviceProvider.GetRequiredService<IGrpcClientResolver>();
            var grpcClient = grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>(fabioUrl, accessToken);

            AppUserAuthRequest request = new AppUserAuthRequest
            {
                AppId = "databaseguide",
            };
            AppUserAuthReply reply = await grpcClient.GetAppUserAuthListAsync(request);
            //提取首个管理员的userid
            var managerId = reply.AppUserAuthList.FirstOrDefault(e => e.PermissionType == 1)?.UserId;

            var db = TenantDb;


            var paramsDto = JsonConvert.DeserializeObject<SyncNewsParamsDto>(this.TaskParams);
            if (paramsDto == null)
            {
                WriteLog($"{nameof(SyncDatabaseJob)}参数解析错误");
                throw new Exception($"{nameof(SyncDatabaseJob)}参数解析错误");

            }

            //取最大的index
            var maxIndex = 999; //TenantDb.Queryable<DatabaseTerraceDemo>().Max(e => e.OrderIndex);


            //取现有的链接名称
            var localUrlNameList = TenantDb.Queryable<DatabaseUrlName>().Where(e => !e.DeleteFlag).ToList();

            var localUrlNameListIn = new List<DatabaseUrlName>();

            var acessUrl = new List<DatabaseAcessUrl>();

            //上次成功的时间
            var lastSuccessTime = DateTime.Now.AddYears(-20); // LogDb.Queryable<SchedulerLogEntity>().Max(e => e.CreateTime);

            //提取重大线上的新闻数据-ContentInfo
            var databaseBackList = GetDataOperater<IDatabaseGuideDataAdapter>().GetDatabaseList(lastSuccessTime, out MessageHand conMessage);

            var newsColumnPermissionBackList = new List<NewsColumnPermissionsBack>();

            var test = databaseBackList.Where(e => e.Status >= 0 && e.IsShow && e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now).ToList();

            //取出栏目的新老id的对应数据
            var IdRelationMapdatabase = TenantDb.Queryable<IdRelationMap>().Where(e => e.Type == 1).ToList();




            //需要存入的新老id的对应数据
            var IdRelationMapIn = new List<IdRelationMap>();

            //地址名称
            var urlName = new List<DatabaseUrlName>();

            var databaseAcessUrl = new List<DatabaseAcessUrl>();


            foreach (var database in databaseBackList)
            {

                var databaseGuideId = "";

                var relate = IdRelationMapdatabase.FirstOrDefault(e => e.OldId == database.OldId.ToString() && e.Type == 1);

                //匹配历史id
                if (relate != null)
                {
                    databaseGuideId = relate.NewId;
                    database.Id = new Guid(databaseGuideId);

                }
                else
                {
                    databaseGuideId = Guid.NewGuid().ToString();
                    database.Id = new Guid(databaseGuideId);
                    IdRelationMapIn.Add(new IdRelationMap
                    {
                        Type = 1,
                        NewId = databaseGuideId,
                        OldId = database.OldId.ToString()
                    });
                }

                //提取链接列表



                var urlInfoList = JsonConvert.DeserializeObject<List<DatabaseAcessUrl>>(database.CustomUrl);

                foreach (var urlInfo in urlInfoList)
                {
                    urlInfo.CreatedTime = DateTimeOffset.UtcNow;
                    urlInfo.DatabaseID = database.Id;
                    urlInfo.Id = Guid.NewGuid();



                    var databaseUrlName = new DatabaseUrlName
                    {
                        Id = Guid.NewGuid(),
                        CreatedTime = DateTimeOffset.UtcNow,
                        DeleteFlag = false,
                        Name = urlInfo.Name,
                        UpdatedTime = null,
                    };

                    var url = localUrlNameList.FirstOrDefault(e => e.Name == urlInfo.Name);
                    if (url == null)
                    {
                        localUrlNameListIn.Add(databaseUrlName);
                    }
                    localUrlNameList.Add(databaseUrlName);
                }

                acessUrl.AddRange(urlInfoList);

            }

            if (conMessage.ex != null)
            {
                throw new JobExecutionException(conMessage.ex);
            }

            //过滤html元素后存入  冗余字段
            databaseBackList.ForEach(e =>
            {
                e.InformationEditor = 1;
                e.ResourceStatisticsEditor = 1;
                e.UseGuideEditor = 1;

                e.InformationText = HTMLFilter.StripHTML(e.Information);
                e.ResourceStatisticsText = HTMLFilter.StripHTML(e.ResourceStatistics);
                e.UseGuideText = HTMLFilter.StripHTML(e.UseGuide);
                e.OrderIndex = ++maxIndex;
            });





            //类型转换到entity
            var databaseList = databaseBackList.Select(p => new DatabaseTerraceDemo
            {

                Id = p.Id,
                IndirectUrl = p.IndirectUrl,
                Information = p.Information,
                Initials = p.Initials,
                Introduction = p.Introduction,
                IsShow = p.IsShow,
                DatabaseProviderID = p.DatabaseProviderID,
                OrderIndex = p.OrderIndex,
                Abbreviation = string.IsNullOrEmpty(p.Abbreviation) ? p.Title : p.Abbreviation,
                ArticleTypes = p.ArticleTypes,
                Cover = p.Cover,
                CreatedTime = p.CreatedTime,
                DeleteFlag = p.DeleteFlag,
                DomainClcs = p.DomainClcs,
                DomainEscs = p.DomainEscs,
                ExpiryBeginTime = p.ExpiryBeginTime,
                ExpiryEndTime = p.ExpiryEndTime,
                Label = p.Label,
                Language = p.Language,
                MonthClickNum = p.MonthClickNum,
                PurchaseType = p.PurchaseType,
                Remark = p.Remark,
                ResourceStatistics = p.ResourceStatistics,
                Status = p.Status,
                Title = p.Title,
                TotalClickNum = p.TotalClickNum,
                UpdatedTime = p.UpdatedTime,
                UseGuide = p.UseGuide,
                UserGroups = p.UserGroups,
                UserTypes = p.UserTypes,
                WhiteList = p.WhiteList,


                InformationEditor = p.InformationEditor,
                ResourceStatisticsEditor = p.ResourceStatisticsEditor,
                UseGuideEditor = p.UseGuideEditor,

                InformationText = p.InformationText,
                ResourceStatisticsText = p.ResourceStatisticsText,
                UseGuideText = p.UseGuideText,


            }).ToList();





            //首先同步到back表中做记录
            using (var tran = db.UseTran())
            {
                try
                {
                    int curCount = 0;//当前已导入的数据
                    var databaseBackListCount = databaseBackList.Count;
                    while (curCount < databaseBackListCount)
                    {

                        var curList = databaseBackList.Skip(curCount).Take(100).ToList();
                        db.Insertable(curList).ExecuteCommand();
                        WriteLog($"{curCount}-{curCount + curList.Count}导入成功");
                        curCount += curList.Count;
                    }



                    tran.CommitTran();
                }
                catch (Exception ex)
                {
                    tran.RollbackTran();
                }

            }

            using (var tran = db.UseTran())
            {
                try
                {
                    int curCount = 0;//当前已导入的数据
                    var newsContentListCount = databaseList.Count;
                    while (curCount < newsContentListCount)
                    {

                        var curList = databaseList.Skip(curCount).Take(100).ToList();
                        db.Insertable(curList).ExecuteCommand();
                        WriteLog($"{curCount}-{curCount + curList.Count}导入成功");
                        curCount += curList.Count;
                    }
                    db.Insertable(localUrlNameListIn).ExecuteCommand();
                    db.Insertable(acessUrl).ExecuteCommand();
                    db.Insertable(IdRelationMapIn).ExecuteCommand();


                    tran.CommitTran();
                }
                catch (Exception ex)
                {
                    tran.RollbackTran();
                }

            }

            var insertContentCount = databaseBackList.Count;

            WriteLog($"新闻内容同步完成,新增{insertContentCount}条");


        //var fulltext = $"{p.Introduction}\r\n{p.InformationText}\r\n{p.UseGuideText}\r\n{p.ResourceStatisticsText}";

            var esNewsList = databaseList.Select(p => new UpsertOwnerNewsRequestParameter
            {
                app_id = "databaseguide",
                app_type = SmartLibrary.Search.EsSearchProxy.Core.Models.OrganNewsType.Page,
                click_count = (int)p.TotalClickNum,
                docId = $"databaseguide_cqu_{p.Id.ToString().Replace('-', '_')}",
                fulltext = $"{p.Introduction}\r\n{p.InformationText}\r\n{p.UseGuideText}\r\n{p.ResourceStatisticsText}",
                keyword = ("").Split(';', StringSplitOptions.RemoveEmptyEntries),
                owner = "cqu",
                pub_time = p.CreatedTime,
                summary = $"{p.Introduction}\r\n{p.InformationText}\r\n{p.UseGuideText}\r\n{p.ResourceStatisticsText}".Substring(0, $"{p.Introduction}\r\n{p.InformationText}\r\n{p.UseGuideText}\r\n{p.ResourceStatisticsText}".Length > 4000 ? 4000 : $"{p.Introduction}\r\n{p.InformationText}\r\n{p.UseGuideText}\r\n{p.ResourceStatisticsText}".Length),
                title = p.Title,               
                update_time = p.UpdatedTime.HasValue ? p.UpdatedTime.Value : DateTimeOffset.MinValue,
                url = $"/#/web_dataBaseDetail?databaseid={p.Id}",
                

            }).ToList();

            var esProxy = serviceProvider.GetService<IEsProxyService>();
            esNewsList.ForEach(async item =>
            {
                var result = await esProxy.UpsertOrganNewsAsync(item);
            });
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
