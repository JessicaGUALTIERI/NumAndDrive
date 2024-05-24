using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
    public class FuelConfig : IEntityTypeConfiguration<Fuel>
    {
        public void Configure(EntityTypeBuilder<Fuel> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Fuel");

            // Primary Key
            modelBuilder.HasKey(x => x.FuelId);

            // Relationships
            modelBuilder
             .HasMany(x => x.Cars)
             .WithOne(x => x.Fuel)
             .HasForeignKey(x => x.CarId)
             .IsRequired();

            modelBuilder
             .HasMany(x => x.Cars)
             .WithOne(x => x.Fuel)
             .HasForeignKey(x => x.FuelId)
             .IsRequired();

            // Properties

            // Datas
            modelBuilder.HasData(
              new Fuel { FuelId = 1, Type = "Essence" },
              new Fuel { FuelId = 2, Type = "Diesel" },
              new Fuel { FuelId = 3, Type = "Hybride" },
              new Fuel { FuelId = 4, Type = "Électrique" },
              new Fuel { FuelId = 5, Type = "Autres" }
              );
        }
    }
}

