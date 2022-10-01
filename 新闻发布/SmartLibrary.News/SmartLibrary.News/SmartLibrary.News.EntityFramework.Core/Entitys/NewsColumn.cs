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
    /// 名    称：新闻栏目
    /// 作    者：张泽军
    /// 创建时间：2021/9/6 10:23:51
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsColumn : Entity<string>
    {
        ///// <summary>
        ///// 主键
        ///// </summary>
        //[Key, StringLength(64)]
        //public string Id { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [StringLength(maximumLength: 100, MinimumLength = 2), Required]
        public string Title { get; set; }

        /// <summary>
        /// 栏目别名
        /// </summary>
        [StringLength(50)]
        public string Alias { get; set; }

        /// <summary>
        /// 自定义标签
        /// </summary>
        [StringLength(500)]
        public string Label { get; set; }

        /// <summary>
        /// 是否多终端同步
        /// </summary>
        [Required]
        public int Terminals { get; set; }

        /// <summary>
        /// 状态 1:启用，2:下架
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 内容扩展项
        /// </summary>
        [StringLength(300), Required]
        public string Extension { get; set; }

        /// <summary>
        /// 栏目地址
        /// </summary>
        [StringLength(500)]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 默认模板
        /// </summary>
        [StringLength(64),Required]
        public string DefaultTemplate { get; set; }

        /// <summary>
        /// 头部模板
        /// </summary>
        [StringLength(64)]
        public string HeadTemplate { get; set; }

        /// <summary>
        /// 尾部模板
        /// </summary>
        [StringLength(64)]
        public string FootTemplate { get; set; }

        /// <summary>
        /// 侧边列表
        /// </summary>
        [StringLength(100)]
        public string SideList { get; set; }

        /// <summary>
        /// 展示的系统信息列表
        /// </summary>
        [StringLength(100)]
        public string SysMesList { get; set; }

        ///<summary>
        ///启用封面
        ///</summary>
        [Required]
        public int IsOpenCover { get; set; }

 

        /// <summary>
        /// 是否登录可用
        /// </summary>
        [Required]
        public int IsLoginAcess { get; set; }

        /// <summary>
        /// 侧边列表/授权访问列表字符串
        /// </summary>
        [StringLength(500)]
        public string VisitingList { get; set; }

        /// <summary>
        /// 是否开启评论
        /// </summary>
        [Required]
        public int IsOpenComment { get; set; }

        ///// <summary>
        ///// 是否开启留言
        ///// </summary>
        //[Required]
        //public int IsOpenLeaveMes { get; set; }

        /// <summary>
        /// 是否开启审核流程
        /// </summary>
        [Required]
        public int IsOpenAudit { get; set; }

        /// <summary>
        /// 审核流程
        /// </summary>
        [StringLength(50)]
        public string AuditFlow { get; set; }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 封面宽
        /// </summary>
        public int CoverWidth { get; set; }

        /// <summary>
        /// 封面高
        /// </summary>
        public int CoverHeight { get; set; }

        /// <summary>
        /// 允许所有用户访问
        /// </summary>
        public bool AcessAll { get; set; }
    }
}
