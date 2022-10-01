/*********************************************************
* 名    称：RedisServiceOption.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220303
* 描    述：Redis服务参数配置
* 更新历史：
*
* *******************************************************/
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.User.Common.Dtos
{
    /// <summary>
    /// Redis服务参数配置
    /// </summary>
    public class RedisServiceOption
    {
        public string Connection { get; set; }
        public ConfigurationOptions Configuration { get; set; }
        public string InstanceName { get; set; }

    }
}
