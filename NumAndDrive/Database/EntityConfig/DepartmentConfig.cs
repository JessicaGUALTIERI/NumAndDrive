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
            modelBuilder.ToTable("Department");

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
                new Department { DepartmentId = 1, Name = "Service non renseigné", CompanyId = 1 },
                new Department { DepartmentId = 2, Name = "Administration", CompanyId = 1 },
                new Department { DepartmentId = 3, Name = "Équipe pédagogique", CompanyId = 1 },
                new Department { DepartmentId = 4, Name = "CDA", CompanyId = 1 },
                new Department { DepartmentId = 5, Name = "BSD", CompanyId = 1 },
                new Department { DepartmentId = 6, Name = "M2i", CompanyId = 1 },
                new Department { DepartmentId = 7, Name = "DevWeb", CompanyId = 1 },
                new Department { DepartmentId = 8, Name = "DFS", CompanyId = 1 },
                new Department { DepartmentId = 9, Name = "BSRC", CompanyId = 1 },
                new Department { DepartmentId = 10, Name = "TSSR", CompanyId = 1 },
                new Department { DepartmentId = 11, Name = "Incubateur", CompanyId = 1 },
                new Department { DepartmentId = 12, Name = "Num&Boost", CompanyId = 1 },
                new Department { DepartmentId = 13, Name = "MSRC", CompanyId = 1 },
                new Department { DepartmentId = 14, Name = "BSRC", CompanyId = 1 }
            );
        }
    }
}

