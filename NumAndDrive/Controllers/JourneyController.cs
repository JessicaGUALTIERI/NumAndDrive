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
using NumAndDrive.Models.Repositories;
using NumAndDrive.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace NumAndDrive.Controllers
{
    [Authorize]
    public class JourneyController : Controller
    {
        private readonly UserManager<User> UserManager;
        private readonly IJourneyService JourneyService;

        public JourneyController(IJourneyService journeyService, UserManager<User> userManager)
        {
            JourneyService = journeyService;
            UserManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var companies = await JourneyService.GetCompanies();
            JourneyCompanyViewModel journeyCompanyViewModel = new JourneyCompanyViewModel
            {
                Company = companies
            };

            return View(journeyCompanyViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(JourneyCompanyViewModel journeyCompanyViewModel, User user)
        {
            string completeAddress = journeyCompanyViewModel.AddressToTrim;
            (string postalAddress,string postalCode,string city) = JourneyService.AddressTrimer(completeAddress);
            Address address = new Address
            {
                PostalAddress = postalAddress,
                PostalCode = postalCode,
                City = city,
            };
            await JourneyService.AddAddressAsync(address);

            var lastAddressId = JourneyService.GetLastAddress();
            Journey journey = new Journey
            {

                DepartureDate = journeyCompanyViewModel.Journey.DepartureDate,
                DepartureHour = journeyCompanyViewModel.Journey.DepartureHour,
                AvailableSpots = journeyCompanyViewModel.Journey.AvailableSpots,
                UserId = UserManager.GetUserId(User),
                AddressDepartingId = lastAddressId,
                AddressIncomingId = journeyCompanyViewModel.Journey.AddressIncomingId,
                CreationDate = DateOnly.FromDateTime(DateTime.UtcNow),
            };
            await JourneyService.AddJourneyAsync(journey);
            

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetJourneys()
        {
            var journeys = await JourneyService.GetJourneysAsync();
            return View(journeys);
        }

        [HttpPost]
        public async Task<IActionResult> BookJourney(int journeyId)
        {
            var journey = JourneyService.GetJourneyById(journeyId);
            Journeys_Users journeys_Users = new Journeys_Users()
            {
                UserId = UserManager.GetUserId(User),
                JourneyId = journeyId
            };
            JourneyService.ReduceAvailableSpotBy1(journey);
            await JourneyService.AddJourneyUsersAsync(journeys_Users);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult JourneyDetails(int journeyId)
        {
            var journey = JourneyService.GetJourneyDetailsById(journeyId);
            return PartialView("_JourneyDetails", journey);

        }

        public IActionResult Edit(int JourneyId)
        {
            var journey = JourneyService.GetJourneyById(JourneyId);
            Console.WriteLine(journey.JourneyId);
            JourneyUpdateViewModel model = new JourneyUpdateViewModel
            {
                DepartureDate = journey.DepartureDate,
                DepartureHour = journey.DepartureHour,
                AvailableSpots = journey.AvailableSpots,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int JourneyId, JourneyUpdateViewModel model)
        {
            string? userId = UserManager.GetUserId(User);
            bool updateSuccess = await JourneyService.UpdateJourneyAsync(JourneyId, model, userId);

            if (updateSuccess)
            {
                return RedirectToAction(nameof(Index)); 
            }
            else
            {
                TempData["ErrorMessage"] = "You are not authorized to update this journey or the journey does not exist.";
                return RedirectToAction(nameof(Error)); 
            }
        }

        [HttpPost]
        public IActionResult Delete(int JourneyId)
        {
            var journey = JourneyService.GetJourneyById(JourneyId);
            if (journey == null)
            {
                return NotFound();
            }

            JourneyService.DeleteJourney(JourneyId);

            return RedirectToAction("Index");
        }
    }
}

