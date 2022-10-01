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
    /// 名    称：Content
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 16:04:12
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class Content  
    {
        public string Id { get; set; }
        ///<summary>
        ///内容标题
        ///</summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容标题 带样式
        /// </summary>
        public string TitleStyle { get; set; }

        ///<summary>
        ///副标题
        ///</summary>
        public string SubTitle { get; set; }

        ///<summary>
        ///所属目录
        ///</summary>
        public string CatalogueID { get; set; }

        ///<summary>
        ///多目录投递
        ///</summary>
        public string RelationCatalogueIDs { get; set; }

        ///<summary>
        ///内容
        ///</summary>
        public string Contents{get;set;}

        ///<summary>
        ///内容的文本
        ///</summary>
        public string ContentsText { get; set; }

        ///<summary>
        ///链接
        ///</summary>
        public string LinkUrl { get; set; }

        ///<summary>
        ///发布人
        ///</summary>
        public string Publisher { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建人名
        /// </summary>
        public string CreatorName { get; set; }

        ///<summary>
        ///发布日期
        ///</summary>
        public DateTime PublishDate { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        public bool Status { get; set; }

        /// <summary>
        /// 点击数
        /// </summary>
        public int HitCount { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortIndex { get; set; }

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
