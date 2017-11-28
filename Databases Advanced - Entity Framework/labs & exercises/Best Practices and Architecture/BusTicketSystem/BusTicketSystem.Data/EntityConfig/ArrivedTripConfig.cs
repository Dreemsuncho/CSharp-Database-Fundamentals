using BusTicketSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTicketSystem.Data.EntityConfig
{
    internal class ArrivedTripConfig : IEntityTypeConfiguration<ArrivedTrip>
    {
        public void Configure(EntityTypeBuilder<ArrivedTrip> builder)
        {
            builder.HasOne(at => at.OriginBusStation)
                .WithMany(obs => obs.OriginArrivedTrips)
                .HasForeignKey(at => at.OriginBusStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(at => at.DestinationBusStation)
                .WithMany(obs => obs.DestinationArrivedTrips)
                .HasForeignKey(at => at.DestinationBusStationId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
