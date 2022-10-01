/*********************************************************
* 名    称：AppDynamicInfoViewModel.cs
* 作    者：刘启平
* 联系方式：电话[13627622058],邮件[83379242@qq.com]
* 创建时间：20210909
* 描    述：应用动态视图模型
* 更新历史：
*
* *******************************************************/
using SmartLibrary.Open.Common.Enums;
using System;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 应用动态视图模型
    /// </summary>
    public class AppDynamicInfoViewModel
    {
        /// <summary>
        /// 动态ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        public Guid AppID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string AppIcon { get; set; }

        /// <summary>
        /// 应用分支ID
        /// </summary>
        public string AppBranchID { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public int InfoType { get; set; }
        /// <summary>
        /// 消息类型名称
        /// </summary>
        public string InfoTypeName
        {
            get
            {
                return ((EnumInfoType)InfoType).ToString();
            }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 消息ID
        /// </summary>
        public Guid InfoID { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息状态,0：正常，1：置顶
        /// </summary>
        public int Status { get; set; }
    }
}
