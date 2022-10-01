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
    /// 名    称：NewsContentsForSearchView
    /// 作    者：张泽军
    /// 创建时间：2021/9/14 9:52:52
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NewsContentsForColumnView
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        public int IndexNum { get; set; }

        /// <summary>
        /// 内容标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 所属子类（标签）
        /// </summary>
        public List<KeyValuePair<string,string>> ParentCatalogue { get; set; }

        //public string[] ParentCatalogueName { get; set; }

        /// <summary>
        /// 启停状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 启停状态名
        /// </summary>
        public string StatusName
        {
            get
            {
                return EnumUtils.GetName(Converter.ToType<NewsContentStatusEnum>(Status));
            }
        }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int? AduitStatus { get; set; }

        /// <summary>
        /// 审核状态名
        /// </summary>
        public string AduitStatusName 
        {
            get
            {
                if (AduitStatus == null)
                    return "";
                return EnumUtils.GetName(Converter.ToType<AuditStatusEnum>(AduitStatus));
            }
        }

        /// <summary>
        /// 下一步审核按钮名称
        /// </summary>
        public string NextAuditBottonName 
        { 
            get 
            {
                return NextAuditStatus.FirstOrDefault().Value.Replace("通过", "");
            } 
        }

        /// <summary>
        /// 下一步审核状态
        /// </summary>
        public List<KeyValuePair<int,string>> NextAuditStatus { get; set; }


        /// <summary>
        /// 发布人
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
