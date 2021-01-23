using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Migrations
{
    // This class is needed to run the dotnet ef commands
    public class ContextFactory : IDesignTimeDbContextFactory<DbContext>
    {
        public DbContext CreateDbContext(string[] args)
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var contentRoot = Path.GetDirectoryName(path);
            
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(contentRoot);
                    
            builder.AddJsonFile("globalsettings.json");
            builder.AddJsonFile(
                $"globalsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true);

            var configuration = builder.Build();
            
            if (configuration["Database:Type"].Equals("SqlServer", StringComparison.InvariantCultureIgnoreCase))
            {
                var builderOptions = new DbContextOptionsBuilder<SqlServerDbConnection.PaymentGatewayContext>();
                builderOptions
                    .UseLoggerFactory(LoggerFactory.Create(
                        builder => { builder.AddConsole(); }
                    )).UseSqlServer(b => { b.MigrationsAssembly("PaymentGateway.Migrations"); });

                return new SqlServerDbConnection.PaymentGatewayContext(builderOptions.Options, configuration);
            }
            
            if (configuration["Database:Type"].Equals("Sqlite", StringComparison.InvariantCultureIgnoreCase))
            {
                var builderOptions = new DbContextOptionsBuilder<SqlLiteDbConnection.PaymentGatewayContext>();
                builderOptions
                    .UseSqlite(b => { b.MigrationsAssembly("PaymentGateway.Migrations"); })
                    .UseLoggerFactory(LoggerFactory.Create(
                        builder => { builder.AddConsole(); }
                    ));

                return new SqlLiteDbConnection.PaymentGatewayContext(builderOptions.Options, configuration);
            }
            
            throw new Exception("Invalid DB engine!");
        }
    }
}