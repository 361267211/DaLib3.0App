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
using System.Diagnostics;

namespace TaskManager.Tasks.Job
{
    /// <summary>
    /// 同步新闻服务
    /// </summary>
    public class SyncNavJob : SmartJobBase
    {
        public override string JobName => "同步信息导航服务";
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
                WriteLog($"{nameof(SyncNavJob)}获取AccessToken失败");
                return;
            }

            //网关地址后期需要替换为根据应用编码动态获取
            var fabioUrl = SchedulerCenter.SmartTaskConfig["FabioUrl"].ToString();
            if (string.IsNullOrWhiteSpace(fabioUrl))
            {
                WriteLog($"{nameof(SyncNavJob)}获取Fabio网关地址失败");
                return;
            }

            var grpcClientResolver = serviceProvider.GetRequiredService<IGrpcClientResolver>();
            var grpcClient = grpcClientResolver.EnsureClient<AppCenterGrpcService.AppCenterGrpcServiceClient>(fabioUrl, accessToken);

            AppUserAuthRequest request = new AppUserAuthRequest
            {
                AppId = "navigation",
            };
            AppUserAuthReply reply = await grpcClient.GetAppUserAuthListAsync(request);
            //提取首个管理员的userid
            var managerId = reply.AppUserAuthList.FirstOrDefault(e => e.PermissionType == 1)?.UserId;

            var db = TenantDb;


            var paramsDto = JsonConvert.DeserializeObject<SyncNewsParamsDto>(this.TaskParams);
            if (paramsDto == null)
            {
                WriteLog($"{nameof(SyncNavJob)}参数解析错误");
                throw new Exception($"{nameof(SyncNavJob)}参数解析错误");

            }





            //从应用中心提取首个管理员
            //var newsAppManager = GetDataOperater<INewsDataAdapter>().GetNewsManager(out MessageHand colMessage);

            //提取重大线上的新闻栏目-ContentPlate
            var navColumnBackList = GetDataOperater<INavDataAdapter>().GetNavColumnList(out MessageHand colMessage);
            if (colMessage.ex != null)
            {
                throw new JobExecutionException(colMessage.ex);
            }

            var columnIds = navColumnBackList.Select(e => e.Id).ToList();

            //提取重大线上的新闻数据-ContentInfo
            var navContentBackList = GetDataOperater<INavDataAdapter>().GetNavContentList(columnIds, out MessageHand conMessage);
            if (conMessage.ex != null)
            {
                throw new JobExecutionException(conMessage.ex);
            }

            var navCatalogueList = GetDataOperater<INavDataAdapter>().GetNavCatalogueList(columnIds, out MessageHand cataMessage);
            if (cataMessage.ex != null)
            {
                throw new JobExecutionException(cataMessage.ex);
            }

            var navColumnPermissionBackList = new List<NavigationColumnPermissionsBack>();

            //取出栏目的新老id的对应数据
            var IdRelationMapColumn = TenantDb.Queryable<IdRelationMap>().Where(e => e.Type == 1).ToList();

            //取出内容的新老id的对应数据
            var IdRelationMapContent = TenantDb.Queryable<IdRelationMap>().Where(e => e.Type == 2).ToList();

            //取出目录的新老id的对应数据
            var IdRelationMapCatalogue = TenantDb.Queryable<IdRelationMap>().Where(e => e.Type == 3).ToList();

            //需要存入的新老id的对应数据
            var IdRelationMapIn = new List<IdRelationMap>();

            //新老id关系的全集
            var relationMaps = new List<IdRelationMap>();




