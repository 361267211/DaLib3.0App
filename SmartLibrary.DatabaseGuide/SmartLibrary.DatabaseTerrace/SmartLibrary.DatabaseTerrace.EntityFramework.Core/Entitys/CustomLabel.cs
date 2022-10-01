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
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core.Entitys
{
    /// 自定义标签管理
    public class CustomLabel:Entity<Guid>
    {

        /// <summary>
        /// 标签名称
        /// </summary>
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]
        public string LabelName { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime Createtime { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>

        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>

        public DateTime UpdateTime { get; set; }

    }
}
