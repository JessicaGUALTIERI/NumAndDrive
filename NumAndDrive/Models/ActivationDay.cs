using System;
using System.ComponentModel.DataAnnotations;
using NumAndDrive.Models;

namespace NumAndDrive.Models
{
	public class ActivationDay
	{
        public int ActivationDayId { get; set; }
		public string Day { get; set; }
        public DateOnly? ArchiveDate { get; set; }

        // many-to-many
        public ICollection<ActivationDays_Journeys> ActivationDays_Journeys { get; set; } = new List<ActivationDays_Journeys>();
    }
}

