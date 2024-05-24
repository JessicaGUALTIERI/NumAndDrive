using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Reward
	{
        public int RewardId { get; set; }
		public string Name { get; set; }
		public string PicturePath { get; set; }
        public string Description { get; set; }

        // many-to-many
        public ICollection<Rewards_Users> Rewards_Users { get; set; } = new List<Rewards_Users>();
    }
}

