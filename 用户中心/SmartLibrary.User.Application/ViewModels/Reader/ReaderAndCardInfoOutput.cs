/*********************************************************
* 名    称：ReaderAndCardInfoOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者及读者卡信息输出
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels.Reader
{
    /// <summary>
    /// 读者及读者卡信息输出
    /// </summary>
    public class ReaderAndCardInfoOutput
    {
        /// <summary>
        /// 读者ID
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
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
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
        /// 头像照片
        /// </summary>
        public string Photo { get; set; }
        public bool IsStaff { get; set; }
        public int StaffStatus { get; set; }
        public bool IdCardIdentity { get; set; }
        public bool MobileIdentity { get; set; }
        public bool EmailIdentity { get; set; }

        public Guid? CardId { get; set; }
        public string CardNo { get; set; }
        public int? CardStatus { get; set; }
        public string CardType { get; set; }
        public DateTime? CardIssueDate { get; set; }
        public DateTime? CardExpireDate { get; set; }
        /// <summary>
        /// 是否可修改读者信息
        /// </summary>
        public bool UpdateReaderInfo { get; set; }
    }
}
