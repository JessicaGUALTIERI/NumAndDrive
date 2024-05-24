using System;
namespace NumAndDrive.Models
{
	public class Filters_Users
	{
		public int FilterId { get; set; }
		public Filter Filter { get; set; } = null!;

		public string UserId { get; set; }
		public User User { get; set; } = null!;
	}
}

