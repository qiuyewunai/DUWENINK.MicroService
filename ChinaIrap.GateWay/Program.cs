
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChinaIrap.GateWay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        public static IWebHost BuildWebHost(string[] args)
        {
            var startUrl = new ConfigurationBuilder().AddCommandLine(args).Build();
            string url = string.Empty;
            if (startUrl != null && !string.IsNullOrEmpty(startUrl["scheme"]))
            {
                url = $"{startUrl["scheme"]}://{startUrl["ip"]}:{startUrl["port"]}";
            }
            IWebHostBuilder builder = new WebHostBuilder();
            //注入WebHostBuilder
            return builder.ConfigureServices(service =>
            {
                service.AddSingleton(builder);
            })
                //加载configuration配置文人年
                .ConfigureAppConfiguration(conbuilder =>
                {
                    conbuilder.AddJsonFile("appsettings.json");
                    conbuilder.AddJsonFile("ocelot.json");
                })
                .UseKestrel()
                .UseUrls(string.IsNullOrEmpty(url) ? "http://*:5000" : url)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
