using System;
using NumAndDrive.Models;

namespace NumAndDrive.Models
{
	public class ActivationDays_Journeys
	{
		public int ActivationDayId { get; set; }
		public ActivationDay ActivationDay { get; set; }

		public int JourneyId { get; set; }
		public Journey Journey { get; set; }

		public DateOnly? ArchiveDate { get; set; }
	}
}

