using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Dto
{
    /// <summary>
    /// 名    称：NewsTemplateDto
    /// 作    者：张泽军
    /// 创建时间：2021/9/16 11:52:30
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsTemplateDto
    {
        ///<summary>
        ///主键
        ///</summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [StringLength(20), Required]
        public string Name { get; set; }

        ///<summary>
        ///头部模板
        ///</summary>
        [StringLength(64), Required]
        public string HeadTemplate { get; set; }

        ///<summary>
        ///尾部模板
        ///</summary>
        [StringLength(64), Required]
        public string FootTemplate { get; set; }

        ///<summary>
        ///描述
        ///</summary>
        [StringLength(300), Required]
        public string Note { get; set; }

        ///<summary>
        ///预览图
        ///</summary>
        [StringLength(100), Required]
        public string PreviewPic { get; set; }

        /// <summary>
        /// 模板列表访问地址 如：/list?id=
        /// </summary>
        [StringLength(100), Required]
        public string TemplateListDirectUrl { get; set; }

        /// <summary>
        /// 模板详情访问地址 如：/detail?id=
        /// </summary>
        [StringLength(100), Required]
        public string TemplateDetailDirectUrl { get; set; }
    }
}
