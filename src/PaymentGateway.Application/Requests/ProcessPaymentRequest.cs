using PaymentGateway.Application.Shared;

namespace PaymentGateway.Application.Requests
{
    public class ProcessPaymentRequest
    {
        public string CardNumber { get; set; }
        
        public int ExpireDateYear { get; set; }
        
        public int ExpireDateMonth { get; set; }
        
        public string CVV { get; set; }
        
        public decimal Amount { get; set; }
        
        public Currency Currency { get; set; }
    }
    
    
}