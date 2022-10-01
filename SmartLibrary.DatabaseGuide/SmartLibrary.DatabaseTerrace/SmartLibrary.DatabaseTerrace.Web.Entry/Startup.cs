/*********************************************************
* 名    称：Startup.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210831
* 描    述：项目启动项，配置中间件，服务注册
* 更新历史：
*
* *******************************************************/
using Furion;
using Grpc.Core;
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.CAP;
using SmartLibrary.Core.Consul;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.Core.Redis;
using SmartLibrary.DatabaseTerrace.Application;
using SmartLibrary.DatabaseTerrace.Application.Services;
using SmartLibrary.DatabaseTerrace.Common.Const;
using SmartLibrary.DatabaseTerrace.Common.Dtos;
using SmartLibrary.DatabaseTerrace.EntityFramework.Core.DbContexts;
using SmartLibrary.DatabaseTerrace.Web.Jwt;
using SmartLibrary.Search.EsSearchProxy.Core;
using SmartLibrary.User.RpcService;
using SmartLibraryUser;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SmartLibrary.DatabaseTerrace.Web
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
            #region Redis缓存注册
            //注册redis缓存
            services.AddStackExchangeRedisCache(options =>
            {
                // 连接字符串，这里也可以读取配置文件
                options.Configuration = SiteGlobalConfig.RedisServer.RedisConnection;


                // 键名前缀
                options.InstanceName = "VipSmart_";
            });

            #endregion

            var controller = typeof(DatabaseTerraceAppService);
            System.Reflection.MethodInfo[] methods = controller.GetMethods();
            var names = methods.Select(e => e.Name).ToList();
            var methodsName = string.Join(';', names);

            #region 多租户配置

            //获取租户信息连接串 
            services.AddTenantDatabasePerSchema<DatabaseTerraceDbContext>(new ConnectionResolverOption
            {
                Key = "default",
                Type = ConnectionResolverType.BySchema,
                ConnectinString = SiteGlobalConfig.SqlServer.SqlConnection,
                MigrationAssembly = SiteGlobalConfig.SqlServer.MigrationAssembly,
                // DBType = DatabaseIntegration.SqlServer
                DBType = DatabaseIntegration.PGSql
            });
            // 注册WebApi服务
            services.AddConsul(SiteGlobalConfig.ServiceRegist.ConsulAddress)
            .AddHttpHealthCheck($"http://{"192.168.21.75"}:{SiteGlobalConfig.ServiceRegist.Port}/Api/Health/Index")
            .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, "192.168.21.75", SiteGlobalConfig.ServiceRegist.Port, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // 注册grpc服务
            services.AddConsul(SiteGlobalConfig.GrpcRegist.ConsulAddress)
            .AddGRPCHealthCheck($"{"192.168.21.75"}:{SiteGlobalConfig.GrpcRegist.Port}")
            .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, "192.168.21.75", SiteGlobalConfig.GrpcRegist.Port, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));
            #endregion
            #region jwt认证
            //添加Jwt认证，用于认证Grpc服务，WebApi只在网关层做认证
            var pubRsa = RSA.Create();
            string pubKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "PubKey.cert");
            if (!File.Exists(pubKeyPath))
            {
                throw new Exception("公钥文件不存在");
            }
            var pubKey = File.ReadAllText(pubKeyPath);
            pubRsa.ImportFromPem(pubKey.AsSpan());

            //  services.AddSingleton<IAuthorizationHandler, JwtHandler>();


            services.AddAuthorization(options =>
            {

                options.AddPolicy(PolicyKey.UnAuthKey, policy => policy.Requirements.Add(new LoginPremissionRequirement(21)));

                options.AddPolicy(PolicyKey.StaffAuth, policy => policy.Requirements.Add(new StaffPremissionRequirement(21)));
                options.AddPolicy(PolicyKey.PortalColumn, policy => policy.Requirements.Add(new PortalColumnPremissionRequirement(21)));
            });
            services.AddSingleton<IAuthorizationHandler, LoginPremissionHandler>();
            services.AddSingleton<IAuthorizationHandler, StaffPremissionHandler>();
            services.AddSingleton<IAuthorizationHandler, PortalColumnPremissionHandler>();

            var descriptor = new ServiceDescriptor(typeof(IGrpcTargetAddressResolver), typeof(CustomGrpcTargetResolver), ServiceLifetime.Scoped);
            //替换为自己实现的Grpc地址获取
            services.Replace(descriptor);


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

                options.AddHttpClient("Center", c =>
                 {
                     c.BaseAddress = new Uri(SiteGlobalConfig.Central.CentralApiUrl);
                 });
            });
            #endregion
            services.AddCorsAccessor();
            services.AddGrpc();
            services.AddControllers().AddInjectWithUnifyResult(
                options =>
                {
                    options.SpecificationDocumentConfigure = spt =>
                    {
                        spt.SwaggerGenConfigure = gen =>
                        {
                            gen.CustomSchemaIds(x => x.FullName);
                        };
                    };
                }
                );
            #region Cap注册
            services.AddCap(x =>
            {
                x.UseSqlServer(x => { x.ConnectionString = SiteGlobalConfig.Cap.SqlConnection; });
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

            #region ES帮助类注册
            services.AddEsSearchProxy(x =>
            {
                x.SiteId = 1;
                x.SitePassword = "SmartCqu_2020";
                x.SiteUserName = "cqu";
                x.EsApiBase = new Uri("http://essmartapi.cqvip.com");
                x.ConnectionTimeOut = TimeSpan.FromSeconds(45);
            });
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCorsAccessor(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.WithExposedHeaders(new[] { PolicyKey.UnAuthKey });
            });
            app.UseAuthentication();
            app.UseAuthorization();
            //使用租户信息中间件
            app.UseMiddleware<TenantInfoMiddleware>();
            app.UseInject(string.Empty);
            //终点路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<HealthService>();
                endpoints.MapGrpcService<DatabaseColumnGrpcService>();
                endpoints.MapGrpcService<DatabaseServiceGrpc>();
                /*                endpoints.MapGrpcService<UserGrpcService.UserGrpcServiceClient>()
                                .WithMetadata(
                                    new Metadata
                                    {
                                        { "Authorization",
                                            //App.GetService<IHttpContextAccessor>().HttpContext?.Request.Headers["111"]
                                         //  app.ApplicationServices.GetRequiredService<IHttpContextAccessor>().HttpContext?.Request.Headers["httpContextAccessor"]
                                         "Beasdasd"

                                      //   endpoints.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext?.Request.Headers["httpContextAccessor"] 
                                        }
                                    });*/

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });

            });
        }
    }

}
