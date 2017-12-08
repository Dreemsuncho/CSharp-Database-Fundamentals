using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamBuilder.Models;

namespace TeamBuilder.Data.Configuration
{
    class EventTeamConfiguration : IEntityTypeConfiguration<EventTeam>
    {
        public void Configure(EntityTypeBuilder<EventTeam> builder)
        {
            builder.ToTable("EventTeams");

            builder.HasKey(et => new { et.EventId, et.TeamId });

            builder.HasOne(et => et.Event)
                .WithMany(e => e.ParticipatingEventTeams)
                .HasForeignKey(et => et.EventId);

            builder.HasOne(et => et.Team)
                .WithMany(t => t.Events)
                .HasForeignKey(et => et.TeamId);
        }
    }
}
