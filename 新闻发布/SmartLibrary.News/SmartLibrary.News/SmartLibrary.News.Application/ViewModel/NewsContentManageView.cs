using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Application.Enums;
using SmartLibrary.News.Utility;
using SmartLibrary.News.Utility.EnumTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：FrontNewsContentView
    /// 作    者：张泽军
    /// 创建时间：2021/9/15 19:17:02
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsContentManageView
    {
        public NewsContentDto Content { get; set; }
        public List<KeyValuePair<int, string>> NextAuditStatus { get; set; }
        //public string NextAuditStatusName 
        //{
        //    get
        //    {
        //        return NextAuditStatus != null ? EnumUtils.GetName(Converter.ToType<AuditStatusEnum>(NextAuditStatus)) : null;
        //    }
        //}
        public List<KeyValuePair<string, string>> ExpendFiledList { get; set; }
    }
}
