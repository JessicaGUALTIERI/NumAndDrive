using System;
namespace NumAndDrive.Models
{
	public class Journeys_Users
	{
		public int JourneyId { get; set; }
		public Journey Journey { get; set; } = null!;

		public string UserId { get; set; }
		public User User { get; set; } = null!;
	}
}

