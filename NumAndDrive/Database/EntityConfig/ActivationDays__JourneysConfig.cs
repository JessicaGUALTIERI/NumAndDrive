using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class ActivationDays__JourneysConfig : IEntityTypeConfiguration<ActivationDays_Journeys>
    {
        public void Configure(EntityTypeBuilder<ActivationDays_Journeys> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("activationDays__journeys");


            // Primary Key
            modelBuilder.HasKey(x => new { x.JourneyId, x.ActivationDayId });


            // Relationships
            modelBuilder
                .HasOne(x => x.Journey)
                .WithMany(x => x.ActivationDays_Journeys)
                .HasForeignKey(x => x.JourneyId)
                .IsRequired();

            modelBuilder
                .HasOne(x => x.ActivationDay)
                .WithMany(x => x.ActivationDays_Journeys)
                .HasForeignKey(x => x.ActivationDayId)
                .IsRequired();

            // Properties

            // Datas
        }
    }
}

