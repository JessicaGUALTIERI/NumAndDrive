using System;
using Microsoft.AspNetCore.Identity;

namespace NumAndDrive.Models
{
	public static class CreateAdmin
	{
        private static readonly UserManager<User> _userManager;

        public static async Task Create()
        {
            string mail = "admin@gmail.com";
            if (await _userManager.FindByEmailAsync(mail) == null)
            {
                User user = new User()
                {
                    LastName = "admin",
                    FirstName = "admin",
                    IsFirstLogin = 1,
                    Email = mail,
                    PhoneNumber = "0707070707",
                    StatusId = 1,
                    DepartmentId = 1,
                    UserTypeId = 11,
                    ProfilePicturePath = "/img/profile-pic-blue.png",
                    UserName = mail,
                };

                await _userManager.CreateAsync(user, "Administrator!1");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}

