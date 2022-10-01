using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Scheduler.Service.Utils
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class ConfigHelper
    {
        public static IConfigurationRoot TaskConfig;
        public static JObject TaskConfigObject;

        static ConfigHelper()
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                TaskConfig = new ConfigurationBuilder()
                               .SetBasePath(path)
                               .AddJsonFile("Config/DefaultSetting.json", optional: true, reloadOnChange: true)
                               .Build();
                TaskConfigObject = JObject.Parse(File.ReadAllText("Config/DefaultSetting.json", Encoding.UTF8));
            }
            catch (Exception ex)
            {
                LogManager.Log("程序启动", "程序启动失败，配置文件加载解析失败" + ex.Message);
            }
        }
    }
}
