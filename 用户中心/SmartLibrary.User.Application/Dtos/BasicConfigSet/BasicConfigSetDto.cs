/*********************************************************
* 名    称：BasicConfigSet.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：用户中心配置相关
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.BasicConfigSet
{
    /// <summary>
    /// 基础配置
    /// </summary>
    public class BasicConfigSetDto
    {
        /// <summary>
        /// 是否开启敏感信息过滤
        /// </summary>
        public bool SensitiveFilter { get; set; }
        /// <summary>
        /// 读者审批
        /// </summary>
        public bool UserInfoConfirm { get; set; }
        /// <summary>
        /// 属性审批
        /// </summary>
        public bool PropertyConfirm { get; set; }
        /// <summary>
        /// 读者卡认领
        /// </summary>
        public bool CardClaim { get; set; }
        /// <summary>
        /// 完善个人信息
        /// </summary>
        public bool UserInfoSupply { get; set; }
    }
    /// <summary>
    /// 用户组列表
    /// </summary>
    public class GroupListDto
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
        /// 读者数量
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// 用户组
    /// </summary>
    public class UserGroupDto
    {
        /// <summary>
        /// 用户组ID
        /// </summary>
        public Guid GroupId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }
    }
    /// <summary>
    /// 用户类型
    /// </summary>
    public class UserTypeDto
    {
        /// <summary>
        /// 用户类型ID
        /// </summary>
        public Guid GroupItemId { get; set; }
        /// <summary>
        /// 用户类型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户类型编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 用户数量
        /// </summary>
        public int Count { get; set; }
    }
}
