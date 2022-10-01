using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.PageParam
{
    /// <summary>
    /// 名    称：NavigationCatalogueParam
    /// 作    者：张泽军
    /// 创建时间：2021/10/21 21:05:47
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationCatalogueParam
    {
        ///<summary>
        ///主键
        ///</summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        ///<summary>
        ///栏目ID
        ///</summary>
        [StringLength(64), Required]
        public string ColumnID { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        [StringLength(50), Required]
        public string Title { get; set; }

        /// <summary>
        /// 内容扩展项键值对
        /// </summary>
        public List<KeyValuePair<string, object>> TitleStyleKV
        {
            get;set;
        }

        ///<summary>
        ///别名
        ///</summary>
        [StringLength(50)]
        public string Alias { get; set; }

        ///<summary>
        ///父级目录ID
        ///</summary>
        [StringLength(64), Required]
        public string ParentID { get; set; }

        ///<summary>
        ///导航类型
        ///</summary>
        [Required]
        public int NavigationType { get; set; }

        ///<summary>
        ///关联目录
        ///</summary>
        [StringLength(64)]
        public string AssociatedCatalog { get; set; }

        ///<summary>
        ///外部链接
        ///</summary>
        [StringLength(500)]
        public string ExternalLinks { get; set; }

        ///<summary>
        ///是否开启新页面
        ///</summary>
        public bool IsOpenNewWindow { get; set; }

        ///<summary>
        ///封面
        ///</summary>
        public string Cover { get; set; }

        ///<summary>
        ///启用状态
        ///</summary>
        [Required]
        public bool Status { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(100)]
        public string Creator { get; set; }

        /// <summary>
        /// 创建人名
        /// </summary>
        public string CreatorName { get; set; }
    }
}
