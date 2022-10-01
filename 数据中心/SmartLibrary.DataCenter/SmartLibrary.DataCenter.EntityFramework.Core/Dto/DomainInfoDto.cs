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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.EntityFramework.Core.Dto
{
  public  class DomainInfoDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 学科编号
        /// </summary>
        public int DomainID { get; set; }

        /// <summary>
        /// 父级编号
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 学科标识
        /// </summary>
        public string DomainIDCode { get; set; }

        /// <summary>
        /// 学科名称
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// 中图对照
        /// </summary>
        public string Contrast { get; set; }

        /// <summary>
        /// 学科等级 1表示一级学科，2表示二级学科
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// 学科类型 0学科，1中图
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 创建类型。0：系统；1：自建
        /// </summary>
        public int CreateType { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string UserKey { get; set; }
    }
}
