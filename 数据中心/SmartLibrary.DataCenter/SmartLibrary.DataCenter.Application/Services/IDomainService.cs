using SmartLibrary.DataCenter.EntityFramework.Core.Dto;
using SmartLibrary.DataCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application.Services
{
    public interface IDomainService
    {
        public Task<List<DomainInfoDto>> GetAllDomains(int type, int level);

        public Task<List<DomainTreeItem>> GetAllDomainTrees(int type, int level);
    }
}