            //栏目的id处理
            foreach (var col in navColumnBackList)
            {


                var newColumnId = "";

                var relate = IdRelationMapColumn.FirstOrDefault(e => e.OldId == col.OldId.ToString());

                //匹配历史id
                if (relate != null)
                {
                    newColumnId = IdRelationMapColumn.First(e => e.OldId == col.OldId.ToString() && e.Type == 1).NewId;

                    relationMaps.Add(new IdRelationMap
                    {
                        NewId = relate.NewId,
                        OldId = relate.OldId,
                        Type = 1
                    });
                }
                else
                {
                    newColumnId = Guid.NewGuid().ToString();

                    IdRelationMapIn.Add(new IdRelationMap
                    {
                        Type = 1,
                        NewId = newColumnId,
                        OldId = col.OldId.ToString()
                    });
                }


                foreach (var cata in navCatalogueList)
                {
                    //添加栏目id
                    if (cata.ColumnID == col.Id)
                    {
                        cata.ColumnID = newColumnId;

                    }




                }


                col.Id = newColumnId;
                //对应的栏目权限
                navColumnPermissionBackList.Add(new NavigationColumnPermissionsBack
                {
                    ColumnID = col.Id,
                    Id = Time2KeyUtils.GetRandOnlyId(),
                    ManagerID = managerId,
                    CreatedTime = DateTimeOffset.UtcNow,
                    DeleteFlag = false,
                    Manager = managerId,
                    Permission = 0
                });



                //添加一个新的目录，用于存放2.2中无目录的导航内容
                navCatalogueList.Add(new NavigationCatalogueBack
                {
                    Id = "",
                    IsOpenNewWindow = true,
                    ColumnID = newColumnId,
                    OldId = 10000 + col.OldId,
                    ParentID = "0",
                    Alias = "栏目直连目录",
                    AssociatedCatalog = "",
                    Cover = "",
                    CreatedTime = DateTimeOffset.UtcNow,
                    Creator = managerId,
                    CreatorName = managerId,
                    DeleteFlag = false,
                    ExternalLinks = "",
                    Level = 0,
                    NavigationType = 1,
                    PathCode = "",
                    Status = true,
                    Title = "栏目直连目录",
                    TitleStyle = "",
                });
            }
            //导航内容的id处理
            foreach (var con in navContentBackList)
            {
                var newsContentId = "";

                if (con.CatalogueID == "0")
                {
                    con.CatalogueID = (10000 + con.OldColumnId).ToString();
                }

                con.Publisher = con.Publisher ?? managerId;
                //匹配历史id
                var relate = IdRelationMapContent.FirstOrDefault(e => e.OldId == con.OldId.ToString());
                if (relate != null)
                {
                    newsContentId = IdRelationMapContent.First(e => e.OldId == con.OldId.ToString() && e.Type == 2).NewId;
                    relationMaps.Add(new IdRelationMap
                    {
                        NewId = relate.NewId,
                        OldId = relate.OldId,
                        Type = 2
                    });
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

                    relationMaps.Add(new IdRelationMap
                    {
                        Type = 2,
                        NewId = newsContentId,
                        OldId = con.OldId.ToString()
                    });
                }
            }
            //目录的id处理
            foreach (var cata in navCatalogueList)
            {
                var newsCatalogId = "";
                var relate = IdRelationMapCatalogue.FirstOrDefault(e => e.OldId == cata.OldId.ToString());

                //匹配历史id
                if (relate != null)
                {
                    newsCatalogId = relate.NewId;
                    relationMaps.Add(new IdRelationMap
                    {
                        NewId = relate.NewId,
                        OldId = relate.OldId.ToString(),
                        Type = 3
                    });
                }
                else
                {
                    newsCatalogId = Guid.NewGuid().ToString();

                    IdRelationMapIn.Add(new IdRelationMap
                    {
                        Type = 3,
                        NewId = newsCatalogId,
                        OldId = cata.OldId.ToString()
                    });

                    relationMaps.Add(new IdRelationMap
                    {
                        Type = 3,
                        NewId = newsCatalogId,
                        OldId = cata.OldId.ToString()
                    });
                }
                cata.Id = newsCatalogId;
            }







