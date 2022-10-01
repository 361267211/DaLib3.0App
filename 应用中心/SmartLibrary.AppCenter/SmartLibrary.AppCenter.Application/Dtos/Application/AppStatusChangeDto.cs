using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 应用启用和停用
    /// </summary>
    public class AppStatusChangeDto
    {
        /// <summary>
        /// 操作类型 1=启用，2=停用
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 是否三方应用
        /// </summary>
        public bool IsThirdApp { get; set; }
    }
}
