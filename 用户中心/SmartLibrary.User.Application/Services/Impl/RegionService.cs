/*********************************************************
* 名    称：RegionService.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：地区获取服务
* 更新历史：
*
* *******************************************************/
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.User.Application.Services.Interface;
using SmartLibrary.User.Application.ViewModels;
using SmartLibrary.User.EntityFramework.Core.Entitys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartLibrary.User.Application.Services.Impl
{
    /// <summary>
    /// 区域服务
    /// </summary>
    public class RegionService : IRegionService, IScoped
    {
        private IRepository<Region> _RegionRepository;

        public RegionService(IRepository<Region> regionRepository)
        {
            _RegionRepository = regionRepository;
        }

        /// <summary>
        /// 获取区域选择
        /// </summary>
        /// <returns></returns>
        public async Task<List<RegionOutput>> GetRegionTree()
        {
            var allRegions = await _RegionRepository.DetachedEntities.ProjectToType<RegionOutput>().OrderBy(x => x.Id).ToListAsync();
            var topNodes = allRegions.Where(x => x.Level == 1).ToList();
            foreach (var node in topNodes)
            {
                GetRegionChildren(node, allRegions);
            }
            return topNodes;
        }


        /// <summary>
        /// 获取地区子级
        /// </summary>
        /// <param name="node"></param>
        /// <param name="orgs"></param>
        private void GetRegionChildren(RegionOutput node, List<RegionOutput> orgs)
        {
            var children = orgs.Where(x => x.PId == node.Id).OrderBy(x => x.Id).ToList();
            if (children.Any())
            {
                node.Children = children;
                foreach (var child in node.Children)
                {
                    GetRegionChildren(child, orgs);
                }
            }
            else
            {
                node.Children = null;
            }

        }
    }
}
