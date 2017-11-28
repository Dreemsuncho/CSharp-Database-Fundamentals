using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusTicketSystem.Models;

namespace BusTicketSystem.Data.EntityConfig
{
    internal class TripConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.Property(t => t.DepartureTime)
                .IsRequired();

            builder.Property(t => t.ArrivalTime)
                .IsRequired();

            builder.HasOne(t => t.OriginBusStation)
                .WithMany(obs => obs.OriginTrips)
                .HasForeignKey(t => t.OriginBusStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.DestinationBusStation)
                .WithMany(dbs => dbs.DestinationTrips)
                .HasForeignKey(t => t.DestinationBusStationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.BusCompany)
                .WithMany(bc => bc.Trips)
                .HasForeignKey(t => t.BusCompanyId);
        }
    }
}
