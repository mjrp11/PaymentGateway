using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PaymentGateway.Application.BankGateway;
using PaymentGateway.Application.Encryption;
using PaymentGateway.Application.Encryption.EncryptionKeys;
using PaymentGateway.Application.Services;
using PaymentGateway.Domain.Repositories;
using PaymentGateway.SqlServerDbConnection.Repositories;

namespace PaymentGateway.InversionOfControl
{
    public static class ApplicationDependencies
    {
        public static void AddApplicationDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<IPaymentService, PaymentService>();
            serviceCollection.TryAddSingleton<ISymmetricEncryption, AesEncryption>();
            serviceCollection.TryAddSingleton<IEncryptionKeys, FakeEncryptionKey>();
            serviceCollection.TryAddSingleton<IBankGateway, FakeBankGateway>();
        }

        public static void AddDatabaseDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddTransient<ICardsRepository, CardsRepository>();
            serviceCollection.TryAddTransient<IPaymentsRepository, PaymentsRepository>();
        }

        public static void AddDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            if (configuration["Database:Type"].Equals("SqlServer", StringComparison.InvariantCultureIgnoreCase))
            {
                serviceCollection.AddDbContext<DbContext, SqlServerDbConnection.PaymentGatewayContext>();
            }
            else if (configuration["Database:Type"].Equals("Sqlite", StringComparison.InvariantCultureIgnoreCase))
            {
                serviceCollection.AddDbContext<DbContext, SqlLiteDbConnection.PaymentGatewayContext>();
            }
            else
            {
                throw new Exception("Invalid DB engine!");
            }
        }
    }
}