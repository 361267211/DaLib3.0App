using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.EntityFramework.Core.Entitys
{
    public class RegisterConfigSet : BaseEntity<Guid>
    {
        /// <summary>
        /// 是否开放注册
        /// </summary>
        public bool OpenRegistion { get; set; }
        /// <summary>
        /// 注册方式
        /// </summary>
        public int RegisteType { get; set; }
        /// <summary>
        /// 注册流程
        /// </summary>
        public int RegisteFlow { get; set; }
        /// <summary>
        /// 协议地址
        /// </summary>
        public string ProtoUrl { get; set; }
    }
}
