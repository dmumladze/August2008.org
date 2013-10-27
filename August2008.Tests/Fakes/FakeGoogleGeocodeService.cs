using System;
using August2008.Common.Interfaces;
using August2008.Model;

namespace August2008.Tests.Fakes
{
    public class FakeGoogleGeocodeService : IGeocodeService
    {
        public Country GetCountry(string country, string countryCode)
        {
            return null;
        }
        public State GetState(string state, string country, string countryCode)
        {
            return null;
        }
        public City GetCity(string city, string state, string postalCode, string country, string countryCode)
        {
            return null;
        }
        public Address GetAddress(string street, string city, string state, string postalCode, string country, string countryCode)
        {
            return null;
        }
        public bool TryGetGeoLocation(PayPalVariables source, out GeoLocation location)
        {
            location = null;
            return false;
        }
    }
}
