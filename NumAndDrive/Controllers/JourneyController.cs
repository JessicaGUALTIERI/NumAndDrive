using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NumAndDrive.Database;
using NumAndDrive.Models;
using NumAndDrive.Models.Repositories;
using NumAndDrive.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NumAndDrive.Controllers
{
    public class JourneyController : Controller
    {
        private readonly NumAndDriveDbContext Db;
        private readonly UserManager<User> UserManager;
        private readonly IJourneyRepository JourneyRepository;

        public JourneyController(NumAndDriveDbContext db, IJourneyRepository journeyRepository)
        {
            Db = db;
            JourneyRepository = journeyRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var journey = Db.Journeys.ToList().Take(5);
            return View(journey);
        }

        [HttpGet]
        public IActionResult Add()
        {
            //var company = Db.Companies.ToList();
            //return View(company);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(JourneyCompanyViewModel journeyCompanyViewModel)
        {
            //var lastJourney = Db.Journeys.LastOrDefault();
            //int lastJourneyId = lastJourney.JourneyId;
            //if (lastJourneyId == null)
            //{
            //    lastJourneyId = 1;
            //}

            //var lastAddress = Db.Addresses.OrderBy(x => x.AddressId).LastOrDefault();
            //int lastAddressId = lastAddress.AddressId;

            string completeAddress = journeyCompanyViewModel.AddressToTrim;
            (string postalAddress,string postalCode,string city) = JourneyRepository.AddressTrimer(completeAddress);

            Address address = new Address
            {
                PostalAddress = postalAddress,
                PostalCode = postalCode,
                City = city,
            };

            Db.Addresses.Add(address);
            Db.SaveChanges();


            //Journey journey = new Journey
            //{
            //    JourneyId = lastJourneyId,
            //    DepartureDate = journeyCompanyViewModel.Journey.DepartureDate,
            //    DepartureHour = journeyCompanyViewModel.Journey.DepartureHour,
            //    AvailableSpots = journeyCompanyViewModel.Journey.AvailableSpots,
            //    UserId = userManager.GetUserId(user),
            //    AddressDepartingId = (int)journeyCompanyViewModel.Address.CompanyId,
            //    AddressIncomingId = journeyCompanyViewModel.Journey.AddressIncomingId,
            //    CreationDate = DateOnly.FromDateTime(DateTime.UtcNow),
            //};



            //Db.Journeys.Add(journey);
            //Db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

