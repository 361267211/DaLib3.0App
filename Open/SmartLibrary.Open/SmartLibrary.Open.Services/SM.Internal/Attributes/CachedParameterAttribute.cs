using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.SM.Internal.Attributes
{
    /// <summary>
    /// 用以标记某参数为缓存key的组成部分，详见<see cref="CachedMethodAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    internal class CachedParameterAttribute : Attribute
    {
    }
}
