using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class ReviewConfig : IEntityTypeConfiguration<Review>
    {

        public void Configure(EntityTypeBuilder<Review> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Review");

            // Primary Key
            modelBuilder.HasKey(x => x.ReviewId);

            // Relationships

            // Properties
            modelBuilder
                .Property(x => x.Rate)
                .HasColumnType("int")
                .IsRequired();

            modelBuilder
                .Property(x => x.Comment)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);

            modelBuilder
                .Property(x => x.UserSenderId)
                .HasColumnType("varchar(255)")
                .IsRequired();

            modelBuilder
                .Property(x => x.UserReceiverId)
                .HasColumnType("varchar(255)")
                .IsRequired();

            // Datas
        }
    }
}

