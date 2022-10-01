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
using SmartLibrary.User.Application;
using SmartLibrary.User.Application.AuthHandler;
using SmartLibrary.User.Application.Dtos.Common;
using SmartLibrary.User.Application.Filter;
using SmartLibrary.User.Application.GrpcService;
using SmartLibrary.User.Application.GrpcService.Impl;
using SmartLibrary.User.Application.Services;
using SmartLibrary.User.Common.Dtos;
using SmartLibrary.User.Common.Extensions;
using SmartLibrary.User.EntityFramework.Core.DbContexts;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SmartLibrary.User.Web
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
                options.InstanceName = "VipSmart.User_";
            });

            //ע��Redis���񣬹��ܱȻ����
            services.AddRedisService(options =>
            {
                // �����ַ���
                options.Connection = SiteGlobalConfig.RedisServer.RedisConnection;
                // ����ǰ׺
                options.InstanceName = "VipSmart.User_";
            });
            #endregion

            #region ���⻧����

            //��ȡ�⻧��Ϣ���Ӵ� 
            services.AddTenantDatabasePerSchema<UserDbContext>(new ConnectionResolverOption
            {
                Key = "default",
                Type = ConnectionResolverType.BySchema,
                ConnectinString = SiteGlobalConfig.SqlServer.SqlConnection,
                MigrationAssembly = SiteGlobalConfig.SqlServer.MigrationAssembly,
                DBType = DatabaseIntegration.PostgreSql
            });


            var ipAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
            Console.WriteLine(ipAddress);

            // ע��WebApi����
            services.AddConsul("http://42.193.20.184:8500/")
            .AddHttpHealthCheck($"http://{ipAddress}:{SiteGlobalConfig.ApiPort}/Api/Health/Index")
            .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, ipAddress, SiteGlobalConfig.ApiPort, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // ע��grpc����
            services.AddConsul("http://42.193.20.184:8500/")
            .AddGRPCHealthCheck($"{ipAddress}:{SiteGlobalConfig.GrpcPort}")
            .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, ipAddress, SiteGlobalConfig.GrpcPort, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));

         /*   // ע��WebApi����
            services.AddConsul(SiteGlobalConfig.ServiceRegist.ConsulAddress)
                    .AddHttpHealthCheck($"http://{SiteGlobalConfig.ApiUrl}:{SiteGlobalConfig.ApiPort}/Api/Health/Index")
                    .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, SiteGlobalConfig.ApiUrl, SiteGlobalConfig.ApiPort, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // ע��grpc����
            services.AddConsul(SiteGlobalConfig.GrpcRegist.ConsulAddress)
                    .AddGRPCHealthCheck($"{SiteGlobalConfig.GrpcUrl}:{SiteGlobalConfig.GrpcPort}")
                    .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, SiteGlobalConfig.GrpcUrl, SiteGlobalConfig.GrpcPort, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));*/
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

            #region �������ע��
            //services.AddRemoteRequest(options =>
            //{
            //    // ����ES����������Ϣ
            //    options.AddHttpClient("Es", c =>
            //    {
            //        c.BaseAddress = new Uri(SiteGlobalConfig.EsProxy.BaseAddress);//new Uri("http://essmartapi.cqvip.com/api/v1/elasticsearch/");
            //        c.DefaultRequestHeaders.Add("Accept", "*/*");
            //        c.DefaultRequestHeaders.Add("User-Agent", "SmartLibarary");//������д��ǰ��������
            //        c.DefaultRequestHeaders.Add("siteid", SiteGlobalConfig.EsProxy.SiteId);
            //        c.DefaultRequestHeaders.Add("siteusername", SiteGlobalConfig.EsProxy.SiteUserName);
            //        c.DefaultRequestHeaders.Add("sitepassword", SiteGlobalConfig.EsProxy.SitePassword);
            //    });
            //});
            #endregion

            services.AddCorsAccessor();
            services.AddGrpc(options => { options.EnableDetailedErrors = true; });
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
                endpoints.MapGrpcService<UserGrpcImpl>();
                endpoints.MapGrpcService<UserForIdentityGrpcImpl>();
                endpoints.MapGrpcService<HealthService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }


    }
}