using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class Filters__UsersConfig : IEntityTypeConfiguration<Filters_Users>
	{
		public void Configure(EntityTypeBuilder<Filters_Users> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Filters__Users");

            // Primary Key
            modelBuilder.HasKey(x => new { x.FilterId, x.UserId });

            // Relationships
            modelBuilder
                .HasOne(x => x.Filter)
                .WithMany(x => x.Filters_Users)
                .HasForeignKey(x => x.FilterId)
                .IsRequired();

            modelBuilder
                .HasOne(x => x.User)
                .WithMany(x => x.Filters_Users)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            // Properties

            // Datas
        }
    }
}

