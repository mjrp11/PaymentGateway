using System;
using PaymentGateway.Application.Shared;

namespace PaymentGateway.Application.Responses
{
    public class ProcessPaymentResponse
    {
        public PaymentStatus PaymentStatus { get; set; }
        public Guid PaymentId { get; set; }
    }
}