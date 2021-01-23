using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PaymentGateway.Application.BankGateway;
using PaymentGateway.Client;
using PaymentGateway.Domain.Entities;
using PaymentGateway.IntegrationTests.Setup;
using PaymentGateway.ViewModels;
using PaymentStatus = PaymentGateway.ViewModels.PaymentStatus;

namespace PaymentGateway.IntegrationTests
{
    public class PaymentGatewayControllerTests
    {
        private PaymentGatewayClient _paymentGatewayClient;
        private HttpClient _client;
        
        // Mock Classes
        private Mock<IConfiguration> _configuration;
        private Mock<IBankGateway> _bankGatewayMock;

        private static readonly IEnumerable<KeyValuePair<string, string>> InitialConfiguration =
            new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Database:Type", "SqlServer"),
                new KeyValuePair<string, string>("Database:SqlServer:ConnectionString", "ConnectionString"),
                new KeyValuePair<string, string>("Encryption:Key", "SOME_KEY_TO_TEST")
            };
        
        // Tests Data
        private Guid _paymentId;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _configuration = new Mock<IConfiguration>();
            _bankGatewayMock = new Mock<IBankGateway>();
            
            foreach (var (key, value) in InitialConfiguration)
            {
                _configuration.SetupGet(x => x[key]).Returns(value);
            }
            
            var factory = new CustomWebApplicationFactory<Startup>();

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, configurationBuilder) =>
                    {
                        configurationBuilder.AddInMemoryCollection(InitialConfiguration);
                    });
                
                builder.ConfigureServices(services =>
                {
                    //services.AddSingleton(x => _configuration.Object);
                    services.AddSingleton(x => _bankGatewayMock.Object);
                });
            }).CreateClient();
            
            _paymentGatewayClient = new PaymentGatewayClient(_client);
        }

        [Test]
        [Order(1)]
        public async Task TestProcessPayment()
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

            _bankGatewayMock.Setup(x => x.ValidatePayment(It.IsAny<Payment>())).Returns(Task.FromResult(new BankResponse
            {
                Status = BankResponseStatus.Success,
                PaymentIdentifier = Guid.NewGuid()
            }));

            var response = await _paymentGatewayClient.ProcessPayment(request);
            
            Assert.IsTrue(response.IsSuccess);
            Assert.AreNotEqual(Guid.Empty, response.PaymentId);

            _paymentId = response.PaymentId;
        }
        
        [Test]
        [Order(2)]
        public async Task TestGetPayment()
        {
            var response = await _paymentGatewayClient.GetPayment(_paymentId);
            
            Assert.IsNotNull(response);
            Assert.AreEqual((decimal) 10.10, response.Amount);
            Assert.AreEqual("************1111", response.CardNumber);
            Assert.AreEqual("10/2021", response.CardExpireDate);
            Assert.AreEqual("EUR", response.Currency);
            Assert.AreEqual(PaymentStatus.Processed, response.PaymentStatus);
        }
    }
}