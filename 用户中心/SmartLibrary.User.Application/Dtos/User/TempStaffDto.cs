/*********************************************************
* 名    称：TempStaffDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：临时馆员数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.User
{
    /// <summary>
    /// 临时馆员创建
    /// </summary>
    public class TempStaffDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 读者姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        public string Edu { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Depart { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 身份证号/护照号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 颁发日期
        /// </summary>
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public string Status { get; set; }
    }
}
