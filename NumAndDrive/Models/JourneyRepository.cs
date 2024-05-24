using System;
using NumAndDrive.Database;

namespace NumAndDrive.Models
{
	public class JourneyRepository
	{
        private readonly NumAndDriveDbContext _database;

        public JourneyRepository(NumAndDriveDbContext db)
        {
            _database = db;
        }

        public IEnumerable<Journey> GetJourneys()
        {
            var journeys = _database.Journeys.ToList();
            return journeys;
        }

        public void Add(Journey journey)
        {
            var journeys = _database.Journeys.ToList();
            if (journeys.Count() > 0)
            {
                var maxId = journeys.Max(x => x.JourneyId);
                journey.JourneyId = maxId + 1;
            } else
            {
                var maxId = 1;
            }
            journeys??= new List<Journey>();
            journeys.Add(journey);
        }
    }
}

