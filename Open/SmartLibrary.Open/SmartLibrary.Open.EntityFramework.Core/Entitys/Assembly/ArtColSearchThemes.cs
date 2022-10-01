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

namespace SmartLibrary.Open.EntityFramework.Core.Entitys.Assembly
{
    /// <summary>
    /// 通过绑定检索主题查数据的文献栏目
    /// </summary>
    public class ArtColSearchThemes : Entity<Guid>
    {
        /// <summary>
        /// 专题文献栏目ID
        /// </summary>

        public Guid AssemblyArticleColumnID { get; set; }
        /// <summary>
        /// 检索中包含的主题词列表--多值字符
        /// </summary>
        [StringLength(40), Required]
        public string SearchThemes { get; set; }
        /// <summary>
        /// 检索中包含的文献类型==多值字符
        /// </summary>
        [StringLength(200), Required]
        public string ArtTypes { get; set; }

        public bool DeleteFlag { get; set; }
    }
}
