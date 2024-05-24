using System;
using System.ComponentModel.DataAnnotations;
using NumAndDrive.Models;

namespace NumAndDrive.Models
{
	public class Address
	{
        public int AddressId { get; set; }
		public string PostalAddress { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }

		public DateOnly? ArchiveDate { get; set; }

		// many-to-one
		public virtual ICollection<Journey> JourneysDeparting { get; set; } = new List<Journey>();

        // many-to-one
        public virtual ICollection<Journey> JourneysIncoming { get; set; } = new List<Journey>();

		// one-to-many
		public int? CompanyId { get; set; }
		public Company Company { get; set; } = null!;

		// many-to-many
		public ICollection<Addresses_Journeys> Addresses_Journeys { get; } = new List<Addresses_Journeys>();
    }
}

