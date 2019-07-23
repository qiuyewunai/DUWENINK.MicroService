using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ChinaIrap.Extentions
{
    /// <summary>
    /// ChinaIrapSwagger扩展
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// 添加ChinaIrapSwagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddChinaIrapSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            //注册Swagger配置
            services.Configure<ChainaIrapSwaggerOptions>(configuration.GetSection("ChinaIrapSwagger"));
            var provider = services.BuildServiceProvider().GetRequiredService<IOptions<ChainaIrapSwaggerOptions>>().Value;
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            //配置Swagger
            services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc(provider.DocName, new Info { Title = provider.TitleInfo, Version = provider.VersionInfo });

                var xmlPath = Path.Combine(basePath, $"{provider.DocName}.xml");
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "江苏勺园科技有限公司api统一认证接口，前置Bearer。示例：Bearer {Token}",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                options.AddSecurityRequirement(
                    new Dictionary<string, IEnumerable<string>>
                    {
                        { "Bearer",
                          Enumerable.Empty<string>()
                        },
                    });

            });
            
            
          ;


            return services;
        }

        /// <summary>
        /// 使用Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseChinaIrapSwagger(this IApplicationBuilder app)
        {
            IOptions<ChainaIrapSwaggerOptions> serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ChainaIrapSwaggerOptions>>();
            app.UseSwagger(c =>
             {
                 c.RouteTemplate = "{documentName}/swagger.json";
             }).UseSwaggerUI(options =>
             {
                 options.SwaggerEndpoint($"/{serviceOptions.Value.DocName}/swagger.json", serviceOptions.Value.DocName);
             });
            return app;
        }



    }
}
