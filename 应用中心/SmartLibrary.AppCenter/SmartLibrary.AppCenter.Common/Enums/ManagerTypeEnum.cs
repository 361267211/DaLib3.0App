using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartLibrary.AppCenter.Common.Attributes;

namespace SmartLibrary.AppCenter.Common.Enums
{
    /// <summary>
    /// 管理员类型
    /// </summary>
    public enum ManagerTypeEnum
    {
        [Enum("管理员")]
        管理员 = 1,

        [Enum("操作员")]
        操作员 = 2,

        [Enum("浏览者")]
        浏览者 = 3
    }
}
