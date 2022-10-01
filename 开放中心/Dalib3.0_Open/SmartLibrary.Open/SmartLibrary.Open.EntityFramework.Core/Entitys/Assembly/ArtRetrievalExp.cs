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
    /// 文献栏目绑定的检索表达式
    public class ArtRetrievalExp:Entity<Guid>
    {
        /// <summary>
        /// 文献栏目名称
        /// </summary>
        [StringLength(100), Required]
        public string Name { get; set; }

        /// <summary>
        /// 专题文献栏目ID
        /// </summary>
        [StringLength(40), Required]
        public Guid AssemblyArticleColumnID { get; set; }

        /// <summary>
        /// 检索表达式 -字符型
        /// </summary>
        [StringLength(200), Required]
        public string Expression { get; set; }

        /// <summary>
        /// 文献类型
        /// </summary>
        [StringLength(40), Required]
        public string ArticleTypes { get; set; }

        /// <summary>
        /// 核心收录
        /// </summary>
        [StringLength(200), Required]
        public string CoreCollection { get; set; }

        /// <summary>
        /// 排序规则
        /// </summary>

        public int Collation { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>

        public bool DeleteFlag { get; set; }

    }
}
