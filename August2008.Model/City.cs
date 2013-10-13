using System;

namespace August2008.Model
{
    public class City
    {
        public int CityId { get; set; }
        public int? StateId { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
