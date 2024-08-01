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
        public int NumberOfUsers { get; set; }
        public int NumberOfJourneys { get; set; }
        public Company Company { get; set; }
        public UploadUsersViewModel UploadUsersViewModel { get; set; }
    }
}

