using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.BaseInfo
{
    /// <summary>
    /// 权限信息
    /// </summary>
    public class AuthInfoEx
    {
        /// <summary>
        /// 是否有前台权限
        /// </summary>
        public bool CanWeb { get; set; }

        /// <summary>
        /// 是否有后台权限
        /// </summary>
        public bool CanAdmin { get; set; }
    }
}
