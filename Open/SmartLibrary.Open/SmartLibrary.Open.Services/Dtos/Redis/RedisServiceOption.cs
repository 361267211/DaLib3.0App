using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.Dtos.Redis
{
    public class RedisServiceOption
    {
        public string Connection { get; set; }
        public ConfigurationOptions Configuration { get; set; }
        public string InstanceName { get; set; }
        public int ConsumerThreadCount { get; set; } = 5;

        public int StreamEntriesCount { get; set; } = 10;

    }
}
