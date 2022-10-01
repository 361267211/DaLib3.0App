using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.PageParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：NewsColumnPermissionsView
    /// 作    者：张泽军
    /// 创建时间：2021/9/17 18:29:26
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsSingleColumnPermissionsView
    {
        public int AuditProcessStatus { get; set; }
        public string AuditProcessName { get; set; }

        public List<ManagerParam> ListPermissions { get; set; }
    }
}
