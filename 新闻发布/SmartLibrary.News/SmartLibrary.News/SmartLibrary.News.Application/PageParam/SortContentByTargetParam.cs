using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.PageParam
{
    /// <summary>
    /// 名    称：SortContentByIndexParam
    /// 作    者：张泽军
    /// 创建时间：2021/10/13 0:27:56
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class SortContentByTargetParam
    {
        public string SourceID { get; set; }
        public string TargetCataID { get; set; }
        public bool IsUp { get; set; }
    }
}
