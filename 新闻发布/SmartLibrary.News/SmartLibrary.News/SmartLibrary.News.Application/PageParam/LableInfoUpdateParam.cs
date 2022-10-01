using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application.PageParam
{
    /// <summary>
    /// 名    称：LableInfoUpdateParam
    /// 作    者：张泽军
    /// 创建时间：2021/10/13 16:27:20
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class LableInfoUpdateParam
    {
        public int Type { get; set; }
        public List<LableUpdateParm> UpdateParmList { get; set; }
    }
}
