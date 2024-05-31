using System;
using NumAndDrive.Models;

namespace NumAndDrive.ViewModels
{
	public class ProfileUserViewModel
	{
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string ProfilePicturePath { get; set; }
		public string UserType { get; set; }
		public string City { get; set; }
		public string Status { get; set; }
		public List<Journey> JourneysCompleted { get; set; }
        public List<Journey> JourneysToCome { get; set; }
    }
}

