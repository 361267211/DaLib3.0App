/*********************************************************
* 名    称：PropertyListItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：属性列表
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.Property
{
    /// <summary>
    /// 属性列表
    /// </summary>
    public class PropertyListItemDto
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool Unique { get; set; }
        /// <summary>
        /// 读者属性
        /// </summary>
        public bool ForReader { get; set; }
        /// <summary>
        /// 卡属性
        /// </summary>
        public bool ForCard { get; set; }
        /// <summary>
        /// 是否列表展示
        /// </summary>
        public bool ShowOnTable { get; set; }
        /// <summary>
        /// 是否可检索
        /// </summary>
        public bool CanSearch { get; set; }
        /// <summary>
        /// 数据状态
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
        /// <summary>
        /// 属性组ID
        /// </summary>
        public Guid? PropertyGroupID { get; set; }
    }
}
