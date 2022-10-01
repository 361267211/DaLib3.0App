using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.Dtos.LoginConfig
{
    /// <summary>
    /// Cas三方接入配置
    /// </summary>
    public class CasConfigDto
    {
        public string UserKeyField { get; set; } = "cas:user";
        public string CasServerAddress { get; set; } = "http://192.168.21.36:10011/cas/login";
        public string ReturnAddress { get; set; } = "http://192.168.21.36:6069/#/loginAccount";
        public string LoginAddress { get; set; }
        public string ResultCallback { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOpen { get; set; }
    }
}
