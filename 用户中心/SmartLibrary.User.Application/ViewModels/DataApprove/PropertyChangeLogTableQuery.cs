/*********************************************************
* 名    称：PropertyChangeLogTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性变更日志查询
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性变更日志查询
    /// </summary>
    public class PropertyChangeLogTableQuery : BaseChangeLogTableQuery
    {
        public PropertyChangeLogTableQuery()
        {
            LogStatus = new List<int>();
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid? PropertyID { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public int? PropertyType { get; set; }
        /// <summary>
        /// 日志状态
        /// </summary>
        public List<int> LogStatus { get; set; }

    }

    public class EncodePropertyChangeLogTableQuery: PropertyChangeLogTableQuery { }
}
