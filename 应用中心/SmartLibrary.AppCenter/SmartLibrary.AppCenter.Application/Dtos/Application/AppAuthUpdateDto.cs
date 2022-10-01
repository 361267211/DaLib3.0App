using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 授权实体
    /// </summary>
    public class AppAuthUpdateDto
    {
        /// <summary>
        /// 应用ID集合
        /// </summary>
        public List<string> AppIds { get; set; }

        /// <summary>
        /// 用户类型/用户分组 ID集合
        /// </summary>
        public List<string> UserSetIds { get; set; }

        /// <summary>
        /// 用户授权类型 1=用户类型，2=用户分组
        /// </summary>
        public int UserSetType { get; set; }

        /// <summary>
        /// 操作类型 0=按应用授权，1=按用户类型/分组授权
        /// </summary>
        public int OperationType { get; set; }
    }
}
