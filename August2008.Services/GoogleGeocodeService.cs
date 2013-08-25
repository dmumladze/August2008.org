using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using August2008.Common.Interfaces;
using August2008.Model;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace August2008.Services
{
    public class GoogleGeocodeService : IGeocodeService
    {
        private string _serviceUrl;
        private ILogger _logger;

        public GoogleGeocodeService(ILogger logger)
        {
            _logger = logger;
            _serviceUrl = ConfigurationManager.AppSettings["GeocodeUrlFormat"];
        }
        public Country GetCountry(string country)
        {
            var geo = GoogleGeocode(null, null, null, country);
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
                if (q.Count() != 0)
                {
                    return q.SingleOrDefault();
                }
            }
            return default(Country);
        }
        public State GetState(string state, string country)
        {
            var geo = GoogleGeocode(null, null, state, country);
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
                            Country = (from c in a.address_components
                                      where c.types.Contains("country")
                                      select c.long_name).SingleOrDefault()
                        };
                if (q.Count() != 0)
                {
                    return q.SingleOrDefault();
                }
            }
            return default(State);
        }
        public City GetCity(string city, string state, string country)
        {
            var geo = GoogleGeocode(null, city = city, state, country);
            if (geo != null)
            {
                var q = from a in geo.results
                        from b in a.address_components
                        where b.types.Contains("locality")
                        select new City {
                            Name = b.long_name,
                            Latitude = a.geometry.location.lat,
                            Longitude = a.geometry.location.lng,
                            State = (from c in a.address_components
                                     where c.types.Contains("administrative_area_level_1")
                                     select c.long_name).SingleOrDefault()
                        };
                if (q.Count() != 0)
                {
                    return q.SingleOrDefault();
                }
            }
            return default(City);
        }
        public Address GetAddress(string street, string city, string state, string country)
        {
            var geo = GoogleGeocode(street, city, state, country);
            return null;
        }
        private GoogleGeoCodeResponse GoogleGeocode(string street, string city, string state, string country) 
        {
            var client = new HttpClient();
            var format = new StringBuilder();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            if (!string.IsNullOrWhiteSpace(street))
            {
                format.Append(street);
                format.Append(" ");
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                format.Append(city);
                format.Append(" ");
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                format.Append(state);
                format.Append(" ");
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                format.Append(country);
            }
            string requestUrl = string.Format(_serviceUrl, format.ToString());
            try
            {                
                var json = client.GetStringAsync(requestUrl).Result;
                return JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(json);
            }
            catch (Exception ex)
            {
                _logger.Error(requestUrl, ex);
            }
            return default(GoogleGeoCodeResponse);
        }
        /// <summary>
        /// Structure that matches up with Google Geocode json format
        /// </summary>
        public class GoogleGeoCodeResponse
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
