using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 名    称：Content
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 16:04:12
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class Content : Entity<string>
    {
        ///<summary>
        ///内容标题
        ///</summary>
        [StringLength(50), Required]
        public string Title { get; set; }

        /// <summary>
        /// 内容标题 带样式
        /// </summary>
        [StringLength(200)]
        public string TitleStyle { get; set; }

        ///<summary>
        ///副标题
        ///</summary>
        [StringLength(200)]
        public string SubTitle { get; set; }

        ///<summary>
        ///所属目录
        ///</summary>
        [StringLength(64), Required]
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
        [StringLength(50, MinimumLength = 2), Required]
        public string Publisher { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [StringLength(100),Required]
        public string Creator { get; set; }

        /// <summary>
        /// 创建人名
        /// </summary>
        public string CreatorName { get; set; }

        ///<summary>
        ///发布日期
        ///</summary>
        [Required]
        public DateTime PublishDate { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        [Required]
        public bool Status { get; set; }

        /// <summary>
        /// 点击数
        /// </summary>
        public int HitCount { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Required]
        public int SortIndex { get; set; }

        ///<summary>
        ///删除标识
        ///</summary>
        public bool DeleteFlag { get; set; }
}
}
