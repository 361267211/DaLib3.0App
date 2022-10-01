using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：ContentProcessLog
    /// 作    者：张泽军
    /// 创建时间：2021/10/18 15:49:14
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ContentProcessLog
    {
        public string EventName { get; set; }
        public string Operator { get; set; }
        public string OperatorName { get; set; }
        public string NoteInfo { get; set; }
        public DateTime OperateTime { get; set; }
    }
}
