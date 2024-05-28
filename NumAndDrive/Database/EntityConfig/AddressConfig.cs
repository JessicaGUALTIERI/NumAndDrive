using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("address");

            // Primary Key
            modelBuilder.HasKey(x => x.AddressId);

            // Relationships
            modelBuilder
                .HasMany(x => x.JourneysDeparting)
                .WithOne(x => x.AddressDeparting)
                .HasForeignKey(x => x.AddressDepartingId)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.JourneysIncoming)
                .WithOne(x => x.AddressIncoming)
                .HasForeignKey(x => x.AddressIncomingId)
                .IsRequired();

            // Properties
            modelBuilder
               .Property(x => x.PostalAddress)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired();

            modelBuilder
                .Property(x => x.City)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder
                .Property(x => x.PostalCode)
                .HasColumnType("varchar")
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder
                .Property(x => x.CompanyId)
                .HasColumnType("int")
                .IsRequired(false);

            // Datas :
            modelBuilder
                .HasData(
                new Address { AddressId = 1, PostalAddress = "86 rue aux arènes", City = "Metz", PostalCode = "57000", CompanyId = 1 });
        }
    }
}

