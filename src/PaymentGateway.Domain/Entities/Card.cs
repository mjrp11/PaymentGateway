using System.Collections.Generic;

namespace PaymentGateway.Domain.Entities
{
    public class Card
    {
        public int CardId { get; protected set; }
        
        public string CardNumber { get; set; }
        
        public string CardNumberMask { get; set; }
        
        public int ExpireDateYear { get; set; }
        
        public int ExpireDateMonth { get; set; }
        
        public string CVV { get; set; }
    }
}