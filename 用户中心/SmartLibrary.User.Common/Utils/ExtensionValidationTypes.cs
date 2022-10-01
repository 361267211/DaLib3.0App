/*********************************************************
* 名    称：身份证校验正则表达式.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：ExtensionValidationTypes
* 更新历史：
*
* *******************************************************/
using Furion.DataValidation;

namespace SmartLibrary.User.Common.Utils
{
    [ValidationType]
    public enum ExtensionValidationTypes
    {
        [ValidationItemMetadata(@"^([A-Z]\d{6,10}(\(\w{1}\))?)$", "港澳身份证格式不正确")]
        HKCard,
        [ValidationItemMetadata(@"^\d{8}|^[a-zA-Z0-9]{10}|^\d{18}$", "台湾身份证格式不正确")]
        TWCard,
        [ValidationItemMetadata(@"^([a-zA-z]|[0-9]){5,17}$", "护照格式不正确")]
        Passport,
        [ValidationItemMetadata(@"^\s*$", "")]
        Empty
    }
}
