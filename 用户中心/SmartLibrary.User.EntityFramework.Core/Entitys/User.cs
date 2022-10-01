using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.EntityFramework.Core.Entitys
{
    public class User : BaseEntity<Guid>
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(200)]
        public string NickName { get; set; }
        /// <summary>
        /// 学工号
        /// </summary>
        [StringLength(200)]
        public string StudentNo { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [StringLength(200)]
        public string Unit { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        [StringLength(200)]
        public string Edu { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        [StringLength(200)]
        public string Title { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [StringLength(200)]
        public string Depart { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
        [StringLength(200)]
        public string College { get; set; }
        /// <summary>
        /// 学院名称
        /// </summary>
        public string CollegeName { get; set; }

        /// <summary>
        /// 系
        /// </summary>
        public string CollegeDepart { get; set; }
        /// <summary>
        /// 系名称
        /// </summary>
        public string CollegeDepartName { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        [StringLength(200)]
        public string Major { get; set; }
        /// <summary>
        /// 年级
        /// </summary>
        [StringLength(200)]
        public string Grade { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        [StringLength(200)]
        public string Class { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Type { get; set; }
        /// <summary>
        /// 用户类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        [Required]
        public int Status { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(200)]
        public string IdCard { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Phone { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [StringLength(200)]
        public string Email { get; set; }
        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [StringLength(100)]
        public string Gender { get; set; }
        /// <summary>
        /// 所在地
        /// </summary>
        [StringLength(200)]
        public string Addr { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        [StringLength(500)]
        public string AddrDetail { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(500)]
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
        public int? StaffStatus { get; set; }
        /// <summary>
        /// 成为馆员时间
        /// </summary>
        public DateTime? StaffBeginTime { get; set; }
        /// <summary>
        /// 身份证号认证
        /// </summary>
        public bool IdCardIdentity { get; set; }
        /// <summary>
        /// 电话认证
        /// </summary>
        public bool MobileIdentity { get; set; }
        /// <summary>
        /// 邮箱认证
        /// </summary>
        public bool EmailIdentity { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 首次登陆时间
        /// </summary>
        public DateTime? FirstLoginTime { get; set; }
        /// <summary>
        /// 最近登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        [Required]
        [StringLength(100)]
        public string UserKey { get; set; }
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuperVisor { get; set; }

    }
}
