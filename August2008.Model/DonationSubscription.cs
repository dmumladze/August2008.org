using System;

namespace August2008.Model
{
    public class DonationSubscription
    {
        public int DonationSubscriptionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RecurrenceTimes { get; set; }
        public string SubscriptionId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public string ProviderXml { get; set; }
    }
}
