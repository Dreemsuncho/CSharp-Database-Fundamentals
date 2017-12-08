using Microsoft.EntityFrameworkCore;
using TeamBuilder.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamBuilder.Data.Configuration
{
    class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(25);
            builder.HasIndex(t => t.Name)
                .IsUnique();

            builder.Property(t => t.Description)
                .HasMaxLength(32);

            builder.Property(t => t.Acronym)
                .IsRequired()
                .HasMaxLength(3);

            builder.HasMany(t => t.Invitations)
                .WithOne(i => i.Team)
                .HasForeignKey(i => i.TeamId);
        }
    }
}
