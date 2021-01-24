using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PaymentGateway.Client;
using PaymentGateway.E2ETests.IoC;
using PaymentGateway.ViewModels;

namespace PaymentGateway.E2ETests
{
    public class E2ETests
    {
        private PaymentGatewayClient _paymentGatewayClient;
        private Guid _paymentId;

        [OneTimeSetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.ConfigureContainer();
            var serviceProvider = services.BuildServiceProvider();

            _paymentGatewayClient = serviceProvider.GetService<PaymentGatewayClient>();
        }

        [Test]
        [Order(1)]
        public async Task ProcessPayment()
        {
            var request = new PaymentRequest
            {
                CardNumber = "4111111111111111",
                ExpireDateYear = 2021,
                ExpireDateMonth = 10,
                CVV = "123",
                Amount = (decimal) 10.10,
                Currency = "EUR"
            }; 
            
            var payment = await _paymentGatewayClient.ProcessPayment(request);
            
            // Since the code returns a Bad request 10% of the time I can't validate if the answer is success.
            Assert.IsNotNull(payment);
            Assert.AreNotEqual(Guid.Empty, payment.PaymentId);
            
            _paymentId = payment.PaymentId;
        }
        
        [Test]
        [Order(2)]
        public async Task GetPayment()
        {
            var paymentInfo = await _paymentGatewayClient.GetPayment(_paymentId);
            
            Assert.AreEqual(paymentInfo.CardExpireDate, "10/2021");
        }
    }
}