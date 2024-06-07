using System;
using Microsoft.AspNetCore.Identity;

namespace NumAndDrive.Models
{
	public class CreateAdmin
	{
        public static async Task Create(IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string mail = "admin@gmail.com";
                string password = "Administrator!1";

                if (await userManager.FindByEmailAsync(mail) == null)
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
                        UserTypeId = 1,
                        ProfilePicturePath = "/img/profile-pic-blue.png",
                        UserName = mail,
                    };

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}

