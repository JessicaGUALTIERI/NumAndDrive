using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Fuel
	{
        public int FuelId { get; set; }
		public string Type { get; set; }

		// many-to-one
		public virtual ICollection<Car> Cars { get; } = new List<Car>();
	}
}

