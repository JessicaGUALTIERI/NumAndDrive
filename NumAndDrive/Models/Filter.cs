using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Filter
	{
        public int FilterId { get; set; }
		public string Name { get; set; }

        // many-to-many
        public ICollection<Filters_Journeys> Filters_Journeys { get; set; } = new List<Filters_Journeys>();

        // many-to-many
        public ICollection<Filters_Users> Filters_Users { get; set; } = new List<Filters_Users>();
    }
}

