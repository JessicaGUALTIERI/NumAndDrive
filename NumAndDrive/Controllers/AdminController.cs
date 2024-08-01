using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NumAndDrive.Models;
using NumAndDrive.Models.Repositories;
using NumAndDrive.Services;
using NumAndDrive.ViewModels;

namespace NumAndDrive.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
 
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            AdminViewModel admin = new AdminViewModel {
                NumberOfUsers = _adminService.GetNumberOfUsers(),
                NumberOfJourneys = _adminService.GetNumberOfJourneys(),
                Company = _adminService.GetCompany()
            };
            return View(admin);
        }

        public IActionResult GetUsers()
        {
            List<User> users = _adminService.GetUsers();
            return View(users);
        }

        [HttpPost]
        public IActionResult GetUsersByName(string searchString)
        {
            SearchUserViewModel user = _adminService.GetUsersByName(searchString);
            return View(user);
        }

        public IActionResult Details(string id)
        {
            User user = _adminService.GetUser(id);
            return View(user);
        }

        public IActionResult Edit(string id)
        {
            User user = _adminService.GetUser(id);
            UserViewModel userViewModel = new UserViewModel
            {
                User = user,
                Statuses = _adminService.GetStatuses(),
                Departments = _adminService.GetDepartments()
            };
            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                _adminService.EditUser(userViewModel);
                return RedirectToAction(nameof(GetUsers));
            }
            userViewModel.Statuses = _adminService.GetStatuses();
            userViewModel.Departments = _adminService.GetDepartments();
            return View(userViewModel);
        }

        public IActionResult Archive(string id)
        {
            User user = _adminService.GetUser(id);
            if (user != null)
            {
                _adminService.ArchiveUser(user);
            }
            return RedirectToAction(nameof(GetUsers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadUsersFromCSVFile(UploadUsersViewModel uploadUsersViewModel)
        {
            if (uploadUsersViewModel.File != null)
            {
                await _adminService.UploadUsersFromCSVFile(uploadUsersViewModel);
            }
           return RedirectToAction(nameof(Index), new AdminViewModel() { UploadUsersViewModel = uploadUsersViewModel });
        }

        public IActionResult Create()
        {
            UserViewModel createSingleUserViewModel = new UserViewModel
            {
                Statuses = _adminService.GetStatuses(),
                Departments = _adminService.GetDepartments()
            };
            return View(createSingleUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                await _adminService.CreateSingleUser(userViewModel);
                return RedirectToAction(nameof(Index));
            }
            userViewModel.Statuses = _adminService.GetStatuses();
            userViewModel.Departments = _adminService.GetDepartments();
            return View(userViewModel);
        }
    }
}

