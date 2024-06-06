using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class ActivationDayConfig : IEntityTypeConfiguration<ActivationDay>
	{
		public void Configure(EntityTypeBuilder<ActivationDay> modelBuilder)
		{
            // Name
            modelBuilder.ToTable("ActivationDay");

            // Primary Key
            modelBuilder.HasKey(x => x.ActivationDayId);

            // Relationships

            // Properties

            // Datas
            modelBuilder.HasData(
                new ActivationDay { ActivationDayId = 1, Day = "Lundi" },
                new ActivationDay { ActivationDayId = 2, Day = "Mardi" },
                new ActivationDay { ActivationDayId = 3, Day = "Mercredi" },
                new ActivationDay { ActivationDayId = 4, Day = "Jeudi" },
                new ActivationDay { ActivationDayId = 5, Day = "Vendredi" },
                new ActivationDay { ActivationDayId = 6, Day = "Samedi" },
                new ActivationDay { ActivationDayId = 7, Day = "Dimanche" }
                );
        }
	}
}

