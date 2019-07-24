/*----------------------------------------------------------------
// Copyright © 2019 Chinairap.All rights reserved. 
// CLR版本：	4.0.30319.42000
// 类 名 称：    RegisterToConsulExtension
// 文 件 名：    RegisterToConsulExtension
// 创建者：      DUWENINK
// 创建日期：	2019/7/22 18:11:16
// 版本	日期					修改人	
// v0.1	2019/7/22 18:11:16	DUWENINK
//----------------------------------------------------------------*/
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
namespace ChinaIrap.Extentions
{
    /// <summary>
    /// 命名空间： ChinaIrap.Extentions.Consul
    /// 创建者：   DUWENINK
    /// 创建日期： 2019/7/22 18:11:16
    /// 类名：     RegisterToConsulExtension(Consul服务注册扩展)
    /// </summary>
    public static class RegisterToConsulExtension
    {
        /// <summary>
        /// 添加consul
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            //配置consul注册地址
            services.Configure<ServiceDiscoveryOptions>(configuration.GetSection("ServiceDiscovery"));
            //配置consul客户端
            services.AddSingleton<IConsulClient>(sp => new ConsulClient(config =>
            {
                var consulOptions = sp.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;
                if (!string.IsNullOrWhiteSpace(consulOptions.Consul.HttpEndPoint))
                {
                    config.Address = new Uri(consulOptions.Consul.HttpEndPoint);
                }
            }));

            return services;
        }

        /// <summary>
        /// 使用consul
        /// 默认的健康检查接口格式是 http://host:port/HealthCheck
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
#if DEBUG
            return app;//do nothing
#else
             IConsulClient consul = app.ApplicationServices.GetRequiredService<IConsulClient>();
            IApplicationLifetime appLife = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            IOptions<ServiceDiscoveryOptions> serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ServiceDiscoveryOptions>>();
            //var features = app.Properties["server.Features"] as FeatureCollection;
            var serverFeatures = app.ServerFeatures.Get<IServerAddressesFeature>();
           
            if (serverFeatures == null) return app;//do nothing
           var  port = new Uri(serverFeatures
               .Addresses
               .FirstOrDefault()).Port;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"application port is :{port}");
            var addressIpv4Hosts = NetworkInterface.GetAllNetworkInterfaces()
            .OrderByDescending(c => c.Speed)
            .Where(c => c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up);

            foreach (var item in addressIpv4Hosts)
            {
                var props = item.GetIPProperties();
                //这是ipv4的ip地址
                var firstIpV4Address = props.UnicastAddresses
                    .Where(c => c.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(c => c.Address)
                    .FirstOrDefault().ToString();
                var serviceId = $"{serviceOptions.Value.ServiceName}_{firstIpV4Address}:{port}";

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    //这个是默认健康检查接口
                    HTTP = $"{Uri.UriSchemeHttp}://{firstIpV4Address}:{port}/HealthCheck",
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    Address = firstIpV4Address.ToString(),
                    ID = serviceId,
                    Name = serviceOptions.Value.ServiceName,
                    Port = port
                };

                consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                //当服务停止后向consul发送的请求
                appLife.ApplicationStopping.Register(() =>
                {
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                });

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"health check service:{httpCheck.HTTP}");
            }

            //register localhost address
            //注册本地地址
            var localhostregistration = new AgentServiceRegistration()
            {
                Checks = new[] { new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                    Interval = TimeSpan.FromSeconds(30),
                    HTTP = $"{Uri.UriSchemeHttp}://localhost:{port}/HealthCheck",
                } },
                Address = "localhost",
                ID = $"{serviceOptions.Value.ServiceName}_localhost:{port}",
                Name = serviceOptions.Value.ServiceName,
                Port = port
            };
            consul.Agent.ServiceRegister(localhostregistration).GetAwaiter().GetResult();
            //当服务停止后向consul发送的请求
            appLife.ApplicationStopping.Register(() =>
            {
                consul.Agent.ServiceDeregister(localhostregistration.ID).GetAwaiter().GetResult();
            });
            app.Map("/HealthCheck", s =>
            {
                s.Run(async context =>
                {
                    await context.Response.WriteAsync("ok");
                });
            });
             app.Map("/Home/Index", s =>
            {
                s.Run(async context =>
                {
                    await context.Response.WriteAsync("微服务页面,已经加入Consul自动发现");
                });
            });
            return app;  
#endif






        }
    }
}
