using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class Rewards__UsersConfig : IEntityTypeConfiguration<Rewards_Users>
	{
		public void Configure(EntityTypeBuilder<Rewards_Users> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Rewards__Users");

            // Primary Key
            modelBuilder.HasKey(x => new { x.RewardId, x.UserId });

            // Relationships
            modelBuilder
                .HasOne(x => x.Reward)
                .WithMany(x => x.Rewards_Users)
                .HasForeignKey(x => x.RewardId)
                .IsRequired();

            modelBuilder
                .HasOne(x => x.User)
                .WithMany(x => x.Rewards_Users)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            // Properties

            // Datas
        }
    }
}

