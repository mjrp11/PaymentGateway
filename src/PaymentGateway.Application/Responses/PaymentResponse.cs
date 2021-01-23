using System;
using PaymentGateway.Application.Shared;

namespace PaymentGateway.Application.Responses
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }
        
        public DateTime PaymentDate { get; set; }
        
        public decimal Amount { get; set; }
        
        public string Currency { get; set; }
        
        public string CardMask { get; set; }
        
        public string CardExpirationDate { get; set; }
        
        public PaymentStatus Status { get; set; }
    }
}