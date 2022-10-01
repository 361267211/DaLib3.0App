using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.PageParam
{
    /// <summary>
    /// 名    称：NewsContentBySearchKeyParm
    /// 作    者：张泽军
    /// 创建时间：2021/9/14 9:47:01
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsContentBySearchKeyParm
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        /// <summary>
        /// 匹配标题 内容
        /// </summary>
        public string SearchKey { get; set; }
    }
}
