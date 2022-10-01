using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.PageParam
{
    /// <summary>
    /// 名    称：NewsContentByColumnParm
    /// 作    者：张泽军
    /// 创建时间：2021/9/10 11:00:31
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsContentByColumnParm
    {
        public int PageIndex { get; set; }
        public int PageSize{ get; set; }
        public string ColumnID { get; set; }
        public int status { get; set; }
        /// <summary>
        /// 匹配标题 内容 发布人
        /// </summary>
        public string SearchKey { get; set; }
        public string LableId { get; set; }
        public int? AuditStatus { get; set; }
        public DateTime? BeginCreateTime{ get; set; }
        public DateTime? EndCreateTime{ get; set; }
        public DateTime? BeginOperateTime{ get; set; }
        public DateTime? EndOperateTime { get; set; }
    }
}
