using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Enum
{
    public enum EnumObtainWay
    {
        [Description("到馆领取")]
        到馆领取 = 0,
        [Description("邮寄")]
        邮寄 = 1,
    }
}
