/*********************************************************
* 名    称：UserChangeListDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：用户信息变更记录
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.DataApprove
{
    /// <summary>
    /// 用户信息变更记录
    /// </summary>
    public class UserChangeListDto
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 变更类型
        /// </summary>
        public int ChangeType { get; set; }
        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public Guid ChangeUserID { get; set; }
        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string ChangeUserName { get; set; }
        /// <summary>
        /// 修改人电话
        /// </summary>
        public string ChangeUserPhone { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 变更内容
        /// </summary>
        public string Content { get; set; }
    }
}
