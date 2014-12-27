using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using August2008.Common;
using August2008.Common.Interfaces;
using August2008.Model;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace August2008.Services
{
    public class GoogleGeocodeService : IGeocodeService
    {
        private static char[] invalidChars1 = new char[] { '[', '#', '!', '@', '#', '$', '%', '_', ']' }; 
        private const string invalidChars2 = "[#!@#$%_]";         

        private IMetadataRepository _metadataRepository;

        public GoogleGeocodeService(IMetadataRepository metadataReposity, ILog log)
        {
            _metadataRepository = metadataReposity;
            Log = log;
            ServiceUrlFormat = ConfigurationManager.AppSettings["GeocodeUrlFormat"];
        }

        private string ServiceUrlFormat;
        private ILog Log;

        public Country GetCountry(string country, string countryCode)
        {
            var geo = GoogleGeocode(null, null, null, null, country, countryCode);
            if (geo != null)
            {
                var q = from a in geo.results
                        from b in a.address_components
                        where b.types.Contains("country")
                        select new Country {
                            Name = b.short_name,
                            FullName = b.long_name,
                            Latitude = a.geometry.location.lat,
                            Longitude = a.geometry.location.lng
                        };
                return q.FirstOrDefault();
            }
            return default(Country);
        }
        public State GetState(string state, string country, string countryCode)
        {
            var geo = GoogleGeocode(null, null, state, null, country, countryCode);
            if (geo != null)
            {
                var q = from a in geo.results
                        from b in a.address_components
                        where b.types.Contains("administrative_area_level_1")
                        select new State {
                            Name = b.short_name,
                            FullName = b.long_name,
                            Latitude = a.geometry.location.lat,
                            Longitude = a.geometry.location.lng,
                            Country = country ?? (from c in a.address_components where c.types.Contains("country")
                                                  select c.long_name).SingleOrDefault()
                        };
                return q.FirstOrDefault();
            }
            return default(State);
        }
        public City GetCity(string city, string state, string postalCode, string country, string countryCode)
        {
            var geo = GoogleGeocode(null, city, state, postalCode, country, countryCode);
            if (geo != null)
            {
                var q = from a in geo.results
                        from b in a.address_components
                        where b.types.Contains("locality") || b.types.Contains("sublocality")
                        select new City {
                            Name = b.long_name,
                            Latitude = a.geometry.location.lat,
                            Longitude = a.geometry.location.lng,

                            PostalCode = (from c in a.address_components
                                          where c.types.Contains("postal_code")
                                          select c.long_name).SingleOrDefault(),

                            State = (from c in a.address_components
                                              where c.types.Contains("administrative_area_level_1")
                                              select c.long_name).SingleOrDefault(),

                            Country = (from c in a.address_components where c.types.Contains("country")
                                       select c.long_name).SingleOrDefault(),
                        };
                if (q.Count() > 1)
                {
                    var choose = from c in q
                                 where ((string.Equals(c.PostalCode, postalCode, StringComparison.OrdinalIgnoreCase) || string.Equals(c.State, state, StringComparison.OrdinalIgnoreCase))
                                            || string.Equals(c.Name, city)) && 
                                        string.Equals(c.Country, country, StringComparison.OrdinalIgnoreCase)
                                    select c;
                    return choose.FirstOrDefault();
                }
                return q.FirstOrDefault();
            }
            return default(City);
        }
        public Address GetAddress(string street, string city, string state, string postalCode, string country, string countryCode)
        {
            var geo = GoogleGeocode(street, city, state, postalCode, country, countryCode);
            if (geo != null)
            {
                var q = from a in geo.results
                        from b in a.address_components
                        where b.types.Contains("locality")
                        select new Address {
                            Street = street ?? string.Format("{0} {1}", (from c in a.address_components where c.types.Contains("street_number") 
                                                                         select c.long_name).SingleOrEmpty(),
                                                                        (from c in a.address_components where c.types.Contains("route") 
                                                                         select c.long_name).SingleOrEmpty()),

                            City = city ?? (from c in a.address_components where c.types.Contains("locality")
                                            select c.long_name).SingleOrDefault(),

                            State = (from c in a.address_components where c.types.Contains("administrative_area_level_1")
                                     select c.long_name).SingleOrDefault(),

                            PostalCode = (from c in a.address_components where c.types.Contains("postal_code")
                                          select c.long_name).SingleOrDefault(),

                            Country = country ?? (from c in a.address_components where c.types.Contains("country")
                                                  select c.long_name).SingleOrDefault(),

                            Latitude = a.geometry.location.lat,
                            Longitude = a.geometry.location.lng,
                        };
                return q.FirstOrDefault();
            }
            return default(Address);
        }
        private GoogleGeocodeResponse GoogleGeocode(string street, string city, string state, string postalCode, string country, string countryCode)
        {
            var client = new HttpClient();
            var addressParts = new List<string>();            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            

            if (!string.IsNullOrWhiteSpace(street))
            {
                addressParts.Add(street);
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                addressParts.Add(city);
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                addressParts.Add(state);
            }
            if (!string.IsNullOrWhiteSpace(postalCode))
            {
                addressParts.Add(postalCode);
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                addressParts.Add(country);
            }
            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                addressParts.Add(countryCode);
            }
            var cleanAddress = RemoveBadChars(string.Join(",", addressParts));
            var requestUrl = string.Format(ServiceUrlFormat, cleanAddress);
            Log.Info(cleanAddress);
            try
            {
                var json = client.GetStringAsync(requestUrl).Result;
                return JsonConvert.DeserializeObject<GoogleGeocodeResponse>(json);
            }
            catch (Exception ex)
            {
                Log.Error(requestUrl, ex);
            }
            return default(GoogleGeocodeResponse);
        }
        public bool TryGetGeoLocation(PayPalVariables source, out GeoLocation location)
        {
            location = new GeoLocation();
            try
            {
                var geoCountry = GetCountry(source.address_country, source.address_country_code);
                var geoCity = GetCity(source.address_city, source.address_state, source.address_zip, source.address_country, source.address_country_code);
                var geoState = GetState(geoCity.State, source.address_country, source.address_country_code);

                City city;
                State state;
                Country country;
                var isCity = _metadataRepository.TryGetCity(geoCity.Name, geoState.Name, geoCity.PostalCode, geoCountry.FullName, out city);
                var isState = _metadataRepository.TryGetState(geoState.Name, geoCountry.FullName, out state);   
                var isCountry = _metadataRepository.TryGetCountry(geoCountry.FullName, out country);                                          

                if (!isCountry)
                {                    
                    country = _metadataRepository.CreateCountry(geoCountry);
                }
                if (!isState)
                {                    
                    geoState.CountryId = country.CountryId;
                    state = _metadataRepository.CreateState(geoState);
                }
                if (!isCity)
                {
                    geoCity.StateId = state.StateId;
                    geoCity.PostalCode = geoCity.PostalCode;
                    city = _metadataRepository.CreateCity(geoCity);
                }
                var address = GetAddress(source.address_street, city.Name, state.FullName, city.PostalCode, country.FullName, country.Name);
                if (address != null)
                {
                    address.CityId = city.CityId;
                    address.StateId = state.StateId;
                    address.CountryId = country.CountryId;
                    location.Address = address;
                }                
                location.City = city;
                location.State = state;
                location.Country = country;
                location.IsGeocoded = true;
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error while Geocoding.", ex);
            }
            return false;
        }
        private string RemoveBadChars(string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.IndexOfAny(invalidChars1) > -1)
            {
                return Regex.Replace(value, invalidChars2, "");
            }
            return value;
        }
        public class GoogleGeocodeResponse
        {
            public string status { get; set; }
            public IEnumerable<results> results { get; set; }
        }
        public class results
        {
            public string formatted_address { get; set; }
            public geometry geometry { get; set; }
            public IEnumerable<string> types { get; set; }
            public IEnumerable<address_component> address_components { get; set; }
        }
        public class geometry
        {
            public string location_type { get; set; }
            public location location { get; set; }
        }
        public class location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
        public class address_component
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public IEnumerable<string> types { get; set; }
        }
    }
}
