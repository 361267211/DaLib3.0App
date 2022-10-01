/*********************************************************
* 名    称：PropertyGroupRuleDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：用户组属性规则
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.UserGroup
{
    /// <summary>
    /// 用户组属性规则
    /// </summary>
    public class PropertyGroupRuleDto
    {
        /// <summary>
        /// 对比操作
        /// </summary>
        public int CompareType { get; set; }
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid PropertyId { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 属性编码
        /// </summary>
        public string PropertyCode { get; set; }
        /// <summary>
        /// 属性值
        /// </summary>
        public string PropertyValue { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 连接方式
        /// </summary>
        public int UnionWay { get; set; }
    }
}
