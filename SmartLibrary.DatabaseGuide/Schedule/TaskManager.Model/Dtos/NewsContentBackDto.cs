using System;
using System.ComponentModel.DataAnnotations;
namespace TaskManager.Model.Dtos
{
    /// <summary>
    /// 名    称：新闻内容
    /// 作    者：张泽军
    /// 创建时间：2021/9/7 15:12:32
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>

    public class NewsContentBackDto
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 新闻栏目ID
        /// </summary>
        public string ColumnID { get; set; }

        /// <summary>
        /// 投递新闻栏目ID
        /// </summary>
        public string ColumnIDs { get; set; }

        /// <summary>
        /// 内容标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容标题 带样式
        /// </summary>
        public string TitleStyle { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 所属子类（标签）
        /// </summary>
        public string ParentCatalogue { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 原作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        public Guid Publisher { get; set; }

        /// <summary>
        /// 发布人姓名
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 启停状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 投递终端
        /// </summary>
        public string Terminals { get; set; }

        /// <summary>
        /// 审核状态 重置栏目流程则重置为待提交
        /// </summary>
        public int AuditStatus { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime ExpirationDate { get; set; }
        /// <summary>
        /// 跳转链接
        /// </summary>
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
        public string AuditProcessJson { get; set; }

        /// <summary>
        /// 扩展字段1
        /// </summary>
        public string ExpendFiled1 { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        public string ExpendFiled2 { get; set; }
        /// <summary>
        /// 扩展字段3
        /// </summary>
        public string ExpendFiled3 { get; set; }
        /// <summary>
        /// 扩展字段4
        /// </summary>
        public string ExpendFiled4 { get; set; }
        /// <summary>
        /// 扩展字段5
        /// </summary>
        public string ExpendFiled5 { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedTime { get; set; }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        public int DeleteFlag { get; set; }


        /// <summary>
        /// 2.2的原始id
        /// </summary>
        public int OldId { get; set; }
    }
}
