using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.ViewModel
{
    /// <summary>
    /// 名    称：NavigationCatalogueView
    /// 作    者：张泽军
    /// 创建时间：2021/10/9 11:29:28
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationCatalogueTreeView
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 父级目录ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 子目录
        /// </summary>
        public List<NavigationCatalogueTreeView> ChildList { get; set; }
    }
}
