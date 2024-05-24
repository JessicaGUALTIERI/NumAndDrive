using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NumAndDrive.Models
{
	public class Car
	{
        public int CarId { get; set; }
		public string Brand { get; set; }
		public string Model { get; set; }
		public string Color { get; set; }
		public string Matriculation { get; set; }
		public string PicturePath { get; set; }

		public DateOnly? ArchiveDate { get; set; }


		// one-to-many
		public int FuelId { get; set; }
		public Fuel Fuel { get; set; } = null!;

		// one-to-one
		public string UserId { get; set; }
		public User User { get; set; }
	}
}

