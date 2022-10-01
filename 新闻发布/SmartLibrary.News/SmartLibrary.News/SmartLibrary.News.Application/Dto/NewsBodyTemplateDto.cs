using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Dto
{
    /// <summary>
    /// 名    称：NewsBodyTemplateDto
    /// 作    者：张泽军
    /// 创建时间：2021/9/16 11:53:42
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsBodyTemplateDto
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
        ///类型
        ///</summary>
        [Required]
        public int Type { get; set; }

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
    }
}
