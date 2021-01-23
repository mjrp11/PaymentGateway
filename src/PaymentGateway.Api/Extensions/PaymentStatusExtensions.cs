using System;
using PaymentGateway.ViewModels;

namespace PaymentGateway.Extensions
{
    public static class PaymentStatusExtensions
    {
        public static PaymentStatus ToViewModels(this Application.Shared.PaymentStatus status)
        {
            switch (status)
            {
                case Application.Shared.PaymentStatus.Processed:
                    return PaymentStatus.Processed;
                case Application.Shared.PaymentStatus.Pending:
                    return PaymentStatus.Pending;
                case Application.Shared.PaymentStatus.Failed:
                    return PaymentStatus.Failed;
                default:
                    return PaymentStatus.Unknown;
            }
        }
    }
}