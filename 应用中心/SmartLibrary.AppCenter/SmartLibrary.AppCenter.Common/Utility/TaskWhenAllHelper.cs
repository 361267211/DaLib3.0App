using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLibrary.AppCenter.Common.Utility
{
    public static class TaskWhenAllHelper
    {
        public static async Task<(TResult1 result1, TResult2 result2)> WhenAllAsync<TResult1, TResult2>(Task<TResult1> task1, Task<TResult2> task2)
        {
            await Task.WhenAll(task1, task2);

            return (task1.Result, task2.Result);
        }
    }
}
