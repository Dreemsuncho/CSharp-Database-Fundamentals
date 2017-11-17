using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfiguration
{
    class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.Property(pm => pm.Type)
                   .IsRequired();

            builder.Property(pm => pm.UserId)
                   .IsRequired();

            builder.Property(pm => pm.BankAccountId)
                   .IsRequired(false);

            builder.Property(pm => pm.CreditCardId)
                   .IsRequired(false);

            builder.HasOne(pm => pm.BankAccount)
                   .WithOne();

            builder.HasOne(pm => pm.CreditCard)
                   .WithOne();

            builder.HasIndex(pm => new { pm.BankAccountId, pm.UserId, pm.CreditCardId })
                .IsUnique();
        }
    }
}
