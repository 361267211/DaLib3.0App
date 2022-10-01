using SmartLibrary.News.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：LableNewsColumnView
    /// 作    者：张泽军
    /// 创建时间：2021/9/23 21:00:32
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class LableNewsColumnView
    {
        public string LableID { get; set; }
        public string LableName { get; set; }

        public List<LableNewsColumn> ColumnList { get; set; }
    }

    public class LableNewsColumn
    {
        public string Cover => "/app_covers/column_cover/新闻发布.png";

        public string ColumnID { get; set; }
        public string Title { get; set; }
        public bool Enable { get; set; }
    }
}
