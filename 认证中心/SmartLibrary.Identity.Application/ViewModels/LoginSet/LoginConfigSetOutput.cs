using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Application.ViewModels
{
    /// <summary>
    /// 登录配置项
    /// </summary>
    public class LoginConfigSetOutput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 登录方式
        /// </summary>
        public int LoginType { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 登录方式名称
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录方式描述
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOpen { get; set; }
        /// <summary>
        /// 登录配置,保存为Json字符
        /// </summary>
        public string LoginConfig { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否需要配置
        /// </summary>
        public bool NeedConfig { get; set; }
    }
}
