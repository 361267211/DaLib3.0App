/*********************************************************
* 名    称：ApproveLogChangeInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性组数据变更
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性组数据变更
    /// </summary>
    public class ApproveLogChangeInput
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public Guid LogID { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool Passed { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
