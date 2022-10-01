/*********************************************************
* 名    称：ReaderInfo.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20211125
* 描    述：待同步读者信息
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace TaskManager.Model.Reader
{
    /// <summary>
    /// 字段类型
    /// </summary>
    public enum EnumFieldType
    {
        文本 = 0,
        数值 = 1,
        时间 = 2,
        是非 = 3,
        属性组 = 4,
        地址 = 5,
        附件 = 6
    }
    /// <summary>
    /// 读者扩展数据
    /// </summary>
    public class ExtFieldInfo
    {
        public string FieldName { get; set; }
        public int FieldType { get; set; }
        public string FieldValue { get; set; }
    }

    /// <summary>
    /// 读者数据
    /// </summary>
    public class ReaderInfo
    {
        public ReaderInfo()
        {
            ExtFields = new List<ExtFieldInfo>();
        }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public List<ExtFieldInfo> ExtFields { get; set; }
        /// <summary>
        /// 主键标识
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
        /// 部门编码
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
        /// 用户类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 用户类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 用户状态
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
        /// 电子邮件
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
        /// 头像
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// 离校时间
        /// </summary>
        public DateTime? LeaveTime { get; set; }
        /// <summary>
        /// 读者同步Id
        /// </summary>
        public string AsyncReaderId { get; set; }
        /// <summary>
        /// 是否馆员
        /// </summary>
        public bool IsStaff { get; set; }
        /// <summary>
        /// 馆员状态
        /// </summary>
        public int? StaffStatus { get; set; }
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
        /// 删除标记
        /// </summary>
        public bool DeleteFlag { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string CardBarCode { get; set; }
        /// <summary>
        /// 物理码
        /// </summary>
        public string CardPhysicNo { get; set; }
        /// <summary>
        /// 认证号
        /// </summary>
        public string CardIdentityNo { get; set; }
        /// <summary>
        /// 卡类型 
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 卡类型名称
        /// </summary>
        public string CardTypeName { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public int CardStatus { get; set; }
        ///// <summary>
        ///// 是否主卡
        ///// </summary>
        //public bool IsPrincipal { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 卡密码
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 原始卡密码
        /// </summary>
        public string OriSecret { get; set; }
        ///// <summary>
        ///// 用途 0:无指定用途 1:临时馆员卡登陆凭据
        ///// </summary>
        //public int Usage { get; set; }
        ///// <summary>
        ///// 系统内置
        ///// </summary>
        //public bool SysBuildIn { get; set; }

    }
}
