using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.PageParam
{
    /// <summary>
    /// 名    称：WorkbenchAuditNewsListParam
    /// 作    者：张泽军
    /// 创建时间：2021/9/23 16:43:59
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class WorkbenchAuditNewsListParam
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string ManagerID { get; set; }
        public int AuditStatus { get; set; }
    }
}