            //为目录的上级目录id，pathCode 进行转换
            foreach (var cata in navCatalogueList)
            {
                if (cata.ParentID != "0" && !string.IsNullOrEmpty(cata.ParentID))
                {
                    cata.ParentID = relationMaps.FirstOrDefault(e => e.OldId == cata.ParentID && e.Type == 3)?.NewId;
                    var paths = cata.PathCode.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var pathCode = new List<string>();
                    foreach (var item in paths)
                    {
                        var code = relationMaps.FirstOrDefault(e => e.OldId == item && e.Type == 3).NewId;

                        pathCode.Add(code);
                    }
                    if (pathCode.Count > 0)
                    {
                        cata.PathCode = string.Join('_', pathCode);
                    }
                }
            }

            //为导航添加目录id/栏目id
            foreach (var con in navContentBackList)
            {
                if (!string.IsNullOrEmpty(con.CatalogueID))
                {
                    var tt = con.CatalogueID;

                    if (con.CatalogueID != "0")
                    {
                        con.CatalogueID = relationMaps.FirstOrDefault(e => e.OldId == con.CatalogueID && e.Type == 3).NewId;
                    }

                    if (relationMaps.Any(e => e.OldId == con.OldId.ToString() && e.Type == 2))
                    {
                        con.Id = relationMaps.FirstOrDefault(e => e.OldId == con.OldId.ToString() && e.Type == 2).NewId;

                    }


                }
                if (con.OldColumnId != 0)
                {
                    con.NewColumnId = relationMaps.FirstOrDefault(e => e.OldId == con.OldColumnId.ToString() && e.Type == 1)?.NewId;
                }
            }

            //所有back数据转换到entity
            var navContentList = navContentBackList.Select(p => new Content
            {
                Id = p.Id,
                Title = p.Title,
                SubTitle = p.SubTitle,
                CatalogueID = p.CatalogueID,
                RelationCatalogueIDs = p.RelationCatalogueIDs,
                Contents = p.Contents,
                ContentsText = HTMLFilter.StripHTML(p.Contents),
                LinkUrl = p.LinkUrl,
                Publisher = p.Publisher,
                PublishDate = p.PublishDate,
                Status = p.Status,
                SortIndex = p.SortIndex,
                DeleteFlag = p.DeleteFlag,
                CreatedTime = p.CreatedTime,
                UpdatedTime = p.UpdatedTime,
                TitleStyle = p.TitleStyle,
                HitCount = p.HitCount,
                Creator = p.Creator,
                CreatorName = p.CreatorName,

            }).ToList();
            var navcataList = navCatalogueList.Select(p => new NavigationCatalogue
            {
                Id = p.Id,
                IsOpenNewWindow = p.IsOpenNewWindow,
                ColumnID = p.ColumnID,
                ParentID = p.ParentID,
                SortIndex = p.SortIndex,
                Alias = p.Alias,
                AssociatedCatalog = p.AssociatedCatalog,
                Cover = p.Cover,
                CreatedTime = p.CreatedTime,
                Creator = p.Creator,
                CreatorName = p.CreatorName,
                DeleteFlag = p.DeleteFlag,
                ExternalLinks = p.ExternalLinks,
                NavigationType = p.NavigationType,
                PathCode = p.PathCode,
                Status = p.Status,
                Title = p.Title,
                TitleStyle = p.TitleStyle,
                UpdatedTime = p.UpdatedTime,




            }).ToList();
            var navColumnList = navColumnBackList.Select(p => new NavigationColumn
            {
                Id = p.Id,
                Title = p.Title,
                IsLoginAcess = p.IsLoginAcess,
                IsOpenFeedback = p.IsOpenFeedback,
                ColumnIcon = p.ColumnIcon,
                CoverHeight = p.CoverHeight,
                CoverWidth = p.CoverWidth,
                SideList = p.SideList,
                UserGroups = p.UserGroups,
                UserTypes = p.UserTypes,
                Label = p.Label,
                Status = p.Status,
                LinkUrl = p.LinkUrl,
                DefaultTemplate = p.DefaultTemplate,
                SysMesList = p.SysMesList,
                DeleteFlag = p.DeleteFlag,
                CreatedTime = p.CreatedTime,
                UpdatedTime = p.UpdatedTime.HasValue ? p.UpdatedTime : DateTimeOffset.MinValue,
                FootTemplate = p.FootTemplate,
                HeadTemplate = p.HeadTemplate,

            }).ToList();

