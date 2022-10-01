/*********************************************************
* 名    称：AppBranchViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210912
* 描    述：应用视图模型
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.SceneManage.Service.Dtos
{
    /// <summary>
    /// 应用视图模型
    /// </summary>
    public class AppListViewModel
    {
        /// <summary>
        /// 应用分支ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 分支名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 服务类型
        /// </summary>
        public string ServiceType { get; set; }
    }
}
