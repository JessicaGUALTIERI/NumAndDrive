using System;
using Microsoft.AspNetCore.Identity;

namespace NumAndDrive.Models.Repositories
{
	public interface IAdminRepository
	{
        Task UploadUsersFromCSVFile(Admin admin);
        string PasswordGenerator();
    }
}

