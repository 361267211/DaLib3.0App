/*********************************************************
* 名    称：PropertyGroupItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：202020302
* 描    述：属性组选项
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Common.Dtos;
using System;

namespace SmartLibrary.User.Application.Dtos.PropertyGroup
{
    /// <summary>
    /// 属性组选项
    /// </summary>
    public class PropertyGroupItemDto
    {
        /// <summary>
        /// 分组项
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 所属分组
        /// </summary>
        public Guid GroupID { get; set; }
        ///// <summary>
        ///// 父级ID
        ///// </summary>
        //public Guid ParentID { get; set; }
        /// <summary>
        /// 分组项目名称
        /// </summary>
        [LogProperty(0, "分组名称", nameof(Name))]
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [LogProperty(1, "分组编码", nameof(Code))]
        public string Code { get; set; }
        ///// <summary>
        ///// 等级
        ///// </summary>
        //public int Level { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int ApproveStatus { get; set; }
        /// <summary>
        /// 系统内置
        /// </summary>
        public bool SysBuildIn { get; set; }
    }
}
