/*********************************************************
* 名    称：UserImportTempDataDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户导入临时数据，考虑到数据量可能大，用中间表代替缓存
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.User
{
    /// <summary>
    /// 用户导入临时数据
    /// </summary>
    public class UserImportTempDataDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 操作ID
        /// </summary>
        public Guid BatchId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string UserGender { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string UserTypeName { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 教育情况
        /// </summary>
        public string Edu { get; set; }
        /// <summary>
        /// 学院编码
        /// </summary>
        public string College { get; set; }
        /// <summary>
        /// 学院名称
        /// </summary>
        public string CollegeName { get; set; }
        /// <summary>
        /// 系编码
        /// </summary>
        public string CollegeDepart { get; set; }
        /// <summary>
        /// 西名称
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
        /// 身份证号
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string AddrDetail { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 卡类型编码
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 卡类型名称
        /// </summary>
        public string CardTypeName { get; set; }
        /// <summary>
        /// 是否包含错误信息
        /// </summary>
        public bool Error { get; set; }
        /// <summary>
        /// 具体错误消息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserKey { get; set; }
    }
}
