using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class FilterConfig : IEntityTypeConfiguration<Filter>
    {
        public void Configure(EntityTypeBuilder<Filter> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Filter");

            // Primary Key
            modelBuilder.HasKey(x => x.FilterId);

            // Relationships

            // Properties

            // Datas
            modelBuilder
                .HasData(
                new Filter { FilterId = 1, Name = "Fumeur" },
                new Filter { FilterId = 2, Name = "Non fumeur" },
                new Filter { FilterId = 3, Name = "Bavard" },
                new Filter { FilterId = 4, Name = "Silencieux" },
                new Filter { FilterId = 5, Name = "Animaux bienvenus" },
                new Filter { FilterId = 6, Name = "Coffre vide" },
                new Filter { FilterId = 7, Name = "Coffre plein" },
                new Filter { FilterId = 8, Name = "En musique" },
                new Filter { FilterId = 9, Name = "Au calme" }
                );

        }
    }
}

