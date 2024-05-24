using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class Journeys__UsersConfig : IEntityTypeConfiguration<Journeys_Users>
	{
		public void Configure(EntityTypeBuilder<Journeys_Users> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Journeys__Users");

            // Primary Key
            modelBuilder.HasKey(x => new { x.JourneyId, x.UserId });

            // Relationships
            modelBuilder.HasOne(x => x.Journey)
                .WithMany(x => x.Journeys_Users)
                .HasForeignKey(x => x.JourneyId)
                .IsRequired();

            modelBuilder.HasOne(x => x.User)
                .WithMany(x => x.Journeys_Users)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            // Properties

            // Datas
        }
    }
}

