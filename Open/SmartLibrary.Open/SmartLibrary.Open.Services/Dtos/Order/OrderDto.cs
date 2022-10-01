using Furion.DataValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 授权订单数据传输对象
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 应用ID
        /// </summary>
        [Required(ErrorMessage = "应用ID不能为空")]
        public Guid AppID { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [Required(ErrorMessage = "客户ID不能为空")]
        public Guid CustomerID { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public int AuthType { get; set; }
        /// <summary>
        /// 开通类型
        /// </summary>
        public int OpenType { get; set; }
        /// <summary>
        /// 开通途径
        /// </summary>
        public int Way { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        [Required(ErrorMessage = "请填写开始日期")]
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 截至日期
        /// </summary>
        [Required(ErrorMessage = "请填写截止日期")]
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [MinLength(2, ErrorMessage = "联系人输入2-50个字符")]
        [MaxLength(50, ErrorMessage = "联系人输入2-50个字符")]
        public string ContactMan { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(20, ErrorMessage = "联系电话输入0-20个字符")]
        [DataValidation(ValidationTypes.PhoneOrTelNumber, "请输入有效联系电话")]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyMan { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500, ErrorMessage = "备注输入0-500个字符")]
        public string Remark { get; set; }
    }
}
