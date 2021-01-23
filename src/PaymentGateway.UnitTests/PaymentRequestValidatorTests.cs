using System.Threading.Tasks;
using NUnit.Framework;
using PaymentGateway.Validators;
using PaymentGateway.ViewModels;

namespace PaymentGateway.UnitTests
{
    public class PaymentRequestValidatorTests
    {
        private PaymentRequestValidator _paymentRequestValidator;
        
        [SetUp]
        public void Setup()
        {
            _paymentRequestValidator = new PaymentRequestValidator();
        }

        [Test]
        public async Task ValidRequest_ReturnsValidRequest()
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

            var result = _paymentRequestValidator.Validate(request);
            
            Assert.IsTrue(result.IsValid);
        }
        
        [Test]
        public async Task InValidCardNumber_ReturnsInValidRequestWith1Error()
        {
            var request = new PaymentRequest
            {
                CardNumber = "1234567890",
                ExpireDateYear = 2021,
                ExpireDateMonth = 10,
                CVV = "123",
                Amount = (decimal) 10.10,
                Currency = "EUR"
            };

            var result = _paymentRequestValidator.Validate(request);
            
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
        }
        
        [Test]
        public async Task InValidCurrency_ReturnsInValidRequestWith1Error()
        {
            var request = new PaymentRequest
            {
                CardNumber = "4111111111111111",
                ExpireDateYear = 2021,
                ExpireDateMonth = 10,
                CVV = "123",
                Amount = (decimal) 10.10,
                Currency = "ASD"
            };

            var result = _paymentRequestValidator.Validate(request);
            
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
        }
        
        [Test]
        public async Task InValidCVV_ReturnsInValidRequestWith1Error()
        {
            var request = new PaymentRequest
            {
                CardNumber = "4111111111111111",
                ExpireDateYear = 2021,
                ExpireDateMonth = 10,
                CVV = "12345",
                Amount = (decimal) 10.10,
                Currency = "EUR"
            };

            var result = _paymentRequestValidator.Validate(request);
            
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
        }
        
        [Test]
        public async Task InValidMonth_ReturnsInValidRequestWith1Error()
        {
            var request = new PaymentRequest
            {
                CardNumber = "4111111111111111",
                ExpireDateYear = 2021,
                ExpireDateMonth = 13,
                CVV = "123",
                Amount = (decimal) 10.10,
                Currency = "EUR"
            };

            var result = _paymentRequestValidator.Validate(request);
            
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
        }
    }
}