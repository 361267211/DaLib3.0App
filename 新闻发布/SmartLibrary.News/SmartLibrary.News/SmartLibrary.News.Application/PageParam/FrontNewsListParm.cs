using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.PageParam
{
    /// <summary>
    /// 名    称：FrontNewsListParm
    /// 作    者：张泽军
    /// 创建时间：2021/9/15 15:47:23
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class FrontNewsListParm
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string ColumnID { get; set; }
        public string LableID { get; set; }
        public string SearchKey { get; set; }
        public int ContentCutLength { get; set; }
    }
}
