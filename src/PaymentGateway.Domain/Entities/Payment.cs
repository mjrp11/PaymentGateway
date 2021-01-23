using System;

namespace PaymentGateway.Domain.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        
        public Guid UniqueId { get; set; }
        
        public Card Card { get; set; }
        
        public decimal Amount { get; set; }
        
        public string Currency { get; set; }
        
        public PaymentStatus Status { get; set; }
        
        public Guid BankPaymentIdentifier { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public DateTime DateModified { get; set; }
    }

    public enum PaymentStatus
    {
        Processed,
        PendingBankRequest,
        FailedOnBankRequest,
        FailedOnDatabaseAccess
    }
}