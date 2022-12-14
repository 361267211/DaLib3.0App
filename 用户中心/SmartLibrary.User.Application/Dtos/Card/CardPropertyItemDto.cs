/*********************************************************
* 名    称：CardPropertyItemDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220301
* 描    述：卡属性选项
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.Card
{
    /// <summary>
    /// 卡属性选项
    /// </summary>
    public class CardPropertyItemDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 卡ID
        /// </summary>
        public Guid CardID { get; set; }
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid PropertyID { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 属性编码
        /// </summary>
        public string PropertyCode { get; set; }

        /// <summary>
        /// 属性类型
        /// </summary>
        public int PropertyType { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string PropertyValue { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool Required { get; set; }
    }
}
