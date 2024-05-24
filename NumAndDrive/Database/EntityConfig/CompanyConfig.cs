using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class CompanyConfig : IEntityTypeConfiguration<Company>
    {

        public void Configure(EntityTypeBuilder<Company> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Company");


            // Primary Key
            modelBuilder.HasKey(x => x.CompanyId);

            // Relationships
            modelBuilder
                .HasMany(x => x.Addresses)
                .WithOne(x => x.Company)
                .HasForeignKey(x => x.CompanyId)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.Departments)
                .WithOne(x => x.Company)
                .HasForeignKey(x => x.CompanyId)
                .IsRequired();

            // Properties :
            modelBuilder
                .Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            // Datas :
            modelBuilder.HasData(
                new Company { CompanyId = 1, Name = "Metz Numeric School" });

        }
    }
}

