using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Model
{
    public class State
    {
        public int StateId { get; set; }
        public int? CountryId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Country { get; set; }
    }
}
