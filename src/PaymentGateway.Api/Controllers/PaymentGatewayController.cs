using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Services;
using PaymentGateway.Extensions;
using PaymentGateway.ViewModels;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentGatewayController(
            IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet, Route("{paymentId}")]
        public async Task<ActionResult<PaymentInformation>> RetrievePaymentDetails(Guid paymentId)
        {
            var serviceResponse = await _paymentService.GetPaymentInformation(paymentId);

            if (serviceResponse == null)
            {
                return NotFound();
            }
            
            return Ok(new PaymentInformation
            {
                PaymentId = serviceResponse.PaymentId,
                Amount = serviceResponse.Amount,
                Currency = serviceResponse.Currency,
                CardNumber = serviceResponse.CardMask,
                PaymentStatus = serviceResponse.Status.ToViewModels(),
                CardExpireDate = serviceResponse.CardExpirationDate,
                PaymentDate = serviceResponse.PaymentDate
            });
        }
        
        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> ProcessPayment([FromBody] PaymentRequest paymentRequest)
        {
            var response = await _paymentService.ProcessPayment(paymentRequest.ToServiceRequest());

            var paymentResponse = response.ToPaymentResponse();
            
            if (paymentResponse.IsSuccess)
            {
                return Ok(paymentResponse);
            }

            return BadRequest(paymentResponse);
        }
    }
}