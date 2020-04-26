using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeGenerator.Entity;
using CodeGenerator.Util;
using Swashbuckle.AspNetCore.Swagger;

namespace CodeGenerator.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //全局错误过滤
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //跨域地址设置
            services.AddCors(c =>
            {
                c.AddPolicy("LimitRequests", policy =>
                {
                    // 支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                    // 注意，http://127.0.0.1:1818 和 http://localhost:1818 是不一样的，尽量写两个
                    policy
                    .WithOrigins("http://127.0.0.1:1818", "http://localhost:8080", "http://localhost:8021", "http://localhost:8081", "http://localhost:1818")
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();
                });
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //配置 AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new DtoMapper());
            });

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });


                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var apiPath = Path.Combine(basePath, "CodeGenerator.WebApi.xml");
                var entityPath = Path.Combine(basePath, "CodeGenerator.Entity.xml");
                //默认的第二个参数是false，这个是controller的注释
                c.IncludeXmlComments(apiPath,true);
                c.IncludeXmlComments(entityPath);

                //为 Swagger 控制器添加备注
                //c.DocumentFilter<FilterSwagger>();
            });



            //使用Autofac替换自带IOC
            #region
            //实例化 AutoFac  容器  
            var containerBuilder = new ContainerBuilder();
            //新模块组件注册
            containerBuilder.RegisterModule<AutofacModule>();
            //将services中的服务填充到Autofac中.
            containerBuilder.Populate(services);
            //创建容器.
            var container = containerBuilder.Build();
            AutofacHelper.Container = container;
            //使用容器创建 AutofacServiceProvider,接管core内置DI容器
            return new AutofacServiceProvider(container);
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env">core获取文件路径   通过注入IHostingEnvironment服务对象来取得Web根目录物理路径</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // 在开发环境中，使用异常页面，这样可以暴露错误堆栈信息，所以不要放在生产环境。
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 在非开发环境中，使用HTTP严格安全传输(or HSTS) 对于保护web安全是非常重要的。
                // 强制实施 HTTPS 在 ASP.NET Core，配合 app.UseHttpsRedirection
                app.UseHsts();
            }

            //跨域，必须位于UserMvc之前
            app.UseCors("AllowAllHeaders");

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                ////根据版本名称倒序 遍历展示
                //typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                //{
                //    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                //});

            });

            //读取静态文件的注入  Core默认只读 wwwroot 文件中的静态文件
            app.UseStaticFiles();
            //重定向到Swagger起始页
            app.Run(ctx =>
            {
                ctx.Response.Redirect("/swagger/index.html"); //可以支持虚拟路径或者index.html这类起始页.
                return Task.FromResult(0);
            });
            // 跳转https
            app.UseHttpsRedirection();
            app.UseMvc();

           
        }
    }

    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //AutoFac自动装载
            builder.RegisterAssemblyTypes(Assembly.Load("CodeGenerator.BusinessService")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
        }
    }
}
