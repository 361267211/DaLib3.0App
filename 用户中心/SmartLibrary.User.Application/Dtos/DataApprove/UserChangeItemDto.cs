/*********************************************************
* 名    称：UserChangeItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：用户变更记录
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.DataApprove
{
    /// <summary>
    /// 用户变更记录
    /// </summary>
    public class UserChangeItemDto
    {
        /// <summary>
        /// 日志主记录
        /// </summary>
        public Guid LogID { get; set; }

        /// <summary>
        /// 读者ID
        /// </summary>
        public Guid UserID { get; set; }
        /// <summary>
        /// 是否字段
        /// </summary>
        public bool IsField { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public int PropertyType { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PropertyCode { get; set; }

        /// <summary>
        /// 旧值
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; set; }
    }
}
