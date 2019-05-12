using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace api
{/// <summary>
/// 中间件
/// </summary>
    public class Startup
    {/// <summary>
/// 中间件
/// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 中间件
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
       /// <summary>
       /// 注册服务
       /// </summary>
       /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();// 添加session服务 
          // services.Configure<CookiePolicyOptions>(options =>
          //  {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
           //     options.CheckConsentNeeded = context => false; // Default is true, make it false
         //       options.MinimumSameSitePolicy = SameSiteMode.None;
          //  });

            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
            var xmlPath = Path.Combine(basePath, "api.xml"); 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.IncludeXmlComments(xmlPath);
            } 
            );


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 配置项
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSession();// 启用session
            


            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
            var  jsonPaths = Path.Combine(basePath, "/swagger/v1/swagger.json");
            var jsPaths = Path.Combine(basePath, "/Scripts/zh.js");
            //配置Swagger
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(jsonPaths, "MJWEB API V1");
                c.InjectJavascript("/Scripts/zh.js");
            });


            app.UseMvc();

        }
    }
}
