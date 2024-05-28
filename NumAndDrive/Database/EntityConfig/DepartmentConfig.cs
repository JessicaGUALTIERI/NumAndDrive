using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("department");

            // Primary Key
            modelBuilder.HasKey(x => x.DepartmentId);

            // Relationships
            modelBuilder
              .HasMany(x => x.Users)
              .WithOne(x => x.Department)
              .HasForeignKey(x => x.DepartmentId)
              .IsRequired(false);

            // Properties :
            modelBuilder
                .Property(x => x.Name)
                .HasMaxLength(255)
                .HasColumnType("varchar")
                .IsRequired();

            modelBuilder
                .Property(x => x.CompanyId)
                .HasColumnType("int")
                .IsRequired();

            modelBuilder
                .Property(x => x.ArchiveDate)
                .HasColumnType("date")
                .IsRequired(false);

            // Datas :
            modelBuilder.HasData(
                new Department { DepartmentId = 1, Name = "CDA", CompanyId = 1 });
        }
    }
}

