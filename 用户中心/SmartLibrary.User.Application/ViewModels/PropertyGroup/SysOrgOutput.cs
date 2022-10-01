/*********************************************************
* 名    称：SysOrgOutput.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：组织机构输出
* 更新历史：
*
* *******************************************************/
using System;
using System.Collections.Generic;

namespace SmartLibrary.User.Application.ViewModels
{
    /// <summary>
    /// 组织机构输出
    /// </summary>
    public class SysOrgOutput
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public string PId { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public int Path { get; set; }
        /// <summary>
        /// 全路径
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 全称
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public List<SysOrgOutput> Children { get; set; }
    }
}
