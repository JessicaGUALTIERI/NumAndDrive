using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class UserConfig : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("User");

            // Primary Key
            modelBuilder.HasKey(x => x.Id);

            // Relationships
            modelBuilder
                .HasOne(x => x.Car)
                .WithOne(x => x.User)
                .HasForeignKey<Car>(x => x.UserId)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.Journeys)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.MessagesSent)
                .WithOne(x => x.UserSender)
                .HasForeignKey(x => x.UserSenderId)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.MessagesReceived)
                .WithOne(x => x.UserReceiver)
                .HasForeignKey(x => x.UserReceiverId)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.MessagesSent)
                .WithOne(x => x.UserSender)
                .HasForeignKey(x => x.UserSenderId)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.MessagesReceived)
                .WithOne(x => x.UserReceiver)
                .HasForeignKey(x => x.UserReceiverId)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.ReviewsSent)
                .WithOne(x => x.UserSender)
                .HasForeignKey(x => x.UserSenderId)
                .IsRequired();

            modelBuilder
                .HasMany(x => x.ReviewsReceived)
                .WithOne(x => x.UserReceiver)
                .HasForeignKey(x => x.UserReceiverId)
                .IsRequired();

            // Properties
            modelBuilder
                .Property(x => x.LastName)
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder
                .Property(x => x.IsFirstLogin)
                .HasColumnType("tinyint")
                .HasMaxLength(1)
                .HasDefaultValue(0);

            modelBuilder
                .Property(x => x.FirstName)
                .HasColumnType("varchar(100)")
                .IsRequired();

            modelBuilder
                .Property(x => x.ProfilePicturePath)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            

            // Datas
        }
    }
}

