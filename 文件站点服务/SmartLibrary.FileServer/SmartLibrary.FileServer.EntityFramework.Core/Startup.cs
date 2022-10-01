using Furion;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.FileServer.EntityFramework.Core.DbContexts;

//using SmartLibrary.FileServer.EntityFramework.Core.DbContexts;

namespace SmartLibrary.FileServer.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}