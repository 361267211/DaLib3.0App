/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 名    称：NavigationCatalogue
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 15:54:48
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationCatalogue
    {
        public string Id { get; set; }
        ///<summary>
        ///栏目ID
        ///</summary>
        public string ColumnID { get; set; }

        ///<summary>
        ///标题
        ///</summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容标题 带样式
        /// </summary>
        public string TitleStyle { get; set; }

        ///<summary>
        ///别名
        ///</summary>
        public string Alias { get; set; }

        ///<summary>
        ///父级目录ID
        ///</summary>
        public string ParentID { get; set; }

        ///<summary>
        ///路径字符串
        ///</summary>
        public string PathCode { get; set; }

        ///<summary>
        ///导航类型
        ///</summary>
        public int NavigationType { get; set; }

        ///<summary>
        ///关联目录
        ///</summary>
        public string AssociatedCatalog { get; set; }

        ///<summary>
        ///外部链接
        ///</summary>
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
        public bool Status { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建人名
        /// </summary>
        public string CreatorName { get; set; }

        ///<summary>
        ///删除标识
        ///</summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }

    }
}
