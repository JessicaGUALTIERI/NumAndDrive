using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Journey
	{
        public int JourneyId { get; set; }
		public TimeOnly DepartureHour { get; set; }
		public DateOnly DepartureDate { get; set; }
		public int AvailableSpots { get; set; }
		public DateOnly CreationDate { get; set; }

        public DateOnly? ArchiveDate { get; set; }

        // one-to-many
        public string UserId { get; set; }
		public User User { get; set; } = null!;

		// one-to-many
		public int AddressDepartingId { get; set; }
		public Address AddressDeparting { get; set; } = null!;

        public int AddressIncomingId { get; set; }
        public Address AddressIncoming { get; set; } = null!;

        // many-to-many
        public ICollection<ActivationDays_Journeys> ActivationDays_Journeys { get; } = new List<ActivationDays_Journeys>();

        // many-to-many
        public ICollection<Addresses_Journeys> Addresses_Journeys { get; } = new List<Addresses_Journeys>();

        // many-to-many
        public ICollection<Filters_Journeys> Filters_Journeys { get; set; } = new List<Filters_Journeys>();

        // many-to-many
        public ICollection<Journeys_Users> Journeys_Users { get; set; } = new List<Journeys_Users>();
    }
}

