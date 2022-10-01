using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：NewsContentsForSearchView
    /// 作    者：张泽军
    /// 创建时间：2021/9/14 9:52:52
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsContentsForSearchView
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        public int IndexNum { get; set; }

        /// <summary>
        /// 新闻栏目ID
        /// </summary>
        public List<KeyValuePair<string,string>> ColumnIDs { get; set; }

        ///// <summary>
        ///// 新闻栏目名称
        ///// </summary>
        //public string[] ColumnNames { get; set; }

        /// <summary>
        /// 内容标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
