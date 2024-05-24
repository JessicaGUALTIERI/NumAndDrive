using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Department
	{
        public int DepartmentId { get; set; }
		public string Name { get; set; }

		public DateOnly? ArchiveDate { get; set; }

		// many-to-one
		public virtual ICollection<User> Users { get; set; } = new List<User>();

		// one-to-many
		public int CompanyId { get; set; }
		public Company Company { get; set; } = null!;
	}
}

