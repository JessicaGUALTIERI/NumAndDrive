using System;
using Microsoft.AspNetCore.Identity;

namespace NumAndDrive.Models
{
	public class User : IdentityUser
	{
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string ProfilePicturePath { get; set; }
		public DateOnly? ArchiveDate { get; set; }

		// many-to-one
		public virtual ICollection<Message> MessagesSent { get; } = new List<Message>();
		public virtual ICollection<Message> MessagesReceived { get; } = new List<Message>();

		// many-to-one
		public virtual ICollection<Journey> Journeys { get; } = new List<Journey>();

		// one-to-many
		public int? DepartmentId { get; set; }
		public Department Department { get; set; } = null!;

		// one-to-many
		public int? UserTypeId { get; set; }
		public UserType UserType { get; set; } = null!;

		// one-to-one
		public Car? Car { get; set; }

		// one-to-many
		public int? StatusId { get; set; }
		public Status Status { get; set; } = null!;

		// many-to-one
		public virtual ICollection<Review> ReviewsSent { get; } = new List<Review>();
		public virtual ICollection<Review> ReviewsReceived { get; } = new List<Review>();

		// many-to-many
		public ICollection<Journeys_Users> Journeys_Users { get; set; } = new List<Journeys_Users>();

		// many-to-many
		public ICollection<Notifications_Users> Notifications_Users { get; set; } = new List<Notifications_Users>();

		// many-to-many
		public ICollection<Rewards_Users> Rewards_Users { get; set; } = new List<Rewards_Users>();

		// many-to-many
		public ICollection<Filters_Users> Filters_Users { get; set; } = new List<Filters_Users>();
	}
}

