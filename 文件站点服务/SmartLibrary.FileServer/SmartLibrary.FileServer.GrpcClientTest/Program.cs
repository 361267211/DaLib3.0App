/*********************************************************
* 名    称：Proggram.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20210826
* 描    述：Grpc调用示例，注意需要通过提供的Client工厂获取调用对象，
*           不要直接使用Channel.FromAddress或直接新建对象，因为重建信道开销很大，
*           并且对象注销后Tcp连接不会立即断开，所以需要重用信道和客户端对象
* 更新历史：
*
* *******************************************************/
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using SmartLibrary.Core.GrpcClientHelper;
using SmartLibrary.FileServer.GrpcClientTest.Service;
using SmartLibraryUser;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GrpcClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var services = new ServiceCollection();
            //注册Grpc客户端工厂
            services.AddSmartGrpcClientFactory();
            //替换网关获取器为自己的实现，该服务用户获取grpc网关地址
            var descriptor = new ServiceDescriptor(typeof(IGrpcTargetAddressResolver), typeof(CustomGrpcTargetResolver), ServiceLifetime.Scoped);
            //替换为自己实现的Grpc地址获取
            services.Replace(descriptor);
            Console.WriteLine("请输入Token");
            var token = Console.ReadLine();

            try
            {
                var serviceProvider = services.BuildServiceProvider();
                //获取SmartGrpcClientFactory<T>，T为grpc客户端服务类型
                var userGrpcClientFactory = serviceProvider.GetService<SmartGrpcClientFactory<UserGrpcService.UserGrpcServiceClient>>();
                //通过orgCode获取endpoint,然后通过endpoint获取client
                var userGrpcClient = userGrpcClientFactory.GetGrpcClient("Test");
                // 在header中加入token，通过授权
                var headers = new Metadata
                {
                    {"Authorization",$"Bearer {token}"}
                };
                var response = await userGrpcClient.GetUserNameAsync(new UserRequest { Id = "testGrpcCall" }, headers);
                Console.WriteLine(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }
    }
}
