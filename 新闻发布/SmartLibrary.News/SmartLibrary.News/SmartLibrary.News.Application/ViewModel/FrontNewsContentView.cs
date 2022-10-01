using SmartLibrary.News.Application.Dto;
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
    public class FrontNewsContentView
    {
        public bool IsShowPublishDate { get; set; }
        public bool IsShowHitCount { get; set; }
        public bool IsShowAuditProcess { get; set; }

        //扩展字段
        public bool IsShowAuthor { get; set; }
        public bool IsShowKeywords { get; set; }
        public bool IsShowExpirationDate { get; set; }
        public bool IsShowJumpLink { get; set; }
        public bool IsShowParentCatalogue { get; set; }
        public bool IsShowExpendFiled1 { get; set; }
        public bool IsShowExpendFiled2 { get; set; }
        public bool IsShowExpendFiled3 { get; set; }
        public bool IsShowExpendFiled4 { get; set; }
        public bool IsShowExpendFiled5 { get; set; }
        public NewsContentDto Content { get; set; }
        public List<FrontNewsAuditProcess> AuditProcessList { get; set; }
        public List<KeyValuePair<string, string>> ExpendFiledList { get; set; }
    }

    public class FrontNewsAuditProcess
    { 
        public int AuditProcess { get; set; }
        public string Name { get; set; }
        public string AuditManager { get; set; }
    }
}
