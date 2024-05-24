using System;
namespace NumAndDrive.Models
{
	public class Notifications_Users
	{
		public int NotificationId { get; set; }
		public Notification Notification { get; set; } = null!;

		public string UserId { get; set; }
		public User User { get; set; } = null!;
	}
}

