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
    public class DatabaseGuideDataAdapter : IDatabaseGuideDataAdapter
    {

        private readonly JObject _adapterParamDic;
        private readonly SqlSugarClient _tenantDb;
        private readonly string totalSql = "SELECT COUNT(1) FROM [ContentInfo] WHERE DeleteFlag = 0 AND ItemType = 0 AND PlateID = 60 AND(Status= 1 OR Status = 6 OR Status = 9) ";
        private readonly string newsColumnSql = @"
SELECT TOP 1000 ID as Id
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
        private readonly string databaseSql = @"--导航内容
SELECT ID as OldId
,[Title]
,DatabaseId as DatabaseProviderID
, Abbreviation as Abbreviation
,ArticleTypes as ArticleTypes
,DomainClcs as DomainClcs
,DomainEscs as DomainEscs
,PurchaseType as PurchaseType
,Language as Language
,'' as Label
,TrialStartTime as ExpiryBeginTime
,TrialEndTime as ExpiryEndTime
,Cover as Cover
,Remark as Remark
,IndirectUrl as  IndirectUrl
,CustomUrl as CustomUrl
,'' as WhiteList
,Introduction as Introduction
,Information as Information
,UseGuide as  UseGuide
,OrderIndex as OrderIndex
,DeleteFlag as DeleteFlag
,Status as Status
,CASE ShowStatus
         WHEN 3 THEN 0
         ELSE 1 END				 
				 as IsShow
,EntranceCountCurrentMonth as MonthClickNum
,EntranceCount as TotalClickNum
,'' as TenantId
,CreateTime as CreatedTime 
,'' as ResourceStatistics
,'' as UserGroups
,'' as UserTypes
,[Level]
,FirstLetter as Initials

FROM
	[DatabaseTerrace] 
WHERE
	DeleteFlag = 0 
	and CustomUrl !=''
	and Level<=1
	";

        public DatabaseGuideDataAdapter(string adapterParamStr, SqlSugarClient tenantDb)
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
        public List<DatabaseTerraceBack> GetDatabaseList(DateTime lastSucessTime, out MessageHand message)
        {

            var sql = databaseSql;
            string isFull = ConfigHelper.TaskConfig.GetSection("Appsettings:IsFullSynchronization").Value;
            if (isFull == "false")
            {
                sql = $" {sql} And Createtime>'{lastSucessTime}'";
            }


            var databaseList = new List<DatabaseTerraceBack>();
            message = new MessageHand { Code = CODE.SUCCED, Context = typeof(DatabaseGuideDataAdapter).FullName.ToString() };
            try
            {

                WriteLog("-----------------开始获取新闻数据--------------------");
                //Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = 202.202.12.5)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl))); User ID = cdstbk; Password = GqmO1uNo;

                //   var sql = string.Format(newsContentSql, string.Join(',', columnIds));
                //  sql = sql + "And Createtime>" + ConfigHelper.TaskConfig.GetSection("Appsettings:newsContentAdaptStartTime").Value;
                using (var db = DataHelper.GetInstance(_adapterParamDic["DatabasConn"].ToString(), (int)DbType.SqlServer))
                {
#if (DEBUG)
                    WriteLog("databaseSql:" + sql);
#endif
                    var dt = db.Ado.GetDataTable(sql);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return databaseList;
                    }
                    WriteLog("条数:" + dt.Rows.Count);

                    var list = dt.DtToList<DatabaseTerraceBackDto>().Select(p => new DatabaseTerraceBack
                    {
                        Id = p.Id,
                        IndirectUrl = p.IndirectUrl,
                        Information = p.Information,
                        Initials = p.Initials,
                        Introduction = p.Introduction,
                        IsShow = p.IsShow,
                        DatabaseProviderID = p.DatabaseProviderID,
                        OrderIndex = p.OrderIndex,
                        Abbreviation = p.Abbreviation,
                        ArticleTypes = p.ArticleTypes?.Replace(',', ';'),
                        Cover = p.Cover,
                        CreatedTime = p.CreatedTime,
                        DeleteFlag = p.DeleteFlag,
                        DomainClcs = p.DomainClcs?.Replace(',', ';'),
                        DomainEscs = p.DomainEscs?.Replace(',', ';'),
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
                        CustomUrl = p.CustomUrl,
                        OldId = p.OldId,



                    }).ToList();



                    var testList = list.Where(e => e.Status >= 0 && e.IsShow && e.ExpiryBeginTime < DateTime.Now && e.ExpiryEndTime > DateTime.Now).ToList();
                    WriteLog(" list.Count:" + list.Count);

                    foreach (var item in list)
                    {

                        databaseList.Add(item);
                    }
                }

                return databaseList;
            }
            catch (Exception ex)
            {
                message.ex = ex;
                return databaseList;
            }
        }


        private void WriteLog(string msg)
        {
            LogManager.Log(string.Format("Adapter_{0}", nameof(DatabaseGuideDataAdapter)), msg);
        }
    }
}
