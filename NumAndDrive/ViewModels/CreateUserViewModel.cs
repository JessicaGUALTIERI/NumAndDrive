using System;
using NumAndDrive.Models;

namespace NumAndDrive.ViewModels
{
	public class CreateUserViewModel
	{
		public User User { get; set; }
		public IEnumerable<Status> Statuses { get; set; }
		public IEnumerable<Department> Departments { get; set; }
	}
}

