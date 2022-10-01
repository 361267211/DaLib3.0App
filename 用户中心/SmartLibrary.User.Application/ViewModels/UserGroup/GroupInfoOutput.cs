/*********************************************************
* 名    称：GroupInfoOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组信息
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.UserGroup;
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 用户组信息
    /// </summary>
    public class GroupInfoOutput
    {
        public GroupInfoOutput()
        {
            Rules = new List<PropertyGroupRuleDto>();
            UserInfos = new List<GroupUserInfoOutput>();
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
        /// 备注
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 数据来源 0:规则创建 1:手动创建
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 创建规则
        /// </summary>
        public List<PropertyGroupRuleDto> Rules { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public List<GroupUserInfoOutput> UserInfos { get; set; }
    }
}
