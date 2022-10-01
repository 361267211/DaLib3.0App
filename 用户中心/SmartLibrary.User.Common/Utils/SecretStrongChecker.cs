/*********************************************************
* 名    称：SecretCheckResult.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：密码强度校验结果
* 更新历史：
*
* *******************************************************/
using System.Text.RegularExpressions;

namespace SmartLibrary.User.Common.Utils
{
    /// <summary>
    /// 密码强度校验结果
    /// </summary>
    public class SecretCheckResult
    {
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool Ok { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Err { get; set; }
    }

    /// <summary>
    /// 密码强度校验器
    /// </summary>
    public static class SecretStrongChecker
    {
        public static SecretCheckResult Check(string secret, int level)
        {
            var Level2Regex = new Regex(@"
                (?=.*[0-9])                     #必须包含数字
                (?=.*[a-z])                     #必须包含小写字母
                (?=.*[A-Z])                     #必须包含大写字母
                (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
                .{8,30}                         #至少8个字符，最多30个字符
            ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            var Level1Regex = new Regex(@"
                (?=.*[0-9])                     #必须包含数字
                (?=.*[a-z])                     #必须包含小写字母
                (?=.*[A-Z])                     #必须包含大写字母
                .{8,30}                         #至少8个字符，最多30个字符
            ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            var Level0Regex = new Regex(@"
                (?=.*[0-9])                     #必须包含数字
                (?=.*[a-zA-Z])                  #必须包含字母
                .{6,30}                         #至少6个字符，最多30个字符
            ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            var result = new SecretCheckResult
            {
                Ok = true,
                Err = ""
            };
            switch (level)
            {
                case 0:
                    if (!Level0Regex.IsMatch(secret))
                    {
                        result.Ok = false;
                        result.Err = "密码需为6-30个字符，且包含数字、字母";
                    }
                    break;
                case 1:

                    if (!Level1Regex.IsMatch(secret))
                    {
                        result.Ok = false;
                        result.Err = "密码需为8-30个字符，且包含数字、大写字母、小写字母";
                    }
                    break;

                case 2:
                    if (!Level2Regex.IsMatch(secret))
                    {
                        result.Ok = false;
                        result.Err = "密码需为8-30个字符，且包含数字、大写字母、小写字母，特殊字符";
                    }
                    break;
                default:
                    if (!Level2Regex.IsMatch(secret))
                    {
                        result.Ok = false;
                        result.Err = "密码需为8-30个字符，且包含数字、大写字母、小写字母，特殊字符";
                    }
                    break;
            }
            return result;
        }
    }
}
