using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 应用 续订/延期/免费试用/预约采购
    /// </summary>
    public class AppActionRequest
    {
        /// <summary>
        /// 操作类型 1=续订，2=延期，3=免费试用，4=预约采购
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 续订/延期时间 纯数字,换算成月
        /// </summary>
        public int TimeNum { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
    }
}
