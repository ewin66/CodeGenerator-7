using Autofac;
using CodeGenerator.BusinessService.Base_SysManage;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Schedule.GenericHost;
using CodeGenerator.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeGenerator.Schedule
{
    class TestJob : BaseJobTrigger
    {
        public TestJob() : base(TimeSpan.Zero, TimeSpan.FromSeconds(10000), new TestJobExcutor())
        {
        }

        public class TestJobExcutor : IJobExecutor
        {
            public TestJobExcutor()
            {
            }

            public void StartJob()
            {
                try
                {

                    var cusService = AutofacHelper.GetService<ICrm_CustomerService>();
                    var s = cusService.GetTheData("039ed57b565cc-6b8ea799-9345-4e1f-84e1-fb3feee192d9");
                    Console.WriteLine("任务执行中");
                }
                catch (Exception ex)
                {
                }
            }

            public void StopJob()
            {
                try
                {
                    Console.WriteLine("任务执行中");
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
