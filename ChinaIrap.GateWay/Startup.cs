using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ChinaIrap.Auth;
using Ocelot.Provider.Polly;
using Ocelot.Provider.Consul;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Options;

namespace ChinaIrap.GateWay
{
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region 注册Metrics 主要用作看板
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var metrics = AppMetrics.CreateDefaultBuilder()
            .Configuration.Configure(
            options =>
            {
                options.AddAppTag("RepairApp");
                options.AddEnvTag("stage");
            })
            .Report.ToInfluxDb(
            options =>
            {
                options.InfluxDb.BaseUri = new Uri($"{builder["InfluxDb:BaseUri"]}");
                options.InfluxDb.Database = builder["InfluxDb:Database"];
                options.InfluxDb.UserName = builder["InfluxDb:UserName"];
                options.InfluxDb.Password = builder["InfluxDb:Password"];
                options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                options.HttpPolicy.FailuresBeforeBackoff = 5;
                options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                options.FlushInterval = TimeSpan.FromSeconds(5);
            })
            .Build();
            Console.WriteLine($"数据库名称:{ builder["InfluxDb:Database"] }");
            services.AddMetrics(metrics);
            services.AddMetricsReportingHostedService();
            services.AddMetricsTrackingMiddleware();
            services.AddMetricsEndpoints();
            #endregion
            #region 注册JWT
            var audienceConfig = Configuration.GetSection("Audience");
            //注入OcelotJwtBearer
            services.AddOcelotJwtBearer(audienceConfig["Issuer"], audienceConfig["Issuer"], audienceConfig["Secret"], "ChinaIrapBearer");
            #endregion

            //注入配置文件，AddOcelot要求参数是IConfigurationRoot类型，所以要作个转换//使用Polly做熔断策略
           // services.AddOcelot(Configuration as ConfigurationRoot).AddPolly();
            var ocelotConfig = new ConfigurationBuilder().AddJsonFile("ocelot.json", false, true).Build();
            //services.AddOcelot(ocelotConfig)
            //        .AddConsul().AddPolly()
            //        .AddConfigStoredInConsul();
            services.AddOcelot(Configuration as ConfigurationRoot).AddConsul().AddPolly().AddConfigStoredInConsul();

            services.Configure<SwaggerOptions>(Configuration.GetSection("Swagger"));
            var provider = services.BuildServiceProvider().GetRequiredService<IOptions<SwaggerOptions>>().Value;


            
            //if(  swaggerConfig["Swagger:UseSwagger"].Value)
            if (provider.UseSwagger)
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc(provider.SwaggerConfig.DocName, new Info { Title = provider.SwaggerConfig.TitleInfo, Version = provider.SwaggerConfig.VersionInfo });
                });

            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //允许跨域
            //app.UseCors("default");
            #region Metrics中间件
            app.UseMetricsAllMiddleware();
            app.UseMetricsAllEndpoints();
            #endregion
            app.UseMvc();
            IOptions<SwaggerOptions> serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<SwaggerOptions>>();
            //if(  swaggerConfig["Swagger:UseSwagger"].Value)
            if (serviceOptions.Value.UseSwagger)
            {
                var apis = serviceOptions.Value.SwaggerConfig.Apis;

                app.UseSwagger()
               .UseSwaggerUI(options =>
               {
                   apis.ForEach(m =>
                   {
                       options.SwaggerEndpoint($"/{m}/swagger.json", m);
                   });
               });

            }
            app.UseOcelot()
               .Wait();
            Console.WriteLine("ocelot 网关已启动完成");
        }
    }
}
