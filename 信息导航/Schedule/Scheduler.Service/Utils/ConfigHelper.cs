using System;
using Microsoft.Extensions.Configuration;

namespace Scheduler.Service.Utils
{
    public class ConfigHelper
    {


        public static IConfigurationRoot TaskConfig;

        static ConfigHelper()
        {
            try
            {
                var path = System.AppDomain.CurrentDomain.BaseDirectory;

                TaskConfig = new ConfigurationBuilder()
                               .SetBasePath(path)
                               .AddJsonFile("Config/DefaultSetting.json", optional: true, reloadOnChange: true)
                               .Build();
            }
            catch (Exception ex)
            {
                LogManager.Log("程序启动", "程序启动失败，配置文件加载解析失败" + ex.Message);
            }

        }


    }
}
