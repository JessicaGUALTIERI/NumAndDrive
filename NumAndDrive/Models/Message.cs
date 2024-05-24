using System;
using System.ComponentModel.DataAnnotations;

namespace NumAndDrive.Models
{
	public class Message
	{
        public int MessageId { get; set; }
		public string PicturePath { get; set; }
		public string Content { get; set; }
		public DateOnly DateSent { get; set; }
		public DateOnly DateReceived { get; set; }

		public DateOnly? ArchiveDate { get; set; }


		// one-to-many
		public string UserSenderId { get; set; }
		public User UserSender { get; set; } = null!;

        // one-to-many
        public string UserReceiverId { get; set; }
		public User UserReceiver { get; set; } = null!;
	}
}

