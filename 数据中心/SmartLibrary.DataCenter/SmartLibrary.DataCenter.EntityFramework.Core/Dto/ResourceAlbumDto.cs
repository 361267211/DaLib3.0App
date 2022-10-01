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

using SmartLibrary.DataCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.EntityFramework.Core.Dto
{
   public class ResourceAlbumDto
    {
        /// <summary>
        /// 专辑编码
        /// </summary>
        public int AlbumCode { get; set; }
        /// <summary>
        /// 父级编码
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 专辑名称
        /// </summary>
        public string AlbumName { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }

        public List<ResourceAlbumDto> ChildAlbum { get; set; }
    }
}
