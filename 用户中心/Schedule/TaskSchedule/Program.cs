using Scheduler.Core;
using System;

namespace TaskSchedule
{
    class Program
    {
        static void Main(string[] args)
        {
            if (SchedulerCenter.Instance.RunAsync().IsCompletedSuccessfully)
            {
                Console.WriteLine("定时任务启动成功！");
            }

            Console.ReadLine();
        }
    }
}
