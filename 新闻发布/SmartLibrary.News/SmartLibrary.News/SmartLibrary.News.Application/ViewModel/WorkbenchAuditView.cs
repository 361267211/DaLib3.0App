using SmartLibrary.News.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：WorkbenchAuditView
    /// 作    者：张泽军
    /// 创建时间：2021/9/23 14:01:05
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class WorkbenchAuditPageView
    {
        public List<WorkbenchAuditView> AuditList { get; set; }
        public PagedList<WorkbenchNews> NewsList { get; set; }
    }

    public class WorkbenchAuditView
    {
        public int AuditStatus { get; set; }
        public string StatusName { get; set; }
        public int Count { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class WorkbenchNews
    { 
        public string ContentID { get; set; }
        public string ColumnID { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
