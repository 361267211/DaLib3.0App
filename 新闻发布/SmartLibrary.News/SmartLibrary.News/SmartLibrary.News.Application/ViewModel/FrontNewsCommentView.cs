using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：FrontNewsCommentView
    /// 作    者：张泽军
    /// 创建时间：2021/9/15 19:50:09
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class FrontNewsCommentView
    {
        public UserInfo User { get; set; }
        public NewsComment Comment { get; set; }
    }

    public class UserInfo
    { 
        public string UserKey { get; set; }

        public string UserName { get; set; }
        public string UserPhoto { get; set; }
    }

    public class NewsComment
    {
        public string Id { get; set; }

        public string ContentID { get; set; }

        public string UserKey { get; set; }

        public string ComContent { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
