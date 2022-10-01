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
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SkyApm.AspNetCore.Diagnostics;
using SkyApm.Diagnostics.CAP;
using SmartLibrary.Core.Consul;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.Core.Redis;
using SmartLibrary.SceneManage.Application;
using SmartLibrary.SceneManage.Application.Grpc.AppService;
using SmartLibrary.SceneManage.Common.Dtos;
using SmartLibrary.SceneManage.EntityFramework.Core.DbContexts;
using SmartLibrary.SceneManage.Web.Jwt;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SmartLibrary.SceneManage.Web
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
            services.AddTenantDatabasePerSchema<SceneManageDbContext>(new ConnectionResolverOption
            {
                Key = "default",
                Type = ConnectionResolverType.BySchema,
                ConnectinString = SiteGlobalConfig.DataBase.SqlConnection,
                MigrationAssembly = SiteGlobalConfig.DataBase.MigrationAssembly,
                DBType = DatabaseIntegration.PGSql
            });


 
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

            services.AddSingleton<IAuthorizationHandler, CustomPremissionHandler>();
            services.AddSingleton<IAuthorizationHandler, DefaultPremissionHandler>();

            services.AddAuthorization(options =>
            {

                options.AddPolicy("CustomPolicy", policy => policy.Requirements.Add(new CustomPremissionRequirement(21)));

                options.AddPolicy("DefaultPolicy", policy => policy.Requirements.Add(new DefaultPremissionRequirement(21)));
            });
            // var descriptor = new ServiceDescriptor(typeof(IGrpcTargetAddressResolver), typeof(CustomGrpcTargetResolver), ServiceLifetime.Scoped);

            services.AddHttpContextAccessor();


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
            services.AddCorsAccessor();
            services.AddGrpc();
            services.AddControllers().AddInjectWithUnifyResult();
            #region Capע��
            //services.AddCap(x =>
            //{
            //    x.UsePostgreSql(x => { x.ConnectionString = SiteGlobalConfig.DataBase.SqlConnection; });
            //    x.UseRabbitMQ(x =>
            //    {
            //        x.HostName = SiteGlobalConfig.Cap.RabbitMQ.HostName;
            //        x.Port = SiteGlobalConfig.Cap.RabbitMQ.Port;
            //        x.VirtualHost = SiteGlobalConfig.Cap.RabbitMQ.VirtualHost;
            //        x.UserName = SiteGlobalConfig.Cap.RabbitMQ.UserName;
            //        x.Password = SiteGlobalConfig.Cap.RabbitMQ.Password;
            //    });
            //    x.FailedRetryCount = 0;
            //    x.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            //});
            #endregion
            #region ȫ��·�ɼ�
            services.AddSkyAPM(ext => ext.AddAspNetCoreHosting().AddCap());
            #endregion


            var ipAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
            Console.WriteLine(ipAddress);


            // ע��WebApi����
            services.AddConsul("http://42.193.20.184:8500")
                    .AddHttpHealthCheck($"http://{ipAddress}:{SiteGlobalConfig.ServiceRegist.Port}/Api/Health/Index")
                    .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, ipAddress, SiteGlobalConfig.ServiceRegist.Port, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // ע��grpc����
            services.AddConsul("http://42.193.20.184:8500")
                    .AddGRPCHealthCheck($"{ipAddress}:{SiteGlobalConfig.GrpcRegist.Port}")
                    .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, ipAddress, SiteGlobalConfig.GrpcRegist.Port, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));
            Console.WriteLine($"ע��ʱ�䣺{DateTime.Now}");
            Console.WriteLine($"http://{ipAddress}:{SiteGlobalConfig.ServiceRegist.Port}/Api/Health/Index");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine($"Configure��ʼʱ�䣺{DateTime.Now}");


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
                x.WithExposedHeaders(new[] { "UnAuth" });
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
                endpoints.MapGrpcService<SceneManageGrpcAppService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });

            Console.WriteLine($"Configure����ʱ�䣺{DateTime.Now}");

        }


    }
}