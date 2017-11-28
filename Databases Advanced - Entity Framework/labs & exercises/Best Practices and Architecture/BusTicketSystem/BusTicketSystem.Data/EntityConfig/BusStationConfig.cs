using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusTicketSystem.Models;

namespace BusTicketSystem.Data.EntityConfig
{
    internal class BusStationConfig : IEntityTypeConfiguration<BusStation>
    {
        public void Configure(EntityTypeBuilder<BusStation> builder)
        {
            builder.Property(bs => bs.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(40)");

            builder.HasOne(bs => bs.Town)
                .WithMany(t => t.BusStations)
                .HasForeignKey(bs => bs.TownId);
        }
    }
}
