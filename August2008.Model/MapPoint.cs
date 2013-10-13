using System;

namespace August2008.Model
{
    public class MapPoint
    {
        public MapPoint()
        {
        }
        public MapPoint(double? latitude, double? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public decimal TotalSum { get; set; }
        public int TotalCount { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
