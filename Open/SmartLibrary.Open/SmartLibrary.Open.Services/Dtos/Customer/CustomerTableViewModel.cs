/*********************************************************
* 名    称：CustomerTableViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210909
* 描    述：客户列表视图模型
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 客户列表视图模型
    /// </summary>
    public class CustomerTableViewModel
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Owner
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// 平台版本
        /// </summary>
        public string PlatformVersion { get; set; }
        /// <summary>
        /// 应用总数
        /// </summary>
        public int AppCount { get; set; }
        /// <summary>
        /// 临期应用数量
        /// </summary>
        public int AppDueSoonCount { get; set; }
        /// <summary>
        /// 过期应用
        /// </summary>
        public int AppExpiredCount { get; set; }

        /// <summary>
        /// 门户地址
        /// </summary>
        public string PortalUrl { get; set; }
        /// <summary>
        /// 后台地址
        /// </summary>
        public string ManageUrl { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 前台登录地址
        /// </summary>
        public string LoginUrl { get; set; }
        /// <summary>
        /// 后台登录地址
        /// </summary>
        public string MgrLoginUrl { get; set; }

        /// <summary>
        /// 机构Logo
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 机构简版Logo
        /// </summary>
        public string SimpleLogoUrl { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}
