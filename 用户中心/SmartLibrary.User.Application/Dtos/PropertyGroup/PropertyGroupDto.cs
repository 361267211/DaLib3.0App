/*********************************************************
* 名    称：PropertyGroupDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20211112
* 描    述：属性组对象
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.Dtos.PropertyGroup
{
    /// <summary>
    /// 属性组对象
    /// </summary>
    public class PropertyGroupDto
    {
        public PropertyGroupDto()
        {
            Items = new List<PropertyGroupItemDto>();
        }
        /// <summary>
        /// 主键标识
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 系统内置则不可修改
        /// </summary>
        public bool SysBuildIn { get; set; }
        /// <summary>
        /// 编码必填
        /// </summary>
        public bool RequiredCode { get; set; }

        public List<PropertyGroupItemDto> Items { get; set; }
    }
}
