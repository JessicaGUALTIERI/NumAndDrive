using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class MessageConfig : IEntityTypeConfiguration<Message>
    {

        public void Configure(EntityTypeBuilder<Message> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Message");

            // Primary Key
            modelBuilder.HasKey(x => x.MessageId);

            // Relationships

            // Properties
            modelBuilder
                .Property(x => x.PicturePath)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder
                .Property(x => x.Content)
                .HasColumnType("text")
                .IsRequired();

            modelBuilder
                .Property(x => x.DateSent)
                .HasColumnType("date")
                .IsRequired();

            modelBuilder
                .Property(x => x.DateReceived)
                .HasColumnType("date")
                .IsRequired();

            modelBuilder
                .Property(x => x.UserSenderId)
                .HasColumnType("varchar(255)")
                .IsRequired();

            modelBuilder
                .Property(x => x.UserReceiverId)
                .HasColumnType("varchar(255)")
                .IsRequired();

            modelBuilder
                .Property(x => x.ArchiveDate)
                .HasColumnType("date")
                .IsRequired(false);

            // Datas
        }
    }
}

