using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class JourneyConfig : IEntityTypeConfiguration<Journey>
    {

        public void Configure(EntityTypeBuilder<Journey> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("journey");

            // Primary Key
            modelBuilder.HasKey(x => x.JourneyId);

            // Relationships

            // Propeties
            modelBuilder
                .Property(x => x.DepartureHour)
                .HasColumnType("time")
                .IsRequired();

            modelBuilder
                .Property(x => x.DepartureDate)
                .HasColumnType("date")
                .IsRequired();

            modelBuilder
                .Property(x => x.AvailableSpots)
                .HasColumnType("int")
                .IsRequired();

            modelBuilder
                .Property(x => x.CreationDate)
                .HasColumnType("date")
                .IsRequired();

            modelBuilder
                .Property(x => x.UserId)
                .HasColumnType("varchar(255)")
                .IsRequired();

            modelBuilder
                .Property(x => x.AddressDepartingId)
                .HasColumnType("int")
                .IsRequired();

            modelBuilder
                .Property(x => x.AddressIncomingId)
                .HasColumnType("int")
                .IsRequired();

            modelBuilder
                .Property(x => x.ArchiveDate)
                .HasColumnType("date")
                .IsRequired(false);

            // Datas
        }
    }
}

