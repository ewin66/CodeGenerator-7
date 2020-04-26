using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Autofac.Extensions.DependencyInjection;
using CodeGenerator.Util;
using CodeGenerator.Entity;
using AutoMapper;
using CodeGenerator.BusinessService.Base_SysManage;
using System.Threading.Tasks;
using CodeGenerator.Entity.Base_SysManage;
using System.Linq;
using System.Reflection;

namespace CodeGenerator.Web
{
    public class Startup
    {


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddLogging();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new DtoMapper());
            });

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            //使用Autofac替换自带IOC
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<AutofacModule>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            AutofacHelper.Container = container;
            return new AutofacServiceProvider(container);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                //区域
                routes.MapRoute(
                    name: "areas",
                    template: "{area=Admin}/{controller=Home}/{action=Index}/{id?}"
                );

                ////默认路由
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
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
