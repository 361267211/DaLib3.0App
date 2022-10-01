/*********************************************************
* 名    称：StaffRoleTableQuery.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：馆员角色查询
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos;
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 馆员角色查询
    /// </summary>
    public class StaffRoleTableQuery : TableQueryBase
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid? RoleId { get; set; }
        /// <summary>
        /// 排除角色Id
        /// </summary>
        public Guid? ExcludeRoleId { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 学工号
        /// </summary>
        public string StudentNo { get; set; }
    }

    public class StaffRoleEncodeTableQuery : StaffRoleTableQuery
    {

    }
}
