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
    /// 数据库供应商
    public class DatabaseProvider : Entity<Guid>
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        [Column("ProviderName")]
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]

        public string ProviderName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        [StringLength(maximumLength: 200, MinimumLength = 0), Required]

        public string Remark { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        [Column("Operator")]
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]
        public string Operator { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        [Column("DeleteFlag")]
        public string DeleteFlag { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Column("Adress")]
        [StringLength(maximumLength:200, MinimumLength = 0), Required]
        public string Adress { get; set; }

        /// <summary>
        /// 收款信息
        /// </summary>
        [Column("Gathering")]
        [StringLength(maximumLength: 500, MinimumLength = 0), Required]
        public string Gathering { get; set; }

        /// <summary>
        /// 联系人信息
        /// </summary>
        [Column("Contact")]
        [StringLength(maximumLength: 500, MinimumLength = 0), Required]

        public string Contact { get; set; }

        /// <summary>
        /// 所属地区
        /// </summary>
        [Column("District")]
        [StringLength(maximumLength: 500, MinimumLength = 0)]

        public string District { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        [Column("Tel")]
        [StringLength(maximumLength: 50, MinimumLength = 0),Required]
        public string Tel { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        [Column("ContractsTel")]
        [StringLength(maximumLength: 50, MinimumLength = 0)]
        public string ContractsTel { get; set; }

    }
}
