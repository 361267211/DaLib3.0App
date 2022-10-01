using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Application.Dtos.Application
{
    /// <summary>
    /// 应用日志详情
    /// </summary>
    public class AppLogDetailDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 日志标题
        /// </summary>
        public string Title {  get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppTilte { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime {  get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
    }
}
