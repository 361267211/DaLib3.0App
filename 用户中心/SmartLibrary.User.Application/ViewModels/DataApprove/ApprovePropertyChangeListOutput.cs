/*********************************************************
* 名    称：ApprovePropertyChangeListOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性变更审批记录
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性变更审批记录
    /// </summary>
    public class ApprovePropertyChangeListOutput
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 属性类型 0:属性 1:属性组
        /// </summary>
        public int PropertyType { get; set; }
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid PropertyID { get; set; }
        /// <summary>
        /// 修改类型
        /// </summary>
        public int ChangeType { get; set; }

        /// <summary>
        /// 修改时间
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
        /// 审批状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 变更内容
        /// </summary>
        public string Content { get; set; }
    }
}
