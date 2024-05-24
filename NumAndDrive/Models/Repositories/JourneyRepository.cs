using System;
using System.Text.RegularExpressions;

namespace NumAndDrive.Models.Repositories
{
    public class JourneyRepository : IJourneyRepository
    {
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
    }
}

