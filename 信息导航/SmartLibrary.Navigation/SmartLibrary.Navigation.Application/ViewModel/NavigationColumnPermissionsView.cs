using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.ViewModel
{
    /// <summary>
    /// 名    称：NavigationColumnPermissionsView
    /// 作    者：张泽军
    /// 创建时间：2021/10/8 11:24:20
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class NavigationColumnPermissionsView
    {
        public string ColumnID { get; set; }
        public int IndexNum { get; set; }
        public string Title { get; set; }
        public List<KeyValuePair<string, string>> ManagerList { get; set; }
    }
}
