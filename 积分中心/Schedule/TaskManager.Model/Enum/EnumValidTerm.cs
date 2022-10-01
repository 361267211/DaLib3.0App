using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Enum
{
    public enum EnumValidTerm
    {
        [Description("永久")]
        永久 = 0,
        [Description("1年")]
        一年 = 1,
        [Description("2年")]
        两年 = 2,
        [Description("3年")]
        三年 = 3,
        [Description("4年")]
        四年 = 4,
        [Description("5年")]
        五年 = 5
    }
}
