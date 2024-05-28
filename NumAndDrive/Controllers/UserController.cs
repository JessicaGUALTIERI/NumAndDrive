using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NumAndDrive.Database;
using NumAndDrive.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NumAndDrive.Controllers
{
    public class UserController : Controller
    {

        public NumAndDriveDbContext Db;
        public UserManager<User> UserManager;

        public UserController(NumAndDriveDbContext db, UserManager<User> userManager)
        {
            Db = db;
            UserManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult getMyId()
        {
            string userId = UserManager.GetUserId(User);
            User userToGet = Db.Users.Find(userId);
            return View(userToGet);
        }
    }
}

