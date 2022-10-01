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

using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys
{
    /// 数据库导航的访问链接
    public class DatabaseAcessUrl : Entity<Guid>
    {
        /// <summary>
        /// 数据库ID
        /// </summary>

        public Guid DatabaseID { get; set; }

        /// <summary>
        /// 链接名称
        /// </summary>
        [StringLength(maximumLength: 50, MinimumLength = 2), Required]
        public string Name { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [StringLength(maximumLength: 500, MinimumLength = 2), Required]
        public string Url { get; set; }


        /// <summary>
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }

    }
}