            //栏目权限表
            var navSysPermissionList = GetDataOperater<INavDataAdapter>().GetSysPermissionList(navColumnList, TenantDb, out colMessage);
            if (colMessage.ex != null)
            {
                throw new JobExecutionException(colMessage.ex);
            }

            //栏目权限表
            var newsColumnPermissionList = navColumnPermissionBackList.Select(p => new NavigationColumnPermissions
            {
                ColumnID = p.ColumnID,
                Id = p.Id,
                ManagerID = p.ManagerID,
                CreatedTime = p.CreatedTime,
                DeleteFlag = p.DeleteFlag,
                Manager = p.Manager,
                Permission = p.Permission

            }).ToList();



            /*            //首先同步到back表中做记录
                        using (var tran = db.UseTran())
                        {
                            try
                            {
                                int curCount = 0;//当前已导入的数据
                                var navContentBackListCount = navContentBackList.Count;
                                while (curCount < navContentBackListCount)
                                {

                                    var curList = navContentBackList.Skip(curCount).Take(100).ToList();
                                    db.Insertable(curList).ExecuteCommand();
                                    WriteLog($"{curCount}-{curCount + curList.Count}导入成功");
                                    curCount += curList.Count;
                                }
                                db.Insertable(navColumnBackList).ExecuteCommand();
                                db.Insertable(navCatalogueList).ExecuteCommand();
                                db.Insertable(navColumnPermissionBackList).ExecuteCommand();
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
                                var newsContentListCount = navContentList.Count;
                                while (curCount < newsContentListCount)
                                {

                                    var curList = navContentList.Skip(curCount).Take(100).ToList();
                                    db.Insertable(curList).ExecuteCommand();
                                    WriteLog($"{curCount}-{curCount + curList.Count}导入成功");
                                    curCount += curList.Count;
                                }
                                db.Insertable(navcataList).ExecuteCommand();
                                db.Insertable(navColumnList).ExecuteCommand();
                                db.Insertable(newsColumnPermissionList).ExecuteCommand();
                                db.Insertable(navSysPermissionList).ExecuteCommand();

                                tran.CommitTran();
                            }
                            catch (Exception ex)
                            {
                                tran.RollbackTran();
                            }

                        }*/

            var insertContentCount = navContentBackList.Count;
            var insertColumnCount = navColumnBackList.Count;
            WriteLog($"新闻内容同步完成,新增{insertContentCount}条");
            WriteLog($"新闻栏目同步完成,新增{insertColumnCount}条");
            WriteLog($"新闻栏目权限同步完成,新增{newsColumnPermissionList.Count}条");
            WriteLog($"新闻权限项同步完成,新增{navSysPermissionList.Count}条");


            //信息导航内容的同步
            var esNewsList = navContentBackList.Select(p => new UpsertOwnerNewsRequestParameter
            {
                app_id = "navigation",
                app_type = SmartLibrary.Search.EsSearchProxy.Core.Models.OrganNewsType.Service,
                click_count = p.HitCount,
                docId = $"navigation_cqu_{p.Id.Replace('-', '_')}",
                fulltext = p.Title,
                keyword = new string[] { },
                owner = "cqu",
                pub_time = p.CreatedTime,
                summary = p.Title,
                title = p.Title,
                update_time = p.UpdatedTime.HasValue ? p.UpdatedTime.Value : DateTimeOffset.MinValue,
                url = $"/#/web_detailspage?id={p.Id}&c_id={p.NewColumnId}"
            }).ToList();

            var esProxy = serviceProvider.GetService<IEsProxyService>();


            var tst = await esProxy.UpsertOrganNewsAsync(esNewsList.FirstOrDefault(e=> e.title.Contains("账号设置")));

            foreach (var item in esNewsList)
            {
                var result = await esProxy.UpsertOrganNewsAsync(item);
                Trace.WriteLine($"{result.Code}:{result.Message}");
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
