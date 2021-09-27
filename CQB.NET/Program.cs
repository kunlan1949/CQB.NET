using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using CQB.NET.Action;

namespace CQB.NET
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            await MainAction.StartAsync().ContinueWith((e) =>
            {
                if (e.IsCompleted)
                {
                    Console.WriteLine("服务启动成功！");
                    Console.ResetColor();
                }
            });

            await host.WaitForShutdownAsync();

        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((hostingContext, configuration) =>
             {
                 configuration.Sources.Clear();

                 IHostEnvironment env = hostingContext.HostingEnvironment;

                 configuration
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

                 IConfigurationRoot configurationRoot = configuration.Build();


             });
    }

 
}
