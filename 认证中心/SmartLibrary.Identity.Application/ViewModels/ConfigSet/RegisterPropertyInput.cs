using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.ViewModels
{
    public class RegisterPropertyInput
    {
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PropertyCode { get; set; }
        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool IsCheck { get; set; }
    }
}
