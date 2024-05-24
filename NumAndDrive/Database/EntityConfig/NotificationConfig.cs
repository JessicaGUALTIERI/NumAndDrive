using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {

        public void Configure(EntityTypeBuilder<Notification> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Notification");

            // Primary Key
            modelBuilder.HasKey(x => x.NotificationId);

            // Relationships

            // Properties

            // Datas
            modelBuilder.HasData(
              new Notification { NotificationId = 1, Type = "Mail" },
              new Notification { NotificationId = 2, Type = "Application" },
              new Notification { NotificationId = 3, Type = "Téléphone" });
        }
    }
}

