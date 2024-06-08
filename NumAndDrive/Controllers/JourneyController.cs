﻿using System;
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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace NumAndDrive.Controllers
{
    [Authorize]
    public class JourneyController : Controller
    {
        private readonly NumAndDriveDbContext Db;
        private readonly UserManager<User> UserManager;
        private readonly IJourneyRepository JourneyRepository;

        public JourneyController(NumAndDriveDbContext db, IJourneyRepository journeyRepository, UserManager<User> userManager)
        {
            Db = db;
            JourneyRepository = journeyRepository;
            UserManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            JourneyCompanyViewModel journeyCompanyViewModel = new JourneyCompanyViewModel
            {
                Company = Db.Companies,
            };

            return View(journeyCompanyViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(JourneyCompanyViewModel journeyCompanyViewModel, User user)
        {
            string completeAddress = journeyCompanyViewModel.AddressToTrim;
            (string postalAddress,string postalCode,string city) = JourneyRepository.AddressTrimer(completeAddress);
            Address address = new Address
            {
                PostalAddress = postalAddress,
                PostalCode = postalCode,
                City = city,
            };

            var result = Db.Addresses.AddAsync(address);
            Db.SaveChanges();

            if (result.IsCompletedSuccessfully)
            {

                var lastAddress = Db.Addresses.OrderByDescending(x => x.AddressId).FirstOrDefault();
                int lastAddressId = lastAddress.AddressId;

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

                Db.Journeys.Add(journey);
                Db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetJourneys()
        {
            var journeys = await Db.Journeys.
                Include(j => j.User)
                .Include(j => j.AddressDeparting)
                .Include(j => j.AddressIncoming)
                .ThenInclude(j => j.Company)
                .ToListAsync();
                
            return View(journeys);
        }

        [HttpPost]
        public IActionResult BookJourney(int journeyId)
        {
            Journey journey = Db.Journeys.Find(journeyId);

            Journeys_Users journeys_Users = new Journeys_Users()
            {
                UserId = UserManager.GetUserId(User),
                JourneyId = journeyId
            };

            journey.AvailableSpots--;
            Db.Journeys_Users.Add(journeys_Users);
            Db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult JourneyDetails(int journeyId)
        {
            var journey = Db.Journeys.
                Where(x => x.JourneyId == journeyId)
                .Include(j => j.User)
                .Include(j => j.AddressDeparting)
                .Include(j => j.AddressIncoming)
                .ThenInclude(j => j.Company)
                .Select(j => new JourneyCompanyViewModel
                {
                    JourneyId = j.JourneyId,
                    UserFirstName = j.User.FirstName,
                    UserLastName = j.User.LastName,
                    AddressDeparting = j.AddressDeparting.City,
                    AddressIncoming = j.AddressIncoming.Company.Name,
                    DepartureDate = j.DepartureDate,
                    DepartureHour = j.DepartureHour,
                    AvailableSpots = j.AvailableSpots,
                })
                .FirstOrDefault();

            return PartialView("_JourneyDetails", journey);

        }
    }
}

