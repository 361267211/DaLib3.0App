using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Tasks.Enum
{
    public enum EnumGroupRuleUnionWay
    {
        [Description("且")]
        且 = 1,
        [Description("或")]
        或 = 2,
    }

}
