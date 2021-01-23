using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.BankGateway
{
    public class FakeBankGateway : IBankGateway
    {
        private readonly ILogger<FakeBankGateway> _logger;

        public FakeBankGateway(ILogger<FakeBankGateway> logger)
        {
            _logger = logger;
        }

        public async Task<BankResponse> ValidatePayment(Payment payment)
        {
            var random = new Random();

            var randomNumber = random.NextDouble();
            
            // 90% change of valid payment
            if (randomNumber < 0.9)
            {
                return new BankResponse
                {
                    Status = BankResponseStatus.Success,
                    PaymentIdentifier = Guid.NewGuid()
                };
            }
            
            // 5% change of not enough money for the payment
            if(randomNumber < 0.95)
            {
                _logger.LogWarning("Not enough money to process this payment");
                
                return new BankResponse
                {
                    Status = BankResponseStatus.LackOfFunds,
                    PaymentIdentifier = Guid.NewGuid()
                };
            }
            
            // 4% change of not enough money for the payment
            if(randomNumber < 0.99)
            {
                _logger.LogWarning("Invalid Card data");
                
                return new BankResponse
                {
                    Status = BankResponseStatus.InvalidCard,
                    PaymentIdentifier = Guid.NewGuid()
                };
            }
            
            _logger.LogError("Some error happen! (There should be some info from the bank response)");
            
            // 1% change of some other error
            return new BankResponse
            {
                Status = BankResponseStatus.UnexpectedError,
                PaymentIdentifier = Guid.NewGuid()
            };
        }
    }
}