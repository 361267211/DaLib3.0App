/*********************************************************
* 名    称：PropertyChangeLogDetailItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：属性变更详情
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.DataApprove
{
    /// <summary>
    /// 属性变更详情
    /// </summary>
    public class PropertyChangeLogDetailItemDto
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public Guid LogId { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 变动字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 变动字段编码
        /// </summary>
        public string FieldCode { get; set; }
        /// <summary>
        /// 变动类型
        /// </summary>
        public int ChangeType { get; set; }
        /// <summary>
        /// 原值
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// 新值
        /// </summary>
        public string NewValue { get; set; }
    }
}
