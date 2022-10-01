/*********************************************************
* 名    称：StaffListItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：馆员列表数据
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.Permission;
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 馆员列表数据
    /// </summary>
    public class StaffListItemOutput
    {
        public StaffListItemOutput()
        {
            Roles = new List<SysRoleDto>();
        }
        public List<SysRoleDto> Roles { get; set; }
        /// <summary>
        /// 馆员ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string Depart { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartName { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 学工号
        /// </summary>
        public string StudentNo { get; set; }
        /// <summary>
        /// 主卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public int? CardStatus { get; set; }
        /// <summary>
        /// 卡状态名称
        /// </summary>
        public string CardStatusName { get; set; }
        /// <summary>
        /// 卡截止日期
        /// </summary>
        public DateTime? CardExpireDate { get; set; }
        /// <summary>
        /// 成为馆员时间
        /// </summary>
        public DateTime? StaffBeginTime { get; set; }
    }
}
