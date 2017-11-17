using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfiguration
{
    class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.Property(ba => ba.Balance)
                   .IsRequired();

            builder.Property(ba => ba.BankName)
                   .IsUnicode()
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(ba => ba.SwiftCode)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasMaxLength(20);
        }
    }
}
