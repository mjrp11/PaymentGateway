using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.SqlServerDbConnection;

namespace PaymentGateway.IntegrationTests.Setup
{
    public class CustomWebApplicationFactory<TStartup> 
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseStartup<TStartup>();
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.RemoveAll<DbContext>();
                services.AddDbContext<DbContext, PaymentGatewayContextForTesting>(options => 
                {
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DbContext>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}