using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Company
	{
        public int CompanyId { get; set; }
		public string Name { get; set; }

		// many-to-one
		public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

        // many-to-one
        public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}

