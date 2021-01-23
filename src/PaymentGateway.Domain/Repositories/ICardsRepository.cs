using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Domain.Repositories
{
    public interface ICardsRepository
    {
        Card GetCard(string cardNumberEncrypted, string cvvEncrypted, int requestExpireDateMonth, int requestExpireDateYear);
    }
}