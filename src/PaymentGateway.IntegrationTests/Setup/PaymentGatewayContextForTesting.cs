using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PaymentGateway.SqlServerDbConnection;

namespace PaymentGateway.IntegrationTests.Setup
{
    public class PaymentGatewayContextForTesting: PaymentGatewayContext
    {
        public PaymentGatewayContextForTesting(DbContextOptions<PaymentGatewayContext> options, IConfiguration configuration) 
            : base(options, configuration)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InMemoryDbForTesting");
        }
    }
}