using System;
using NumAndDrive.Models;

namespace NumAndDrive.ViewModels
{
	public class JourneyCompanyViewModel
	{
		public Journey Journey { get; set; }
		public IEnumerable<Company> Company { get; set; }
		public string AddressToTrim { get; set; }
	}
}

