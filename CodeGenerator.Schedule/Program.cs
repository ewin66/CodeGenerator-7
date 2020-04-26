using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CodeGenerator.Entity;
using CodeGenerator.Util;
using System;
using System.Configuration;
using System.Reflection;
using System.Threading;

namespace CodeGenerator.Schedule
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 依赖注入

            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new DtoMapper());
            });

            var builder = new ConfigurationBuilder()
                  .SetBasePath(AppContext.BaseDirectory)
                  .AddJsonFile("appsettings.json");

            var Configuration = builder.Build();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddLogging();


            //使用Autofac替换自带IOC
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<AutofacModule>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            AutofacHelper.Container = container;


            IServiceProvider serviceProvider = new AutofacServiceProvider(container);
            #endregion


            using (var cts = new CancellationTokenSource())
            {
                CancellationToken token = cts.Token;

                //测试定时任务
                var test = new TestJob();

                //任务开始
                test.StartAsync(token);

                Console.WriteLine("任务开始执行!");

                //Task.Factory.StartNew(() =>
                //{
                //    //10秒后停止
                //    Thread.Sleep(10000);
                //    //任务结束
                //    test.StopAsync(token);
                //});

                Console.ReadLine();

            }
        }

        public class AutofacModule : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                //AutoFac自动装载
                builder.RegisterAssemblyTypes(Assembly.Load("CodeGenerator.BusinessService")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
                //builder.RegisterAssemblyTypes(Assembly.Load("CodeGenerator.DataRepository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
                //builder.RegisterType<HomeService>().AsImplementedInterfaces();
                //builder.RegisterType<Base_UserService>().AsImplementedInterfaces();
                //注册Service中的对象,Service中的类要以Service结尾，否则注册失败
                //builder.RegisterAssemblyTypes(Assembly.Load("CodeGenerator.BusinessService")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
                //注册Repository中的对象,Repository中的类要以Repository结尾，否则注册失败
                //builder.RegisterAssemblyTypes(Assembly.Load("CodeGenerator.DataRepository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
            }
        }
    }
}
