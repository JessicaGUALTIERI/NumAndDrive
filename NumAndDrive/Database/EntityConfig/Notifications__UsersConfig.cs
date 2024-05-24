using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class Notifications__UsersConfig : IEntityTypeConfiguration<Notifications_Users>
	{
        public void Configure(EntityTypeBuilder<Notifications_Users> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Notifications__Users");

            // Primary Key
            modelBuilder.HasKey(x => new {
                x.NotificationId,
                x.UserId
            });

            // Relationships
            modelBuilder.HasOne(x => x.Notification)
                .WithMany(x => x.Notifications_Users)
                .HasForeignKey(x => x.NotificationId)
                .IsRequired();

            modelBuilder.HasOne(x => x.User)
                .WithMany(x => x.Notifications_Users)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            // Properties

            // Datas
        }
    }
}

