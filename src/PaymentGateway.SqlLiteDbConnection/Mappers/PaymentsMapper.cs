using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.SqlLiteDbConnection.Mappers
{
    public class PaymentsMapper: IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            
            builder.HasKey(payment => payment.PaymentId);
            builder.Property(payment => payment.UniqueId).IsRequired();
            builder.Property(payment => payment.Currency).IsRequired().HasMaxLength(3);
            builder.Property(payment => payment.Amount).IsRequired().HasPrecision(20, 3);
            builder.Property(payment => payment.Status).IsRequired();
            builder.Property(payment => payment.BankPaymentIdentifier);
            builder.Property(payment => payment.DateCreated).IsRequired();
            builder.Property(payment => payment.DateModified);

            builder.HasOne(payment => payment.Card).WithMany().IsRequired();
            
            builder.HasIndex(payment => payment.UniqueId);
        }
    }
}