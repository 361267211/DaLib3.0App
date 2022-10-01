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
using SmartLibrary.DatabaseTerrace.Application.Services;
using SmartLibrary.Navigation.Application;
using SmartLibrary.Navigation.Application.Services;
using SmartLibrary.Navigation.Common.Const;
using SmartLibrary.Navigation.Common.Dtos;
using SmartLibrary.Navigation.EntityFramework.Core.DbContexts;
using SmartLibrary.Navigation.Utility;
using SmartLibrary.Navigation.Web.Jwt;
using SmartLibrary.Search.EsSearchProxy.Core;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SmartLibrary.Navigation.Web
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
                ConnectinString = SiteGlobalConfig.SqlServer.SqlConnection,
                MigrationAssembly = SiteGlobalConfig.SqlServer.MigrationAssembly,
                DBType = DatabaseIntegration.PGSql
            });
            // ע��WebApi����
            services.AddConsul(SiteGlobalConfig.ServiceRegist.ConsulAddress)
                    .AddHttpHealthCheck($"http://{SiteGlobalConfig.ServiceRegist.Url}:{SiteGlobalConfig.ServiceRegist.Port}/Api/Health/Index")
                    .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, SiteGlobalConfig.ServiceRegist.Url, SiteGlobalConfig.ServiceRegist.Port, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // ע��grpc����
            services.AddConsul(SiteGlobalConfig.GrpcRegist.ConsulAddress)
                    .AddGRPCHealthCheck($"{SiteGlobalConfig.ServiceRegist.Url}:{SiteGlobalConfig.GrpcRegist.Port}")
                    .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, SiteGlobalConfig.ServiceRegist.Url, SiteGlobalConfig.GrpcRegist.Port, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));
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
                options.AddPolicy(PolicyKey.UnAuthKey, policy => policy.Requirements.Add(new LoginCheckPremissionRequirement(21)));
                options.AddPolicy(PolicyKey.StaffAuth, policy => policy.Requirements.Add(new ColumnPremissionRequirement(21)));
                options.AddPolicy(PolicyKey.PortalColumn, policy => policy.Requirements.Add(new PortalColumnPremissionRequirement(21)));
            });
            services.AddSingleton<IAuthorizationHandler, LoginCheckPremissionHandler>();
            services.AddSingleton<IAuthorizationHandler, ColumnPremissionHandler>();
            services.AddSingleton<IAuthorizationHandler, PortalColumnPremissionHandler>();
            var descriptor = new ServiceDescriptor(typeof(IGrpcTargetAddressResolver), typeof(CustomGrpcTargetResolver), ServiceLifetime.Scoped);

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
                    ClockSkew = TimeSpan.FromHours(60)
                };
            });
            #endregion

            #region �������ע��
            services.AddRemoteRequest(options =>
            {
                // ����ES����������Ϣ
                options.AddHttpClient("Es", c =>
                {
                    c.BaseAddress = new Uri(SiteGlobalConfig.EsProxy.BaseAddress);//new Uri("http://essmartapi.cqvip.com/api/v1/elasticsearch/");
                    c.DefaultRequestHeaders.Add("Accept", "*/*");
                    c.DefaultRequestHeaders.Add("User-Agent", "SmartLibarary");//������д��ǰ��������
                    c.DefaultRequestHeaders.Add("siteid", SiteGlobalConfig.EsProxy.SiteId);
                    c.DefaultRequestHeaders.Add("siteusername", SiteGlobalConfig.EsProxy.SiteUserName);
                    c.DefaultRequestHeaders.Add("sitepassword", SiteGlobalConfig.EsProxy.SitePassword);
                });
            });
            #endregion

            services.AddEsSearchProxy(x =>
            {
                x.ConnectionTimeOut = TimeSpan.FromSeconds(30);
                x.SiteId = SiteGlobalConfig.EsProxy.SiteId.ToInt();
                x.SitePassword = SiteGlobalConfig.EsProxy.SitePassword;
                x.SiteUserName = SiteGlobalConfig.EsProxy.SiteUserName;
            });

            services.AddCorsAccessor();
            services.AddGrpc(options => { options.EnableDetailedErrors = true; });
            services.AddControllers().AddInjectWithUnifyResult();
            
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

            services.AddScoped<IGrpcTargetAddressResolver, CustomGrpcTargetResolver>();
            services.AddHttpClient("webapi2.2", x =>
            {
                x.BaseAddress = new Uri(SiteGlobalConfig.OldSite.OldSiteUri);
            

            });
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
            //ʹ���⻧��Ϣ�м��
            app.UseMiddleware<TenantInfoMiddleware>();
            app.UseInject(string.Empty);
            //�յ�·��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<UserService>();
                endpoints.MapGrpcService<HealthService>();
                endpoints.MapGrpcService<NavigationGrpc>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }


    }
}