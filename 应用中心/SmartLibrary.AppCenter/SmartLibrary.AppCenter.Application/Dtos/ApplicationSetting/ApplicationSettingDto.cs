using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.ApplicationSetting
{
    /// <summary>
    /// 应用中心设置
    /// </summary>
    public class ApplicationSettingDto
    {
        /// <summary>
        /// 是否显示推荐收费应用
        /// </summary>
        public bool IsShowChargeApp { get; set; }

        /// <summary>
        /// 是否需要登录
        /// </summary>
        public bool IsNeedLogin { get; set; }
    }
}
