using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：WorkbenchPublishView
    /// 作    者：张泽军
    /// 创建时间：2021/9/23 11:52:01
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class WorkbenchPublishView
    {
        public int ColumnCounts { get; set; }

        public int ContentCounts { get; set; }

        public List<KeyValuePair<string, string>> ColumnList { get; set; }
        public List<KeyValuePair<string, string>> ContentList { get; set; }
    }
}
