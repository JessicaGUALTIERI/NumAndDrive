using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Status
	{
        public int StatusId { get; set; }
		public string Type { get; set; }

		// many-to-one
		public virtual ICollection<User> Users { get; } = new List<User>();
	}
}

