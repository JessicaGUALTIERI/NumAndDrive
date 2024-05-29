using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NumAndDrive.Database;
using NumAndDrive.Models;

namespace NumAndDrive.Controllers;

public class HomeController : Controller
{
    private NumAndDriveDbContext Db { get; set; }
    

    private readonly ILogger<HomeController> _logger;


    public HomeController(NumAndDriveDbContext db)
    {
        Db = db;
    }

    public IActionResult Index()
    {
        var user = Db.Users.ToList();
        return View(user);
    }
}

