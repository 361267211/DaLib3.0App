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
    /// 通过导入/链接/上传等方式 添加的固定文献
    public class ArtByImported : Entity<Guid>
    {

        /// <summary>
        /// 文献栏目id
        /// </summary>
        public Guid ArtColumnId { get; set; }

        /// <summary>
        /// 文献ID
        /// </summary>

        public Int64 ArticleID { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>

        public int OrderIndex { get; set; }

        /// <summary>
        /// 评语
        /// </summary>
        [StringLength(400)]
        public string Comment { get; set; }



        /// <summary>
        /// 删除标志
        /// </summary>

        public bool DeleteFlag { get; set; }

    }
}
