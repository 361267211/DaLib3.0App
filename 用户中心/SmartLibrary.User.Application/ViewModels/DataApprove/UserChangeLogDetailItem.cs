/*********************************************************
* 名    称：UserChangeLogDetailInfo.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户信息变更日志记录
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户信息变更日志记录
    /// </summary>
    public class UserChangeLogDetailInfo
    {
        public UserChangeLogDetailInfo()
        {
            Users = new List<UserChangeLogDetailUser>();
            Details = new List<UserChangeLogDetailItem>();
        }

        public List<UserChangeLogDetailUser> Users { get; set; }
        public List<UserChangeLogDetailItem> Details { get; set; }
    }

    public class UserChangeLogDetailUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
        public string College { get; set; }
        /// <summary>
        /// 学院名称
        /// </summary>
        public string CollegeName { get; set; }
    }

    public class UserChangeLogDetailItem
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段编码
        /// </summary>
        public string FieldCode { get; set; }
        /// <summary>
        /// 变动类型
        /// </summary>
        public int ChangeType { get; set; }
        /// <summary>
        /// 原值
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; set; }
    }
}
