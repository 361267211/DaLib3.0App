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

namespace TaskManager.Tasks.Job
{
    /// <summary>
    /// 同步新闻服务
    /// </summary>
    public class SyncNewsJob : SmartJobBase
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

            var accessToken = GetAccessToken(serviceProvider, SchedulerCenter.SmartTaskConfig, SchedulerEntity.TenantId);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                WriteLog($"{nameof(SyncNewsJob)}获取AccessToken失败");
                return;
            }

            //网关地址后期需要替换为根据应用编码动态获取
            var fabioUrl = SchedulerCenter.SmartTaskConfig["FabioUrl"].ToString();
            if (string.IsNullOrWhiteSpace(fabioUrl))
            {
                WriteLog($"{nameof(SyncNewsJob)}获取Fabio网关地址失败");
                return;
            }

            var grpcClientResolver = serviceProvider.GetRequiredService<IGrpcClientResolver>();
            var grpcClient = grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>(fabioUrl, accessToken);

            AppUserAuthRequest request = new AppUserAuthRequest
            {
                AppId = "news",
            };
            AppUserAuthReply reply = await grpcClient.GetAppUserAuthListAsync(request);
            //提取首个管理员的userid
            var managerId = reply.AppUserAuthList.FirstOrDefault(e => e.PermissionType == 1)?.UserId;
 
            var db = TenantDb;


            var paramsDto = JsonConvert.DeserializeObject<SyncNewsParamsDto>(this.TaskParams);
            if (paramsDto == null)
            {
                WriteLog($"{nameof(SyncNewsJob)}参数解析错误");
                throw new Exception($"{nameof(SyncNewsJob)}参数解析错误");

            }





            //从应用中心提取首个管理员
            //var newsAppManager = GetDataOperater<INewsDataAdapter>().GetNewsManager(out MessageHand colMessage);

            //提取重大线上的新闻栏目-ContentPlate
            var newsColumnBackList = GetDataOperater<INewsDataAdapter>().GetNewsColumnList(out MessageHand colMessage);
            if (colMessage.ex != null)
            {
                throw new JobExecutionException(colMessage.ex);
            }

            var columnIds = newsColumnBackList.Select(e => e.Id).ToList();

            //提取重大线上的新闻数据-ContentInfo
            var newsContentBackList = GetDataOperater<INewsDataAdapter>().GetNewsContentList(columnIds, out MessageHand conMessage);

            var newsColumnPermissionBackList = new List<NewsColumnPermissionsBack>();

            //取出栏目的新老id的对应数据
            var IdRelationMapColumn = TenantDb.Queryable<IdRelationMap>().Where(e => e.Type == 1).ToList();

            //取出内容的新老id的对应数据
            var IdRelationMapContent = TenantDb.Queryable<IdRelationMap>().Where(e => e.Type == 2).ToList();


            //需要存入的新老id的对应数据
            var IdRelationMapIn = new List<IdRelationMap>();



            foreach (var col in newsColumnBackList)
            {

                var newColumnId = "";
                //匹配历史id
                if (IdRelationMapColumn.Any(e => e.OldId == col.OldId.ToString()))
                {
                    newColumnId = IdRelationMapColumn.First(e => e.OldId == col.OldId.ToString()).NewId;
                }
                else
                {
                    newColumnId = Guid.NewGuid().ToString();

                    IdRelationMapIn.Add(new IdRelationMap
                    {
                        Type = 1,
                        NewId = newColumnId,
                        OldId = col.OldId.ToString()
                    }) ;
                }

                foreach (var con in newsContentBackList)
                {
                    if (con.ColumnID == col.Id)
                    {
                        con.ColumnID = newColumnId;
                        con.ColumnIDs = newColumnId;
                    }
                }


                col.Id = newColumnId;
                //对应的栏目权限
                newsColumnPermissionBackList.Add(new NewsColumnPermissionsBack
                {
                    ColumnID = col.Id,
                    Id = Time2KeyUtils.GetRandOnlyId(),
                    ManagerID = managerId,
                    CreatedTime = DateTimeOffset.UtcNow,
                    DeleteFlag = false,
                    Manager = managerId,
                    Permission = 0
                });
            }



            foreach (var con in newsContentBackList)
            {
                var newsContentId = "";
                //匹配历史id
                if (IdRelationMapContent.Any(e => e.OldId == con.OldId.ToString()))
                {
                    newsContentId = IdRelationMapContent.First(e => e.OldId == con.OldId.ToString()).NewId;
                    con.Id = newsContentId;
                }
                else
                {
                    newsContentId = Guid.NewGuid().ToString();

                    IdRelationMapIn.Add(new IdRelationMap
                    {
                        Type = 2,
                        NewId = newsContentId,
                        OldId = con.OldId.ToString()
                    });
                }

            }



            if (conMessage.ex != null)
            {
                throw new JobExecutionException(conMessage.ex);
            }



            //类型转换到entity
            var newsContentList = newsContentBackList.Select(p => new NewsContent
            {
                AuditProcessJson = p.AuditProcessJson,
                AuditStatus = p.AuditStatus,
                Author = p.Author,
                ColumnID = p.ColumnID,
                ColumnIDs = p.ColumnIDs,
                Content = p.Content,
                Cover = p.Cover,
                CreatedTime = p.CreatedTime,
                DeleteFlag = false,
                ExpendFiled1 = p.ExpendFiled1,
                ExpendFiled2 = p.ExpendFiled2,
                ExpendFiled3 = p.ExpendFiled3,
                ExpendFiled4 = p.ExpendFiled4,
                ExpendFiled5 = p.ExpendFiled5,
                ExpirationDate = p.ExpirationDate,
                HitCount = p.HitCount,
                Id = p.Id,
                JumpLink = p.JumpLink,
                Keywords = p.Keywords,
                OrderNum = p.OrderNum,
                ParentCatalogue = p.ParentCatalogue,
                PublishDate = p.PublishDate,
                Publisher = p.Publisher,
                PublisherName = p.PublisherName,
                Status = p.Status,
                SubTitle = p.SubTitle,
                Terminals = p.Terminals,
                Title = p.Title,
                TitleStyle = p.TitleStyle,
                UpdatedTime = p.UpdatedTime.HasValue ? p.UpdatedTime : DateTimeOffset.MinValue
            }).ToList();

            var newsColumnList = newsColumnBackList.Select(p => new NewsColumn
            {
                Id = p.Id,
                Title = p.Title,
                Alias = p.Alias,
                Label = p.Label,
                Terminals = p.Terminals,
                Status = p.Status,
                Extension = p.Extension,
                LinkUrl = p.LinkUrl,
                DefaultTemplate = p.DefaultTemplate,
                SideList = p.SideList,
                SysMesList = p.SysMesList,
                IsOpenCover = p.IsOpenCover,
                VisitingList = p.VisitingList,
                IsOpenComment = p.IsOpenComment,
                IsOpenAudit = p.IsOpenAudit,
                AuditFlow = p.AuditFlow,
                DeleteFlag = p.DeleteFlag,
                CreatedTime = p.CreatedTime,
                UpdatedTime = p.UpdatedTime.HasValue ? p.UpdatedTime : DateTimeOffset.MinValue,
                FootTemplate = p.FootTemplate,
                HeadTemplate = p.HeadTemplate,

            }).ToList();

            //组织权限列表
            var newsSysPermissionList = GetDataOperater<INewsDataAdapter>().GetSysPermissionList(newsColumnList, TenantDb, out colMessage);
            if (colMessage.ex != null)
            {
                throw new JobExecutionException(colMessage.ex);
            }

            var newsColumnPermissionList = newsColumnPermissionBackList.Select(p => new NewsColumnPermissions
            {
                ColumnID = p.ColumnID,
                Id = p.Id,
                ManagerID = p.ManagerID,
                CreatedTime = p.CreatedTime,
                DeleteFlag = p.DeleteFlag,
                Manager = p.Manager,
                Permission = p.Permission

            }).ToList();

