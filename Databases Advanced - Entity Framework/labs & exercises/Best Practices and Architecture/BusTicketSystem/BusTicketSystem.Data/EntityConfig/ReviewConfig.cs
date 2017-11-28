using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusTicketSystem.Models;

namespace BusTicketSystem.Data.EntityConfig
{
    internal class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.Content)
                .IsRequired();

            builder.Property(r => r.PublishDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(r => r.Company)
                .WithMany(bs => bs.Reviews)
                .HasForeignKey(r => r.CompanyId);

            builder.HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
