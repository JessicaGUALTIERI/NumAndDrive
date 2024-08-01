using System;
namespace NumAndDrive.Models.Repositories
{
	public interface IJourneyService
	{
		(string postalAddress, string postalCode, string city) AddressTrimer(string addressToTrim);
	}
}

