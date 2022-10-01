using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.PageParam
{
    /// <summary>
    /// 名    称：FrontContentListParm
    /// 作    者：张泽军
    /// 创建时间：2021/10/22 17:47:32
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class FrontContentListParm
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        [Required]
        public string ColumnID { get; set; }
        public string CataID { get; set; }
        public string SearchKey { get; set; }
        public int ContentCutLength { get; set; } = 100;
    }
}
