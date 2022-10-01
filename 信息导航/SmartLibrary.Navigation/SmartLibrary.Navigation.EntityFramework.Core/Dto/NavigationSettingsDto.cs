using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.EntityFramework.Core.Dto
{
    /// <summary>
    /// 名    称：NavigationSettingsDto
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 16:31:35
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationSettingsDto
    {
        ///<summary>
        ///主键
        ///</summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// 敏感词启用
        /// </summary>
        [Required]
        public bool SensitiveWords { get; set; }

        /// <summary>
        /// 评论启用
        /// </summary>
        [Required]
        public bool Comments { get; set; }
    }
}
