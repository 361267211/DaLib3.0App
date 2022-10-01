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
    public class ArtByUpload:Entity<Guid>
    {
        /// <summary>
        /// 文献栏目id
        /// </summary>
        public Guid ArtColumnId { get; set; }
        /// <summary>
        /// 文献名称
        /// </summary>
        [StringLength(100), Required]
        public string Title { get; set; }

        /// <summary>
        /// 作者名称
        /// </summary>
        [StringLength(100), Required]
        public string Author { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>

        public int OrderIndex { get; set; }

        /// <summary>
        /// 出版年份
        /// </summary>
        [StringLength(20), Required]
        public string Date { get; set; }

        /// <summary>
        /// 推荐评语
        /// </summary>
        [StringLength(500)]
        public string Comment { get; set; }

        /// <summary>
        /// 上传文件的地址id
        /// </summary>

        public Guid File { get; set; }

        /// <summary>
        /// 资源绑定的链接
        /// </summary>
        [StringLength(100)]
        public string Url { get; set; }

        /// <summary>
        /// 类型 1-文件 2-链接
        /// </summary>

        public string Type { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>

        public bool DeleteFlag { get; set; }
    }
}
