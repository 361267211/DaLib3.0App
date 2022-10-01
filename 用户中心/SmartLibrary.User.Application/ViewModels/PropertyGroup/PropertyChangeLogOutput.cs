/*********************************************************
* 名    称：GroupChangeLogTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性组变更日志记录查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性组变更日志记录查询
    /// </summary>
    public class GroupChangeLogTableQuery : TableQueryBase
    {
        public Guid GroupId { get; set; }
    }
    public class PropertyChangeLogOutput
    {
        public PropertyChangeLogOutput()
        {
            Logs = new PagedList<ChangeLogMain>();
            LogItems = new List<ChangeLogItem>();
        }
        public PagedList<ChangeLogMain> Logs { get; set; }
        public List<ChangeLogItem> LogItems { get; set; }
    }


    public class ChangeLogMain
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; set; }
        /// <summary>
        /// 变更操作人
        /// </summary>
        public string ChangeUserName { get; set; }
        /// <summary>
        /// 变更概述
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 变更类型
        /// </summary>
        public int ChangeType { get; set; }
    }
    /// <summary>
    /// 变更详情
    /// </summary>
    public class ChangeLogItem
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        public Guid LogID { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 变动字段编码
        /// </summary>
        public string FieldCode { get; set; }
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
