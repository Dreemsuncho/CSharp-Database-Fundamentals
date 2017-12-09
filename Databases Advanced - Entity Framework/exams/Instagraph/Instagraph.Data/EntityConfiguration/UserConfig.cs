using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Instagraph.Models;

namespace Instagraph.Data.EntityConfiguration
{
    class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Username)
                .HasMaxLength(30)
                .IsRequired();
            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.Property(u => u.Password)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasOne(u => u.ProfilePicture)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.ProfilePictureId);
        }
    }
}
