using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("UserType");

            // Primary Key
            modelBuilder.HasKey(x => x.UserTypeId);

            // Relationships
            modelBuilder
                .HasMany(x => x.Users)
                .WithOne(x => x.UserType)
                .HasForeignKey(x => x.UserTypeId)
                .IsRequired(false);

            // Properties
            modelBuilder
                .Property(x => x.TypeName)
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();

            // Datas
            modelBuilder.HasData(
                new UserType { UserTypeId = 1, TypeName = "Nouveau-elle venu-e" },
                new UserType { UserTypeId = 2, TypeName = "Sébastien Loeb" },
                new UserType { UserTypeId = 3, TypeName = "Auto-tamponneur" },
                new UserType { UserTypeId = 4, TypeName = "Boîte de nuit mobile" },
                new UserType { UserTypeId = 5, TypeName = "Grand-e voyageur-euse" },
                new UserType { UserTypeId = 6, TypeName = "Grand-e bavard-e" },
                new UserType { UserTypeId = 7, TypeName = "Pas du matin" },
                new UserType { UserTypeId = 8, TypeName = "Copilote au top" },
                new UserType { UserTypeId = 9, TypeName = "Compteur-euse d'histoires" },
                new UserType { UserTypeId = 10, TypeName = "Ronfleur-euse" },
                new UserType { UserTypeId = 11, TypeName = "Mamie au volant" });

        }
    }
}

