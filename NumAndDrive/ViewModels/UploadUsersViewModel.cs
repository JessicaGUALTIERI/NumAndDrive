using System;
namespace NumAndDrive.ViewModels
{
	public class UploadUsersViewModel
	{
		public IFormFile File { get; set; }
		public int NumberUsersNotValidated { get; set; }
	}
}

