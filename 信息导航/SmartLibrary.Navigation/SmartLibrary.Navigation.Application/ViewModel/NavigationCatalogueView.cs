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
    public class NavigationCatalogueView
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int IndexNum { get; set; }

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
        /// 启用状态
        /// </summary>
        public bool Status { get; set; }

        public string StatusName { get { return Status ? "正常" : "下架"; } }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建人名
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortIndex { get; set; }


        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否关联其他目录
        /// </summary>
        public bool IsAssociated { get; set; }

        /// <summary>
        /// 子目录
        /// </summary>
        public List<NavigationCatalogueView> ChildList { get; set; }
    }
}
