using System;

namespace PaymentGateway.ViewModels
{
    public class PaymentResponse
    {
        public PaymentStatus Status { get; set; }
        
        public bool IsSuccess => Status == PaymentStatus.Processed;
        
        public Guid PaymentId { get; set; }
    }
}