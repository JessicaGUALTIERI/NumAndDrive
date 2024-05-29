using System;
using Microsoft.AspNetCore.Identity;
using NumAndDrive.ViewModels;

namespace NumAndDrive.Models.Repositories
{
	public interface IAdminRepository
	{
        Task UploadUsersFromCSVFile(Admin admin);
        string PasswordGenerator();
        bool IsUserValid(User user);
        bool IsEditUserViewModelValid(EditUserViewModel editUserViewModel);
    }
}

