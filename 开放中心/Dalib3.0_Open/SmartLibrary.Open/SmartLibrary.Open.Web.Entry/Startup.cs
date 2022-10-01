/*********************************************************
* ��    �ƣ�Startup.cs
* ��    �ߣ�����ƽ
* ��ϵ��ʽ���绰[13627622058],�ʼ�[83379242@qq.com]
* ����ʱ�䣺20210831
* ��    ������Ŀ����������м��������ע��
* ������ʷ��
*
* *******************************************************/
using kiwiho.Course.MultipleTenancy.EFcore.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using SmartLibrary.Open.Application;
using SmartLibrary.Open.Application.GrpcAppServices.Service;
using SmartLibrary.Open.Common.Dtos;
using SmartLibrary.Open.EntityFramework.Core.DbContexts;
using SmartLibrary.Open.Services.AppManagement.Redis;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SmartLibrary.Open.Web
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
                options.InstanceName = "VipSmart.Open_";
            });


            //ע��Redis���񣬹��ܱȻ����
            services.AddRedisService(options =>
            {
                // �����ַ���
                options.Connection = SiteGlobalConfig.RedisServer.RedisConnection;
                // ����ǰ׺
                options.InstanceName = "VipSmart.Open_";
            });

            #endregion

            #region ���⻧����

            //��ȡ�⻧��Ϣ���Ӵ� 
            services.AddTenantDatabasePerSchema<OpenDbContext>(new ConnectionResolverOption
            {
                Key = "default",
                Type = ConnectionResolverType.Default,
                ConnectinString = SiteGlobalConfig.Database.SqlConnection,
                MigrationAssembly = SiteGlobalConfig.Database.MigrationAssembly,
                DBType = DatabaseIntegration.PostgreSql
            });


            var ipAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
            Console.WriteLine(ipAddress);

            // ע��WebApi����
            services.AddConsul(SiteGlobalConfig.ServiceRegist.ConsulAddress)
                    .AddHttpHealthCheck($"http://{ipAddress}:{SiteGlobalConfig.ServiceRegist.Port}/Api/Health/Index")
                    .RegisterService(SiteGlobalConfig.ServiceRegist.ServiceName, ipAddress, SiteGlobalConfig.ServiceRegist.Port, SiteGlobalConfig.ServiceRegist.ServiceTags.Split(','));
            // ע��grpc����
            services.AddConsul(SiteGlobalConfig.GrpcRegist.ConsulAddress)
                    .AddGRPCHealthCheck($"{ipAddress}:{SiteGlobalConfig.GrpcRegist.Port}")
                    .RegisterService(SiteGlobalConfig.GrpcRegist.ServiceName, ipAddress, SiteGlobalConfig.GrpcRegist.Port, SiteGlobalConfig.GrpcRegist.ServiceTags.Split(','));


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
            services.AddGrpc(option => option.EnableDetailedErrors = true);
            services.AddControllers().AddInjectWithUnifyResult();
            
            #region Capע��
            services.AddCap(x =>
            {
                x.UsePostgreSql(x => { x.ConnectionString = SiteGlobalConfig.Database.SqlConnection; });
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

            services.AddHttpClient(nameof(SiteGlobalConfig.FileServer),
                x => x.BaseAddress = new Uri(SiteGlobalConfig.FileServer));


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
            //ʹ���⻧��Ϣ�м��
            app.UseMiddleware<TenantInfoMiddleware>();
            app.UseAuthorization();
            app.UseInject(string.Empty);
            //�յ�·��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<HealthService>();
                endpoints.MapGrpcService<SearchBoxTitleItemService>();
                endpoints.MapGrpcService<OpacTemplateService>();
                endpoints.MapGrpcService<BookDonationTemplateGrpcService>();
                endpoints.MapGrpcService<AssemblyDeliveryService>();
                endpoints.MapGrpcService<AppManageGrpcService>();
                endpoints.MapGrpcService<AppRouteGrpcAppService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }


    }
}