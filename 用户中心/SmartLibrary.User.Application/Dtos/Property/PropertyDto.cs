/*********************************************************
* 名    称：PropertyDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：属性对象
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Common.Dtos;
using System;

namespace SmartLibrary.User.Application.Dtos.Property
{
    /// <summary>
    /// 属性对象
    /// </summary>
    public class PropertyDto
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [LogProperty(0, "属性名称", nameof(Name))]
        public string Name { get; set; }
        /// <summary>
        /// 用于描述读者
        /// </summary>
        [LogProperty(1, "用户属性", nameof(ForReader))]
        public bool ForReader { get; set; }
        /// <summary>
        /// 用于描述卡
        /// </summary>
        [LogProperty(2, "卡属性", nameof(ForCard))]
        public bool ForCard { get; set; }
        /// <summary>
        /// 属性标识
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        [LogProperty(3, "描述信息", nameof(Intro))]
        public string Intro { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 是否可检索
        /// </summary>
        [LogProperty(4, "是否可检索", nameof(CanSearch))]
        public bool CanSearch { get; set; }
        /// <summary>
        /// 是否列表显示
        /// </summary>
        [LogProperty(5, "是否列表显示", nameof(ShowOnTable))]
        public bool ShowOnTable { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        [LogProperty(6, "是否必填", nameof(Required))]
        public bool Required { get; set; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        [LogProperty(7, "是否唯一", nameof(Unique))]
        public bool Unique { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int ApproveStatus { get; set; }
    }
}
