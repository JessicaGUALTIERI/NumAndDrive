using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NumAndDrive.Database;
using NumAndDrive.Models;
using NumAndDrive.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NumAndDrive.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private NumAndDriveDbContext _db;
        private UserManager<User> _userManager;

        public UserController(NumAndDriveDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            string id = _userManager.GetUserId(User);
            User user = _db.Users.Find(id);

            int? userTypeId = user.UserTypeId;

            if (userTypeId == null)
                userTypeId = 11;

            UserType userType = _db.UserTypes.Find(userTypeId);

            int? statusId = user.StatusId;
            Status status = _db.Statuses.Find(statusId);

            List<Journey> journeys = _db.Journeys
                .Include(x => x.AddressDeparting)
                .Where(x => x.UserId == id)
                .ToList();

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly now = TimeOnly.FromDateTime(DateTime.Now);

            List<Journey> journeysCompleted = journeys.Where(x => (x.DepartureDate == today && x.DepartureHour < now) || (x.DepartureDate < today))
                .ToList();

            List<Journey> journeysToCome = journeys.Where(x => (x.DepartureDate == today && x.DepartureHour > now) || (x.DepartureDate > today))
                .ToList();
                      
            ProfileUserViewModel profileUser = new ProfileUserViewModel
            {
                LastName = user.LastName,
                FirstName = user.FirstName,
                ProfilePicturePath = user.ProfilePicturePath,
                UserType = userType.TypeName,
                City = null,
                Status = status.Type,
                JourneysCompleted = journeysCompleted,
                JourneysToCome = journeysToCome
            };
            return View(profileUser);
        }

        public IActionResult Settings()
        {
            return View();
        }
    }
}

