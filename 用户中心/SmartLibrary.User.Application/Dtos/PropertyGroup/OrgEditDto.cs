/*********************************************************
* 名    称：OrgEditDto.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220302
* 描    述：组织机构管理
* 更新历史：
*
* *******************************************************/
using System;

namespace SmartLibrary.User.Application.Dtos.PropertyGroup
{
    /// <summary>
    /// 组织机构管理
    /// </summary>
    public class OrgEditDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid? ID { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public string PId { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
