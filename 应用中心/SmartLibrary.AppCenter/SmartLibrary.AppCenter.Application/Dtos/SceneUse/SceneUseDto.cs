using System.Collections.Generic;

namespace SmartLibrary.AppCenter.Application.Dtos.SceneUse
{
    /// <summary>
    /// 应用中心-栏目信息
    /// </summary>
    public class SceneUseDto
    {
        /// <summary>
        /// 更多链接
        /// </summary>
        public string MoreUrl { get; set; }

        /// <summary>
        /// 栏目集合
        /// </summary>
        public List<SceneUseItemDto> Items { get; set; }
    }

    /// <summary>
    /// 每一个栏目信息
    /// </summary>
    public class SceneUseItemDto
    {
        /// <summary>
        /// 栏目ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用集合
        /// </summary>
        public List<SceneUseAppInfoDto> Apps { get; set; }
    }

    /// <summary>
    /// 栏目下的应用信息
    /// </summary>
    public class SceneUseAppInfoDto
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 前台访问地址
        /// </summary>
        public string FrontUrl { get; set; }
    }
}
