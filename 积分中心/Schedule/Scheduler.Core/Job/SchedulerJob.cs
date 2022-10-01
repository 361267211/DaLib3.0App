using Quartz;
using Scheduler.Service;
using Scheduler.Service.Entity;
using Scheduler.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Job
{
    /// <summary>
    /// 管理调度JOB,根据ScheduleEntity表中任务信息，对后台任务进行管理
    /// </summary>
    [DisallowConcurrentExecution]
    public class SchedulerJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                //获取任务集合
                var listTask = new SchedulerDataService(SchedulerCenter.Instance.SmartTaskConfig).GetSchedulerEntityList(true);

                //当前调度正在运行的
                var currentListTask = SchedulerCenter.Instance.SchedulerEntityList;

                var statusJobList = (from p in listTask
                                     from q in currentListTask
                                     where p.Id == q.Id && p.JobStatus != q.JobStatus
                                     select p).ToList();

                //暂停恢复 TODO
                foreach (var item in statusJobList)
                {
                    switch (item.JobStatus)
                    {
                        case JobStatus.RUN:
                            SchedulerCenter.Instance.ResumeSchedulerJob(item);
                            break;
                        case JobStatus.PAUSE:
                            SchedulerCenter.Instance.PauseSchedulerJob(item);
                            break;
                        default:
                            break;
                    }
                }

                //修改的任务
                var updateJobList = (from p in listTask
                                     from q in currentListTask
                                     where p.Id == q.Id && (p.TaskParam != q.TaskParam || p.AssemblyFullName != q.AssemblyFullName
                                     || p.AdapterAssemblyFullName != q.AdapterAssemblyFullName
                                     || p.AdapterClassFullName != q.AdapterClassFullName
                                     || p.AdapterParm != q.AdapterParm || p.ClassFullName != q.ClassFullName
                                     || p.Cron != q.Cron || p.TenantId != q.TenantId
                                     )
                                     select p).ToList();

                foreach (var item in updateJobList)
                {
                    if (SchedulerCenter.Instance.UpdateSchedulerJob(item).GetAwaiter().GetResult())
                    {
                        Console.WriteLine("UpdateSchedulerJob Succed,Id:" + item.Id);

                        LogManager.Info(string.Format("修改任务:{0}", JsonHelper.SerializeObject(item)));
                    }
                    else
                    {
                        Console.WriteLine("UpdateSchedulerJob Failed,Id:" + item.Id);
                    }
                }

                //新增的任务(TaskID在原集合不存在)
                var addJobList = (from p in listTask
                                  where !(from q in currentListTask select q.Id).Contains(p.Id)
                                  select p).ToList();

                foreach (var item in addJobList)
                {
                    if (SchedulerCenter.Instance.CreateSchedulerJob(item).GetAwaiter().GetResult())
                    {
                        Console.WriteLine("CreateSchedulerJob Succed Id:" + item.Id);

                        LogManager.Info(string.Format("新增任务:{0}", JsonHelper.SerializeObject(item)));
                    }
                    else
                    {
                        Console.WriteLine("CreateSchedulerJob Failed,Id:" + item.Id);
                    }
                }

                //删除的任务
                var deleteJobList = (from p in currentListTask
                                     where !(from q in listTask select q.Id).Contains(p.Id)
                                     select p).ToList();
                foreach (var item in deleteJobList)
                {
                    if (SchedulerCenter.Instance.DeleteSchedulerJob(item).GetAwaiter().GetResult())
                    {
                        Console.WriteLine("DeleteSchedulerJob Succed Id:" + item.Id);

                        LogManager.Info(string.Format("删除任务:{0}", JsonHelper.SerializeObject(item)));
                    }
                    else
                    {
                        Console.WriteLine("DeleteSchedulerJob Failed,Id:" + item.Id);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("同步任务异常,ex:{0}", ex.Message));
            }

            Console.WriteLine("RUN_IDS:" + string.Join(';', SchedulerCenter.Instance.SchedulerEntityList.Where(c => c.JobStatus == JobStatus.RUN).Select(c => c.Id).ToList()));
            Console.WriteLine("PAUSE_IDS:" + string.Join(';', SchedulerCenter.Instance.SchedulerEntityList.Where(c => c.JobStatus == JobStatus.PAUSE).Select(c => c.Id).ToList()));

            Console.WriteLine("-------------------------------------------------------------");

            return Task.CompletedTask;
        }
    }
}
