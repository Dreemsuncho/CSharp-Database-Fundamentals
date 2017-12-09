using Instagraph.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.EntityConfiguration
{
    class UserFollowerConfig : IEntityTypeConfiguration<UserFollower>
    {
        public void Configure(EntityTypeBuilder<UserFollower> builder)
        {
            builder.HasKey(uf=>new{uf.FollowerId,uf.UserId});

            builder.HasOne(uf=>uf.User)
                .WithMany(u=>u.Followers)
                .HasForeignKey(u=>u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(uf=>uf.Follower)
                .WithMany(u=>u.UsersFollowing)
                .HasForeignKey(u=>u.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
