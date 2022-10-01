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
   public class DatabaseDefaultTemplate:Entity<Guid>
    {
        /// <summary>
        /// 模板编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        
        [StringLength(maximumLength: 200, MinimumLength = 0)]
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(maximumLength: 200, MinimumLength = 0)]
        public string Remark { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        [StringLength(maximumLength: 200, MinimumLength = 0)]
        public string Pic { get; set; }
        /// <summary>
        /// 删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }

    }
}
