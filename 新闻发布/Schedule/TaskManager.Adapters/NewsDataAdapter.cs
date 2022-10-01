/*********************************************************
 * 名    称：NewsDataAdapter
 * 作    者：刘启平
 * 联系方式：电话[13627622058],邮件[83379242@qq.com]
 * 创建时间：2021/12/7 18:23:36
 * 描    述：新闻数据适配器
 *
 * 更新历史：
 *
 * *******************************************************/
using Newtonsoft.Json.Linq;
using Scheduler.Service;
using Scheduler.Service.Entity;
using Scheduler.Service.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Util;
using TaskManager.Model.Dtos;
using TaskManager.Model.Entities;
using TaskManager.Model.Standard;

namespace TaskManager.Adapters
{
    public class NewsDataAdapter : INewsDataAdapter
    {

        private readonly JObject _adapterParamDic;
        private readonly SqlSugarClient _tenantDb;
        private readonly string totalSql = "SELECT COUNT(1) FROM [ContentInfo] WHERE DeleteFlag = 0 AND ItemType = 0 AND PlateID = 60 AND(Status= 1 OR Status = 6 OR Status = 9) ";
        private readonly string newsColumnSql = @"
SELECT TOP 100 ID as Id
,PlateName AS Title
,PlateName AS Alias 
,'' As Label
,'' As Terminal
,Status As Status
,'' As Extension
,'' As LinkUrl
,'1' As DefaultTemplate
,'' As SideList
,'' As SysMesList
,'0' As IsOpenCover
, cast(Height as varchar)  +'x'+  cast(Width as varchar)   As CoverSize
,'0' As IsLoginAcess
,'' As VisitingList
,'0' As IsOpenComment
,'0' As IsOpenAudit
,'0;8' As AuditFlow
,'0' As DeleteFlag
,NULL As TenantId
,CreateTime As CreateTime
,NULL As UpdatedTime
,'' As FootTemplate
,'' As HeadTemplate
,ID As OldId
FROM [dbo].[ContentPlate] 

WHERE ( [PlateSign] = N'XWZX' OR [PlateSign] = N'TZGG' OR [PlateSign] = N'Advs' OR [PlateSign] = N'HDJZ') AND [DeleteFlag] = N'0'
";
        private readonly string newsContentSql = @"SELECT top 500 PlateID AS ColumnID
,PlateID AS ColumnIDs
,[Title]
      ,'' AS TitleStyle	  
      ,[SubTitle]
	  ,'BV9bK6IAbRrELwXb' AS ParentCatalogue
      ,[Content]
      ,[Pic] AS Cover
      ,[Author]
      ,Publisher AS PublisherName
   --   ,'' AS PublisherName
      --,[Createtime] AS PublishDate
      ,'1' AS [Status]
	  ,'1;2;3;4' AS Terminals
	  ,'8' AS [AuditStatus]
      ,[Keywords]
	  ,CONVERT(DATETIME,'2022-12-31 00:00:00',20) AS ExpirationDate
      ,[OutUrl] AS JumpLink
      ,[ClickNum] AS HitCount
      ,[OrderIndex] AS OrderNum
      ,'' AS AuditProcessJson
	  ,'' AS ExpendFiled1
	  ,'' AS ExpendFiled2
      ,'' AS ExpendFiled3
      ,'' AS ExpendFiled4
	  ,'' AS ExpendFiled5
      ,0 AS [DeleteFlag]
      ,NULL AS TenantId
	  ,Createtime AS CreatedTime
	  ,Updatetime AS UpdatedTime
      ,ID As OldId
  FROM [ContentInfo] WHERE DeleteFlag=0 AND ItemType=0 AND PlateID in ({0}) AND (Status=1 OR Status=6 OR Status=9)";

        public NewsDataAdapter(string adapterParamStr, SqlSugarClient tenantDb)
        {
            _tenantDb = tenantDb;
            if (!string.IsNullOrWhiteSpace(adapterParamStr))
            {
                _adapterParamDic = JObject.Parse(adapterParamStr);
            }
            else
            {
                _adapterParamDic = new JObject();
            }
        }

