/*********************************************************
* 名    称：UserTempDataListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者导入中间表数据
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者导入中间表数据
    /// </summary>
    public class UserTempDataListItemDto
    {
        /// <summary>
        /// 操作ID
        /// </summary>
        public Guid BatchId { get; set; }
        public string UserName { get; set; }
        public string UserGender { get; set; }
        public string UserPhone { get; set; }
        public string UserType { get; set; }
        public string UserTypeName { get; set; }
        public string StudentNo { get; set; }
        public string Unit { get; set; }
        public string Edu { get; set; }
        public string College { get; set; }
        public string CollegeName { get; set; }
        public string CollegeDepart { get; set; }
        public string CollegeDepartName { get; set; }
        public string Major { get; set; }
        public string Grade { get; set; }
        
        public string Class { get; set; }
        
        public string IdCard { get; set; }
        
        public string Email { get; set; }

        public DateTime? Birthday { get; set; }
        
        public string Addr { get; set; }
        public string AddrDetail { get; set; }
        public string CardNo { get; set; }
        
        public string CardType { get; set; }
        public string CardTypeName { get; set; }
        public bool Error { get; set; }
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
}
