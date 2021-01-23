using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.SqlLiteDbConnection.Repositories
{
    public class PaymentsRepository: IPaymentsRepository
    {
        private readonly DbContext _dbContext;

        public PaymentsRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save(Payment payment)
        {
            if (payment.PaymentId == 0)
            {
                _dbContext.Add(payment);
            }
            else
            {
                _dbContext.Update(payment);
            }

            _dbContext.SaveChanges();
        }

        public Payment GetPayment(Guid paymentId)
        {
            return _dbContext.Set<Payment>()
                .Include(payment => payment.Card)
                .FirstOrDefault(payment => payment.UniqueId.Equals(paymentId));
        }
    }
}