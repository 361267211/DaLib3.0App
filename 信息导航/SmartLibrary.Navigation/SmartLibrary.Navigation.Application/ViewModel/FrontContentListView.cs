using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.ViewModel
{
    /// <summary>
    /// 名    称：FrontContentListView
    /// 作    者：张泽军
    /// 创建时间：2021/10/22 17:48:57
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class FrontContentListView
    {
        public string ContentID { get; set; }
        public string ColumnID { get; set; }
        public string Title { get; set; }
        public bool IsShowContent { get; set; }
        public string Content { get; set; }
        public bool IsShowPublishDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsShowHitCount { get; set; }
        public int? HitCount { get; set; }
        public string LinkUrl { get; set; }

    }
}
