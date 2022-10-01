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
using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MiniExcelLibs;
using SmartLibrary.DataCenter.EntityFramework.Core.Dto;
using SmartLibrary.DataCenter.EntityFramework.Core.Entitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application.Services
{
    public class DatabaseService : IDatabaseService, IScoped
    {
        private readonly IRepository<ProviderResource> _providerResourceRepository;
        private readonly IRepository<DatabaseCollectKind> _databaseCollectKindRepository;
        private readonly IRepository<DatabaseProvider> _databaseProviderRepository;
        private readonly IRepository<ResourceAlbum> _resourceAlbumRepository;


        public DatabaseService(
            IRepository<ProviderResource> providerResourceRepository,
            IRepository<DatabaseCollectKind> databaseCollectKindRepository,
            IRepository<DatabaseProvider> databaseProviderRepository,
             IRepository<ResourceAlbum> resourceAlbumRepository
            )
        {
            _providerResourceRepository = providerResourceRepository;
            _databaseCollectKindRepository = databaseCollectKindRepository;
            _databaseProviderRepository = databaseProviderRepository;
            _resourceAlbumRepository = resourceAlbumRepository;
        }

        /// <summary>
        /// 查询所有可获取的数据库商资源
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProviderResourceItem>> GetAllAvailableProviderResource()
        {
            return _providerResourceRepository.Where(e => !e.DeleteFlag).ProjectToType<ProviderResourceItem>().ToList();
        }

        public async Task<List<DatabaseCollectKindItem>> GetAllDatabaseCollectKindResource()
        {
            return _databaseCollectKindRepository.AsQueryable().ProjectToType<DatabaseCollectKindItem>().ToList();
        }

        public async Task<List<DatabaseProviderItem>> GetAllDatabaseProvider()
        {
            //  var ttt= _databaseProviderRepository.AsQueryable(e => !e.DeleteFlag).ToList();
            return _databaseProviderRepository.AsQueryable().ProjectToType<DatabaseProviderItem>().ToList();
        }

        public async Task<List<ResourceAlbumItem>> GetResourceAlbum(string provider)
        {
            var queryAlbum = _resourceAlbumRepository.AsQueryable(e => e.Provider == provider);

            var query = from a in queryAlbum
                        let re = queryAlbum.Where(p => p.ParentId == a.AlbumCode).ToList()
                        where a.ParentId == 0
                        select new ResourceAlbumDto
                        {
                            AlbumCode = a.AlbumCode,
                            ParentId = a.ParentId,
                            AlbumName = a.AlbumName,
                            ChildAlbum = re.Adapt<List<ResourceAlbumDto>>(),
                            Provider = a.Provider,
                        };


            List<ResourceAlbumItem> albumItemList = new List<ResourceAlbumItem>();
            var list = query.ToList();
            foreach (var item in list)
            {
                var album = item.ChildAlbum.Adapt<List<ResourceAlbumItem>>();
                ResourceAlbumItem resource = new ResourceAlbumItem();
                resource.AlbumName = item.AlbumName;
                resource.AlbumCode = item.AlbumCode;
                resource.ChildAlbum.AddRange(album);
                albumItemList.Add(resource);
            }
           
            return albumItemList;

        }

 
    }
}
