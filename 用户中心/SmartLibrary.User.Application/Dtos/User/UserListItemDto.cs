/*********************************************************
* 名    称：UserListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户列表数据
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.User
{
    /// <summary>
    /// 用户列表数据
    /// </summary>
    public class UserListItemDto
    {
        public UserListItemDto()
        {
            Properties = new List<UserPropertyItemDto>();
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 学工号
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
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
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
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
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
        /// 首次登陆时间
        /// </summary>
        public DateTime? FirstLoginTime { get; set; }
        /// <summary>
        /// 最近登陆时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 用户附加属性
        /// </summary>
        public List<UserPropertyItemDto> Properties { get; set; }


        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 卡条码
        /// </summary>
        public string CardBarCode { get; set; }
        /// <summary>
        /// 卡物理码
        /// </summary>
        public string CardPhysicNo { get; set; }
        /// <summary>
        /// 卡认证码
        /// </summary>
        public string CardIdentityNo { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public int? CardStatus { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        public string CardType { get; set; }
        public DateTime? CardIssueDate { get; set; }
        public DateTime? CardExpireDate { get; set; }
    }
}
