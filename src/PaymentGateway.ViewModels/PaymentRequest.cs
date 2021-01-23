using System;

namespace PaymentGateway.ViewModels
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }
        
        public int ExpireDateYear { get; set; }
        
        public int ExpireDateMonth { get; set; }
        
        public string CVV { get; set; }
        
        public decimal Amount { get; set; }
        
        public string Currency { get; set; }
    }
}