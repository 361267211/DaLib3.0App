/*********************************************************
 * 名    称：IgnoreMappingAttribute
 * 作    者：张祖琪
 * 联系方式：电话[13883914813],邮件[361267211@qq.com]
 * 创建时间：2021/8/05 16:57:45
 * 描    述：自定义特性 添加了此特性的属性不会被配置到siteglobalconfig上。
 *
 * 更新历史：
 *
 * *******************************************************/

using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.LinqBuilder;
using Mapster;
using SmartLibrary.DataCenter.EntityFramework.Core.Dto;
using SmartLibrary.DataCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application.Services
{
    public class DomainService : IDomainService, IScoped
    {

        private readonly IRepository<DomainInfo> _domainInfoRepository;


        public DomainService(
            IRepository<DomainInfo> domainInfoRepository

            )
        {
            _domainInfoRepository = domainInfoRepository;
        }

        /// <summary>
        /// 查所有学科分类-平级
        /// </summary>
        /// <returns></returns>
        public async Task<List<DomainInfoDto>> GetAllDomains(int type, int level)
        {
            var lamda = LinqExpression.Create<DomainInfo>(e => 1 == 1);
            lamda = lamda.AndIf(type != 0, e => e.Type == type.ToString());
            lamda = lamda.AndIf(level != 0, e => e.Level <= level);

            return _domainInfoRepository.AsQueryable(lamda).ProjectToType<DomainInfoDto>().ToList();
        }

        /// <summary>
        /// 查所有学科分类树-树型
        /// </summary>
        /// <returns></returns>
        public async Task<List<DomainTreeItem>> GetAllDomainTrees(int type, int level)
        {
            //查找所有权限
            var allDomains = await GetAllDomains(type, level);

            var trees = new List<DomainTreeItem>();

            foreach (var item in allDomains.Where(e => e.Level == 1))
            {
                var domainNode = item.Adapt<DomainTreeItem>();
                //查该一级学科的所有子级学科
                var secondaryNodeList = allDomains.Where(e => e.ParentID == item.DomainID && e.DomainIDCode != item.DomainIDCode).ToList();

                var nextNode = this.GetNextNode(domainNode, secondaryNodeList);

                domainNode.NextNodes.AddRange(nextNode);

                trees.Add(domainNode);
            }

            return trees;
        }

        /// <summary>
        /// 取本层级学科
        /// </summary>
        /// <param name="thisNode">本层级的学科节点</param>
        /// <param name="secondaryNodeList">本层级的所有下级学科节点的集合</param>
        /// <returns></returns>
        private List<DomainTreeItem> GetNextNode(DomainTreeItem thisNode, List<DomainInfoDto> secondaryNodeList)
        {
            //没有次级学科
            if (secondaryNodeList.Count() == 0)
            {
                return new List<DomainTreeItem> ();
            }

            var retList = new List<DomainTreeItem>();

            //查所有的下一级学科
            var secondaryList = secondaryNodeList.Where(e => e.DomainIDCode.StartsWith(thisNode.DomainIDCode) && e.Level == (thisNode.Level + 1)).ToList();

            //填充本级节点的 nextNode
            foreach (var item in secondaryList)
            {
                var domainNode = item.Adapt<DomainTreeItem>();
                domainNode.NextNodes.AddRange( this.GetNextNode(domainNode, secondaryNodeList) );
                retList.Add(domainNode);
            }

            return retList;

        }
    }
}