/*
            //首先同步到back表中做记录
            using (var tran = db.UseTran())
            {
                try
                {
                    int curCount = 0;//当前已导入的数据
                    var newsContentBackListCount = newsContentBackList.Count;
                    while (curCount < newsContentBackListCount)
                    {

                        var curList = newsContentBackList.Skip(curCount).Take(100).ToList();
                        db.Insertable(curList).ExecuteCommand();
                        WriteLog($"{curCount}-{curCount + curList.Count}导入成功");
                        curCount += curList.Count;
                    }
              //      db.Insertable(newsColumnBackList).ExecuteCommand();
                    //   db.Insertable(newsColumnPermissionBackList).ExecuteCommand();
                    db.Insertable(IdRelationMapIn).ExecuteCommand();


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
                    var newsContentListCount = newsContentList.Count;
                    while (curCount < newsContentListCount)
                    {

                        var curList = newsContentList.Skip(curCount).Take(100).ToList();
                        db.Insertable(curList).ExecuteCommand();
                        WriteLog($"{curCount}-{curCount + curList.Count}导入成功");
                        curCount += curList.Count;
                    }

                  //  db.Insertable(newsColumnList).ExecuteCommand();
                   // db.Insertable(newsColumnPermissionList).ExecuteCommand();
                 //   db.Insertable(newsSysPermissionList).ExecuteCommand();

                    tran.CommitTran();
                }
                catch (Exception ex)
                {
                    tran.RollbackTran();
                }

            }
*/


            var insertContentCount = newsContentBackList.Count;
            var insertColumnCount = newsColumnBackList.Count;
            WriteLog($"新闻内容同步完成,新增{insertContentCount}条");
            WriteLog($"新闻栏目同步完成,新增{insertColumnCount}条");
            WriteLog($"新闻栏目权限同步完成,新增{newsColumnPermissionList.Count}条");
            WriteLog($"新闻权限项同步完成,新增{newsSysPermissionList.Count}条");

            


            var esNewsList = newsContentBackList.Select(p => new UpsertOwnerNewsRequestParameter
            {
                app_id = "news",
                app_type = SmartLibrary.Search.EsSearchProxy.Core.Models.OrganNewsType.News,
                click_count = p.HitCount,
                docId = $"news_cqu_{p.Id.Replace('-', '_')}",
                fulltext = HTMLFilter.StripHTML(p.Content),
                keyword = (p.Keywords ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries),
                owner = "cqu",
                pub_time = p.CreatedTime,
                summary = HTMLFilter.StripHTML(p.Content).Substring(0, Math.Min(HTMLFilter.StripHTML(p.Content).Length, 256)),
                title = p.Title,
                update_time = p.UpdatedTime.HasValue ? p.UpdatedTime.Value : DateTimeOffset.MinValue,
                url = $"/#/web_newsDetails?id={p.Id}&cid={p.ColumnID}",
                
                
                
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
