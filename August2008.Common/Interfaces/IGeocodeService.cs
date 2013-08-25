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
        Country GetCountry(string country); 
        State GetState(string state, string country);
        City GetCity(string city, string state, string country);
        Address GetAddress(string street, string city, string state, string postalCode, string country);        
    }
}
