using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NumAndDrive.Database;
using NumAndDrive.ViewModels;

namespace NumAndDrive.Models.Repositories
{
    public class JourneyService : IJourneyService
    {
        private readonly NumAndDriveDbContext Db;

        public JourneyService(NumAndDriveDbContext db)
        {
            Db = db;
        }

        /// <summary>
        /// allows to split a complete address into 3 distinct parts: a street address, a postal code, and a city based on the place of the postal code 
        /// </summary>
        /// <param name="addressToTrim"></param>
        /// <returns>returns 3 strings, a postal code, a street address and a city</returns>
        public (string postalAddress, string postalCode, string city) AddressTrimer(string addressToTrim)
        {
            var regex = new Regex(@"(\d{5})");
            var match = regex.Match(addressToTrim);

            string postalCode = match.Value;

            int indexCodePostal = addressToTrim.IndexOf(postalCode);

            string postalAddress = addressToTrim.Substring(0, indexCodePostal).Trim();
            string city = addressToTrim.Substring(indexCodePostal + postalCode.Length).Trim();

            return (postalAddress, postalCode, city);
        }

        /// <summary>
        /// Get all the companies in the database
        /// </summary>
        /// <returns>List async of companies</returns>
        public async Task<List<Company>> GetCompanies()
        {
            return await Db.Companies.ToListAsync();
        }

        /// <summary>
        /// Add async the complete adresse to the database
        /// </summary>
        /// <param name="adress"></param>
        /// <returns></returns>
        public async Task AddAddressAsync(Address adress)
        {
            await Db.Addresses.AddAsync(adress);
            await Db.SaveChangesAsync();
            
        }

        /// <summary>
        /// Add async entire journey to the database
        /// </summary>
        /// <param name="journey"></param>
        /// <returns></returns>
        public async Task AddJourneyAsync(Journey journey)
        {
            await Db.Journeys.AddAsync(journey);
            await Db.SaveChangesAsync();
        }

        /// <summary>
        /// Allows to get the last added int address in the dabatase
        /// </summary>
        /// <returns>ID of the last address</returns>
        public int GetLastAddress()
        {
            var lastAddress = Db.Addresses.OrderByDescending(x => x.AddressId).FirstOrDefault();
            return lastAddress.AddressId;
        }

        /// <summary>
        /// Retrieves all journeys with associated addresses and destination company information after performing the necessary join operations
        /// </summary>
        /// <returns>List of journeys async</returns>
        public async Task<List<Journey>> GetJourneysAsync()
        {
            return await Db.Journeys.
                Include(j => j.User)
                .Include(j => j.AddressDeparting)
                .Include(j => j.AddressIncoming)
                .ThenInclude(j => j.Company)
                .ToListAsync();
        }

        /// <summary>
        /// Allows to get a journey by his id
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns>a single journey</returns>
        public Journey GetJourneyById(int journeyId)
        {
            return Db.Journeys.Find(journeyId);
        }

        /// <summary>
        /// Inserts into the database, in the Address_Users join table, the ID of the user who made the reservation as well as the ID of the reserved journey.        /// </summary>
        /// <param name="journeys_Users"></param>
        /// <returns></returns>
        public async Task AddJourneyUsersAsync(Journeys_Users journeys_Users)
        {
            await Db.Journeys_Users.AddAsync(journeys_Users);
            await Db.SaveChangesAsync();
        }

        /// <summary>
        /// Reduces the available spots on a journey by 1 after a user has booked a trip
        /// </summary>
        /// <param name="journey"></param>
        public void ReduceAvailableSpotBy1(Journey journey)
        {
            journey.AvailableSpots--;
        }

        /// <summary>
        /// Allows retrieving the details of a journey, including the first and last names of the journey creator, departure time and date, arrival and departure addresses, and available spots through join operations.
        /// </summary>
        /// <param name="journeyId"></param>
        /// <returns>A single journey</returns>
        public JourneyCompanyViewModel? GetJourneyDetailsById(int journeyId)
        {
            var journey =  Db.Journeys.
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

            if(journey != null)
            {
                return journey;
            }

            return null;
        }

        /// <summary>
        /// Allows to update the departure date, hours and available spots
        /// </summary>
        /// <param name="journeyId"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateJourneyAsync(int journeyId, JourneyUpdateViewModel model, string userId)
        {
            var journey = await Db.Journeys
                .Include(j => j.User)
                .FirstOrDefaultAsync(j => j.JourneyId == journeyId);

            if (journey == null)
            {
                return false;
            }

            if (journey.UserId != userId)
            {
                return false; 
            }

            journey.DepartureDate = model.DepartureDate;
            journey.DepartureHour = model.DepartureHour;
            journey.AvailableSpots = model.AvailableSpots;

            Db.Journeys.Update(journey);
            await Db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Allows to delete a journey
        /// </summary>
        /// <param name="journeyId"></param>
        public void DeleteJourney(int journeyId)
        {
            var journey = GetJourneyById(journeyId);
            if (journey != null)
            {
                Db.Journeys.Remove(journey);
                Db.SaveChanges();
            }
        }
    }
}

