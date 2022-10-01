/*********************************************************
* 名    称：GroupUserInfoDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组数据信息
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.UserGroup
{
    /// <summary>
    /// 用户组数据信息
    /// </summary>
    public class GroupUserInfoDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public int SourceFrom { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
    }

}
