using System;
namespace NumAndDrive.Models
{
	public class Addresses_Journeys
	{
		public int AddressId { get; set; }
		public Address Address { get; set; }

		public int JourneyId { get; set; }
		public Journey Journey { get; set; }
	}
}

