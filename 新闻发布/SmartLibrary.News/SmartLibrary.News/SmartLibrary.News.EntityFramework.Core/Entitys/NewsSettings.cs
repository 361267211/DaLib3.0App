using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 名    称：信息导航应用设置
    /// 作    者：张泽军
    /// 创建时间：2021/9/6 11:23:51
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsSettings: Entity<string>
    {
        ///// <summary>
        ///// 主键
        ///// </summary>
        //[Key, StringLength(64)]
        //public string Id { get; set; }

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

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
