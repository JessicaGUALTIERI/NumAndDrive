using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Notification
	{
        public int NotificationId { get; set; }
		public string Type { get; set; }

        // many-to-many
        public ICollection<Notifications_Users> Notifications_Users { get; set; } = new List<Notifications_Users>();
    }
}

