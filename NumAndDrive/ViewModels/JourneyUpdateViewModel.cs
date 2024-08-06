using System;
namespace NumAndDrive.ViewModels
{
	public class JourneyUpdateViewModel
	{
        public DateOnly DepartureDate { get; set; }
        public TimeOnly DepartureHour { get; set; }
        public int AvailableSpots { get; set; }
        public int journeyId { get; set; }
    }
}

