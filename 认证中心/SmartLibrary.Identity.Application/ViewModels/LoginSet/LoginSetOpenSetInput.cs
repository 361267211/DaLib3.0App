using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.ViewModels
{
    public class LoginSetOpenSetInput
    {
        /// <summary>
        /// 配置ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen { get; set; }
    }
}
