/*********************************************************
* 名    称：PropertyGroupOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20211112
* 描    述：属性信息
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 属性组输出
    /// </summary>
    public class PropertyGroupOutput
    {
        public PropertyGroupOutput()
        {
            Items = new List<PropertyGroupItemOutput>();
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

        public List<PropertyGroupItemOutput> Items { get; set; }
    }
}
