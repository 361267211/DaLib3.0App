using SmartLibrary.News.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.PageParam
{
    /// <summary>
    /// 名    称：NewsColumnParam
    /// 作    者：张泽军
    /// 创建时间：2021/9/26 9:42:45
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsColumnParam
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, StringLength(64)]
        public string Id { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [StringLength(maximumLength: 50, MinimumLength = 2), Required]
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
        /// 内容扩展项键值对
        /// </summary>
        public List<KeyValuePair<string, string>> ExtensionKV { get; set; }

        /// <summary>
        /// 栏目地址
        /// </summary>
        [StringLength(500)]
        public string LinkUrl { get; set; }

        /// <summary>
        /// 默认模板
        /// </summary>
        [StringLength(64), Required]
        public string DefaultTemplate { get; set; }

        /// <summary>
        /// 默认模板
        /// </summary>
        [StringLength(64)]
        public string HeadTemplate { get; set; }

        /// <summary>
        /// 默认模板
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

        ///<summary>
        ///封面尺寸
        ///</summary>
        public string CoverSize { get; set; }

        /// <summary>
        /// 是否登录可用
        /// </summary>
        [Required]
        public int IsLoginAcess { get; set; }

        ///// <summary>
        ///// 授权访问列表字符串
        ///// </summary>
        //[StringLength(500)]
        //public string VisitingList { get; set; }

        /// <summary>
        /// 授权访问列表字符串对象化
        /// </summary>
        public VisitingListModel VisitingListModel { get; set; }

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
        public int CoverWidth { get;  set; }
        public int CoverHeight { get;  set; }

        public bool AcessAll { get; set; }
    }
}
