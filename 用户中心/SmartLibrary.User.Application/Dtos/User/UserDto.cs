/*********************************************************
* 名    称：UserDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户信息编辑
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Common.Dtos;
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.User
{
    /// <summary>
    /// 用户信息编辑
    /// </summary>
    public class UserDto
    {
        public UserDto()
        {
            Properties = new List<UserPropertyDto>();
            GroupIds = new List<Guid>();
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户来源
        /// </summary>
        [LogProperty(-1, "用户来源", nameof(SourceFrom))]
        public int SourceFrom { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [LogProperty(0, "姓名", nameof(Name))]
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [LogProperty(1, "昵称", nameof(NickName))]
        public string NickName { get; set; }
        /// <summary>
        /// 学工号
        /// </summary>
        [LogProperty(2, "学工号", nameof(StudentNo))]
        public string StudentNo { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [LogProperty(3, "单位名称", nameof(Unit))]
        public string Unit { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [LogProperty(4, "性别", nameof(Gender))]
        public string Gender { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        [LogProperty(5, "学历", nameof(Edu))]
        public string Edu { get; set; }
        /// <summary>
        /// 出生年月
        /// </summary>
        [LogProperty(6, "出生年月", nameof(Birthday))]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        [LogProperty(7, "职称", nameof(Title))]
        public string Title { get; set; }
        /// <summary>
        /// 所在地
        /// </summary>
        [LogProperty(8, "所在地", nameof(Addr))]
        public string Addr { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        [LogProperty(9, "部门编码", nameof(Depart))]
        public string Depart { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        [LogProperty(9, "部门名称", nameof(DepartName))]
        public string DepartName { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        [LogProperty(10, "详细地址", nameof(AddrDetail))]
        public string AddrDetail { get; set; }
        /// <summary>
        /// 学院编码
        /// </summary>
        [LogProperty(11, "学院编码", nameof(College))]
        public string College { get; set; }
        /// <summary>
        /// 学院名称
        /// </summary>
        [LogProperty(11, "学院名称", nameof(CollegeName))]
        public string CollegeName { get; set; }
        /// <summary>
        /// 所在系编码
        /// </summary>
        [LogProperty(12, "系编码", nameof(CollegeDepart))]
        public string CollegeDepart { get; set; }
        /// <summary>
        /// 所在系名称
        /// </summary>
        [LogProperty(12, "系名称", nameof(CollegeDepartName))]
        public string CollegeDepartName { get; set; }
        /// <summary>
        /// 最后登录日期
        /// </summary>
        [LogProperty(12, "最后登录日期", nameof(LastLoginTime))]
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        [LogProperty(13, "专业", nameof(Major))]
        public string Major { get; set; }
        /// <summary>
        /// 离校时间
        /// </summary>
        [LogProperty(14, "离校日期", nameof(LeaveTime))]
        public DateTime? LeaveTime { get; set; }
        /// <summary>
        /// 年级
        /// </summary>
        [LogProperty(15, "年级", nameof(Grade))]
        public string Grade { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [LogProperty(16, "状态", nameof(Status))]
        public int Status { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        [LogProperty(17, "班级", nameof(Class))]
        public string Class { get; set; }
        /// <summary>
        /// 类型编码
        /// </summary>
        [LogProperty(18, "用户类型编码", nameof(Type))]
        public string Type { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        [LogProperty(18, "用户类型名称", nameof(TypeName))]
        public string TypeName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [LogProperty(20, "身份证号", nameof(IdCard))]
        public string IdCard { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [LogProperty(19, "手机", nameof(Phone))]
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [LogProperty(21, "邮箱", nameof(Email))]
        public string Email { get; set; }
        /// <summary>
        /// 头像照片
        /// </summary>
        [LogProperty(22, "头像", nameof(Photo))]
        public string Photo { get; set; }
        /// <summary>
        /// 是否馆员
        /// </summary>
        public bool IsStaff { get; set; }
        /// <summary>
        /// 馆员状态
        /// </summary>
        public int StaffStatus { get; set; }
        /// <summary>
        /// 成为馆员时间
        /// </summary>
        public DateTime? StaffBeginTime { get; set; }

        /// <summary>
        /// 用户属性
        /// </summary>
        public List<UserPropertyDto> Properties { get; set; }
        /// <summary>
        /// 用户组Id
        /// </summary>
        public List<Guid> GroupIds { get; set; }
        ///// <summary>
        ///// 用户标识
        ///// </summary>
        //public string UserKey { get; set; }
    }
}
