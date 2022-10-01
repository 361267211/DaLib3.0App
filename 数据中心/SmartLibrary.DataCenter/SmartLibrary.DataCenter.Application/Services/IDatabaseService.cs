
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.DataCenter.Application.Services
{
    public interface IDatabaseService
    {
        Task<List<ProviderResourceItem>> GetAllAvailableProviderResource();
        Task<List<DatabaseCollectKindItem>> GetAllDatabaseCollectKindResource();
        Task<List<DatabaseProviderItem>> GetAllDatabaseProvider();
        Task<List<ResourceAlbumItem>> GetResourceAlbum(string provider);
    }
}
