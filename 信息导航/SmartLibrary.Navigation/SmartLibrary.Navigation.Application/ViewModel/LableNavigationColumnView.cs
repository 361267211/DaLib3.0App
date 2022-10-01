using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.ViewModel
{
    /// <summary>
    /// 名    称：LableNavigationColumnView
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 14:23:03
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class LableNavigationColumnView
    {
        public string LableID { get; set; }
        public string LableName { get; set; }
        public List<LableNavigationColumn> ColumnList { get; set; }
    }

    public class LableNavigationColumn
    {
        public string ColumnID { get; set; }
        public string Title { get; set; }
        public bool Enable { get; set; }
        public string Cover => "/app_covers/column_cover/信息导航.png";
    }
}
