using PaymentGateway.Application.Responses;
using PaymentResponse = PaymentGateway.ViewModels.PaymentResponse;

namespace PaymentGateway.Extensions
{
    public static class ProcessPaymentResponseExtensions
    {
        public static PaymentResponse ToPaymentResponse(this ProcessPaymentResponse processPaymentResponse)
        {
            return new PaymentResponse
            {
                PaymentId = processPaymentResponse.PaymentId,
                Status = processPaymentResponse.PaymentStatus.ToViewModels()
            };
        }
    }
}