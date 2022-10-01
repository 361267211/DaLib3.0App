/*********************************************************
* 名    称：GroupBriefInfoOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组概要信息
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.UserGroup;
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户组概要信息
    /// </summary>
    public class GroupBriefInfoOutput
    {
        public GroupBriefInfoOutput()
        {
            Rules = new List<PropertyGroupRuleDto>();
        }
        /// <summary>
        /// 用户组ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 最近同步时间
        /// </summary>
        public DateTime? LastSyncTime { get; set; }
        /// <summary>
        /// 用户组数据来源
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 分组人数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 总人数
        /// </summary>
        public int TotalCount { get; set; }
        public decimal Percent
        {
            get
            {
                return (decimal)Count / TotalCount;
            }
        }
        /// <summary>
        /// 创建规则
        /// </summary>
        public List<PropertyGroupRuleDto> Rules { get; set; }
    }
}
