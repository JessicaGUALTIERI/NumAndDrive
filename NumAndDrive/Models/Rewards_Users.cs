using System;
namespace NumAndDrive.Models
{
	public class Rewards_Users
	{
		public int RewardId { get; set; }
		public Reward Reward { get; set; } = null!;

		public string UserId { get; set; }
		public User User { get; set; } = null!;
	}
}

