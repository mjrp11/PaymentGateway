using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Migrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(c => c.AddConsole())
                .AddTransient(provider => new ContextFactory().CreateDbContext(args))
                .BuildServiceProvider();

            var context = serviceProvider.GetService<DbContext>();

            context.Database.Migrate();
        }
    }
}