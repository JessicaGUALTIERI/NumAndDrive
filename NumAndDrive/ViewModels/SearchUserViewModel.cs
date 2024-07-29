using System;
using NumAndDrive.Models;

namespace NumAndDrive.ViewModels
{
	public class SearchUserViewModel
	{
		public List<User> Users { get; set; }
		public string Query { get; set; }
	}
}

