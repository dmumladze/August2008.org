using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using August2008.Model;

namespace August2008.Common.Interfaces
{
    public interface IGeocodeService
    {
        Country GetCountry(string country, string countryCode);
        State GetState(string state, string country, string countryCode);
        City GetCity(string city, string state, string postalCode, string country, string countryCode);
        Address GetAddress(string street, string city, string state, string postalCode, string country, string countryCode);

        bool TryGetGeoLocation(PayPalTransaction source, out GeoLocation location); 
    }
}
