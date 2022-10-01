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
    /// 名    称：LableInfo
    /// 作    者：张泽军
    /// 创建时间：2021/9/9 16:25:24
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class LableInfo: Entity<string>
    {
        /////<summary>
        /////主键
        /////</summary>
        //[Key, StringLength(64)]
        //public string Id { get; set; }

        ///<summary>
        ///名称
        ///</summary>
        [StringLength(20), Required]
        public string Name { get; set; }

        ///<summary>
        ///所属类别（1新闻栏目，2新闻）
        ///</summary>
        [Required]
        public int Type { get; set; }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
