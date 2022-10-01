using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：NewsColumnContentManage
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 22:39:47
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsColumnContentManage
    {
        public List<AuditStatusCountView> AuditStatusCountList { get; set; }
        public List<KeyValuePair<string, string>> LableList { get; set; }

        public bool IsHasCatalogue { get; set; }
    }
}
