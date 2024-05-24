using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Review
	{
        public int ReviewId { get; set; }
		public int Rate { get; set; }
		public string Comment { get; set; }

        // one-to-many
        public string UserSenderId { get; set; } // Foreign key
        public User UserSender { get; set; } = null!;

        public string UserReceiverId { get; set; }
        public User UserReceiver { get; set; } = null!;
    }
}

