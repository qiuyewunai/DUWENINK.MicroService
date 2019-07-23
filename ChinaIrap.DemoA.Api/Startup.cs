using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinaIrap.Auth;
using ChinaIrap.Extentions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChinaIrap.DemoA.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            //读取配置文件
            var audienceConfig = Configuration.GetSection("Audience");
            services.AddOcelotPolicyJwtBearer(audienceConfig["Issuer"], audienceConfig["Issuer"], audienceConfig["Secret"], "ChinaIrapBearer", "Permission", "/api/denied");

            //这个集合模拟用户权限表,可从数据库中查询出来
            var permission = new List<Permission> {
                              new Permission {  Url="/api/values", Name="system"},
                              new Permission {  Url="/", Name="system"}
                          };
            services.AddSingleton(permission);
            //记录一下日志情况
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
            services.AddConsul(Configuration);
            services.AddMvc();
            //添加公司的swagger
            services.AddChinaIrapSwagger(Configuration);//现有mvc后有swagger
        }

       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseConsul();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "Default",
            //        template: "{controller}/{action}/{id?}",
            //        defaults: new { controller = "Home", action = "Index" }
            //    );
            //});
            app.UseMvcWithDefaultRoute();
            app.UseChinaIrapSwagger();//使用swagger
        }
    }
}
