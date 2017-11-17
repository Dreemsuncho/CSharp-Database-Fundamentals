using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfiguration
{
    class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.Property(cc => cc.Limit)
                   .IsRequired();

            builder.Property(cc => cc.MoneyOwed)
                   .IsRequired();

            builder.Property(cc => cc.ExpirationDate)
                   .IsRequired();

            builder.Ignore(cc => cc.LimitLeft);
        }
    }
}
