using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Identity.Common.Dtos
{
    public class RedisServiceOption
    {
        public string Connection { get; set; }
        public ConfigurationOptions Configuration { get; set; }
        public string InstanceName { get; set; }

    }
}
