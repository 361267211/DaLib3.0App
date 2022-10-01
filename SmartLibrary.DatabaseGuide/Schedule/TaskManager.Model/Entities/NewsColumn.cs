/*********************************************************
 * 名    称：NewsColumn
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：新闻栏目。
 *
 * 更新历史：
 *
 * *******************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Entities
{
    /// <summary>
    /// 名    称：新闻栏目
    /// 作    者：张泽军
    /// 创建时间：2021/9/6 10:23:51
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    [SqlSugar.SugarTable("NewsColumn")]
    public class NewsColumn
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 栏目别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 自定义标签
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 是否多终端同步
        /// </summary>
        public int Terminals { get; set; }

        /// <summary>
        /// 状态 1:启用，2:下架
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 内容扩展项
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 栏目地址
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// 默认模板
        /// </summary>
        public string DefaultTemplate { get; set; }

        /// <summary>
        /// 头部模板
        /// </summary>
        public string HeadTemplate { get; set; }

        /// <summary>
        /// 尾部模板
        /// </summary>
        public string FootTemplate { get; set; }

        /// <summary>
        /// 侧边列表
        /// </summary>
        public string SideList { get; set; }

        /// <summary>
        /// 展示的系统信息列表
        /// </summary>
        public string SysMesList { get; set; }

        ///<summary>
        ///启用封面
        ///</summary>
        public int IsOpenCover { get; set; }

        ///<summary>
        ///封面尺寸
        ///</summary>
        public string CoverSize { get; set; }

        /// <summary>
        /// 是否登录可用
        /// </summary>
        public int IsLoginAcess { get; set; }

        /// <summary>
        /// 侧边列表/授权访问列表字符串
        /// </summary>
        public string VisitingList { get; set; }

        /// <summary>
        /// 是否开启评论
        /// </summary>
        public int IsOpenComment { get; set; }

        ///// <summary>
        ///// 是否开启留言
        ///// </summary>
        //[Required]
        //public int IsOpenLeaveMes { get; set; }

        /// <summary>
        /// 是否开启审核流程
        /// </summary>
        public int IsOpenAudit { get; set; }

        /// <summary>
        /// 审核流程
        /// </summary>
        public string AuditFlow { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public bool DeleteFlag { get; set; }
    }
}
