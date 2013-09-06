using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class DonationModel
    {
        public int DonationId { get; set; }
        public int DonationProviderId { get; set; }
        public int UserId { get; set; }
        public string ExternalId { get; set; }
        public string ExternaStatus { get; set; } 
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string DisplayName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ProviderName { get; set; } 
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string UserMessage { get; set; }
        public DateTime DateDonated { get; set; }
    }
}