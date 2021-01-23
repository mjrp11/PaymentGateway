using System;
using System.Threading.Tasks;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;

namespace PaymentGateway.Application.Services
{
    public interface IPaymentService
    {
        Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request);
        Task<PaymentResponse> GetPaymentInformation(Guid paymentId);
    }
}