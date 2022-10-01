using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Navigation.Application.PageParam
{
    /// <summary>
    /// 名    称：ChangeContentStatusParam
    /// 作    者：张泽军
    /// 创建时间：2021/10/25 15:57:52
    /// 联系方式：电话[13608338767],邮件[qxyywy@qq.com]
    /// 描    述：
    /// </summary>
    public class ChangeContentStatusParam
    {
        /// <summary>
        /// 内容ID集合
        /// </summary>
        public string[] ContentIDList { get; set; }

        /// <summary>
        /// 是否正常
        /// </summary>
        public bool IsNormal { get; set; }
    }
}
