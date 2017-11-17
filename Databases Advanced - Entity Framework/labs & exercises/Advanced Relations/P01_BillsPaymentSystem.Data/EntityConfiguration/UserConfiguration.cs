using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_BillsPaymentSystem.Models;

namespace P01_BillsPaymentSystem.Data.EntityConfiguration
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName)
                   .IsUnicode()
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.LastName)
                   .IsUnicode()
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Email)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasMaxLength(80);

            builder.Property(u => u.Password)
                   .IsUnicode(false)
                   .IsRequired()
                   .HasMaxLength(25);
        }
    }
}
