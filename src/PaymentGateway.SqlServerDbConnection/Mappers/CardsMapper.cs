using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.SqlServerDbConnection.Mappers
{
    public class CardsMapper: IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("Cards");
            
            builder.HasKey(card => card.CardId);
            builder.Property(card => card.CardNumber).IsRequired().HasMaxLength(100);
            builder.Property(card => card.CardNumberMask).IsRequired().HasMaxLength(100);
            builder.Property(card => card.CVV).IsRequired().HasMaxLength(100);
            builder.Property(card => card.ExpireDateMonth).IsRequired();
            builder.Property(card => card.ExpireDateYear).IsRequired();

            builder.HasIndex(card => new {card.CardNumber, card.CVV, card.ExpireDateMonth, card.ExpireDateYear});
        }
    }
}