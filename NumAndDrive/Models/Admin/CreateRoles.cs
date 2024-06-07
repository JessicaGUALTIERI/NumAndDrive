using System;
using Microsoft.AspNetCore.Identity;
using static System.Formats.Asn1.AsnWriter;
using System.Data;

namespace NumAndDrive.Models
{
    public class CreateRoles
    {
        public static async Task Create(IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
	}
}

