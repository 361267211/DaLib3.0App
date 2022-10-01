/*********************************************************
* 名    称：GroupListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组列表数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户组列表数据
    /// </summary>
    public class GroupListItemOutput
    {
        /// <summary>
        /// 用户组ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
        /// <summary>
        /// 读者数
        /// </summary>
        public int UserCount { get; set; }
    }
}