        /// <summary>
        /// 分页获取读者待同步数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<NewsContentBack> GetNewsContentList(List<string> columnIds, out MessageHand message)
        {
            var newsList = new List<NewsContentBack>();
            message = new MessageHand { Code = CODE.SUCCED, Context = typeof(NewsDataAdapter).FullName.ToString() };
            try
            {

                WriteLog("-----------------开始获取新闻数据--------------------");
                //Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 202.202.12.5)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl))); User ID = cdstbk; Password = GqmO1uNo;

                var sql = string.Format(newsContentSql, string.Join(',', columnIds));
                //  sql = sql + "And Createtime>" + ConfigHelper.TaskConfig.GetSection("Appsettings:newsContentAdaptStartTime").Value;
                using (var db = DataHelper.GetInstance(_adapterParamDic["NewsConn"].ToString(), (int)DbType.SqlServer))
                {
#if (DEBUG)
                    WriteLog("NewsContentSql:" + sql);
#endif
                    var startTime = ConfigHelper.TaskConfig.GetSection("Appsettings:newsContentAdaptStartTime").Value;
                    var beginTime = Convert.ToDateTime(startTime);

                    var beginYear = beginTime.Year;

                    while (beginTime < DateTime.Now)
                    {
                        var finSql = sql + $" And Createtime>'{beginTime.ToString("yyyy-MM-dd")}'   And Createtime<='{beginTime.AddYears(1).ToString("yyyy-MM-dd")}'";
                        var dt = db.Ado.GetDataTable(finSql);

                        WriteLog("条数:" + dt.Rows.Count);

                        var list = dt.DtToList<NewsContentBackDto>().Select(p => new NewsContentBack
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
                            SubTitle = p.Title,//2.2不一定又副标题，3.0副标题必填
                            Terminals = p.Terminals,
                            Title = p.Title,
                            TitleStyle = p.TitleStyle,
                            UpdatedTime = p.UpdatedTime.HasValue ? p.UpdatedTime : DateTimeOffset.MinValue,
                            OldId = p.OldId,
                        }).ToList();

                        WriteLog(" list.Count:" + list.Count);

                        foreach (var item in list)
                        {
                            item.Id = Time2KeyUtils.GetRandOnlyId();
                            newsList.Add(item);
                        }

    

                        beginTime = beginTime.AddYears(1);
                    }

                   // var dt = db.Ado.GetDataTable(sql);
             
                }

                return newsList;
            }
            catch (Exception ex)
            {
                message.ex = ex;
                return newsList;
            }
        }

        /// <summary>
        /// 取新闻对应的栏目数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<NewsColumnBack> GetNewsColumnList(out MessageHand message)
        {
            var newsColumnList = new List<NewsColumnBack>();
            message = new MessageHand { Code = CODE.SUCCED, Context = typeof(NewsDataAdapter).FullName.ToString() };
            try
            {

                WriteLog("-----------------开始获取新闻数据--------------------");
                //Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 202.202.12.5)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl))); User ID = cdstbk; Password = GqmO1uNo;
                using (var db = DataHelper.GetInstance(_adapterParamDic["NewsConn"].ToString(), (int)DbType.SqlServer))
                {
#if (DEBUG)
                    WriteLog("NewsColumnSql:" + newsColumnSql);
#endif
                    var dt = db.Ado.GetDataTable(newsColumnSql);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return newsColumnList;
                    }
                    WriteLog("条数:" + dt.Rows.Count);

                    var list = dt.DtToList<NewsColumnBackDto>().Select(p => new NewsColumnBack
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
                        IsOpenAudit = 0,
                        AuditFlow = p.AuditFlow,
                        DeleteFlag = p.DeleteFlag,
                        CreatedTime = p.CreatedTime,
                        UpdatedTime = p.UpdatedTime.HasValue ? p.UpdatedTime : DateTimeOffset.MinValue,
                        FootTemplate = p.FootTemplate,
                        HeadTemplate = p.HeadTemplate,
                        OldId = p.OldId,

                    }).ToList();

                    WriteLog(" list.Count:" + list.Count);

                    foreach (var item in list)
                    {
                        //  item.Id = Time2KeyUtils.GetRandOnlyId();
                        newsColumnList.Add(item);
                    }
                }

