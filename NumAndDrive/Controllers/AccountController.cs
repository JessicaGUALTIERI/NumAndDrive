using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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

    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly NumAndDriveDbContext Db;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, NumAndDriveDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            Db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    string userId = _userManager.GetUserId(User);
                    var user = Db.Users.Find(userId);
                    if (user.IsFirstLogin == 0)
                    { 
                        Db.SaveChanges();
                        return View("FirstLogin");
                    }
                    return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult FirstLogin()
        {
            ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel()
            {
                Statuses = Db.Statuses,
                Departments = Db.Departments
            };

            return View(changePasswordViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> FirstLogin(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                _userManager.ResetPasswordAsync(user, token, change.NewPassword);


                user.IsFirstLogin = 1;
                Db.SaveChanges();
                _userManager.UpdateAsync(user);
                _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", "Home");
            }

            return View("FirstLogin");

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

