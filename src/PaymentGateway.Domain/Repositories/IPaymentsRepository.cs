using System;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Domain.Repositories
{
    public interface IPaymentsRepository
    {
        void Save(Payment payment);
        Payment GetPayment(Guid paymentId);
    }
}