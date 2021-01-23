using System.Threading.Tasks;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.BankGateway
{
    public interface IBankGateway
    {
        public Task<BankResponse> ValidatePayment(Payment payment);
    }
}