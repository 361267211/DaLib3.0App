/*********************************************************
* 名    称：UserTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：用户查询条件
* 更新历史：
*
* *******************************************************/
using SmartLibrary.ScoreCenter.Application.Dtos;
using System;
using System.Collections.Generic;

namespace SmartLibrary.ScoreCenter.Application.ViewModels
{
    /// <summary>
    /// 用户组查询
    /// </summary>
    public class GroupUserTableQuery : TableQueryBase
    {
        public GroupUserTableQuery()
        {
            GroupIds = new List<string>();
        }
        public List<string> GroupIds { get; set; }
    }
    /// <summary>
    /// 用户类型查询
    /// </summary>
    public class TypeUserTableQuery : TableQueryBase
    {
        public TypeUserTableQuery()
        {
            UserTypes = new List<string>();
        }
        public List<string> UserTypes { get; set; }
    }
    /// <summary>
    /// 用户查询
    /// </summary>

    public class UserTableQuery : TableQueryBase
    {
        public string Name { get; set; }
        public string NickName { get; set; }
        public string StudentNo { get; set; }
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
        public int? Status { get; set; }
        public string IDCard { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthdayStartTime { get; set; }
        public DateTime? BirthdayEndTime { get; set; }
        public DateTime? BirthdayEndCompareTime
        {
            get
            {
                if (BirthdayEndTime.HasValue)
                {
                    return BirthdayEndTime.Value.AddDays(1);
                }
                return null;
            }
        }
        public string Gender { get; set; }
        public string Addr { get; set; }
        public string AddrDetail { get; set; }
        public Guid? GroupID { get; set; }
        public int? SourceFrom { get; set; }
        public DateTime? LastLoginStartTime { get; set; }
        public DateTime? LastLoginEndTime { get; set; }
        public DateTime? LastLoginEndCompareTime
        {
            get
            {
                if (LastLoginEndTime.HasValue)
                {
                    return LastLoginEndTime.Value.AddDays(1);
                }
                return null;
            }
        }

        public DateTime? LeaveStartTime { get; set; }
        public DateTime? LeaveEndTime { get; set; }
        public DateTime? LeaveEndCompareTime
        {
            get
            {
                if (LeaveEndTime.HasValue)
                {
                    return LeaveEndTime.Value.AddDays(1);
                }
                return null;
            }
        }
        public string CardNo { get; set; }
        public string CardBarCode { get; set; }
        public string CardPhysicNo { get; set; }
        public string CardIdentityNo { get; set; }
        public bool? CardIsPrincipal { get; set; }
        public string CardType { get; set; }
        public string CardTypeName { get; set; }
        public int? CardStatus { get; set; }
        public DateTime? CardIssueStartTime { get; set; }
        public DateTime? CardIssueEndTime { get; set; }
        public DateTime? CardIssueEndCompareTime
        {
            get
            {
                if (CardIssueEndTime.HasValue)
                {
                    return CardIssueEndTime.Value.AddDays(1);
                }
                return null;
            }
        }
        public DateTime? CardExpireStartTime { get; set; }
        public DateTime? CardExpireEndTime { get; set; }
        public DateTime? CardExpireEndCompareTime
        {
            get
            {
                if (CardExpireEndTime.HasValue)
                {
                    return CardExpireEndTime.Value.AddDays(1);
                }
                return null;
            }
        }
        /// <summary>
        /// 是否馆员
        /// </summary>
        public bool? IsStaff { get; set; }
    }
}
