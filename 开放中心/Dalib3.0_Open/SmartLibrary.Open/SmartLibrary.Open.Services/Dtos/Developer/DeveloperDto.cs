using Furion.DataValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos
{
    /// <summary>
    /// 开发者数据模型
    /// </summary>
    public class DeveloperDto
    {
        /// <summary>
        /// 开发者ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请输入供应商名称")]
        [MaxLength(50, ErrorMessage = "供应商名称最多输入50个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "请输入账号")]
        [MaxLength(50, ErrorMessage = "账号最多输入20个字符")]
        public string Account { get; set; }
        /// <summary>
        /// 密码，需要加密
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>

        public string Email { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Required(ErrorMessage = "请输入联系电话")]
        [DataValidation(ValidationTypes.PhoneOrTelNumber, ErrorMessage = "请输入联系电话")]
        public string Mobile { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(50, ErrorMessage = "地址最多输入50个字符")]
        public string Address { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(500, ErrorMessage = "描述信息最多输入500个字符")]
        public string Desc { get; set; }
    }
}
