using System;
using PaymentGateway.Application.Shared;

namespace PaymentGateway.Application.Extensions
{
    public static class PaymentsStatusExtensions
    {
        public static PaymentStatus ToApplicationPaymentStatus(this Domain.Entities.PaymentStatus status)
        {
            switch (status)
            {
                case Domain.Entities.PaymentStatus.Processed:
                    return PaymentStatus.Processed;
                case Domain.Entities.PaymentStatus.PendingBankRequest:
                    return PaymentStatus.Pending;
                case Domain.Entities.PaymentStatus.FailedOnBankRequest:
                case Domain.Entities.PaymentStatus.FailedOnDatabaseAccess:
                    return PaymentStatus.Failed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}