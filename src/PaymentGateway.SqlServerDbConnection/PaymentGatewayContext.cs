using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.SqlServerDbConnection
{
    public class PaymentGatewayContext: DbContext
    {
        private readonly IConfiguration _configuration;
        
        public PaymentGatewayContext(
            DbContextOptions<PaymentGatewayContext> options,
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_configuration["Database:SqlServer:ConnectionString"]);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentGatewayContext).Assembly);
        }
    }
}