using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：ProntScenesNewsContentView
    /// 作    者：张泽军
    /// 创建时间：2021/11/11 16:12:46
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ProntScenesNewsContentView
    {
        public string ContentID { get; set; }
        public string Lables { get; set; }
        public string Title { get; set; }
        public string JumpLink { get; set; }
        public string ExternalLink { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
