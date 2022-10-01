using Quartz;
using Scheduler.Core;
using Scheduler.Service.Utils;
using System;
using System.Reflection;

namespace TaskManager.Tasks
{
    [DisallowConcurrentExecution]
    public abstract class SmartJobBase : JobBase
    {

        /// <summary>
        /// 适配器引用（根据任务配置生成）,DataOperater被调用时，会反射生成任务需要的适配器，用户自定义参数
        /// </summary>
        public T GetDataOperater<T>()
        {

            if (SchedulerEntity != null && !string.IsNullOrEmpty(SchedulerEntity.AdapterAssemblyFullName) && !string.IsNullOrEmpty(SchedulerEntity.AdapterClassFullName))
            {
                try
                {
                    Assembly assembly = Assembly.Load(new AssemblyName(SchedulerEntity.AdapterAssemblyFullName));
                    Type type = assembly.GetType(SchedulerEntity.AdapterClassFullName);

                    var parm = new object[] { SchedulerEntity.AdapterParm, TenantDb };
                    return (T)Activator.CreateInstance(type, parm);

                }
                catch (Exception ex)
                {
                    LogManager.Log(JobName, "适配器初始化异常,可能是配置适配器路径不正确" + ex);
                    throw new JobExecutionException(ex.Message);
                }
            }
            return default(T);

        }
    }
}
