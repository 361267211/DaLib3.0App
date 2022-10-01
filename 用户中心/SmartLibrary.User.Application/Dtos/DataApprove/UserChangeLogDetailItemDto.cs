/*********************************************************
* 名    称：UserChangeLogDetailInfoDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：用户信息变更详情记录
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.DataApprove
{
    /// <summary>
    /// 用户信息变更详情记录
    /// </summary>
    public class UserChangeLogDetailInfoDto
    {
        public UserChangeLogDetailInfoDto()
        {
            Users = new List<UserChangeLogDetailUserDto>();
            Details = new List<UserChangeLogDetailItemDto>();
        }
        /// <summary>
        /// 产生信息变更的用户
        /// </summary>
        public List<UserChangeLogDetailUserDto> Users { get; set; }
        /// <summary>
        /// 每个用户信息变更的明细
        /// </summary>
        public List<UserChangeLogDetailItemDto> Details { get; set; }
    }

    /// <summary>
    /// 用户变更过记录
    /// </summary>
    public class UserChangeLogDetailUserDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 院系
        /// </summary>
        public string College { get; set; }
        /// <summary>
        /// 学院名称
        /// </summary>
        public string CollegeName { get; set; }
    }
    /// <summary>
    /// 变更信息明细记录
    /// </summary>
    public class UserChangeLogDetailItemDto
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 字段编码
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
