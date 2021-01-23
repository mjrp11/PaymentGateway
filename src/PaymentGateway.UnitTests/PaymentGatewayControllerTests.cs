using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;
using PaymentGateway.Application.Services;
using PaymentGateway.Controllers;
using PaymentGateway.ViewModels;
using PaymentResponse = PaymentGateway.ViewModels.PaymentResponse;
using PaymentStatus = PaymentGateway.Application.Shared.PaymentStatus;

namespace PaymentGateway.UnitTests
{
    public class PaymentGatewayControllerTests
    {
        private PaymentGatewayController _paymentGatewayController;
        private Mock<IPaymentService> _paymentServiceMock;
        
        [SetUp]
        public void Setup()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            
            _paymentGatewayController = new PaymentGatewayController(_paymentServiceMock.Object);
        }

        [Test]
        public async Task ProcessPayment_ValidRequest_ReturnsOkResultWithIdAndSuccess()
        {
            var request = new PaymentRequest
            {
                CardNumber = "1234567892",
                ExpireDateYear = 2021,
                ExpireDateMonth = 10,
                CVV = "123",
                Amount = (decimal) 10.10,
                Currency = "EUR"
            };

            var paymentId = Guid.NewGuid();

            _paymentServiceMock.Setup(x => x.ProcessPayment(It.IsAny<ProcessPaymentRequest>()))
                .Returns(Task.FromResult(new ProcessPaymentResponse
                {
                    PaymentStatus = PaymentStatus.Processed,
                    PaymentId = paymentId
                }));

            var response = await _paymentGatewayController.ProcessPayment(request);
            
            Assert.IsInstanceOf<OkObjectResult>(response.Result);

            var okObjectResult = (OkObjectResult) response.Result;
            
            Assert.IsInstanceOf<PaymentResponse>(okObjectResult.Value);
            
            var paymentResponse = (PaymentResponse) okObjectResult.Value;
            
            Assert.IsTrue(paymentResponse.IsSuccess);
            Assert.AreEqual(paymentId, paymentResponse.PaymentId);
        }
        
        [Test]
        public async Task ProcessPayment_InValidRequest_ReturnsBadRequest()
        {
            var request = new PaymentRequest
            {
                CardNumber = "1234567892",
                ExpireDateYear = 2021,
                ExpireDateMonth = 10,
                CVV = "123",
                Amount = (decimal) 10000000000000.10,
                Currency = "EUR"
            };

            var paymentId = Guid.NewGuid();

            _paymentServiceMock.Setup(x => x.ProcessPayment(It.IsAny<ProcessPaymentRequest>()))
                .Returns(Task.FromResult(new ProcessPaymentResponse
                {
                    PaymentStatus = PaymentStatus.Failed,
                    PaymentId = paymentId
                }));

            var response = await _paymentGatewayController.ProcessPayment(request);
            
            Assert.IsInstanceOf<BadRequestObjectResult>(response.Result);

            var okObjectResult = (BadRequestObjectResult) response.Result;
            
            Assert.IsInstanceOf<PaymentResponse>(okObjectResult.Value);
            
            var paymentResponse = (PaymentResponse) okObjectResult.Value;
            
            Assert.IsFalse(paymentResponse.IsSuccess);
            Assert.AreEqual(paymentId, paymentResponse.PaymentId);
        }
    }
}