using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class DonationModel
    {
        public int DonationId { get; set; }
        public string ProviderName { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string UserMessage { get; set; }
        public DateTime DateDonated { get; set; }
    }
}