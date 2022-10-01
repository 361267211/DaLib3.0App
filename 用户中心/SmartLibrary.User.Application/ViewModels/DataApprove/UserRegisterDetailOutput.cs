/*********************************************************
* 名    称：UserRegisterDetailOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：注册审批详情
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 注册审批详情
    /// </summary>
    public class UserRegisterDetailOutput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Unit { get; set; }
        public string Edu { get; set; }
        public string Title { get; set; }
        public string Depart { get; set; }
        public string DepartName { get; set; }
        public string College { get; set; }
        public string CollegeName { get; set; }
        public string CollegeDepart { get; set; }
        public string CollegeDepartName { get; set; }
        public string Major { get; set; }
        public string Grade { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        public string Addr { get; set; }
        public string AddrDetail { get; set; }
        public string Photo { get; set; }
    }
}
