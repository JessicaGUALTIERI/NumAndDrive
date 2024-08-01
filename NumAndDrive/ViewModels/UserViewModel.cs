using System;
using System.ComponentModel.DataAnnotations;
using NumAndDrive.Models;

namespace NumAndDrive.ViewModels
{
	public class UserViewModel
	{
        public User User { get; set; }

        public IEnumerable<Status> Statuses { get; set; }
        public IEnumerable<Department> Departments { get; set; }
	}
}


