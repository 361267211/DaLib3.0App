using Furion;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.DbContexts;

//using SmartLibrary.DatabaseTerrace.EntityFramework.Core.DbContexts;

namespace SmartLibrary.DatabaseTerrace.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}