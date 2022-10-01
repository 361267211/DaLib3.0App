using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Enum
{
    public enum EnumScoreType
    {
        [Description("减积分")]
        减积分 = -1,
        [Description("加积分")]
        加积分 = 1
    }
}
