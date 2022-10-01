using SmartLibrary.News.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.Dto.Permission
{
    /// <summary>
    /// 名    称：PermissionMenu
    /// 作    者：张泽军
    /// 创建时间：2021/10/20 16:28:52
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class PermissionMenu: SysMenuPermission
    {
        public List<string> ListPermission { get; set; }
    }
}
