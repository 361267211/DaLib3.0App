using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.PageParam
{
    /// <summary>
    /// 名    称：ContentParam
    /// 作    者：张泽军
    /// 创建时间：2021/10/21 21:14:45
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ContentParam
    {
        ///<summary>
        ///主键
        ///</summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        ///<summary>
        ///内容标题
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
        public string Contents { get; set; }

        ///<summary>
        ///链接
        ///</summary>
        public string LinkUrl { get; set; }

        ///<summary>
        ///发布人
        ///</summary>
        [StringLength(50, MinimumLength = 2), Required]
        public string Publisher { get; set; }

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
    }
}
