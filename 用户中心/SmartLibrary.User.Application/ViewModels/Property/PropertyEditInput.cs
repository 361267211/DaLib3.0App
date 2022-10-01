/*********************************************************
* 名    称：PropertyEditInput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：属性编辑输入
* 更新历史：
*
* *******************************************************/
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性编辑输入
    /// </summary>
    public class PropertyEditInput
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "请填写属性名称")]
        [MaxLength(20, ErrorMessage = "属性名称最多输入20个字符")]
        public string Name { get; set; }
        /// <summary>
        /// 用于描述读者
        /// </summary>
        public bool ForReader { get; set; }
        /// <summary>
        /// 用于描述卡
        /// </summary>
        public bool ForCard { get; set; }
        /// <summary>
        /// 属性标识
        /// </summary>
        [Required(ErrorMessage = "请填写属性标识")]
        [MaxLength(20, ErrorMessage = "属性标识最多输入20个字符")]
        public string Code { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        [MaxLength(20, ErrorMessage = "属性说明最多输入50个字符")]
        public string Intro { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 是否可检索
        /// </summary>
        public bool CanSearch { get; set; }
        /// <summary>
        /// 是否列表显示
        /// </summary>
        public bool ShowOnTable { get; set; }
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }
        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool Unique { get; set; }

    }
}
