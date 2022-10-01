using Furion.DatabaseAccessor;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Navigation.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 名    称：NavigationCatalogue
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 15:54:48
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationCatalogue : Entity<string>
    {
        ///<summary>
        ///栏目ID
        ///</summary>
        [StringLength(64), Required]
        public string ColumnID { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        [StringLength(500), Required]
        public string Title { get; set; }

        /// <summary>
        /// 内容标题 带样式
        /// </summary>
        [StringLength(200)]
        public string TitleStyle { get; set; }

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
        ///路径字符串
        ///</summary>
        [StringLength(500)]
        public string PathCode { get; set; }

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
        /// 排序号
        /// </summary>
        [Required]
        public int SortIndex { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(100), Required]
        public string Creator { get; set; }

        /// <summary>
        /// 创建人名
        /// </summary>
        [StringLength(64)]
        public string CreatorName { get; set; }

        ///<summary>
        ///删除标识
        ///</summary>
        public bool DeleteFlag { get; set; }

    }
}
