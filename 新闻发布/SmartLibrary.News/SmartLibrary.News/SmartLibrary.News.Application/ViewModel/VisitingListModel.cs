using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：VisitingListModel
    /// 作    者：张泽军
    /// 创建时间：2021/10/18 17:29:31
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class VisitingListModel
    {
        public int Type { get; set; }
        public List<KeyValuePair<string, string>> VisitList { get; set; }
    }
}
