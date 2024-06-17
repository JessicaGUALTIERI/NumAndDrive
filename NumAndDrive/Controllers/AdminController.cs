using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NumAndDrive.Models;
using NumAndDrive.Models.Repositories;
using NumAndDrive.ViewModels;

namespace NumAndDrive.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
 
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public IActionResult Index()
        {
            AdminViewModel admin = new AdminViewModel { NumberOfUsers = _adminRepository.GetNumberOfUsersInDatabase() };
            return View(admin);
        }

        public IActionResult GetUsers()
        {
            List<User> users = _adminRepository.GetUsers();
            return View(users);
        }

        [HttpPost]
        public IActionResult GetUsersByName(string searchString)
        {
            List<User> user = _adminRepository.GetUsersByName(searchString);
            return View(user);
        }

        public IActionResult Details(string id)
        {
            User user = _adminRepository.GetUserDetails(id);
            return View(user);
        }

        public IActionResult Edit(string id)
        {
            User user = _adminRepository.GetUserDetails(id);
            EditUserViewModel editUserViewModel = new EditUserViewModel {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Statuses = _adminRepository.GetStatuses(),
                Departments = _adminRepository.GetDepartments()
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditUserViewModel newUser, string id)
        {
            if (ModelState.IsValid && newUser != null && _adminRepository.IsEditUserViewModelValid(newUser))
            {
                User user = _adminRepository.GetUserDetails(id);

                if (user != null)
                {
                    _adminRepository.EditUser(newUser, user);
                }
                return RedirectToAction(nameof(GetUsers));
            }
            return View(newUser);
        }

        public IActionResult Archive(string id)
        {
            User user = _adminRepository.GetUserDetails(id);

            _adminRepository.ArchiveUser(user);

            return RedirectToAction(nameof(GetUsers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadUsersFromCSVFile(AdminViewModel admin)
        {
           await _adminRepository.UploadUsersFromCSVFile(admin);
           return RedirectToAction(nameof(Index));
        }

        public IActionResult CreateSingleUser()
        {
            CreateUserViewModel createSingleUserViewModel = new CreateUserViewModel
            {
                Statuses = _adminRepository.GetStatuses(),
                Departments = _adminRepository.GetDepartments()
            };
            return View(createSingleUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSingleUser(CreateUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                await _adminRepository.CreateSingleUser(userViewModel);
                return RedirectToAction(nameof(Index));
            }
            userViewModel.Statuses = _adminRepository.GetStatuses();
            userViewModel.Departments = _adminRepository.GetDepartments();
            return View(userViewModel);
        }

        public IActionResult DownloadCsv()
        {
            var bytes = _adminRepository.GetCSVFile();
            return File(bytes, "application/octet-stream", "users_not_created.csv");
        }
    }
}

