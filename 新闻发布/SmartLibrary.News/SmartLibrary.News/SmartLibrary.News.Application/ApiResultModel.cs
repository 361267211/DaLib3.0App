using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.News.Application
{
    /// <summary>
    /// 名    称：ApiResultInfoModel
    /// 作    者：张泽军
    /// 创建时间：2021/9/8 19:19:34
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ApiResultInfoModel<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }

    public class ApiResultInfoModel : ApiResultInfoModel<object> { }
}
