using System;
using NumAndDrive.Models;

namespace NumAndDrive.ViewModels
{
	public class JourneyCompanyViewModel
	{
		public Journey Journey { get; set; }
		public IEnumerable<Company> Company { get; set; }
		public string AddressToTrim { get; set; }

        public string AddressDeparting { get; set; }
        public string AddressIncoming { get; set; }
        public string CompanyName { get; set; }
		public string UserFirstName { get; set; }
		public string UserLastName { get; set; }
		public DateOnly DepartureDate { get; set; }
		public TimeOnly DepartureHour { get; set; }
		public int AvailableSpots { get; set; }
		public int JourneyId { get; set; }

	}
}



