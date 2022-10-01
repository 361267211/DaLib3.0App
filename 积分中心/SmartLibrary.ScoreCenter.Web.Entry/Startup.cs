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
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.CAP;
using SmartLibrary.Core.Consul;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.Core.Redis;
using SmartLibrary.DatabaseTerrace.Application.Services;
using SmartLibrary.Dtm.Dtm;
using SmartLibrary.ScoreCenter.Application;
using SmartLibrary.ScoreCenter.Application.AuthHandler;
using SmartLibrary.ScoreCenter.Application.Dtos.Common;
using SmartLibrary.ScoreCenter.Common.Dtos;
using SmartLibrary.ScoreCenter.Common.Extensions;
using SmartLibrary.ScoreCenter.EntityFramework.Core.DbContexts;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SmartLibrary.ScoreCenter.Web
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
                options.InstanceName = "VipSmart.ScoreCenter_";
            });

            //注册Redis服务，功能比缓存多
            services.AddRedisService(options =>
            {
                // 连接字符串
                options.Connection = SiteGlobalConfig.RedisServer.RedisConnection;
                // 键名前缀
                options.InstanceName = "VipSmart.ScoreCenter_";
            });

            #endregion

            #region 多租户配置

            //获取租户信息连接串 
            services.AddTenantDatabasePerSchema<ScoreCenterDbContext>(new ConnectionResolverOption
            {
                Key = "default",
                Type = ConnectionResolverType.BySchema,
                ConnectinString = SiteGlobalConfig.SqlServer.SqlConnection,
                MigrationAssembly = SiteGlobalConfig.SqlServer.MigrationAssembly,
                DBType = DatabaseIntegration.PGSql
            });
            // 注册WebApi服务
            services.AddConsul(SiteGlobalConfig.ServiceRegist.ConsulAddress)
            .AddHttpHealthCheck($"http://{SiteGlobalConfig.ApiUrl}:{SiteGlobalConfig.ApiPort}/Api/Health/Index")
            .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, SiteGlobalConfig.ApiUrl, SiteGlobalConfig.ApiPort, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // 注册grpc服务
            services.AddConsul(SiteGlobalConfig.GrpcRegist.ConsulAddress)
            .AddGRPCHealthCheck($"{SiteGlobalConfig.GrpcUrl}:{SiteGlobalConfig.GrpcPort}")
            .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, SiteGlobalConfig.GrpcUrl, SiteGlobalConfig.GrpcPort, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));
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
            #region 授权认证
            services.AddScoped<IAuthorizationHandler, TokenAuthHandler>();
            services.AddScoped<IAuthorizationHandler, StaffAuthHandler>();
            services.AddScoped<IAuthorizationHandler, ReaderAuthHandler>();
            services.AddScoped<IAuthorizationHandler, PermitReaderAuthHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyKey.TokenAuth, p => p.Requirements.Add(new TokenAuthRequirement()));
                options.AddPolicy(PolicyKey.StaffAuth, p => p.Requirements.Add(new StaffAuthRequirement()));
                options.AddPolicy(PolicyKey.ReaderAuth, p => p.Requirements.Add(new ReaderAuthRequirement()));
                options.AddPolicy(PolicyKey.PermitReaderAuth, p => p.Requirements.Add(new PermitReaderAuthRequirement()));
            });
            services.AddScoped<IGrpcTargetAddressResolver, CustomGrpcTargetResolver>();
            #endregion
            #region 请求代理注册
            #endregion
            services.AddCorsAccessor();
            services.AddGrpc();
            services.AddControllers().AddInjectWithUnifyResult();
            services.AddDtmClient(x =>
            {
                x.DtmUrl = SiteGlobalConfig.DtmUrl;
                x.ConnectionString = SiteGlobalConfig.SqlServer.SqlConnection;
                x.UsePgSql();
            });
            #region Cap注册
            services.AddCap(x =>
            {
                x.GroupNamePrefix = SiteGlobalConfig.Owner;
                x.DefaultGroupName = SiteGlobalConfig.AppCode;
                x.UsePostgreSql(x => { x.ConnectionString = SiteGlobalConfig.SqlServer.SqlConnection; });
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
            //使用租户信息中间件
            app.UseMiddleware<TenantInfoMiddleware>();
            app.UseAuthorization();
            app.UseInject(string.Empty);
            //终点路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<HealthService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }


    }
}