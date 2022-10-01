/*********************************************************
* 名    称：TempStaffInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：临时馆员
* 更新历史：
*
* *******************************************************/
using Furion.DataValidation;
using SmartLibrary.User.Common.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 临时馆员
    /// </summary>
    public class TempStaffInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 读者姓名
        /// </summary>
        [Required(ErrorMessage = "请输入姓名")]
        [MaxLength(20, ErrorMessage = "姓名最多输入20个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 学工号
        /// </summary>
        [Required(ErrorMessage = "请输入学工号")]
        public string StudentNo { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        [MaxLength(50, ErrorMessage = "单位名称最多输入50个字符")]
        public string Unit { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        [MaxLength(20, ErrorMessage = "学历最多输入50个字符")]
        public string Edu { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [MaxLength(30, ErrorMessage = "部门最多输入50个字符")]
        public string Depart { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
        [MaxLength(30, ErrorMessage = "学院最多输入30个字符")]
        public string College { get; set; }
        /// <summary>
        /// 系
        /// </summary>
        [MaxLength(30, ErrorMessage = "系最多输入30个字符")]
        public string CollegeDepart { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        [MaxLength(30, ErrorMessage = "职称最多输入30个字符")]
        public string Title { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "请输入手机号码")]
        [DataValidation(ValidationTypes.PhoneNumber, ErrorMessage = "请输入正确格式手机号码")]
        public string Phone { get; set; }
        /// <summary>
        /// 身份证号/护照号
        /// </summary>
        [DataValidation(ValidationPattern.AtLeastOne, ExtensionValidationTypes.Empty, ValidationTypes.IDCard, ExtensionValidationTypes.HKCard, ExtensionValidationTypes.TWCard, ExtensionValidationTypes.Passport, ErrorMessage = "请输入正确格式身份证号或护照号")]
        public string IdCard { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 颁发日期
        /// </summary>
        [Required(ErrorMessage = "请输入有效期")]
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        [Required(ErrorMessage = "请输入有效期")]
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        [Required(ErrorMessage = "请选择状态")]
        public int Status { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Photo { get; set; }
    }
}