                return newsColumnList;
            }
            catch (Exception ex)
            {
                message.ex = ex;
                return newsColumnList;
            }
        }

        /// <summary>
        /// 组织需要获取的权限项目
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<SysMenuPermission> GetSysPermissionList(List<NewsColumn> columns, SqlSugarClient client, out MessageHand message)
        {
            message = new MessageHand { Code = CODE.SUCCED, Context = typeof(NewsDataAdapter).FullName.ToString() };
            List<SysMenuPermission> perList = new List<SysMenuPermission>();
            try
            {

                foreach (var column in columns)
                {
                    #region 新增栏目菜单权限
                    var columnID = column.Id;
                    var columnName = column.Title;

                    var maxPath = client.Queryable<SysMenuPermission>().Where(d => d.Pid == "1").Max(d => d.Path);
                    var maxPathID = int.Parse(maxPath) + 1;
                    var parentFullPath = $"1-{maxPathID}";
                    //栏目管理

                    var perColumn = new List<SysMenuPermission>
                    {
                    new SysMenuPermission { Id = Guid.NewGuid(), Pid = "1-1-1", Path = maxPathID.ToString(), FullPath = $"1-1-1-{maxPathID}", Name = "更新新闻栏目API", Type = 5, Router = "路由地址", Component = "组件地址", Permission = $"api:news:news-column-update:{columnID}", Remark = "更新新闻栏目API", IsSysMenu = false,Visible=true,CreatedTime=DateTimeOffset.UtcNow },
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-2",Path = maxPathID.ToString(), FullPath = $"1-1-2-{maxPathID}", Name="删除新闻栏目API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-column-delete:{columnID}",Remark="删除新闻栏目API",IsSysMenu = false,Visible=true,CreatedTime=DateTimeOffset.UtcNow },
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid="1-1-4",Path=maxPathID.ToString(),FullPath=$"1-1-4-{maxPathID}", Name="获取新闻栏目API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-column-get:{columnID}",Remark="获取新闻栏目API",IsSysMenu = false,Visible=true,CreatedTime=DateTimeOffset.UtcNow },
               };

                    perList.AddRange(perColumn);
                    //当前栏目
                    var perContent =
                        new[]
                        {
                     new SysMenuPermission{Id=Guid.NewGuid(),Pid="1",Path=maxPathID.ToString(),FullPath=parentFullPath, Name=columnName,Type=1,Router="路由地址",Component=$"/admin_programInfo?id={columnID}",Permission=$"column:{columnID}",Remark=columnName,IsSysMenu = false,Visible=true,Sort=maxPathID,CreatedTime=DateTimeOffset.UtcNow },
                     //*-1 新增
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="1",FullPath=$"{parentFullPath}-1", Name="新增",Type=4,Router="路由地址",Component="组件地址",Permission="add",Remark="新增",IsSysMenu = false,Visible=true ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-1",Path="1",FullPath=$"{parentFullPath}-1-1", Name="添加新闻内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-add:{columnID}",Remark="添加新闻内容API",IsSysMenu = false,Visible=true,CreatedTime=DateTimeOffset.UtcNow },
                      //*-2 上下架
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="2",FullPath=$"{parentFullPath}-2", Name="下架",Type=4,Router="路由地址",Component="组件地址",Permission="offshelf",Remark="下架",IsSysMenu = false,Visible=true,CreatedTime=DateTimeOffset.UtcNow },
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-2",Path="1",FullPath=$"{parentFullPath}-2-1", Name="批量下架内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-off-shelf:{columnID}",Remark="批量下架内容API",IsSysMenu = false,Visible=true ,CreatedTime=DateTimeOffset.UtcNow},
                      //*-3 删除
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="3",FullPath=$"{parentFullPath}-3", Name="删除",Type=4,Router="路由地址",Component="组件地址",Permission="delete",Remark="删除",IsSysMenu = false,Visible=true ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-3",Path="1",FullPath=$"{parentFullPath}-3-1", Name="批量删除内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-delete:{columnID}",Remark="批量删除内容API",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      //*-4 编辑
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="4",FullPath=$"{parentFullPath}-4", Name="编辑",Type=4,Router="路由地址",Component="组件地址",Permission="edit",Remark="编辑",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-4",Path="1",FullPath=$"{parentFullPath}-4-1", Name="修改新闻内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-update:{columnID}",Remark="修改新闻内容API",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      //*-5 排序
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="5",FullPath=$"{parentFullPath}-5", Name="排序",Type=4,Router="路由地址",Component="组件地址",Permission="sort",Remark="排序",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-5",Path="1",FullPath=$"{parentFullPath}-5-1", Name="内容弹窗输入序号排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:sort-content-by-index:{columnID}",Remark="内容弹窗输入序号排序API",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-5",Path="2",FullPath=$"{parentFullPath}-5-2", Name="内容拖动排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:sort-content-by-target:{columnID}",Remark="内容拖动排序API",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      //*-6 查询
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="6",FullPath=$"{parentFullPath}-6", Name="查询",Type=3,Router="路由地址",Component="组件地址",Permission="query",Remark="查询",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-6",Path="1",FullPath=$"{parentFullPath}-6-1", Name="获取新闻内容列表API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-get-by-column:{columnID}",Remark="获取新闻内容列表API",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-6",Path="2",FullPath=$"{parentFullPath}-6-2", Name="获取后台新闻详情API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-manage-get:{columnID}",Remark="获取后台新闻详情API",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-6",Path="3",FullPath=$"{parentFullPath}-6-3", Name="获取内容操作日志API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:content-process-log-get:{columnID}",Remark="获取内容操作日志API",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-6",Path="4",FullPath=$"{parentFullPath}-6-4", Name="获取后台新闻栏目状态集合以及新闻内容标签集合API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-column-content-manage-get:{columnID}",Remark="获取后台新闻栏目状态集合以及新闻内容标签集合API",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      //*-7 审核
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}",Path="7",FullPath=$"{parentFullPath}-7", Name="审核",Type=3,Router="路由地址",Component="组件地址",Permission="audit",Remark="审核",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                      new SysMenuPermission{Id=Guid.NewGuid(),Pid=$"{parentFullPath}-7",Path="1",FullPath=$"{parentFullPath}-7-1", Name="更新新闻内容审核状态API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:news:news-content-update-audit-status:{columnID}",Remark="更新新闻内容审核状态API",IsSysMenu = false,Visible=true  ,CreatedTime=DateTimeOffset.UtcNow},
                        };
                    perList.AddRange(perContent);
                    #endregion

                }
                return perList;
            }
            catch (Exception ex)
            {
                message.ex = ex;
                return perList;

            }


        }

        private void WriteLog(string msg)
        {
            LogManager.Log(string.Format("Adapter_{0}", nameof(NewsDataAdapter)), msg);
        }
    }
}
