using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class CarConfig : IEntityTypeConfiguration<Car>
    {


        public void Configure(EntityTypeBuilder<Car> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("car");


            // Primary Key
            modelBuilder.HasKey(x => x.CarId);

            // Relationships

            // Properties
            modelBuilder
                .Property(x => x.Brand)
                .HasMaxLength(15)
                .HasColumnType("varchar")
                .IsRequired();

            modelBuilder
                .Property(x => x.Model)
                .HasMaxLength(15)
                .HasColumnType("varchar")
                .IsRequired();

            modelBuilder
                .Property(x => x.Color)
                .HasMaxLength(20)
                .HasColumnType("varchar")
                .IsRequired();

            modelBuilder
                .Property(x => x.Matriculation)
                .HasMaxLength(15)
                .HasColumnType("varchar")
                .IsRequired();

            modelBuilder
                .Property(x => x.PicturePath)
                .HasMaxLength(255)
                .HasColumnType("varchar")
                .IsRequired();

            modelBuilder
                .Property(x => x.FuelId)
                .HasColumnType("int")
                .IsRequired();

            modelBuilder
                .Property(x => x.UserId)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder
                .Property(x => x.ArchiveDate)
                .HasColumnType("date")
                .IsRequired(false);

            // Datas
        }
    }
}

