using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusTicketSystem.Models;

namespace BusTicketSystem.Data.EntityConfig
{
    internal class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.Property(ba => ba.AccountNumber)
                .IsRequired()
                .HasColumnType("NVARCHAR(40)");

            builder.Property(ba => ba.Balance)
                .IsRequired()
                .HasColumnType("MONEY");

            builder.HasOne(ba => ba.Customer)
                .WithOne(c => c.BankAccount)
                .HasForeignKey<BankAccount>(ba=>ba.CustomerId);
        }
    }
}
