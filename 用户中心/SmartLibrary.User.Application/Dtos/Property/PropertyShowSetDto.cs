/*********************************************************
* 名    称：PropertyShowSetDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：设置属性是否在列表展示
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.Property
{
    /// <summary>
    /// 设置属性是否在列表显示
    /// </summary>
    public class PropertyShowSetDto
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 是否在用户列表展示
        /// </summary>
        public bool ShowOnTable { get; set; }
    }
}
