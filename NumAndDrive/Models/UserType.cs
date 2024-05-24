using System;
namespace NumAndDrive.Models
{
	public class UserType
	{
		public int UserTypeId { get; set; }
		public string TypeName { get; set; }

		// many-to-one
		public virtual ICollection<User> Users { get; } = new List<User>();
	}
}

