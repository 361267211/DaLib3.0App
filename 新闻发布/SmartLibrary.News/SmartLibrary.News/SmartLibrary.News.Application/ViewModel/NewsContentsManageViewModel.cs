using SmartLibrary.News.Application.Dto;
using SmartLibrary.News.Utility.EnumTool;
using SmartLibrary.News.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.News.Application.Enums;
using SmartLibrary.News.EntityFramework.Core.Entitys;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：NewsContentsManageViewModel
    /// 作    者：张泽军
    /// 创建时间：2021/9/9 19:19:31
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsContentsManageViewModel
    {
        /// <summary>
        /// 返回查询的新闻列表
        /// </summary>
        public PagedList<NewsContentsForColumnView> NewsContents { get; set; }

        /// <summary>
        /// 返回查询的新闻对应状态以及其数量
        /// </summary>
        public List<AuditStatusCountView> AuditStatusCountList { get; set; }
    }

    public class AuditStatusCountView
    { 
        public int AuditStatus { get; set; }

        public string Name 
        {
            get { 
                return EnumUtils.GetName(Converter.ToType<AuditStatusEnum>(AuditStatus)); 
            }
            set { }
        }

        public int Counts { get; set; }
    }
}
