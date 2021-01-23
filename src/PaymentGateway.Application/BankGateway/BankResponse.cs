using System;

namespace PaymentGateway.Application.BankGateway
{
    public class BankResponse
    {
        public Guid PaymentIdentifier { get; set; }
        
        public BankResponseStatus Status { get; set; }
    }
}