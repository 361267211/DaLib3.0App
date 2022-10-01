/*********************************************************
* 名    称：PropertyChangeItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：属性变更日志
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.DataApprove
{
    /// <summary>
    /// 属性变更日志
    /// </summary>
    public class PropertyChangeItemDto
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        public Guid LogID { get; set; }
        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid PropertyID { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 前值
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// 变更后值
        /// </summary>
        public string NewValue { get; set; }
        /// <summary>
        /// 字段编码
        /// </summary>
        public string FieldCode { get; set; }
    }
}
