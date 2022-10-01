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
    public class NavDataAdapter : INavDataAdapter
    {

        private readonly JObject _adapterParamDic;
        private readonly SqlSugarClient _tenantDb;
        private readonly string totalSql = "SELECT COUNT(1) FROM [ContentInfo] WHERE DeleteFlag = 0 AND ItemType = 0 AND PlateID = 60 AND(Status= 1 OR Status = 6 OR Status = 9) ";
        private readonly string navColumnSql = @"
--导航栏目
SELECT TOP 1000 ID as Id
,PlateName AS Title
,'' As Label
,Status As Status
,'' As LinkUrl
,'1' As DefaultTemplate
,'' as ColumnIcon
,'0' As SideList
,'0' As SysMesList
,Height as CoverHeight
,Width  As CoverWidth
,'0' As IsLoginAcess
,'' As UserTypes
,'0' As IsOpenFeedBack
,'0' As DeleteFlag
,NULL As TenantId
,CreateTime As CreatedTime
,NULL As UpdatedTime
,'' As FootTemplate
,'' As HeadTemplate
,ID As OldId
FROM [dbo].[ContentPlate] 

WHERE ContentType=1 
and DeleteFlag=0
";
        private readonly string navContentSql = @"SELECT  PlateID AS ColumnID
,ID as Id
,PlateID AS OldColumnId
,ParentId as CatalogueID
,'' as RelationCatalogueIDs
,Content as Contents
,'' as ContentsText
,'' as Creator
, OutUrl as LinkUrl
,ClickNum as HitCount
,Createtime as PublishData
,0 as DeleteFlag
,Publisher as Publisher
,Status as Status
,SubTitle as SubTitle
,Title as Title
,'' as TitleStyle
,Createtime AS CreatedTime
,Updatetime AS UpdatedTime
,ID as OldId
,OrderIndex as SortIndex
FROM
	[ContentInfo] 
WHERE
	DeleteFlag = 0 
	AND ItemType = 0 
	AND PlateID IN ({0})
	AND Status = 1";
        private readonly string navCatalogSql = @"
SELECT  PlateID AS ColumnID
,[Level]
,Title as Title
,'' as Alias 
,ParentId as ParentId
,Path as PathCode
,1 as NavigationType
,'' as AssociatedCatalog
,OutUrl as ExterbalLinks
,0 as IsOpenNewWindow
,'' as Cover
,1 as Status
,OrderIndex as SortIndex
,'' as Creator
,0 as DeleteFlag
,Createtime AS CreatedTime
,Updatetime AS UpdatedTime
,'' as CreatorName
,'' as TitleStyle
,ID as OldId
FROM
	[ContentInfo] 
WHERE
	DeleteFlag = 0 
	AND ItemType = 1
	AND PlateID IN ({0})
	AND Status = 1
";
        public NavDataAdapter(string adapterParamStr, SqlSugarClient tenantDb)
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
        public List<ContentBack> GetNavContentList(List<string> columnIds, out MessageHand message)
        {
            var navList = new List<ContentBack>();
            message = new MessageHand { Code = CODE.SUCCED, Context = typeof(NavDataAdapter).FullName.ToString() };
            try
            {

                WriteLog("-----------------开始获取新闻数据--------------------");
                //Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 202.202.12.5)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl))); User ID = cdstbk; Password = GqmO1uNo;

                var sql = string.Format(navContentSql, string.Join(',', columnIds));
                sql = sql + "And Createtime>" + ConfigHelper.TaskConfig.GetSection("Appsettings:newsContentAdaptStartTime").Value;
                using (var db = DataHelper.GetInstance(_adapterParamDic["NavConn"].ToString(), (int)DbType.SqlServer))
                {
#if (DEBUG)
                    WriteLog("NavContentSql:" + sql);
#endif
                    var dt = db.Ado.GetDataTable(sql);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return navList;
                    }
                    WriteLog("条数:" + dt.Rows.Count);

                    var list = dt.DtToList<ContentBackDto>().Select(p => new ContentBack
                    {

                        Id = Guid.NewGuid().ToString(),
                        CatalogueID = p.CatalogueID,
                        RelationCatalogueIDs = p.RelationCatalogueIDs,
                        Contents = p.Contents,
                        ContentsText = StringUtils.FilterHTML(p.Contents),
                        Creator = p.Creator,
                        CreatorName = p.CreatorName,
                        LinkUrl = p.LinkUrl,
                        HitCount = p.HitCount,
                        PublishDate = p.PublishDate,
                        DeleteFlag = p.DeleteFlag,
                        Publisher = p.Publisher,
                        Status = p.Status,
                        SubTitle = p.SubTitle,
                        SortIndex = p.SortIndex,
                        Title = p.Title,
                        TitleStyle = p.TitleStyle,
                        CreatedTime = DateTimeOffset.UtcNow,
                        UpdatedTime = null,
                        OldId = p.OldId,
                        OldColumnId=p.OldColumnId
                    }).ToList();

                    WriteLog(" list.Count:" + list.Count);

                    foreach (var item in list)
                    {
                        item.Id = Time2KeyUtils.GetRandOnlyId();
                        navList.Add(item);
                    }
                }

                return navList;
            }
            catch (Exception ex)
            {
                message.ex = ex;
                return navList;
            }
        }

        public List<NavigationCatalogueBack> GetNavCatalogueList(List<string> columnIds, out MessageHand message)
        {
            var navList = new List<NavigationCatalogueBack>();
            message = new MessageHand { Code = CODE.SUCCED, Context = typeof(NavDataAdapter).FullName.ToString() };
            try
            {

                WriteLog("-----------------开始获取新闻数据--------------------");
                //Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 202.202.12.5)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl))); User ID = cdstbk; Password = GqmO1uNo;

                var sql = string.Format(navCatalogSql, string.Join(',', columnIds));
                sql = sql + "And Createtime>" + ConfigHelper.TaskConfig.GetSection("Appsettings:newsContentAdaptStartTime").Value;
                using (var db = DataHelper.GetInstance(_adapterParamDic["NavConn"].ToString(), (int)DbType.SqlServer))
                {
#if (DEBUG)
                    WriteLog("NavCatalogSql:" + sql);
#endif
                    var dt = db.Ado.GetDataTable(sql);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return navList;
                    }
                    WriteLog("条数:" + dt.Rows.Count);

                    var list = dt.DtToList<NavigationCatalogueBackDto>().Select(p => new NavigationCatalogueBack
                    {

                        Id = Guid.NewGuid().ToString(),
                        ColumnID = p.ColumnID,
                        Title = p.Title,
                        Alias = p.Alias,
                        ParentID = p.ParentID,
                        PathCode = p.PathCode,
                        NavigationType = p.NavigationType,
                        AssociatedCatalog = p.AssociatedCatalog,
                        ExternalLinks = p.ExternalLinks,
                        IsOpenNewWindow = p.IsOpenNewWindow,
                        Cover = p.Cover,
                        Status = p.Status,
                        SortIndex = p.SortIndex,
                        Creator = p.Creator,
                        DeleteFlag = p.DeleteFlag,
                        CreatedTime = p.CreatedTime,
                        UpdatedTime = null,// p.UpdatedTime,
                        CreatorName = p.CreatorName,
                        TitleStyle = p.TitleStyle,
                        OldId = p.OldId,
                        

                    }).ToList();

                    WriteLog(" list.Count:" + list.Count);

                    foreach (var item in list)
                    {
                        item.Id = Time2KeyUtils.GetRandOnlyId();
                        navList.Add(item);
                    }
                }

                return navList;
            }
            catch (Exception ex)
            {
                message.ex = ex;
                return navList;
            }
        }



        /// <summary>
        /// 取新闻对应的栏目数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<NavigationColumnBack> GetNavColumnList(out MessageHand message)
        {
            var navColumnList = new List<NavigationColumnBack>();
            message = new MessageHand { Code = CODE.SUCCED, Context = typeof(NavDataAdapter).FullName.ToString() };
            try
            {

                WriteLog("-----------------开始获取新闻数据--------------------");
                //Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 202.202.12.5)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl))); User ID = cdstbk; Password = GqmO1uNo;
                using (var db = DataHelper.GetInstance(_adapterParamDic["NavConn"].ToString(), (int)DbType.SqlServer))
                {
#if (DEBUG)
                    WriteLog("NavigationColumnSql:" + navColumnSql);
#endif
                    var dt = db.Ado.GetDataTable(navColumnSql);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return navColumnList;
                    }
                    WriteLog("条数:" + dt.Rows.Count);

                    var list = dt.DtToList<NavigationColumnBackDto>().Select(p => new NavigationColumnBack
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Label = p.Label,
                        Status = p.Status,
                        LinkUrl = p.LinkUrl,
                        DefaultTemplate = p.DefaultTemplate,
                        ColumnIcon = p.ColumnIcon,
                        SideList = p.SideList,
                        SysMesList = p.SysMesList,
                        CoverHeight = p.CoverHeight,
                        CoverWidth = p.CoverWidth,
                        IsLoginAcess = p.IsLoginAcess,
                        UserTypes = p.UserTypes,
                        IsOpenFeedback = p.IsOpenFeedback,
                        DeleteFlag = p.DeleteFlag,
                        UserGroups = p.UserGroups,
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
                        navColumnList.Add(item);
                    }
                }

                return navColumnList;
            }
            catch (Exception ex)
            {
                message.ex = ex;
                return navColumnList;
            }
        }

        /// <summary>
        /// 组织需要获取的权限项目
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<SysMenuPermission> GetSysPermissionList(List<NavigationColumn> columns, SqlSugarClient client, out MessageHand message)
        {
            message = new MessageHand { Code = CODE.SUCCED, Context = typeof(NavDataAdapter).FullName.ToString() };
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

                     perList.AddRange (new List<SysMenuPermission>
                    {
                     new SysMenuPermission { Id = Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow, Pid = "1-1-1", Path = maxPathID.ToString(), FullPath = $"1-1-1-{maxPathID}", Name = "更新信息导航栏目API", Type = 5, Router = "路由地址", Component = "组件地址", Permission = $"api:navigation:navigation-column-update:{columnID}", Remark = "更新信息导航栏目API", IsSysMenu = false,Visible=true },
                     new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid="1-1-2",Path = maxPathID.ToString(), FullPath = $"1-1-2-{maxPathID}", Name="删除信息导航栏目API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-column-delete:{columnID}",Remark="删除信息导航栏目API",IsSysMenu = false,Visible=true },
                     new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid="1-1-4",Path=maxPathID.ToString(),FullPath=$"1-1-4-{maxPathID}", Name="获取信息导航栏目API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-column-get:{columnID}",Remark="获取信息导航栏目API",IsSysMenu = false,Visible=true },

               });

                    perList.AddRange(
                        new List<SysMenuPermission>
                        {
                            new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid="1",Path=maxPathID.ToString(),FullPath=parentFullPath, Name=columnName,Type=1,Router="路由地址",Component=$"/admin_programInfo?id={columnID}",Permission=$"column:{columnID}",Remark=columnName,IsSysMenu = false,Visible=true,Sort=maxPathID },
                     //*-1 内容管理
                     new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=parentFullPath,Path="1",FullPath=$"{parentFullPath}-1", Name="内容管理",Type=1,Router="路由地址",Component="组件地址",Permission=$"content:{columnID}",Remark="内容管理",IsSysMenu = false,Visible=true },
                     //*-1-1 新增
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1",Path="1",FullPath=$"{parentFullPath}-1-1", Name="新增",Type=4,Router="路由地址",Component="组件地址",Permission="add",Remark="新增",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1-1",Path="1",FullPath=$"{parentFullPath}-1-1-1", Name="添加信息导航内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-add:{columnID}",Remark="添加信息导航内容API",IsSysMenu = false,Visible=true },
                      //*-1-2 上下架
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1",Path="2",FullPath=$"{parentFullPath}-1-2", Name="上下架",Type=4,Router="路由地址",Component="组件地址",Permission="onoffshelf",Remark="上下架",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1-2",Path="1",FullPath=$"{parentFullPath}-1-2-1", Name="批量下架/上架内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:change-content-status:{columnID}",Remark="批量下架/上架内容API",IsSysMenu = false,Visible=true },
                      //*-1-3 删除
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1",Path="3",FullPath=$"{parentFullPath}-1-3", Name="删除",Type=4,Router="路由地址",Component="组件地址",Permission="delete",Remark="删除",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1-3",Path="1",FullPath=$"{parentFullPath}-1-3-1", Name="批量删除内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-delete:{columnID}",Remark="批量删除内容API",IsSysMenu = false,Visible=true },
                      //*-1-4 编辑
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1",Path="4",FullPath=$"{parentFullPath}-1-4", Name="编辑",Type=4,Router="路由地址",Component="组件地址",Permission="edit",Remark="编辑",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1-4",Path="1",FullPath=$"{parentFullPath}-1-4-1", Name="修改信息导航内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-update:{columnID}",Remark="修改信息导航内容API",IsSysMenu = false,Visible=true },
                      //*-1-5 排序
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1",Path="5",FullPath=$"{parentFullPath}-1-5", Name="排序",Type=4,Router="路由地址",Component="组件地址",Permission="sort",Remark="排序",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1-5",Path="1",FullPath=$"{parentFullPath}-1-5-1", Name="内容弹窗输入序号排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:sort-content-by-index:{columnID}",Remark="内容弹窗输入序号排序API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1-5",Path="2",FullPath=$"{parentFullPath}-1-5-2", Name="内容拖动排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:sort-content-by-target:{columnID}",Remark="内容拖动排序API",IsSysMenu = false,Visible=true },
                      //*-1-6 查询
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1",Path="6",FullPath=$"{parentFullPath}-1-6", Name="查询",Type=3,Router="路由地址",Component="组件地址",Permission="query",Remark="查询",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1-6",Path="1",FullPath=$"{parentFullPath}-1-6-1", Name="获取信息导航内容列表API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-list:{columnID}",Remark="获取信息导航内容列表API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1-6",Path="2",FullPath=$"{parentFullPath}-1-6-2", Name="获取信息导航内容API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content:{columnID}",Remark="获取信息导航内容API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-1-6",Path="3",FullPath=$"{parentFullPath}-1-6-3", Name="获取内容操作日志API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:content-process-log-get:{columnID}",Remark="获取内容操作日志API",IsSysMenu = false,Visible=true },

                      //*-1 目录管理
                     new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=parentFullPath,Path="2",FullPath=$"{parentFullPath}-2", Name="目录管理",Type=1,Router="路由地址",Component="组件地址",Permission=$"catalogue:{columnID}",Remark="目录管理",IsSysMenu = false,Visible=true },
                     //*-2-1 新增
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2",Path="1",FullPath=$"{parentFullPath}-2-1", Name="新增",Type=4,Router="路由地址",Component="组件地址",Permission="add",Remark="新增",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-1",Path="1",FullPath=$"{parentFullPath}-2-1-1", Name="添加信息导航目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-add:{columnID}",Remark="添加信息导航目录API",IsSysMenu = false,Visible=true },
                      //*-2-2 上下架
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2",Path="2",FullPath=$"{parentFullPath}-2-2", Name="上下架",Type=4,Router="路由地址",Component="组件地址",Permission="onoffshelf",Remark="上下架",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-2",Path="1",FullPath=$"{parentFullPath}-2-2-1", Name="批量下架/上架目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:change-navigation-catalogue-status:{columnID}",Remark="批量下架/上架目录API",IsSysMenu = false,Visible=true },
                      //*-2-3 删除
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2",Path="3",FullPath=$"{parentFullPath}-2-3", Name="删除",Type=4,Router="路由地址",Component="组件地址",Permission="delete",Remark="删除",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-3",Path="1",FullPath=$"{parentFullPath}-2-3-1", Name="批量删除目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-delete:{columnID}",Remark="批量删除目录API",IsSysMenu = false,Visible=true },
                      //*-2-4 编辑
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2",Path="4",FullPath=$"{parentFullPath}-2-4", Name="编辑",Type=4,Router="路由地址",Component="组件地址",Permission="edit",Remark="编辑",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-4",Path="1",FullPath=$"{parentFullPath}-2-4-1", Name="修改信息导航目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-update:{columnID}",Remark="修改信息导航目录API",IsSysMenu = false,Visible=true },
                      //*-2-5 排序
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2",Path="5",FullPath=$"{parentFullPath}-2-5", Name="排序",Type=4,Router="路由地址",Component="组件地址",Permission="sort",Remark="排序",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-5",Path="1",FullPath=$"{parentFullPath}-2-5-1", Name="目录弹窗输入序号排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:sort-catalogue-by-index:{columnID}",Remark="目录弹窗输入序号排序API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-5",Path="2",FullPath=$"{parentFullPath}-2-5-2", Name="目录拖动排序API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:sort-catalogue-by-target:{columnID}",Remark="目录拖动排序API",IsSysMenu = false,Visible=true },
                      //*-2-6 查询
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2",Path="6",FullPath=$"{parentFullPath}-2-6", Name="查询",Type=3,Router="路由地址",Component="组件地址",Permission="query",Remark="查询",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-6",Path="1",FullPath=$"{parentFullPath}-2-6-1", Name="获取信息导航目录列表API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-list-get:{columnID}",Remark="获取信息导航目录列表API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-6",Path="2",FullPath=$"{parentFullPath}-2-6-2", Name="获取信息导航目录API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:navigation-catalogue-get:{columnID}",Remark="获取信息导航目录API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-6",Path="3",FullPath=$"{parentFullPath}-2-6-3", Name="获取目录操作日志API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:catalogue-process-log-get:{columnID}",Remark="获取目录操作日志API",IsSysMenu = false,Visible=true },
                      new SysMenuPermission{Id=Guid.NewGuid(),CreatedTime=DateTimeOffset.UtcNow,Pid=$"{parentFullPath}-2-6",Path="4",FullPath=$"{parentFullPath}-2-6-4", Name="获取全部信息导航目录树API",Type=5,Router="路由地址",Component="组件地址",Permission=$"api:navigation:all-navigation-catalogue-process-tree-list-get:{columnID}",Remark="获取全部信息导航目录树API",IsSysMenu = false,Visible=true },
                        }

                        );


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
            LogManager.Log(string.Format("Adapter_{0}", nameof(NavDataAdapter)), msg);
        }
    }
}
