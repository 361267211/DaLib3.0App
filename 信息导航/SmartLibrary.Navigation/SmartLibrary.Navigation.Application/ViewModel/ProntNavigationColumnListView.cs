using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.ViewModel
{
    /// <summary>
    /// 名    称：ProntNavigationColumnListView
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 14:24:24
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ProntNavigationColumnListView
    {
        /// <summary>
        /// 栏目ID
        /// </summary>
        public string ColumnID { get; set; }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 目录列表
        /// </summary>
        public List<ProntNavigationCatalogue> CatalogueList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProntNavigationCatalogue
    {
        public string ColumnID { get; set; }
        public string CatalogueID { get; set; }
        public string Name { get; set; }
        public bool IsOpenNewWindow { get; set; }
        public string ExternalLinks { get; set; }
        public int NavigationType { get; set; }

        /// <summary>
        /// 该目录下的内容ID,名称
        /// </summary>
        public List<KeyValuePair<string, string>> ContentList { get; set; }

        public List<ProntNavigationCatalogue> CatalogueList { get; set; }
    }
}
