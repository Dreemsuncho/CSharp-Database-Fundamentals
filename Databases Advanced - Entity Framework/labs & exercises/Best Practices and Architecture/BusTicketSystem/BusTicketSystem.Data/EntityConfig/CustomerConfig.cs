using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusTicketSystem.Models;

namespace BusTicketSystem.Data.EntityConfig
{
    internal class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Ignore(c => c.FullName);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasColumnType("VARCHAR(40)");

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasColumnType("VARCHAR(40)");

            builder.Property(c => c.DateOfBirth)
                .IsRequired()
                .HasColumnType("DATE");

            builder.HasOne(c => c.HomeTown)
                .WithMany(ht => ht.CustomerHomeTowns)
                .HasForeignKey(c => c.HomeTownId);
        }
    }
}
