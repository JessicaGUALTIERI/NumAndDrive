using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NumAndDrive.Models;

namespace NumAndDrive.Database.EntityConfig
{
	public class RewardConfig : IEntityTypeConfiguration<Reward>
    {

        public void Configure(EntityTypeBuilder<Reward> modelBuilder)
        {
            // Name
            modelBuilder.ToTable("Reward");

            // Primary Key
            modelBuilder.HasKey(x => x.RewardId);

            // Relationships

            // Properties

            // Datas
            modelBuilder.HasData(
                new Reward { RewardId = 1, Name = "Voyageur-euse en herbe", PicturePath = "", Description = "Effectuer son premier voyage en passager-e" },
                new Reward { RewardId = 2, Name = "Apprenti-e conducteur-rice", PicturePath = "", Description = "Effectuer son premier voyage en conducteur-rice" },
                new Reward { RewardId = 3, Name = "Contrôle qualité", PicturePath = "", Description = "Donner son premier avis" },
                new Reward { RewardId = 4, Name = "Étoile montante", PicturePath = "", Description = "Recevoir son premier avis" });
        }
    }
}

