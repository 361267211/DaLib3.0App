﻿using Furion;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.DataCenter.EntityFramework.Core.DbContexts;

//using SmartLibrary.DataCenter.EntityFramework.Core.DbContexts;

namespace SmartLibrary.DataCenter.EntityFramework.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}