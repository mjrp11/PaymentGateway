using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PaymentGateway.Application.BankGateway;
using PaymentGateway.Application.Encryption;
using PaymentGateway.Application.Encryption.EncryptionKeys;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Services;
using PaymentGateway.Application.Shared;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Repositories;
using PaymentStatus = PaymentGateway.Application.Shared.PaymentStatus;

namespace PaymentGateway.UnitTests
{
    public class PaymentServiceTests
    {
        private PaymentService _paymentService;
        
        private Mock<IPaymentsRepository> _paymentsRepositoryMock;
        private ISymmetricEncryption _symmetricEncryptionMock;
        private Mock<ICardsRepository> _cardsRepositoryMock;
        private Mock<IBankGateway> _bankGatewayMock;
        
        [SetUp]
        public void Setup()
        {
            _paymentsRepositoryMock = new Mock<IPaymentsRepository>();
            _cardsRepositoryMock = new Mock<ICardsRepository>();
            _bankGatewayMock = new Mock<IBankGateway>();

            var encryptionKeyMock = new Mock<IEncryptionKeys>();
            encryptionKeyMock.Setup(x => x.GetKey()).Returns("SOME_KEY_TO_TEST");
            
            _symmetricEncryptionMock = new AesEncryption(encryptionKeyMock.Object);
            
            _paymentService = new PaymentService(
                _paymentsRepositoryMock.Object, _symmetricEncryptionMock,
                _cardsRepositoryMock.Object, _bankGatewayMock.Object);
        }

        [Test]
        public async Task ProcessPayment_ValidRequest_ReturnsResultWithIdAndSuccess()
        {
            var request = new ProcessPaymentRequest
            {
                CardNumber = "1234567892",
                ExpireDateYear = 2021,
                ExpireDateMonth = 10,
                CVV = "123",
                Amount = (decimal) 10.10,
                Currency = Currency.EUR
            };

            _cardsRepositoryMock.Setup(x => x.GetCard(It.IsAny<string>(), It.IsAny<string>(), 10, 2021));
            _paymentsRepositoryMock.Setup(x => x.Save(It.IsAny<Payment>()));
            _bankGatewayMock.Setup(x => x.ValidatePayment(It.IsAny<Payment>())).Returns(Task.FromResult(new BankResponse
            {
                Status = BankResponseStatus.Success,
                PaymentIdentifier = Guid.NewGuid()
            }));
            
            var response = await _paymentService.ProcessPayment(request);
            
            Assert.AreEqual(PaymentStatus.Processed,response.PaymentStatus);
            Assert.AreNotEqual(Guid.Empty,response.PaymentId);
        }
        
        [Test]
        public async Task ProcessPayment_BankReturnsError_ReturnsResultWithIdAndFailed()
        {
            var request = new ProcessPaymentRequest
            {
                CardNumber = "1234567892",
                ExpireDateYear = 2021,
                ExpireDateMonth = 10,
                CVV = "123",
                Amount = (decimal) 10.10,
                Currency = Currency.EUR
            };

            _cardsRepositoryMock.Setup(x => x.GetCard(It.IsAny<string>(), It.IsAny<string>(), 10, 2021));
            _paymentsRepositoryMock.Setup(x => x.Save(It.IsAny<Payment>()));
            _bankGatewayMock.Setup(x => x.ValidatePayment(It.IsAny<Payment>())).Returns(Task.FromResult(new BankResponse
            {
                Status = BankResponseStatus.InvalidCard,
                PaymentIdentifier = Guid.NewGuid()
            }));
            
            var response = await _paymentService.ProcessPayment(request);
            
            Assert.AreEqual(PaymentStatus.Failed,response.PaymentStatus);
            Assert.AreNotEqual(Guid.Empty,response.PaymentId);
        }
    }
}