using ApolloOption;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Events;
using SmartLibrary.FileServer.Common.Dtos;
using System;
using Winton.Extensions.Configuration.Consul;

namespace SmartLibrary.FileServer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                var baseConfig = configBuilder.Build();//configRoot 生命周期是瞬时的
                if (baseConfig["Setup:ConfigType"] == "Consul")
                {
                    var configSchema = baseConfig["Setup:ConsulConfigSchema"];
                    var configAddress = baseConfig["Setup:ConsulConfigAddress"];
                    //使用consul客户端加载consul配置
                    configBuilder.AddConsul(configSchema, options =>
                    {
                        options.ConsulConfigurationOptions = cco =>
                        {
                            cco.Address = new Uri(configAddress);
                        };
                        //配置热更新 动态加载
                        options.ReloadOnChange = true;
                        options.Optional = true;
                    });

                    var finalConfig = configBuilder.Build();//configRoot1 生命周期是瞬时的
                    OptionRegister.ConsulConfigInit(finalConfig, typeof(SiteGlobalConfig));
                    ChangeToken.OnChange(() => finalConfig.GetReloadToken(), () =>
                    {
                        //Console.WriteLine("监听到了");
                        OptionRegister.ConsulConfigInit(finalConfig, typeof(SiteGlobalConfig));
                    });
                }
                if (baseConfig["Setup:ConfigType"] == "Apollo")
                {
                    configBuilder.AddApollo(baseConfig.GetSection("Apollo"));
                }
            })
            .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.ConfigureServices((context, services) =>
                 {
                     if (context.Configuration["Setup:ConfigType"] == "Apollo")
                     {
                         services.ConfigInitAndBindChange(context.Configuration, typeof(SiteGlobalConfig));
                     }
                 });
                 webBuilder.UseKestrel(x =>
                 {
                     x.ListenAnyIP(SiteGlobalConfig.ListenPort.GrpcPort, x => { x.Protocols = HttpProtocols.Http2; });
                     x.ListenAnyIP(SiteGlobalConfig.ListenPort.ApiPort);
                 });
                 webBuilder.Inject().UseStartup<Startup>();

                 webBuilder.UseSerilog((hostContext, loggerConfig) =>
                 {
                     //配置serilog作为中间件
                     loggerConfig
                       .MinimumLevel.ControlledBy(new Serilog.Core.LoggingLevelSwitch
                       {
                           MinimumLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), SiteGlobalConfig.Serilog.MinLevel)
                       })
                       .Enrich.FromLogContext()
                       //配置数据写入Exceptionless
                       .WriteTo.Exceptionless(SiteGlobalConfig.Exceptionless.Key, SiteGlobalConfig.Exceptionless.Url);
                 });
             });


    }
}