using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusTicketSystem.Models;

namespace BusTicketSystem.Data.EntityConfig
{
    internal class TownConfig : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.Property(t => t.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(40)");

            builder.Property(t => t.Country)
                .IsRequired()
                .HasColumnType("VARCHAR(40)");
        }
    }
}
