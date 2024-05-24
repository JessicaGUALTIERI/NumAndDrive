using System;
namespace NumAndDrive.Models
{
	public class Filters_Journeys
	{
		public int FilterId { get; set; }
		public Filter Filter { get; set; } = null!;

		public int JourneyId { get; set; }
		public Journey Journey { get; set; } = null!;
	}
}

