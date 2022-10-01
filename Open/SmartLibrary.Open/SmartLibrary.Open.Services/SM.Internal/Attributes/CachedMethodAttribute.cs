using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.SM.Internal.Attributes
{
    /// <summary>
    /// 标记某方法的返回值将被缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class CachedMethodAttribute : Attribute
    {
        public CachedMethodAttribute(string key)
        {
            Key = key;
        }
        /// <summary>
        /// 缓存key，使用string Format可以缓存不同参数的方法
        /// </summary>
        public string Key { get;  }

        /// <summary>
        /// 绝对过期时间，秒为单位
        /// </summary>
        public uint ExpireOn { get; set; }
    }
}
