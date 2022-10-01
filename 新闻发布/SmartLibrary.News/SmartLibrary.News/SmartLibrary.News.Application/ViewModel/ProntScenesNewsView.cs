using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.ViewModel
{
    /// <summary>
    /// 名    称：ProntScenesNewsView
    /// 作    者：张泽军
    /// 创建时间：2021/11/11 16:07:21
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ProntScenesNewsView
    {
        public string ColumnID { get; set; }
        public string ColumnName { get; set; }
        public string DefaultTemplate { get; set; }
        public string JumpLink { get; set; }

        public List<ProntScenesChildCatorageView> Childs { get; set; }
        public List<ProntScenesNewsContentView> ContentList { get; set; }
    }

    /// <summary>
    /// 名    称：ProntScenesNewsView
    /// 作    者：张泽军
    /// 创建时间：2021/11/11 16:07:21
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ProntScenesNewsCquView
    {
        public string ColumnID { get; set; }
        public string ColumnName { get; set; }
        public string DefaultTemplate { get; set; }
        public string JumpLink { get; set; }
 
    }
    /// <summary>
    /// 栏目子类
    /// </summary>
    public class ProntScenesChildCatorageView
    {
        /// <summary>
        /// 子类名称
        /// </summary>
        public string ChildCatorage { get; set; }
        /// <summary>
        /// 新闻列表
        /// </summary>
        public List<ProntScenesNewsContentView> ContentList { get; set; }
    }
}
