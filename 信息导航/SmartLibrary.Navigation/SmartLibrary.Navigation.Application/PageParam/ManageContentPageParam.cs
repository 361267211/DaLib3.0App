using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.PageParam
{
    /// <summary>
    /// 名    称：ManageContentPageParam
    /// 作    者：张泽军
    /// 创建时间：2021/10/12 14:56:00
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ManageContentPageParam
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string ColumnID { get; set; }
        public string Keywords { get; set; }
        public string CataID { get; set; }
        public bool? Status { get; set; }
    }
}
