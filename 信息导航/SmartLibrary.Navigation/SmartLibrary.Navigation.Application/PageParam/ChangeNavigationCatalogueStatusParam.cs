using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.PageParam
{
    /// <summary>
    /// 名    称：ChangeNavigationCatalogueStatusParam
    /// 作    者：张泽军
    /// 创建时间：2021/10/12 17:11:39
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ChangeNavigationCatalogueStatusParam
    {
        public string[] CataIDList { get; set; }
        public bool IsNormal { get; set; }
    }
}
