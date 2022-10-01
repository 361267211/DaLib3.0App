/*********************************************************
* 名    称：UserUpdateMergeOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：读者更新合并
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 读者更新合并
    /// </summary>
    public class UserUpdateMergeOutput
    {
        /// <summary>
        /// 读者ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 重复手机号
        /// </summary>
        public bool RepeatePhone { get; set; }
        /// <summary>
        /// 重复身份证号
        /// </summary>
        public bool RepeateIdCard { get; set; }

    }
}
