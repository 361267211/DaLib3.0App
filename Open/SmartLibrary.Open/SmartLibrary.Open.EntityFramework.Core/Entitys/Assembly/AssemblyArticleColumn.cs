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
    /// 记录汇编专题的某个文献栏目下文献来源的信息，分4大类
    /// 1.导入/上传/链接等方式 添加的固定文献信息
    /// 2.通过 检索表达式 关联的动态文献集合
    /// 3.通过管理员收藏夹关联的动态文献集合
    /// 4.通过系统推荐关联的动态文献集合
    public class AssemblyArticleColumn:Entity<Guid>
    {
        /// <summary>
        /// 文献栏目名称
        /// </summary>
        [StringLength(40), Required]
        public string Name { get; set; }
        [StringLength(40), Required]
        /// <summary>
        /// 专题ID
        /// </summary>

        public Guid AssemblyID { get; set; }



        /// <summary>
        /// 栏目的前端模板值
        /// </summary>
        [StringLength(100), Required]
        public string Template { get; set; }



        public int ArtBindType { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>

        public bool DeleteFlag { get; set; }


    }
}
