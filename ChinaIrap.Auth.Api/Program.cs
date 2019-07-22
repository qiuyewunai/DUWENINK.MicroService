using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChinaIrap.Auth.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "ChinaIrap.Auth.Api";
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var startUrl = new ConfigurationBuilder().AddCommandLine(args).Build();
            string url= string.Empty;
            if (startUrl != null && !string.IsNullOrEmpty(startUrl["scheme"]))
            {
                url = $"{startUrl["scheme"]}://{startUrl["ip"]}:{startUrl["port"]}";
            }
     
         return WebHost.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   var env = hostingContext.HostingEnvironment;
                   config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                   config.AddEnvironmentVariables();
               })
            .UseUrls( string.IsNullOrEmpty(url)?"http://*:5001": url)
                .UseStartup<Startup>()
                .Build();

        }
            
    }
}
