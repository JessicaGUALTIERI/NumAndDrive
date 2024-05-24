using System;
namespace NumAndDrive.Models.Repositories
{
	public interface IJourneyRepository
	{
		(string postalAddress, string postalCode, string city) AddressTrimer(string addressToTrim);
	}
}

