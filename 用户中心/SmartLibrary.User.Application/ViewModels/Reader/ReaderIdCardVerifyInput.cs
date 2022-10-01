/*********************************************************
* 名    称：ReaderIdCardVerifyInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：身份证号验证
* 更新历史：
*
* *******************************************************/
using Furion.DataValidation;
using SmartLibrary.User.Common.Utils;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 身份证号验证
    /// </summary>
    public class ReaderIdCardVerifyInput
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        [Required(ErrorMessage ="身份证号必填")]
        [DataValidation(ValidationPattern.AtLeastOne, ExtensionValidationTypes.TWCard, ExtensionValidationTypes.Passport, ExtensionValidationTypes.HKCard, ValidationTypes.IDCard,ErrorMessage = "身份证号格式错误")]
        public string IdCard { get; set; }
    }
}
