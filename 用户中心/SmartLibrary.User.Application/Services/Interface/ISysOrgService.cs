/*********************************************************
* 名    称：ISysOrgService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：组织机构服务
* 更新历史：
*
* *******************************************************/
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Interface
{
    /// <summary>
    /// 组织机构服务
    /// </summary>
    public interface ISysOrgService
    {
        /// <summary>
        /// 创建组织机构
        /// </summary>
        /// <param name="orgData"></param>
        /// <returns></returns>
        Task<Guid> CreateOrg(OrgEditDto orgData);
        /// <summary>
        /// 编辑组织机构
        /// </summary>
        /// <param name="orgData"></param>
        /// <returns></returns>
        Task<Guid> UpdateOrg(OrgEditDto orgData);
        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteOrg(Guid id);
        /// <summary>
        /// 获取组织机构
        /// </summary>
        /// <returns></returns>
        Task<List<SysOrgOutput>> GetOrgTree();
        /// <summary>
        /// 获取组织机构列表
        /// </summary>
        /// <returns></returns>
        Task<List<SysOrgOutput>> GetOrgList();
    }
}
