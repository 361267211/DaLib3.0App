using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.Open.Services.SM.Internal.Attributes
{
    /// <summary>
    /// 标记某方法在事务包围的环境下使用，即如果失败自动回滚操作
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class TransactionScopeEnabledAttribute : Attribute
    {
        //todo 暂未提供拦截器
    }
}
