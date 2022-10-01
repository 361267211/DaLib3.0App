/*********************************************************
* 名    称：PropertySearchSetDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：设置属性是否可检索
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.Property
{
    /// <summary>
    /// 设置属性是否可检索
    /// </summary>
    public class PropertySearchSetDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 是否可检索
        /// </summary>
        public bool CanSearch { get; set; }
    }
}
