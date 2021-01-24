using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PaymentGateway.Client;

namespace PaymentGateway.E2ETests.IoC
{
    public static class DependencyResolver
    {
        public static void ConfigureContainer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddConfiguration();
            serviceCollection.AddClients();
        }

        private static void AddConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConfiguration>(_ =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("globalsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"globalsettings.{environment}.json", optional: true, reloadOnChange: true)
                    .AddInMemoryCollection(TestContext.Parameters.Names.Select(x => new KeyValuePair<string, string>(x, TestContext.Parameters.Get(x))));

                return builder.Build();
            });
        }

        private static void AddClients(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();

                return new PaymentGatewayClient(configuration["PaymentGatewayBaseUrl"]);
            });
        }
    }
}