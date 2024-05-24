using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using NumAndDrive.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NumAndDrive.Models
{
	public class Admin
	{
		[Required]
        public IFormFile File { get; set; }
        public NumAndDriveDbContext Db { get; set; }
        public UserManager<User> _userManager;
    }
}

