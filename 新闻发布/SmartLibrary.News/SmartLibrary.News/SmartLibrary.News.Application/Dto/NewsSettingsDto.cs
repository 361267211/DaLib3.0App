using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Dto
{
    /// <summary>
    /// 名    称：NewsSettingsDto
    /// 作    者：张泽军
    /// 创建时间：2021/9/9 15:07:58
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsSettingsDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// 敏感词启用
        /// </summary>
        [Required]
        public int SensitiveWords { get; set; }

        /// <summary>
        /// 评论启用
        /// </summary>
        [Required]
        public int Comments { get; set; }
    }
}
