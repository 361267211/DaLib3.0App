/*********************************************************
* 名    称：AppBranchViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210912
* 描    述：应用类型视图模型
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用类型视图
    /// </summary>
    public class AppBranchViewModel
    {
        /// <summary>
        /// 应用分支ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 部署环境
        /// </summary>
        public string DeployeeID { get; set; }
        /// <summary>
        /// 分支名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 当前版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 是否主分支
        /// </summary>
        public bool IsMaster { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
