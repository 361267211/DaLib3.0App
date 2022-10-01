using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Model.Enum
{
    public enum EnumManualProcessStatus
    {
        [Description("待处理")]
        待处理 = 0,
        [Description("处理中")]
        处理中 = 1,
        [Description("完成")]
        完成 = 2,
        [Description("失败")]
        失败 = -1,
    }
}
