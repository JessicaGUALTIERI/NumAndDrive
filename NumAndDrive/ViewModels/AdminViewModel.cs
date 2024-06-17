using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using NumAndDrive.Database;
using NumAndDrive.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace NumAndDrive.ViewModels
{
	public class AdminViewModel
	{
		[Required]
        public IFormFile File { get; set; }
        public NumAndDriveDbContext Db { get; set; }
        public UserManager<User> _userManager;
        public int NumberOfUsers { get; set; }
    }
}

