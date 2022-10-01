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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.CAP;
using SmartLibrary.Core.Consul;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.Identity.Application;
using SmartLibrary.Identity.Application.AuthHandler;
using SmartLibrary.Identity.Application.Dtos.Common;
using SmartLibrary.Identity.Application.Filter;
using SmartLibrary.Identity.Application.Services;
using SmartLibrary.Identity.Common.Dtos;
using SmartLibrary.Identity.Common.Extensions;
using SmartLibrary.Identity.EntityFramework.Core.DbContexts;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SmartLibrary.Identity.Web
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
                options.InstanceName = "VipSmart.Identity_";
            });

            //ע��Redis���񣬹��ܱȻ����
            services.AddRedisService(options =>
            {
                // �����ַ���
                options.Connection = SiteGlobalConfig.RedisServer.RedisConnection;
                // ����ǰ׺
                options.InstanceName = "VipSmart.Identity_";
            });

            #endregion

            #region ���⻧����

            //��ȡ�⻧��Ϣ���Ӵ� 
            services.AddTenantDatabasePerSchema<IdentityDbContext>(new ConnectionResolverOption
            {
                Key = "default",
                Type = ConnectionResolverType.BySchema,
                ConnectinString = SiteGlobalConfig.SqlServer.SqlConnection,
                MigrationAssembly = SiteGlobalConfig.SqlServer.MigrationAssembly,
                DBType = DatabaseIntegration.PGSql
            });
            // ע��WebApi����
            services.AddConsul(SiteGlobalConfig.ServiceRegist.ConsulAddress)
                    .AddHttpHealthCheck($"http://{SiteGlobalConfig.ApiUrl}:{SiteGlobalConfig.ApiPort}/Api/Health/Index")
                    .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, SiteGlobalConfig.ApiUrl, SiteGlobalConfig.ApiPort, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // ע��grpc����
            services.AddConsul(SiteGlobalConfig.GrpcRegist.ConsulAddress)
                    .AddGRPCHealthCheck($"{SiteGlobalConfig.GrpcUrl}:{SiteGlobalConfig.GrpcPort}")
                    .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, SiteGlobalConfig.GrpcUrl, SiteGlobalConfig.GrpcPort, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));
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

            #region ��Ȩ��֤

            services.AddScoped<IAuthorizationHandler, TokenAuthHandler>();
            services.AddScoped<IAuthorizationHandler, StaffAuthHandler>();
            services.AddScoped<IAuthorizationHandler, ReaderAuthHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyKey.TokenAuth, p => p.Requirements.Add(new TokenAuthRequirement()));
                options.AddPolicy(PolicyKey.StaffAuth, p => p.Requirements.Add(new StaffAuthRequirement()));
                options.AddPolicy(PolicyKey.ReaderAuth, p => p.Requirements.Add(new ReaderAuthRequirement()));
            });
            services.AddScoped<IGrpcTargetAddressResolver, CustomGrpcTargetResolver>();
            services.AddScoped(typeof(Lazy<>));

            #endregion

            #region �������ע��

            services.AddHttpClient();

            #endregion

            services.AddCorsAccessor();
            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
            });
            services.AddControllers().AddMvcFilter<CapActionEventFilter>().AddInjectWithUnifyResult();

            #region Capע��

            services.AddCap(x =>
            {
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

            #region ȫ��·�ɼ�

            services.AddSkyAPM(ext => ext.AddAspNetCoreHosting().AddCap());

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCorsAccessor(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.SetPreflightMaxAge(TimeSpan.FromDays(2));
                x.WithExposedHeaders(new[] { PolicyKey.UnAuthKey });
            });
            app.UseAuthentication();
            //ʹ���⻧��Ϣ�м��
            app.UseMiddleware<TenantInfoMiddleware>();
            app.UseAuthorization();
            app.UseInject(string.Empty);
            //�յ�·��
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