/*********************************************************
* 名    称：Startup.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210831
* 描    述：项目启动项，配置中间件，服务注册
* 更新历史：
*
* *******************************************************/
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.CAP;
using SmartLibrary.AppCenter.Application;
using SmartLibrary.AppCenter.Application.Services.Grpc;
using SmartLibrary.AppCenter.Common.Dtos;
using SmartLibrary.AppCenter.Common.Utility;
using SmartLibrary.AppCenter.EntityFramework.Core.DbContexts;
using SmartLibrary.AppCenter.Web.Jwt;
using SmartLibrary.Core.Consul;
using SmartLibrary.Search.EsSearchProxy.Core;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SmartLibrary.AppCenter.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            #region Redis缓存注册

            //注册redis缓存
            services.AddStackExchangeRedisCache(options =>
            {
                // 连接字符串，这里也可以读取配置文件
                options.Configuration = SiteGlobalConfig.RedisServer.RedisConnection;
                // 键名前缀
                options.InstanceName = "VipSmart_AppCenter_";
            });

            //添加easycaching操作Redis
            services.AddEasyCaching(options =>
            {
                options.UseRedis(config =>
                {
                    config.DBConfig.Configuration = SiteGlobalConfig.RedisServer.RedisConnection;
                    config.SerializerName = "json"; //这个名字要和下面withJson的名字一样
                }, "default");
                options.WithJson("json");
            });

            #endregion

            #region 多租户配置

            //获取租户信息连接串 
            services.AddTenantDatabasePerSchema<AppCenterDbContext>(new ConnectionResolverOption
            {
                Key = "default",
                Type = ConnectionResolverType.BySchema,
                ConnectinString = SiteGlobalConfig.Database.SqlConnection,
                MigrationAssembly = SiteGlobalConfig.Database.MigrationAssembly,
                DBType = DatabaseIntegration.PostgreSql
            });


            var ipAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
            Console.WriteLine(ipAddress);

            // 注册WebApi服务
            services.AddConsul(SiteGlobalConfig.ServiceRegist.ConsulAddress)
                    .AddHttpHealthCheck($"http://{ipAddress}:{SiteGlobalConfig.ServiceRegist.Port}/Api/Health/Index")
                    .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, ipAddress, SiteGlobalConfig.ServiceRegist.Port, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // 注册grpc服务
            services.AddConsul(SiteGlobalConfig.GrpcRegist.ConsulAddress)
                    .AddGRPCHealthCheck($"{ipAddress}:{SiteGlobalConfig.GrpcRegist.Port}")
                    .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, ipAddress, SiteGlobalConfig.GrpcRegist.Port, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));

            #endregion

            #region jwt认证

            //添加Jwt认证，用于认证Grpc服务，WebApi只在网关层做认证
            var pubRsa = RSA.Create();
            string pubKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "PubKey.cert");
            if (!File.Exists(pubKeyPath))
                throw new Exception("公钥文件不存在");
            var pubKey = File.ReadAllText(pubKeyPath);
            pubRsa.ImportFromPem(pubKey.AsSpan());

            //添加授权策略
            services.AddSingleton<IAuthorizationHandler, FrontPremissionHandler>();
            services.AddSingleton<IAuthorizationHandler, BackPremissionHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("front", policy => policy.Requirements.Add(new FrontPremissionRequirement()));
                options.AddPolicy("back", policy => policy.Requirements.Add(new BackPremissionRequirement()));
            });

            //认证
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = SiteGlobalConfig.JwtAuth.Issuer,
                    ValidateAudience = true,
                    ValidAudience = SiteGlobalConfig.JwtAuth.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(pubRsa),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            #endregion

            #region 请求代理注册
            services.AddRemoteRequest(options =>
            {
                // 配置ES检索基本信息
                options.AddHttpClient("Es", c =>
                {
                    c.BaseAddress = new Uri(SiteGlobalConfig.EsProxy.BaseAddress);//new Uri("http://essmartapi.cqvip.com/api/v1/elasticsearch/");
                    c.DefaultRequestHeaders.Add("Accept", "*/*");
                    c.DefaultRequestHeaders.Add("User-Agent", "SmartLibarary");//建议填写当前服务名称
                    c.DefaultRequestHeaders.Add("siteid", SiteGlobalConfig.EsProxy.SiteId);
                    c.DefaultRequestHeaders.Add("siteusername", SiteGlobalConfig.EsProxy.SiteUserName);
                    c.DefaultRequestHeaders.Add("sitepassword", SiteGlobalConfig.EsProxy.SitePassword);
                });
            });
            #endregion

            services.AddHttpContextAccessor();
            services.AddCorsAccessor();
            services.AddGrpc(options => { options.EnableDetailedErrors = true; });
            services.AddControllers().AddInjectWithUnifyResult();


            #region Cap注册

            services.AddCap(x =>
            {
                x.UsePostgreSql(x => { x.ConnectionString = SiteGlobalConfig.Cap.SqlConnection; });
                x.UseRabbitMQ(x =>
                {
                    x.HostName = SiteGlobalConfig.Cap.RabbitMQ.HostName;
                    x.Port = SiteGlobalConfig.Cap.RabbitMQ.Port;
                    x.VirtualHost = SiteGlobalConfig.Cap.RabbitMQ.VirtualHost;
                    x.UserName = SiteGlobalConfig.Cap.RabbitMQ.UserName;
                    x.Password = SiteGlobalConfig.Cap.RabbitMQ.Password;
                });
                x.FailedRetryCount = 5;
                x.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });

            #endregion

            #region 全链路采集
            services.AddSkyAPM(ext => ext.AddAspNetCoreHosting().AddCap());
            #endregion

            //添加检索包
            services.AddEsSearchProxy(x =>
            {
                x.SiteId = SiteGlobalConfig.EsProxy.SiteId.ToInt();
                x.SitePassword = SiteGlobalConfig.EsProxy.SitePassword;
                x.SiteUserName = SiteGlobalConfig.EsProxy.SiteUserName;
                x.EsApiBase = new Uri("http://essmartapi.cqvip.com");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders();
            //请求日志
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseCorsAccessor(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.SetPreflightMaxAge(TimeSpan.FromDays(2));
                x.WithExposedHeaders(new[] { "UnAuth" });
            });
            app.UseAuthentication();
            //使用租户信息中间件
            app.UseMiddleware<TenantInfoMiddleware>();
            app.UseAuthorization();
            app.UseInject(string.Empty);
            //终点路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<HealthService>();
                endpoints.MapGrpcService<AppCenterGrpcServiceServer>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }


    }
}