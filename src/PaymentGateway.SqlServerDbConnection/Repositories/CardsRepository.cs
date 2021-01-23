using System.Linq;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.SqlServerDbConnection.Repositories
{
    public class CardsRepository: ICardsRepository
    {
        private readonly DbContext _dbContext;

        public CardsRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Card GetCard(
            string cardNumberEncrypted, 
            string cvvEncrypted, 
            int requestExpireDateMonth,
            int requestExpireDateYear)
        {
            return _dbContext.Set<Card>().FirstOrDefault(card =>
                card.CardNumber.Equals(cardNumberEncrypted) && card.CVV.Equals(cvvEncrypted) &&
                card.ExpireDateMonth == requestExpireDateMonth && card.ExpireDateYear == requestExpireDateYear);
        }
    }
}