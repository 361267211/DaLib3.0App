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
    /// 维护管理数据库平台
    public class DatabaseTerrace : Entity<Guid>
    {

        /// <summary>
        /// 平台/数据库名称
        /// </summary>
        [Column("Title")]
        [StringLength(maximumLength: 100, MinimumLength = 2), Required]
        public string Title { get; set; }

        /// <summary>
        /// 简称/别名
        /// </summary>
        [Column("Abbreviation")]
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string Abbreviation { get; set; }

        /// <summary>
        /// 数据库供应商ID
        /// </summary>
        [Column("DatabaseProviderID")]
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]
        public string DatabaseProviderID { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        [Column("Initials")]
        [StringLength(maximumLength: 20, MinimumLength = 0), Required]
        public string Initials { get; set; }

        /// <summary>
        /// 文献类型 1：图书；3：期刊文献；4：学位论文；5：标准
        /// </summary>
        [Column("ArticleTypes")]
        [StringLength(maximumLength: 100, MinimumLength = 0), Required]
        public string ArticleTypes { get; set; }



        /// <summary>
        /// 中图分类
        /// </summary>
        [Column("DomainClcs")]
        [StringLength(maximumLength: 50, MinimumLength = 0)]
        public string DomainClcs { get; set; }

        /// <summary>
        /// 学科分类
        /// </summary>
        [Column("DomainEscs")]
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]

        public string DomainEscs { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        [Column("PurchaseType")]
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]

        public string PurchaseType { get; set; }

        /// <summary>
        /// 主要语言
        /// </summary>
        [Column("Language")]
        [StringLength(maximumLength: 50, MinimumLength = 0), Required]
        public string Language { get; set; }

        /// <summary>
        /// 自定义标签
        /// </summary>
        [Column("Label")]
        [StringLength(maximumLength: 1000, MinimumLength = 0)]
        public string Label { get; set; }

        /// <summary>
        /// 效期开始时间
        /// </summary>
        [Column("ExpiryBeginTime")]
        public DateTime ExpiryBeginTime { get; set; }

        /// <summary>
        /// 效期失效时间
        /// </summary>
        [Column("ExpiryEndTime")]
        public DateTime ExpiryEndTime { get; set; }

        /// <summary>
        /// 封面图标
        /// </summary>
        [Column("Cover")]
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string Cover { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [Column("Remark")]
        [StringLength(maximumLength: 200, MinimumLength = 0)]
        public string Remark { get; set; }


        /// <summary>
        /// 间接访问
        /// </summary>
        [Column("IndirectUrl")]
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string IndirectUrl { get; set; }

        /// <summary>
        /// 限定访问用户名单
        /// </summary>
        [Column("WhiteList")]
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string WhiteList { get; set; }

        /// <summary>
        /// 数据库简介-仅纯文本
        /// </summary>
        [Column("Introduction")]
        public string Introduction { get; set; }

        /// <summary>
        /// 详细介绍
        /// </summary>
        [Column("Information")]
        public string Information { get; set; }

        /// <summary>
        /// 详细介绍编辑器
        /// </summary>
        public int InformationEditor { get; set; }

        /// <summary>
        /// 详细介绍文本内容
        /// </summary>
        public string InformationText { get; set; }

        /// <summary>
        /// 使用指南
        /// </summary>
        public string UseGuide { get; set; }

        /// <summary>
        /// 使用指南编辑器
        /// </summary>
        public int UseGuideEditor { get; set; }

        /// <summary>
        /// 使用指南文本内容
        /// </summary>
        public string UseGuideText { get; set; }

        /// <summary>
        /// 资源统计
        /// </summary>
        public string ResourceStatistics { get; set; }

        /// <summary>
        /// 资源统计编辑器
        /// </summary>
        public int ResourceStatisticsEditor { get; set; }

        /// <summary>
        /// 资源统计文本内容
        /// </summary>
        public string ResourceStatisticsText { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        [Column("OrderIndex")]
        public int OrderIndex { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [Column("DeleteFlag")]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 状态 1-正常，2-推荐
        /// </summary>
        [Column("Status")]
        public int Status { get; set; }

        /// <summary>
        /// 导航显示
        /// </summary>
        [Column("IsShow")]
        public bool IsShow { get; set; }

        /// <summary>
        /// 当月点击数量
        /// </summary>
        public Int64 MonthClickNum { get; set; }
        /// <summary>
        /// 总点击数量
        /// </summary>
        public Int64 TotalClickNum { get; set; }
        /// <summary>
        /// 可访问的用户类型
        /// </summary>
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string UserTypes { get; set; }
        /// <summary>
        /// 可访问的用户分组
        /// </summary>
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string UserGroups { get; set; }

        /// <summary>
        /// 资源供应源标识
        /// </summary>
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string ResourceProvider { get; set; }
    }
}
