using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class Addresses__JourneysConfig : IEntityTypeConfiguration<Addresses_Journeys>
    {
        public void Configure(EntityTypeBuilder<Addresses_Journeys> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("addresses__journeys");

            // Primary Key
            modelBuilder.HasKey(x => new {
                x.AddressId,
                x.JourneyId
            });

            // Relationships
            modelBuilder
                .HasOne(x => x.Address)
                .WithMany(x => x.Addresses_Journeys)
                .HasForeignKey(x => x.AddressId)
                .IsRequired();

            modelBuilder
                .HasOne(x => x.Journey)
                .WithMany(x => x.Addresses_Journeys)
                .HasForeignKey(x => x.JourneyId)
                .IsRequired();

            // Properties

            // Datas
        }
    }
}

