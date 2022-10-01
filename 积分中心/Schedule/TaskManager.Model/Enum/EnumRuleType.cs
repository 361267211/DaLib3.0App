using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Enum
{
    public enum EnumRuleType
    {
        [Description("用户组")]
        用户组 = 0,
        [Description("用户类型")]
        用户类型=1,
    }
}
