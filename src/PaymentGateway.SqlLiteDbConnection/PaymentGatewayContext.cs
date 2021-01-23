using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.SqlLiteDbConnection
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
                .UseSqlite(_configuration["Database:Sqlite:ConnectionString"]);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentGatewayContext).Assembly);
        }
    }
}