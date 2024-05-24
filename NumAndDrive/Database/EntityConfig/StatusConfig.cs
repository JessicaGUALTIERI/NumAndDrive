using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class StatusConfig : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Status");

            // Primary Key
            modelBuilder.HasKey(x => x.StatusId);

            // Relationships
            modelBuilder
                .HasMany(x => x.Users)
                .WithOne(x => x.Status)
                .HasForeignKey(x => x.StatusId)
                .IsRequired(false);

            // Properties

            // Datas
            modelBuilder.HasData(
                new Status { StatusId = 1, Type = "Intervenant-e" },
                new Status { StatusId = 2, Type = "Administrateur-trice" },
                new Status { StatusId = 3, Type = "Apprenant-e" },
                new Status { StatusId = 4, Type = "Formateur-trice" });
        }
    }
}

