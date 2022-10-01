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

namespace SmartLibrary.News.EntityFramework.Core.Entitys
{
    /// <summary>
    /// 新闻内容中转表
    /// </summary>
    public class NewsContentBack:Entity<string>
    {
        ///// <summary>
        ///// 主键
        ///// </summary>
        //public string Id { get; set; }

        /// <summary>
        /// 新闻栏目ID
        /// </summary>
        [StringLength(64), Required]
        public string ColumnID { get; set; }

        /// <summary>
        /// 投递新闻栏目ID
        /// </summary>
        [StringLength(400)]
        public string ColumnIDs { get; set; }

        /// <summary>
        /// 内容标题
        /// </summary>
        [StringLength(100), Required]
        public string Title { get; set; }

        /// <summary>
        /// 内容标题 带样式
        /// </summary>
        [StringLength(200)]
        public string TitleStyle { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [StringLength(50)]
        public string SubTitle { get; set; }

        /// <summary>
        /// 所属子类（标签）
        /// </summary>
        [StringLength(500)]
        public string ParentCatalogue { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        [StringLength(500)]
        public string Cover { get; set; }

        /// <summary>
        /// 原作者
        /// </summary>
        [StringLength(50)]
        public string Author { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        [StringLength(100), Required]
        public string Publisher { get; set; }

        /// <summary>
        /// 发布人姓名
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        [Required]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 启停状态
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 投递终端
        /// </summary>
        [StringLength(100)]
        public string Terminals { get; set; }

        /// <summary>
        /// 审核状态 重置栏目流程则重置为待提交
        /// </summary>
        [Required]
        public int AuditStatus { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        [StringLength(100)]
        public string Keywords { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }
        /// <summary>
        /// 跳转链接
        /// </summary>
        [StringLength(100)]
        public string JumpLink { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        public int HitCount { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// 新闻审核流程记录 重置流程则更改为待提交（仅针对前台显示）
        /// </summary>
        [StringLength(1000)]
        public string AuditProcessJson { get; set; }

        /// <summary>
        /// 扩展字段1
        /// </summary>
        [StringLength(200)]
        public string ExpendFiled1 { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        [StringLength(200)]
        public string ExpendFiled2 { get; set; }
        /// <summary>
        /// 扩展字段3
        /// </summary>
        [StringLength(200)]
        public string ExpendFiled3 { get; set; }
        /// <summary>
        /// 扩展字段4
        /// </summary>
        [StringLength(200)]
        public string ExpendFiled4 { get; set; }
        /// <summary>
        /// 扩展字段5
        /// </summary>
        [StringLength(200)]
        public string ExpendFiled5 { get; set; }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 外部链接
        /// </summary>
        [StringLength(500)]
        public string ExternalLink { get; set; }


    }
}
