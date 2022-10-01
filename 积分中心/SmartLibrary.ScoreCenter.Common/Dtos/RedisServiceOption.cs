/*********************************************************
* 名    称：RedisServiceOption.cs
* 作    者：刘孟
* 联系方式：电话[13629774594],邮件[1450873843@qq.com]
* 创建时间：20220304
* 描    述：Redis服务配置
* 更新历史：
*
* *******************************************************/
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.ScoreCenter.Common.Dtos
{
    /// <summary>
    /// Redis服务配置
    /// </summary>
    public class RedisServiceOption
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        public ConfigurationOptions Configuration { get; set; }
        /// <summary>
        /// 实例名称
        /// </summary>
        public string InstanceName { get; set; }

    }
}
