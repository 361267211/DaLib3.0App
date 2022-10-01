using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.ViewModel
{
    /// <summary>
    /// 名    称：ProntScenesNvaigationView
    /// 作    者：张泽军
    /// 创建时间：2021/11/11 17:08:50
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ProntScenesNavaigationView
    {
        public string ColumnID { get; set; }
        public string ColumnName { get; set; }
        public string JumnpLink { get; set; }
        public List<ProntScenesNavaigationCatalogueView> CatalogueList { get; set; }
    }

    public class ProntScenesNavaigationCatalogueView
    { 
        public string Id { get; set; }
        public string Title { get; set; }
        public int NavigationType { get; set; }
        public string ExternalLinks { get; set; }
        public string JumpLink { get; set; }
        public string Cover { get; set; }
        public bool IsOpenNewWindow { get; set; }
    }
}
