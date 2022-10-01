/*********************************************************
* 名    称：UserImportResultDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户导入结果
* 更新历史：
*
* *******************************************************/

namespace SmartLibrary.User.Application.Dtos.User
{
    /// <summary>
    /// 用户导入结果
    /// </summary>
    public class UserImportResultDto
    {
        /// <summary>
        /// 成功条数
        /// </summary>
        public int SucCount { get; set; }
        /// <summary>
        /// 失败条数
        /// </summary>
        public int ErrCount { get; set; }
    }
}
