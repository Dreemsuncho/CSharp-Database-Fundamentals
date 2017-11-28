using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusTicketSystem.Models;

namespace BusTicketSystem.Data.EntityConfig
{
    internal class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(40)");

            builder.Property(c => c.Nationality)
                .IsRequired()
                .HasColumnType("VARCHAR(10)");
        }
    }
}
