/*********************************************************
* 名    称：UserInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户输入
* 更新历史：
*
* *******************************************************/
using Furion.DataValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户输入
    /// </summary>
    public class UserInput
    {
        public UserInput()
        {
            Properties = new List<UserPropertyInput>();
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "请输入用户名称")]
        [MaxLength(20, ErrorMessage = "姓名最多输入20个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Edu { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Depart { get; set; }
        /// <summary>
        /// 学院系
        /// </summary>
        public string College { get; set; }
        /// <summary>
        /// 系
        /// </summary>
        public string CollegeDepart { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
        /// <summary>
        /// 年级
        /// </summary>
        public string Grade { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string Class { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Required(ErrorMessage = "请输入联系电话")]
        [DataValidation(ValidationTypes.PhoneNumber, ErrorMessage = "请输入正确格式手机号码")]
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 所在地
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string AddrDetail { get; set; }
        /// <summary>
        /// 头像照片
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 离校时间
        /// </summary>
        public DateTime? LeaveTime { get; set; }
        /// <summary>
        /// 是否馆员
        /// </summary>
        public bool IsStaff { get; set; }
        /// <summary>
        /// 馆员状态
        /// </summary>
        public int StaffStatus { get; set; }
        ///// <summary>
        ///// QQ开放ID
        ///// </summary>
        //public string QQID { get; set; }
        ///// <summary>
        ///// QQ绑定时间
        ///// </summary>
        //public DateTime? QQBindTime { get; set; }
        ///// <summary>
        ///// 微信开放ID
        ///// </summary>
        //public string WeChatID { get; set; }
        ///// <summary>
        ///// 微信绑定时间
        ///// </summary>
        //public DateTime? WeChatBindTime { get; set; }
        ///// <summary>
        ///// 身份证认证
        ///// </summary>
        //public bool IdCardIdentity { get; set; }
        ///// <summary>
        ///// 电话认证
        ///// </summary>
        //public bool MobileIdentity { get; set; }
        ///// <summary>
        ///// 邮箱认证
        ///// </summary>
        //public bool EmailIdentity { get; set; }
        /// <summary>
        /// 用户属性
        /// </summary>
        public List<UserPropertyInput> Properties { get; set; }
        /// <summary>
        /// 首次登陆时间
        /// </summary>
        public DateTime? FirstLoginTime { get; set; }
        /// <summary>
        /// 最近登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
    }
}
