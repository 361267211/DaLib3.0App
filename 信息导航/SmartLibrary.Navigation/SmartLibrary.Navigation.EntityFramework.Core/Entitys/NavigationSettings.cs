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
    /// 名    称：NavigationSettings
    /// 作    者：张泽军
    /// 创建时间：2021/9/30 15:26:18
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationSettings: Entity<string>
    {
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

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
