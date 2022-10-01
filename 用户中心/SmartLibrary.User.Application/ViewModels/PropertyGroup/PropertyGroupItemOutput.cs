/*********************************************************
* 名    称：PropertyGroupItemOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性组选项输出
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性组选项输出
    /// </summary>
    public class PropertyGroupItemOutput
    {
        /// <summary>
        /// 分组项
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 所属分组
        /// </summary>
        public Guid GroupID { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid ParentID { get; set; }
        /// <summary>
        /// 分组项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 数据状态，0：未激活，1：正常
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审批状态，0：待审批，1：正常
        /// </summary>
        public int ApproveStatus { get; set; }

    }
}
