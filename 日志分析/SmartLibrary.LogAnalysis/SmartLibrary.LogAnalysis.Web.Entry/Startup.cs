/*********************************************************
* ��    �ƣ�Startup.cs
* ��    �ߣ�����
* ��ϵ��ʽ���绰[13629774594],�ʼ�[1450873843@qq.com]
* ����ʱ�䣺20210831
* ��    ������Ŀ����������м��������ע��
* ������ʷ��
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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Polly;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.CAP;
using SmartLibrary.Core.Consul;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.Core.Redis;
using SmartLibrary.DatabaseTerrace.Application.Services;
using SmartLibrary.LogAnalysis.Application;
using SmartLibrary.LogAnalysis.Common.Dtos;
using SmartLibrary.LogAnalysis.EntityFramework.Core.DbContexts;
using SmartLibrary.LogAnalysis.Web.Jwt;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SmartLibrary.LogAnalysis.Web
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
            #region Redis����ע��
            //ע��redis����
            services.AddStackExchangeRedisCache(options =>
            {
                // �����ַ���������Ҳ���Զ�ȡ�����ļ�
                options.Configuration = SiteGlobalConfig.RedisServer.RedisConnection;
                // ����ǰ׺
                options.InstanceName = "VipSmart_";
            });

            #endregion

            #region ���⻧����

            //��ȡ�⻧��Ϣ���Ӵ� 
            services.AddTenantDatabasePerSchema<AssetDbContext>(new ConnectionResolverOption
            {
                Key = "default",
                Type = ConnectionResolverType.BySchema,
                ConnectinString = SiteGlobalConfig.Database.SqlConnection,
                MigrationAssembly = SiteGlobalConfig.Database.MigrationAssembly,
                DBType = DatabaseIntegration.PGSql
            });
            // ע��WebApi����
            services.AddConsul(SiteGlobalConfig.ServiceRegist.ConsulAddress)
            .AddHttpHealthCheck($"http://{SiteGlobalConfig.ServiceRegist.Url}:{SiteGlobalConfig.ServiceRegist.Port}/Api/Health/Index")
            .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, SiteGlobalConfig.ServiceRegist.Url, SiteGlobalConfig.ServiceRegist.Port, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // ע��grpc����
            services.AddConsul(SiteGlobalConfig.GrpcRegist.ConsulAddress)
            .AddGRPCHealthCheck($"{SiteGlobalConfig.GrpcRegist.Url}:{SiteGlobalConfig.GrpcRegist.Port}")
            .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, SiteGlobalConfig.GrpcRegist.Url, SiteGlobalConfig.GrpcRegist.Port, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));
            #endregion
            #region jwt��֤
            //���Jwt��֤��������֤Grpc����WebApiֻ�����ز�����֤
            var pubRsa = RSA.Create();
            string pubKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "PubKey.cert");
            if (!File.Exists(pubKeyPath))
            {
                throw new Exception("��Կ�ļ�������");
            }
            var pubKey = File.ReadAllText(pubKeyPath);
            pubRsa.ImportFromPem(pubKey.AsSpan());


            services.AddAuthorization(options =>
            {

                options.AddPolicy("Premission1", policy => policy.Requirements.Add(new CustomPremissionRequirement(21)));

                options.AddPolicy("Premission2", policy => policy.Requirements.Add(new DefaultPremissionRequirement(21)));
            });
            services.AddSingleton<IAuthorizationHandler, CustomPremissionHandler>();
            services.AddSingleton<IAuthorizationHandler, DefaultPremissionHandler>();
            var descriptor = new ServiceDescriptor(typeof(IGrpcTargetAddressResolver), typeof(CustomGrpcTargetResolver), ServiceLifetime.Scoped);
            //�滻Ϊ�Լ�ʵ�ֵ�Grpc��ַ��ȡ
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
        
            services.AddCorsAccessor();
            services.AddGrpc();
            services.AddControllers().AddInjectWithUnifyResult();
            #region Capע��
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
            #region ȫ��·�ɼ�
            services.AddSkyAPM(ext => ext.AddAspNetCoreHosting().AddCap());
            #endregion

            services.AddHttpClient(nameof(SiteGlobalConfig.IndexStatisticsLink), x =>
            {
                x.BaseAddress = new Uri(SiteGlobalConfig.IndexStatisticsLink);
            }).AddTransientHttpErrorPolicy(x =>
                x.WaitAndRetryAsync(3, (y) => TimeSpan.FromMilliseconds(100 * y))); ;

            services.AddHttpClient("webapi2.2", x =>
            {
                x.BaseAddress = new Uri(SiteGlobalConfig.OldSite.OldSiteUri);

            }).AddTransientHttpErrorPolicy(x =>
                x.WaitAndRetryAsync(3, (y) => TimeSpan.FromMilliseconds(100 * y))); ;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCorsAccessor();
            app.UseAuthentication();
            app.UseAuthorization();
            //ʹ���⻧��Ϣ�м��
            app.UseMiddleware<TenantInfoMiddleware>();
            app.UseInject(string.Empty);
            //�յ�·��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<UserService>();
                endpoints.MapGrpcService<HealthService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }


    }
}