using System;
using NumAndDrive.ViewModels;

namespace NumAndDrive.Models.Repositories
{
	public interface IJourneyService
	{
		(string postalAddress, string postalCode, string city) AddressTrimer(string addressToTrim);
		Task<List<Company>> GetCompanies();
		Task AddAddressAsync(Address address);
		Task AddJourneyAsync(Journey journey);
		int GetLastAddress();
        Task<List<Journey>> GetJourneysAsync();
		Journey GetJourneyById(int journeyId);
        Task AddJourneyUsersAsync(Journeys_Users journeys_Users);
		void ReduceAvailableSpotBy1(Journey journey);
		JourneyCompanyViewModel? GetJourneyDetailsById(int journeyId);
		Task<bool> UpdateJourneyAsync(int journeyId, JourneyUpdateViewModel model, string userId);
		void DeleteJourney(int journeyId);
    }
}

