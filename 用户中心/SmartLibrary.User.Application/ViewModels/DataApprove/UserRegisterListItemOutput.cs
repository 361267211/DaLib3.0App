/*********************************************************
* 名    称：UserRegisterListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：注册审批列表信息
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 注册审批列表信息
    /// </summary>
    public class UserRegisterListItemOutput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 注册用户ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 注册用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 注册用户手机
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int Status { get; set; }
    }
}
