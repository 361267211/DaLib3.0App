using Furion;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.News.EntityFramework.Core.DbContexts;

//using SmartLibrary.News.EntityFramework.Core.DbContexts;

namespace SmartLibrary.News.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}