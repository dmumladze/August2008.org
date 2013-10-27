using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Model
{
    public class Donation
    {
        public int DonationId { get; set; }
        public int DonationProviderId { get; set; }
        public int UserId { get; set; }
        public string ExternalId { get; set; }
        public string ExternalStatus { get; set; }
        public bool IsCompleted { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? DonationSubscriptionId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ProviderName { get; set; }
        public string DisplayName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string UserMessage { get; set; }
        public string TransactionType { get; set; }
        public DateTime DateDonated { get; set; }
        public string ProviderXml { get; set; } 
        public IDictionary<string, string> ProviderData { get; set; }
    }
}
