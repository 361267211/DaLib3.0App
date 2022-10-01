using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Enum
{
    public enum EnumSourceFrom
    {
        [Description("直接添加")]
        直接添加=0,
        [Description("规则查询")]
        规则查询=1,
    }
}
