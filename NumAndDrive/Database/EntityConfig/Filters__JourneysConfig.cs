using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class Filters__JourneysConfig : IEntityTypeConfiguration<Filters_Journeys>
	{
		public void Configure(EntityTypeBuilder<Filters_Journeys> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Filters__Journeys");

            // Primary Key
            modelBuilder.HasKey(x => new {
                x.FilterId,
                x.JourneyId
            });

            // Relationships
            modelBuilder
                .HasOne(x => x.Filter)
                .WithMany(x => x.Filters_Journeys)
                .HasForeignKey(x => x.FilterId)
                .IsRequired();

            modelBuilder
                .HasOne(x => x.Journey)
                .WithMany(x => x.Filters_Journeys)
                .HasForeignKey(x => x.JourneyId)
                .IsRequired();

            // Properties

            // Datas
        }
    }
}

