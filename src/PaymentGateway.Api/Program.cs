using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PaymentGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureAppConfiguration(builder =>
                {
                    var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                    var uri = new UriBuilder(codeBase);
                    var path = Uri.UnescapeDataString(uri.Path);
                    var contentRoot = Path.GetDirectoryName(path);
                    
                    builder.SetBasePath(contentRoot);
                    
                    builder.AddJsonFile("globalsettings.json");
                    builder.AddJsonFile(
                        $"globalsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true);
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConsole();
                    
                    if (bool.Parse(context.Configuration["Logging:File:Enabled"]))
                    {
                        Enum.TryParse(context.Configuration["Logging:File:MinLogLevel"], out LogLevel minLogLevel);
                        
                        builder.AddFile(context.Configuration["Logging:File:Path"], minLogLevel);
                    }
                });
    }
}