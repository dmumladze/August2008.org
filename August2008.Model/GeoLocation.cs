using System;

namespace August2008.Model
{
    public class GeoLocation
    {        
        public Address Address { get; set; }
        public City City { get; set; }
        public State State { get; set; }  
        public Country Country { get; set; }
        public bool IsGeocoded { get; set; }            
    }
}
