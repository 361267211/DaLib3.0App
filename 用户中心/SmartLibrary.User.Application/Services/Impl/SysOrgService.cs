/*********************************************************
* 名    称：SysOrgService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：组织机构管理服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Dtos.PropertyGroup;
using SmartLibrary.User.Application.Services.Extensions;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 组织机构服务
    /// </summary>
    public class SysOrgService : ISysOrgService, IScoped
    {
        private readonly IDistributedIDGenerator _idGenerator;
        private readonly IRepository<SysOrg> _orgRepository;
        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="orgRepository"></param>
        /// <param name="idGenerator"></param>
        public SysOrgService(IRepository<SysOrg> orgRepository
            , IDistributedIDGenerator idGenerator)
        {
            _orgRepository = orgRepository;
            _idGenerator = idGenerator;
        }
        /// <summary>
        /// 创建组织机构
        /// </summary>
        /// <param name="orgData"></param>
        /// <returns></returns>
        public async Task<Guid> CreateOrg(OrgEditDto orgData)
        {
            orgData.ID = _idGenerator.CreateGuid(orgData.ID);
            var emptyGuid = Guid.Empty.ToString();
            var orgEntity = orgData.Adapt<SysOrg>();
            var pOrg = await _orgRepository.DetachedEntities.FirstOrDefaultAsync(x => x.FullPath == orgData.PId);
            if (pOrg == null)
            {
                var maxPath = 0;
                if (await _orgRepository.DetachedEntities.AnyAsync(x => x.Pid == emptyGuid))
                {
                    maxPath = await _orgRepository.DetachedEntities.Where(x => x.Pid == emptyGuid).MaxAsync(x => x.Path);
                }
                var path = ++maxPath;
                var fullPath = $".{path}.";
                orgEntity.Path = path;
                orgEntity.FullPath = fullPath;
                orgEntity.Pid = emptyGuid;
                orgEntity.FullName = await this.GetFullName(orgEntity);
                var orgEntry = await _orgRepository.InsertAsync(orgEntity);
                return orgEntry.Entity.Id;
            }
            else
            {
                var maxPath = 0;
                if (await _orgRepository.DetachedEntities.AnyAsync(x => x.Pid == pOrg.FullPath))
                {
                    maxPath = await _orgRepository.DetachedEntities.Where(x => x.Pid == pOrg.FullPath).MaxAsync(x => x.Path);
                }
                var path = ++maxPath;
                var fullPath = $"{pOrg.FullPath}{path}.";
                orgEntity.Path = path;
                orgEntity.FullPath = fullPath;
                orgEntity.Pid = pOrg.FullPath;
                orgEntity.FullName = await this.GetFullName(orgEntity);
                var orgEntry = await _orgRepository.InsertAsync(orgEntity);
                return orgEntry.Entity.Id;
            }
        }
        /// <summary>
        /// 修改组织机构
        /// </summary>
        /// <param name="orgData"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateOrg(OrgEditDto orgData)
        {
            var orgEntity = await _orgRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == orgData.ID);
            if (orgEntity == null)
            {
                throw Oops.Oh("未找到组织机构信息");
            }
            orgEntity.Name = orgData.Name;
            orgEntity.Code = orgData.Code;
            orgEntity.Remark = orgData.Remark;
            orgEntity.UpdateTime = DateTime.Now;
            orgEntity.FullName = await this.GetFullName(orgEntity);
            await _orgRepository.UpdateAsync(orgEntity);
            return orgEntity.Id;
        }

        /// <summary>
        /// 获取部门全称
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetFullName(SysOrg orgEntity)
        {
            var fullNameList = new List<string>();
            fullNameList.Add(orgEntity.Name);
            var topChainList = await _orgRepository.DetachedEntities.Where(x => orgEntity.FullPath.StartsWith(x.FullPath)).ToListAsync();
            if (topChainList.Any())
            {
                this.ComponentFullName(orgEntity, topChainList, fullNameList);
            }
            fullNameList.Reverse();
            return string.Join("/", fullNameList);
        }

        private void ComponentFullName(SysOrg orgEntity, List<SysOrg> topChainList, List<string> fullNameList)
        {
            var pNode = topChainList.FirstOrDefault(x => x.FullPath == orgEntity.Pid);
            if (pNode != null)
            {
                fullNameList.Add(pNode.Name);
                ComponentFullName(pNode, topChainList, fullNameList);
            }

        }
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOrg(Guid id)
        {
            var orgEntity = await _orgRepository.DetachedEntities.FirstOrDefaultAsync(x => x.Id == id);
            if (orgEntity == null)
            {
                throw Oops.Oh("未找到组织机构信息");
            }
            orgEntity.DeleteFlag = true;
            orgEntity.UpdateTime = DateTime.Now;
            await _orgRepository.UpdateAsync(orgEntity);
            return true;
        }
        /// <summary>
        /// 获取机构树
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysOrgOutput>> GetOrgTree()
        {
            var allOrgs = await _orgRepository.DetachedEntities.Where(x => !x.DeleteFlag).Select(x => new SysOrgOutput
            {
                ID = x.Id,
                PId = x.Pid,
                Name = x.Name,
                Code = x.Code,
                Remark = x.Remark,
                FullPath = x.FullPath,
                FullName = x.FullName,
                Path = x.Path,
            }).ToListAsync();
            var topNodes = allOrgs.Where(x => x.PId == "" || x.PId == null || x.PId == Guid.Empty.ToString()).ToList();
            foreach (var node in topNodes)
            {
                GetOrgChildren(node, allOrgs);
            }
            return topNodes;
        }


        private void GetOrgChildren(SysOrgOutput node, List<SysOrgOutput> orgs)
        {
            var children = orgs.Where(x => x.PId == node.FullPath).OrderBy(x => x.Path).ToList();
            if (children.Any())
            {
                node.Children = children;
                foreach (var child in node.Children)
                {
                    GetOrgChildren(child, orgs);
                }
            }
            else
            {
                node.Children = null;
            }

        }

        public async Task<List<SysOrgOutput>> GetOrgList()
        {
            var allOrgs = await _orgRepository.DetachedEntities.Where(x => !x.DeleteFlag).Select(x => new SysOrgOutput
            {
                ID = x.Id,
                PId = x.Pid,
                Name = x.Name,
                Code = x.Code,
                Remark = x.Remark,
                FullPath = x.FullPath,
                FullName = x.FullName,
                Path = x.Path,
            }).ToListAsync();
            return allOrgs;
        }

    }
}
