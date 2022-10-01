using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：ProntNewsColumnListView
    /// 作    者：张泽军
    /// 创建时间：2021/9/15 18:59:25
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ProntNewsColumnListView
    {
        public string ColumnID { get; set; }
        public string Name { get; set; }
        public string DefaultTemplate { get; set; }
        /// <summary>
        /// 该栏目下的新闻的所有新闻标签
        /// </summary>
        public List<KeyValuePair<string, string>> LableList { get; set; }
    }
}
